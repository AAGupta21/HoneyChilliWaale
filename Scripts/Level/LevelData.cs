using Godot;
using Godot.Collections;

public partial class LevelData : Node
{
    [Export()] private Texture2D _baseImage = new Texture2D();
    [Export()] private Array<Texture2D> _winningSymbols = new Array<Texture2D>();
    [Export()] private Array<int> _winningIndex = new Array<int>();
}