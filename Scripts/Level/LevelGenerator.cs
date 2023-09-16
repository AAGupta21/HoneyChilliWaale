using Godot;
using System;
using System.Diagnostics;
using Godot.NativeInterop;

public partial class LevelGenerator : Node
{
	[Export()] private Levels _levels;
	[Export()] private Sprite2D _targetImage;
	[Export()] private Button _inputSymbolPrefab;
	[Export()] private HBoxContainer _inputPanel;
	[Export()] private Texture2D _defaultSprite;

	public void LoadLevel(int index)
	{
		var data = _levels.GetLevelData(index);
		
		Debug.Print("LevelName : " + data._baseImage);

		foreach (var VARIABLE in data._winningSymbols)
		{
			
		}
	}

	public void ClearLevel()
	{
		_targetImage.Texture = _defaultSprite;
		
		
	}
}
