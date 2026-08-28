using Godot;

public partial class GameOver : CanvasLayer {
	private Button retryButton;
	private Button exitButton;

	public override void _Ready()
{
		retryButton = GetNode<Button>("GameOver_UI/VBoxContainer/Retry_Button");
		exitButton = GetNode<Button>("GameOver_UI/VBoxContainer/Exit_Button");

		retryButton.Pressed += Retry;
		exitButton.Pressed += Exit;
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
		GetTree().Paused = false;
		GetTree().ReloadCurrentScene();
	}

	private void Exit()
	{
		GetTree().Quit();
	}
}
