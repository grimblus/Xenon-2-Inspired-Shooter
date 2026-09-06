using Godot;

public partial class ShopScreen : Control
{
	[Export] public string NextLevelPath = "res://level_2.tscn";

	[Export] public int MissilePrice = 100;
	[Export] public int DronePrice = 150;
	[Export] public int DodgePrice = 200;

	private Button nextLevelButton;
	private Button exitButton;
	private Label shopLabel;
	private Label scoreLabel;
	private Tween pulseTween;

	public override void _Ready()
	{
		nextLevelButton = GetNode<Button>("VBoxContainer/NextLevel_Button");
		exitButton = GetNode<Button>("VBoxContainer/Exit_Button");
		shopLabel = GetNode<Label>("VBoxContainer/Shop_Label");
		scoreLabel = GetNode<Label>("VBoxContainer/Score_Label");

		nextLevelButton.Pressed += GoToNextLevel;
		exitButton.Pressed += Exit;

		SetupItem("Item1", MissilePrice, () => GameState.Instance.MissileBuy, () => GameState.Instance.MissileBuy = true);
		SetupItem("Item2", DronePrice, () => GameState.Instance.DroneBuy, () => GameState.Instance.DroneBuy = true);
		SetupItem("Item3", DodgePrice, () => GameState.Instance.DodgeBuy, () => GameState.Instance.DodgeBuy = true);

		UpdateScoreLabel();
		CallDeferred(nameof(StartPulse));
	}

	private void SetupItem(string itemNode, int price, System.Func<bool> isOwned, System.Action markOwned)
	{
		var priceLabel = GetNode<Label>($"VBoxContainer/Items_HBox/{itemNode}_Panel/{itemNode}_VBox/{itemNode}_Price");
		var buyButton = GetNode<Button>($"VBoxContainer/Items_HBox/{itemNode}_Panel/{itemNode}_VBox/{itemNode}_Buy");

		void Refresh()
		{
			if (isOwned())
			{
				priceLabel.Text = "Owned";
				buyButton.Disabled = true;
				buyButton.Text = "Owned";
			}
			else
			{
				priceLabel.Text = $"{price} pts";
				buyButton.Disabled = false;
				buyButton.Text = "Buy";
			}
		}

		buyButton.Pressed += () =>
		{
			if (isOwned())
				return;

			if (GameState.Instance.TrySpend(price))
			{
				markOwned();
				UpdateScoreLabel();
			}

			Refresh();
		};

		Refresh();
	}

	private void UpdateScoreLabel()
	{
		scoreLabel.Text = $"Score: {GameState.Instance.Score:D7}";
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
