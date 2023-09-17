using Godot;
using System;
using System.Diagnostics;
using Godot.Collections;
using Godot.NativeInterop;

public partial class LevelGenerator : Node
{
	[Export()] private Color _colorButtonDefault;
	[Export()] private Color _colorButtonToggled;
	
	[Export()] private string _inputSymbolPrefabPath;
	[Export()] private string _levelDataPath;
	[Export()] private string _symbolPath;
	
	[Export()] private Node _targetParent;
	[Export()] private Node _targetMainParent;
	
	[Export()] private HBoxContainer _inputPanel;
	
	[Export()] private RichTextLabel _richTextLabel;
	[Export()] private Node2D _winPanel;
	[Export()] private Node2D _loosePanel;
	[Export()] private Button _contButton;
	[Export()] private Button _contExitButton;
	[Export()] private Control _resultPanel;

	[Export()] private RichTextLabel _text1;
	[Export()] private RichTextLabel _text2;
	[Export()] private RichTextLabel _text3;
	[Export()] private RichTextLabel _text4;

	private Levels _levelData;
	private Sprite2D _mainNode;
	private int _selectedIndex;
	private Dictionary<int, int> _indexDict;
	private Array<int> _allSelectedIndexes;
	private LevelData _data;
	private int _currentLevel;

	public void Setup()
	{
		_levelData = (ResourceLoader.Load(_levelDataPath) as PackedScene).Instantiate() as Levels;
	}

	public void LoadLevel(int index)
	{
		_indexDict = new Dictionary<int, int>();
		_allSelectedIndexes = new Array<int>();
		_currentLevel = index;
		_data = _levelData.GetLevelData(index);
		int nodeIndex = 1;
		
		_data._allNodesIndex.Shuffle();
		
		foreach (var n in _data._allNodesIndex)
		{
			var sprite = (ResourceLoader.Load(_symbolPath + "_" + n + ".tscn") as PackedScene).Instantiate() as Sprite2D;

			var buttonNode = ResourceLoader.Load(_inputSymbolPrefabPath) as PackedScene;
			var button = buttonNode.Instantiate() as Button;

			// button.AddThemeStyleboxOverride("default", styleBox);

			button.SelfModulate = _colorButtonDefault;
			
			button.AddChild(sprite);
			sprite.Position = new Vector2(75f, 75f);
			sprite.Scale = new Vector2(0.55f, 0.55f);
			
			button.AddChild(sprite);
			_inputPanel.AddChild(button);
			_indexDict.Add(nodeIndex, n);
			nodeIndex++;
			button.ButtonUp += (() =>
			{
				button.SelfModulate = _colorButtonToggled;
				_selectedIndex = button.GetIndex();
				ButtonOnButtonUp();
			});
		}

		var targetNode = (ResourceLoader.Load(_data._targetNodePath) as PackedScene).Instantiate();
		_targetParent.AddChild(targetNode);

		_mainNode = (ResourceLoader.Load(_data._targetNodePath) as PackedScene).Instantiate() as Sprite2D;
		_targetMainParent.AddChild(_mainNode);
		_mainNode.Scale = new Vector2(1.5f, 1.5f);
		for (int i = 0; i < _mainNode.GetChildCount(); i++)
		{
			_mainNode.GetChild<Sprite2D>(i).Visible = false;
		}
	}

	private void ButtonOnButtonUp()
	{
		var i = _indexDict[_selectedIndex];
		if (!_allSelectedIndexes.Contains(i))
		{
			_inputPanel.GetChild<Button>(_selectedIndex).SelfModulate = _colorButtonToggled;
			_allSelectedIndexes.Add(i);
			_allSelectedIndexes.Sort();
			CheckWin(i);
		}
	}

	private void CheckWin(int index)
	{
		if (_data._rightNodesIndex.Contains(index))
		{
			string nodeName = "S" + index;
			var tempNode = _mainNode.FindChild(nodeName);
			if (tempNode != null)
			{
				_mainNode.GetChild<Sprite2D>(tempNode.GetIndex()).Visible = true;
			}

			var str = "";

			foreach (var i in _allSelectedIndexes)
			{
				str += _data._nodesText[i] + " ";
			}

			_richTextLabel.Text = str;
			if (_data._rightNodesIndex.Count == _allSelectedIndexes.Count)
			{
				_resultPanel.Visible = true;
				_currentLevel = _levelData.GetNextLevel(_currentLevel);
				SetText(_currentLevel);
				_winPanel.Visible = true;
				_contExitButton.Visible = false;
				_loosePanel.Visible = false;
				_contButton.Visible = true;
				//Load the text based on level.
				if (_currentLevel != -1)
				{
					_contButton.Text = "Proceed";
					//Load Next level.
					_contButton.ButtonUp += () =>
					{
						_resultPanel.Visible = false;
						_winPanel.Visible = false;
						_loosePanel.Visible = false;
						_contButton.Visible = false;
						_contExitButton.Visible = false;
						SetText(0);
						ClearLevel();
						LoadLevel(_currentLevel);
					};
				}
				else
				{
					_contButton.Text = "Main Menu";
					//Load Main Menu. Game finished.
					_contButton.ButtonUp += () =>
					{
						_resultPanel.Visible = false;
						_winPanel.Visible = false;
						_loosePanel.Visible = false;
						_contButton.Visible = false;
						_contExitButton.Visible = false;
						SetText(0);
						GetTree().ReloadCurrentScene();
					};
				}
			}
		}
		else
		{
			_resultPanel.Visible = true;
			_winPanel.Visible = false;
			_loosePanel.Visible = true;
			_contExitButton.Visible = true;
			_contButton.Visible = false;
			_contExitButton.ButtonUp += () =>
			{
				_resultPanel.Visible = false;
				_winPanel.Visible = false;
				_loosePanel.Visible = false;
				_contButton.Visible = false;
				_contExitButton.Visible = false;
				SetText(0);
				GetTree().ReloadCurrentScene();
			};
			//Loose
		}
	}

	private void SetText(int index)
	{
		switch (index)
		{
			case 2:
				_text1.Visible = true;
				break;
			case 3:
				_text1.Visible = true;
				_text2.Visible = true;
				break;
			case 4:
				_text1.Visible = true;
				_text2.Visible = true;
				_text3.Visible = true;
				break;
			case -1:
				
				_text1.Visible = true;
				_text2.Visible = true;
				_text3.Visible = true;
				_text4.Visible = true;
				break;
			default:
				_text1.Visible = false;
				_text2.Visible = false;
				_text3.Visible = false;
				_text4.Visible = false;
				break;
		}
	}

	public void ClearLevel()
	{
		for (int i = _inputPanel.GetChildCount() - 1; i > 0; i--)
		{
			_inputPanel.GetChild(i).QueueFree();
		}

		_richTextLabel.Text = "";
		_mainNode = null;
		_targetMainParent.GetChild(0).QueueFree();
		_targetParent.GetChild(0).QueueFree();
		_indexDict.Clear();
		_allSelectedIndexes.Clear();
		_data = null;
	}

	public void RestartLevel()
	{
		_allSelectedIndexes.Clear();
		_richTextLabel.Text = "";

		for (int i = 0; i < _inputPanel.GetChildCount(); i++)
		{
			_inputPanel.GetChild<Button>(i).SelfModulate = _colorButtonDefault;
		}
		
		for (int i = 0; i < _mainNode.GetChildCount(); i++)
		{
			_mainNode.GetChild<Sprite2D>(i).Visible = false;
		}
	}
}
