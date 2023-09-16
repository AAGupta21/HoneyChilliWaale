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
	
	[Export()] private Texture2D _defaultSprite;

	private Levels _levelData;
	private Node2D _mainNode;
	private int _selectedIndex;
	private Dictionary<int, int> _indexDict;
	private Array<int> _allSelectedIndexes;
	private LevelData _data;

	public override void _Ready()
	{
		base._Ready();
		
		_levelData = (ResourceLoader.Load(_levelDataPath) as PackedScene).Instantiate() as Levels;
		_indexDict = new Dictionary<int, int>();
		_allSelectedIndexes = new Array<int>();
		LoadLevel(0);
	}

	public void LoadLevel(int index)
	{
		_data = _levelData.GetLevelData(index);
		int nodeIndex = 0;
		
		foreach (var n in _data._allNodesIndex)
		{
			var sprite = (ResourceLoader.Load(_symbolPath + "_" + n + ".tscn") as PackedScene).Instantiate() as Sprite2D;

			var buttonNode = ResourceLoader.Load(_inputSymbolPrefabPath) as PackedScene;
			var button = buttonNode.Instantiate() as Button;

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
				_selectedIndex = button.GetIndex();
				ButtonOnButtonUp();
			});
		}

		var targetNode = (ResourceLoader.Load(_data._targetNodePath) as PackedScene).Instantiate();
		_targetParent.AddChild(targetNode);

		_mainNode = (ResourceLoader.Load(_data._targetNodePath) as PackedScene).Instantiate() as Node2D;
		_targetMainParent.AddChild(_mainNode);
		_mainNode.Scale = new Vector2(1.5f, 1.5f);
	}

	private void ButtonOnButtonUp()
	{
		Debug.Print(_selectedIndex + " , " + _indexDict[_selectedIndex]);
		var i = _indexDict[_selectedIndex];
		if (_allSelectedIndexes.Contains(i))
		{
			_inputPanel.GetChild<Button>(_selectedIndex).SelfModulate = _colorButtonDefault;
			_allSelectedIndexes.Remove(i);
		}
		else
		{
			_inputPanel.GetChild<Button>(_selectedIndex).SelfModulate = _colorButtonToggled;
			_allSelectedIndexes.Add(i);
			CheckWin();
		}
	}

	private void CheckWin()
	{
		Debug.Print("Checking Win...");
		bool won = true;

		if (_data._rightNodesIndex.Count != _allSelectedIndexes.Count)
		{
			won = false;
		}
		else
		{
			foreach (var i in _data._rightNodesIndex)
			{
				if (!_allSelectedIndexes.Contains(i))
				{
					won = false;
					break;
				}
			}
		}

		if(won)
			Debug.Print("Won");
		else
			Debug.Print("Not Yet...");
	}

	public void ClearLevel()
	{
		for (int i = 0; i < _inputPanel.GetChildCount(); i++)
		{
			_inputPanel.GetChild(i).Dispose();
		}

		_mainNode = null;
		_targetMainParent.GetChild(0).Dispose();
		_targetParent.GetChild(0).Dispose();
		_indexDict.Clear();
		_allSelectedIndexes.Clear();
		_data = null;
	}
}
