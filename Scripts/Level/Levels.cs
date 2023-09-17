using System.Diagnostics;
using Godot;
using Godot.Collections;

public partial class Levels : Node
{
	[Export()] private Array<LevelData> _levels = new Array<LevelData>();

	public LevelData GetLevelData(int index)
	{
		if (_levels.Count < index)
			return new LevelData();
		
		return _levels[index];
	}

	public int GetNextLevel(int level)
	{
		Debug.Print("Level : " + level);
		level++;
		if (level >= _levels.Count)
		{
			Debug.Print("Return Level : -1");
			return -1;
		}

		Debug.Print("Return Level : " + level);
		return level;
	}
}
