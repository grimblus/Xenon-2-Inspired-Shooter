using Godot;

/// <summary>
/// Pause overlay toggled with the pause action (Escape) during gameplay (embedded in main.tscn,
/// inherited by level_2.tscn since it instances main.tscn). Offers Resume / Retry / Exit.
/// </summary>
public partial class PauseScreen : CanvasLayer
{
	private Button resumeButton;
	private Button retryButton;
	private Button exitButton;
	private Label pauseLabel;
	private Tween pulseTween;

	/// <summary>Caches UI node references and wires up the button handlers. Runs even while the game is paused.</summary>
	public override void _Ready()
	{
		ProcessMode = ProcessModeEnum.Always;
		resumeButton = GetNode<Button>("Pause_UI/VBoxContainer/Resume_Button");
		retryButton = GetNode<Button>("Pause_UI/VBoxContainer/Retry_Button");
		exitButton = GetNode<Button>("Pause_UI/VBoxContainer/Exit_Button");
		pauseLabel = GetNode<Label>("Pause_UI/VBoxContainer/Pause_Label");
		resumeButton.Pressed += Resume;
		retryButton.Pressed += Retry;
		exitButton.Pressed += Exit;
		pauseLabel.PivotOffset = pauseLabel.Size / 2f;
		pauseLabel.Resized += () => pauseLabel.PivotOffset = pauseLabel.Size / 2f;
	}

	/// <summary>Loops a gentle scale pulse on the title label. Pivot is recomputed here (not just in _Ready) since by now layout has definitely settled.</summary>
	private void StartPulse()
	{
		pulseTween?.Kill();
		pauseLabel.PivotOffset = pauseLabel.Size / 2f;
		pulseTween = pauseLabel.CreateTween();
		pulseTween.SetLoops();
		pulseTween.TweenProperty(pauseLabel, "scale", new Vector2(1.15f, 1.15f), 0.6f)
			.SetTrans(Tween.TransitionType.Sine)
			.SetEase(Tween.EaseType.InOut);
		pulseTween.TweenProperty(pauseLabel, "scale", Vector2.One, 0.6f)
			.SetTrans(Tween.TransitionType.Sine)
			.SetEase(Tween.EaseType.InOut);
	}

	/// <summary>Toggles the pause menu on the pause action, unless the GameOver/Congrats screen already has focus.</summary>
	public override void _UnhandledInput(InputEvent @event)
	{
		if (!@event.IsActionPressed("pause") || !CanTogglePause())
			return;

		if (Visible)
			Resume();
		else
			Open();
	}

	/// <summary>True as long as neither the death nor victory screen is currently showing.</summary>
	private bool CanTogglePause()
	{
		var gameOver = GetNodeOrNull<CanvasLayer>("../CanvasLayer_GameOver");
		if (gameOver != null && gameOver.Visible)
			return false;

		var congrats = GetNodeOrNull<CanvasLayer>("../CanvasLayer_Congrats");
		if (congrats != null && congrats.Visible)
			return false;

		return true;
	}

	/// <summary>Shows the pause menu, freezes the game tree, and starts the title's pulse animation.</summary>
	private void Open()
	{
		Visible = true;
		GetTree().Paused = true;
		StartPulse();
	}

	/// <summary>Hides the pause menu and unfreezes the game tree.</summary>
	private void Resume()
	{
		pulseTween?.Kill();
		Visible = false;
		GetTree().Paused = false;
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
}
