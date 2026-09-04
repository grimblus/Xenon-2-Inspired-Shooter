using Godot;

public partial class GameOver : CanvasLayer {
	private Button retryButton;
	private Button exitButton;
	private Label gameOverLabel;
	private Tween pulseTween;

	public override void _Ready()
	{
		ProcessMode = ProcessModeEnum.Always;

		retryButton = GetNode<Button>("GameOver_UI/VBoxContainer/Retry_Button");
		exitButton = GetNode<Button>("GameOver_UI/VBoxContainer/Exit_Button");
		gameOverLabel = GetNode<Label>("GameOver_UI/VBoxContainer/GameOver_Label");

		retryButton.Pressed += Retry;
		exitButton.Pressed += Exit;

		gameOverLabel.PivotOffset = gameOverLabel.Size / 2f;
		gameOverLabel.Resized += () => gameOverLabel.PivotOffset = gameOverLabel.Size / 2f;
	}

	public void ShowGameOver()
	{
		Visible = true;
		StartPulse();
	}

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

	private void Retry()
	{
		pulseTween?.Kill();
		GetTree().Paused = false;
		GetTree().ReloadCurrentScene();
	}

	private void Exit()
	{
		GetTree().Quit();
	}
}
