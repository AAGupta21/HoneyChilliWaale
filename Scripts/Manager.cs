using Godot;

public partial class Manager : Node
{
    [Export()] private Button _playButton;
    [Export()] private Node2D _mainMenu;
    [Export()] private string _levelGeneratorPath;
    private LevelGenerator _levelGenerator;
    
    public override void _Ready()
    {
        base._Ready();
        _playButton.ButtonUp += PlayButtonOnButtonUp;
        _mainMenu.Visible = true;
    }

    private void PlayButtonOnButtonUp()
    {
        _mainMenu.Visible = false;
        _levelGenerator = (ResourceLoader.Load(_levelGeneratorPath) as PackedScene).Instantiate() as LevelGenerator;
        AddChild(_levelGenerator);
        _levelGenerator.Setup();
        _levelGenerator.LoadLevel(1);
    }
}