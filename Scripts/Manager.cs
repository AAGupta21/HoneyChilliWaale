using Godot;

public partial class Manager : Node
{
    [Export()] private Button _playButton;
    
    [Export()] private Sprite2D _popupSprite;
    [Export()] private Sprite2D _howToPlaySprite;
    [Export()] private Sprite2D _creditsSprite;
    [Export()] private Sprite2D _exitApplicationSprite;

    [Export()] private Button _howToPlayButton;
    [Export()] private Button _creditsButton;
    [Export()] private Button _exitApplicationPopupButton;
    [Export()] private Button _agreeApplicationQuitButton;
    [Export()] private Button _disagreeApplicationQuitButton;
    [Export()] private Button _closePopupButton;
    [Export()] private Node2D _audioPopUp;
    [Export()] private AudioStreamPlayer2D _stream;
    [Export()] private Button _audioOn;
    [Export()] private Button _audioOff;
    [Export()] private Sprite2D _audioOffParent;
    [Export()] private Sprite2D _audioOnParent;

    [Export()] private Node2D _mainMenu;
    [Export()] private string _levelGeneratorPath;
    private LevelGenerator _levelGenerator;
    
    public override void _Ready()
    {
        base._Ready();
        _playButton.ButtonUp += PlayButtonOnButtonUp;
        _howToPlayButton.ButtonUp += HowToPlayButton;
        _creditsButton.ButtonUp += CreditViewButton;
        _exitApplicationPopupButton.ButtonUp += ExitApplicationPopupButton;
        _closePopupButton.ButtonUp += ClosePopupButton;
        _agreeApplicationQuitButton.ButtonUp += ApplicationQuitYesButton;
        _audioOn.ButtonUp += () =>
        {
            AudioButtonOnToggled(true);
        };

        _audioOff.ButtonUp += () =>
        {
            AudioButtonOnToggled(false);
        };
        
        _audioOn.Visible = true;
        _mainMenu.Visible = true;
    }

    private void AudioButtonOnToggled(bool buttonpressed)
    {
        if (buttonpressed)
        {
            _stream.Playing = false;
            _audioOnParent.Visible = false;
            _audioOffParent.Visible = true;
        }
        else
        {
            _stream.Playing = true;
            _audioOnParent.Visible = true;
            _audioOffParent.Visible = false;
        }
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
        
        PopupScreenVisiblity((true));
        _howToPlaySprite.Visible = true; 
        
    }
    
    private void CreditViewButton()
    {
        GD.Print("Credit View Tab");
        PopupScreenVisiblity((true));
        _creditsSprite.Visible = true; 
    }
    
    private void ExitApplicationPopupButton()
    {
        GD.Print("Exit application tab");
        PopupScreenVisiblity((true));
        _exitApplicationSprite.Visible = true;

    }

    private void PopupScreenVisiblity(bool state)
    {
        _audioPopUp.Visible = !state;
        _mainMenu.Visible = !state;
        _popupSprite.Visible = state;
    }

    private void ClosePopupButton()
    {
        _audioPopUp.Visible = true;
        _howToPlaySprite.Visible = false; 
        _creditsSprite.Visible = false; 
        _exitApplicationSprite.Visible = false;
        PopupScreenVisiblity((false));
        GD.Print("Exit application tab");

    }
    
    private void ApplicationQuitYesButton()
    {
        GD.Print("Exit application tab");
        GetTree().Quit();
    }
    
    private void ApplicationQuitNoButton()
    {
        _howToPlaySprite.Visible = false; 
        _creditsSprite.Visible = false; 
        _exitApplicationSprite.Visible = false;
        PopupScreenVisiblity((false));

        GD.Print("Exit application tab");

    }
    
    
    
    
}