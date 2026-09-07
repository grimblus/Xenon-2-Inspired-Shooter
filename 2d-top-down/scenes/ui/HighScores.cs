using Godot;

/// <summary>
/// All-time high scores overlay (embedded in main.tscn, reached from GameOver). Rebuilds
/// its list from ScoreManager each time it's shown.
/// </summary>
public partial class HighScores : CanvasLayer
{
	private VBoxContainer scoresContainer;
	private Button backButton;

	/// <summary>Caches UI node references and wires up the Back button. Runs even while the game is paused.</summary>
	public override void _Ready()
	{
		ProcessMode = ProcessModeEnum.Always;

		scoresContainer = GetNode<VBoxContainer>("HighScores_UI/Scores_Container");
		backButton = GetNode<Button>("HighScores_UI/Back_Button");

		backButton.Pressed += GoBack;
	}

	/// <summary>Shows the screen with a freshly rebuilt scores list.</summary>
	public void ShowScores()
	{
		Visible = true;
		RefreshDisplay();
	}

	/// <summary>Clears and rebuilds the scores list from ScoreManager.HighScores, one label per entry.</summary>
	private void RefreshDisplay()
	{
		foreach (Node child in scoresContainer.GetChildren())
		{
			child.QueueFree();
		}

		var scores = ScoreManager.Instance.HighScores;
		for (int i = 0; i < scores.Count; i++)
		{
			var label = new Label();
			label.Text = $"{i + 1}. {scores[i]:D7}";
			label.HorizontalAlignment = HorizontalAlignment.Center;
			label.AddThemeFontSizeOverride("font_size", 32);
			label.AddThemeFontOverride("font", GD.Load<FontFile>("res://fonts/pixel_font.png"));
			scoresContainer.AddChild(label);
		}
	}

	/// <summary>Hides this screen and shows the Game Over screen instead.</summary>
	private void GoBack()
	{
		Visible = false;

		var gameOverLayer = GetNode<CanvasLayer>("../CanvasLayer_GameOver");
		gameOverLayer.Visible = true;
	}
}
