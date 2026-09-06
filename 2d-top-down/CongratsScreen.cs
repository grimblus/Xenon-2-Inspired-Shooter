using Godot;

public partial class CongratsScreen : CanvasLayer
{
	private Button menuButton;
	private Button exitButton;
	private Label congratsLabel;
	private Label scoreLabel;
	private Tween pulseTween;

	public override void _Ready()
	{
		ProcessMode = ProcessModeEnum.Always;

		menuButton = GetNode<Button>("Congrats_UI/VBoxContainer/MainMenu_Button");
		exitButton = GetNode<Button>("Congrats_UI/VBoxContainer/Exit_Button");
		congratsLabel = GetNode<Label>("Congrats_UI/VBoxContainer/Congrats_Label");
		scoreLabel = GetNode<Label>("Congrats_UI/VBoxContainer/Score_Label");

		menuButton.Pressed += ReturnToMenu;
		exitButton.Pressed += Exit;
	}

	public void ShowCongrats()
	{
		Visible = true;
		scoreLabel.Text = $"Final Score: {GameState.Instance.Score:D7}";
		StartPulse();
	}

	private void StartPulse()
	{
		pulseTween?.Kill();

		congratsLabel.PivotOffset = congratsLabel.Size / 2f;

		pulseTween = congratsLabel.CreateTween();
		pulseTween.SetLoops();

		pulseTween.TweenProperty(congratsLabel, "scale", new Vector2(1.15f, 1.15f), 0.6f)
			.SetTrans(Tween.TransitionType.Sine)
			.SetEase(Tween.EaseType.InOut);

		pulseTween.TweenProperty(congratsLabel, "scale", Vector2.One, 0.6f)
			.SetTrans(Tween.TransitionType.Sine)
			.SetEase(Tween.EaseType.InOut);
	}

	private void ReturnToMenu()
	{
		pulseTween?.Kill();
		GetTree().Paused = false;
		GetTree().ChangeSceneToFile("res://main_menu.tscn");
	}

	private void Exit()
	{
		GetTree().Quit();
	}
}
