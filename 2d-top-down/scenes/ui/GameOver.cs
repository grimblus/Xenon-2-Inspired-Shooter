using Godot;

/// <summary>
/// Game Over overlay shown when the player dies (embedded in main.tscn). Displays a
/// pulsing title with Retry / Exit / Scores buttons, the last also linking to HighScores.
/// </summary>
public partial class GameOver : CanvasLayer {
	private Button retryButton;
	private Button exitButton;
	private Button scoresButton;
	private Label gameOverLabel;
	private Tween pulseTween;

	/// <summary>Caches UI node references and wires up the button/keyboard handlers. Runs even while the game is paused.</summary>
	public override void _Ready()
	{
		ProcessMode = ProcessModeEnum.Always;
		retryButton = GetNode<Button>("GameOver_UI/VBoxContainer/Retry_Button");
		exitButton = GetNode<Button>("GameOver_UI/VBoxContainer/Exit_Button");
		scoresButton = GetNode<Button>("GameOver_UI/VBoxContainer/Scores_Button");
		gameOverLabel = GetNode<Label>("GameOver_UI/VBoxContainer/GameOver_Label");
		retryButton.Pressed += Retry;
		exitButton.Pressed += Exit;
		scoresButton.Pressed += ShowScores;
		gameOverLabel.PivotOffset = gameOverLabel.Size / 2f;
		gameOverLabel.Resized += () => gameOverLabel.PivotOffset = gameOverLabel.Size / 2f;
	}

	/// <summary>Shows the screen and starts the title's pulse animation.</summary>
	public void ShowGameOver()
	{
		Visible = true;
		StartPulse();
	}

	/// <summary>Loops a gentle scale pulse on the title label. Pivot is recomputed here (not just in _Ready) since by now layout has definitely settled.</summary>
	private void StartPulse()
	{
		pulseTween?.Kill();
		gameOverLabel.PivotOffset = gameOverLabel.Size / 2f;
		pulseTween = gameOverLabel.CreateTween();
		pulseTween.SetLoops();
		pulseTween.TweenProperty(gameOverLabel, "scale", new Vector2(1.15f, 1.15f), 0.6f)
			.SetTrans(Tween.TransitionType.Sine)
			.SetEase(Tween.EaseType.InOut);
		pulseTween.TweenProperty(gameOverLabel, "scale", Vector2.One, 0.6f)
			.SetTrans(Tween.TransitionType.Sine)
			.SetEase(Tween.EaseType.InOut);
	}

	/// <summary>Keyboard shortcuts for Retry/Exit while this screen is visible.</summary>
	public override void _UnhandledInput(InputEvent @event)
	{
		if (!Visible)
			return;
		if (@event.IsActionPressed("retry"))
		{
			Retry();
		}
		else if (@event.IsActionPressed("exit"))
		{
			Exit();
		}
	}

	/// <summary>Resets score/upgrades, unpauses the game, and reloads the current level from scratch.</summary>
	private void Retry()
	{
		pulseTween?.Kill();
		GameState.Instance.Reset();
		GetTree().Paused = false;
		GetTree().ReloadCurrentScene();
	}

	/// <summary>Quits the game.</summary>
	private void Exit()
	{
		GetTree().Quit();
	}

	/// <summary>Hides this screen and shows the high scores screen instead.</summary>
	private void ShowScores()
	{
		Visible = false;
		var highScores = GetNode<HighScores>("../CanvasLayer_HighScores");
		highScores.ShowScores();
	}
}
