using Godot;
using Godot.Collections;

public partial class LevelData : Node
{
	[Export()] public Array<int> _rightNodesIndex = new Array<int>();
	[Export()] public Dictionary<int, string> _nodesText = new Dictionary<int, string>();
	[Export()] public Array<int> _allNodesIndex = new Array<int>();
	[Export()] public string _targetNodePath = "";
}
