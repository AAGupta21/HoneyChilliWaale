using Godot;
using Godot.Collections;

public partial class LevelData : Node
{
    [Export()] public Texture2D _baseImage = new Texture2D();
    [Export()] public Array<Texture2D> _winningSymbols = new Array<Texture2D>();
    [Export()] public Array<int> _winningIndex = new Array<int>();
}