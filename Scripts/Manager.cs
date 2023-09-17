using Godot;

public partial class Manager : Node
{
    [Export()] private Button _playButton;
    [Export()] private Button _howToPlayButton;
    [Export()] private Button _creditsButton;
    [Export()] private Button _exitButton;

    [Export()] private Node2D _mainMenu;
    [Export()] private string _levelGeneratorPath;
    private LevelGenerator _levelGenerator;
    
    public override void _Ready()
    {
        base._Ready();
        _playButton.ButtonUp += PlayButtonOnButtonUp;
        _howToPlayButton.ButtonUp += PlayButtonOnButtonUp;
        _creditsButton.ButtonUp += PlayButtonOnButtonUp;
        _exitButton.ButtonUp += PlayButtonOnButtonUp;
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

    private void HowToPlayButton()
    {
        GD.Print("How TO PLay Tab");
    }
    
    private void CreditViewButton()
    {
        GD.Print("Credit View Tab");
        
    }
    
    private void ExitApplicationButton()
    {
        GD.Print("Exit application tab");

    }
    
    
}