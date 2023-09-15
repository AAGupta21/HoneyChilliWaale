using Godot;
using Godot.Collections;

public partial class Levels : Node
{
    [Export()] private Array<LevelData> _levels = new Array<LevelData>();
}