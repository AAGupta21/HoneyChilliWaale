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
		if (level >= _levels.Count)
		{
			return -1;
		}

		return ++level;
	}
}
