using Godot;

public partial class ShopScreen : Control
{
	[Export] public string NextLevelPath = "res://level_2.tscn";

	private Button nextLevelButton;
	private Button exitButton;
	private Label shopLabel;
	private Tween pulseTween;

	public override void _Ready()
	{
		nextLevelButton = GetNode<Button>("VBoxContainer/NextLevel_Button");
		exitButton = GetNode<Button>("VBoxContainer/Exit_Button");
		shopLabel = GetNode<Label>("VBoxContainer/Shop_Label");

		nextLevelButton.Pressed += GoToNextLevel;
		exitButton.Pressed += Exit;

		CallDeferred(nameof(StartPulse));
	}

	private void StartPulse()
	{
		pulseTween?.Kill();

		shopLabel.PivotOffset = shopLabel.Size / 2f;

		pulseTween = shopLabel.CreateTween();
		pulseTween.SetLoops();

		pulseTween.TweenProperty(shopLabel, "scale", new Vector2(1.15f, 1.15f), 0.6f)
			.SetTrans(Tween.TransitionType.Sine)
			.SetEase(Tween.EaseType.InOut);

		pulseTween.TweenProperty(shopLabel, "scale", Vector2.One, 0.6f)
			.SetTrans(Tween.TransitionType.Sine)
			.SetEase(Tween.EaseType.InOut);
	}

	private void GoToNextLevel()
	{
		GetTree().ChangeSceneToFile(NextLevelPath);
	}

	private void Exit()
	{
		GetTree().Quit();
	}
}
