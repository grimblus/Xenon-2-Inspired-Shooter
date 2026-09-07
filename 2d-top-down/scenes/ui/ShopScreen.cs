using Godot;

/// <summary>
/// Between-levels shop: displays current score and three purchasable upgrades
/// (missile, drone, dodge), spending score via GameState. Next Level and Exit buttons.
/// </summary>
public partial class ShopScreen : Control
{
	[Export] public string NextLevelPath = "res://scenes/levels/level_2.tscn";

	[Export] public int MissilePrice = 100;
	[Export] public int DronePrice = 150;
	[Export] public int DodgePrice = 200;

	private Button nextLevelButton;
	private Button exitButton;
	private Label shopLabel;
	private Label scoreLabel;
	private Tween pulseTween;

	/// <summary>Caches UI node references, wires up navigation buttons, sets up each shop item, and starts the title's pulse animation.</summary>
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

	/// <summary>
	/// Wires up one shop item's price/buy-button UI: shows price or "Owned", and on click
	/// either completes the purchase (spending score, marking owned) or flashes the price
	/// red if funds are insufficient.
	/// </summary>
	private void SetupItem(string itemNode, int price, System.Func<bool> isOwned, System.Action markOwned)
	{
		var priceLabel = GetNode<Label>($"VBoxContainer/Items_HBox/{itemNode}_Panel/{itemNode}_VBox/{itemNode}_Price");
		var buyButton = GetNode<Button>($"VBoxContainer/Items_HBox/{itemNode}_Panel/{itemNode}_VBox/{itemNode}_Buy");
		Tween flashTween = null;

		// Briefly flashes the price label red to indicate a failed purchase attempt.
		void FlashInsufficientFunds()
		{
			flashTween?.Kill();
			priceLabel.Modulate = Colors.Red;

			flashTween = priceLabel.CreateTween();
			flashTween.TweenProperty(priceLabel, "modulate", Colors.White, 0.4f)
				.SetTrans(Tween.TransitionType.Sine)
				.SetEase(Tween.EaseType.Out);
		}

		// Syncs the price label/buy button to whether this item is currently owned.
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
				Refresh();
			}
			else
			{
				FlashInsufficientFunds();
			}
		};

		Refresh();
	}

	/// <summary>Refreshes the score label from GameState's current score.</summary>
	private void UpdateScoreLabel()
	{
		scoreLabel.Text = $"Score: {GameState.Instance.Score:D7}";
	}

	/// <summary>Loops a gentle scale pulse on the "Shop" title label. Pivot is recomputed via CallDeferred since this runs from _Ready, before layout has necessarily settled.</summary>
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

	/// <summary>Advances to the next level.</summary>
	private void GoToNextLevel()
	{
		GetTree().ChangeSceneToFile(NextLevelPath);
	}

	/// <summary>Quits the game.</summary>
	private void Exit()
	{
		GetTree().Quit();
	}
}
