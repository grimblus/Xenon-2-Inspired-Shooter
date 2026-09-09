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

	/// <summary>Caches UI node references and wires up the button handlers. Runs even while the game is paused.</summary>
	public override void _Ready()
	{
		ProcessMode = ProcessModeEnum.Always;
		resumeButton = GetNode<Button>("Pause_UI/VBoxContainer/Resume_Button");
		retryButton = GetNode<Button>("Pause_UI/VBoxContainer/Retry_Button");
		exitButton = GetNode<Button>("Pause_UI/VBoxContainer/Exit_Button");
		resumeButton.Pressed += Resume;
		retryButton.Pressed += Retry;
		exitButton.Pressed += Exit;
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

	/// <summary>Shows the pause menu and freezes the game tree.</summary>
	private void Open()
	{
		Visible = true;
		GetTree().Paused = true;
	}

	/// <summary>Hides the pause menu and unfreezes the game tree.</summary>
	private void Resume()
	{
		Visible = false;
		GetTree().Paused = false;
	}

	/// <summary>Resets score/upgrades, unpauses the game, and reloads the current level from scratch.</summary>
	private void Retry()
	{
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
