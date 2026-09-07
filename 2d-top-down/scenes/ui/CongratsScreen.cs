using Godot;

/// <summary>
/// Victory overlay shown when the boss dies (embedded in level_2.tscn, since it's boss-only).
/// Displays the final score and offers Main Menu / Exit buttons. Mirrors GameOver's pattern.
/// </summary>
public partial class CongratsScreen : CanvasLayer
{
	private Button menuButton;
	private Button exitButton;
	private Label congratsLabel;
	private Label scoreLabel;
	private Tween pulseTween;

	/// <summary>Caches UI node references and wires up the button handlers. Runs even while the game is paused.</summary>
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

	/// <summary>Shows the screen with the current final score and starts the title's pulse animation.</summary>
	public void ShowCongrats()
	{
		Visible = true;
		scoreLabel.Text = $"Final Score: {GameState.Instance.Score:D7}";
		StartPulse();
	}

	/// <summary>Loops a gentle scale pulse on the title label. Pivot is recomputed here (not in _Ready) since by now layout has definitely settled.</summary>
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

	/// <summary>Unpauses the game and returns to the main menu.</summary>
	private void ReturnToMenu()
	{
		pulseTween?.Kill();
		GetTree().Paused = false;
		GetTree().ChangeSceneToFile("res://scenes/ui/main_menu.tscn");
	}

	/// <summary>Quits the game.</summary>
	private void Exit()
	{
		GetTree().Quit();
	}
}
