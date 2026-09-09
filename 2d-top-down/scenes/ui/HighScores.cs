using Godot;

/// <summary>
/// All-time high scores overlay (embedded in main.tscn, reached from GameOver). Rebuilds
/// its list from ScoreManager each time it's shown.
/// </summary>
public partial class HighScores : CanvasLayer
{
	private VBoxContainer scoresContainer;
	private Button backButton;
	private Label highScoresLabel;
	private Tween pulseTween;

	/// <summary>Caches UI node references and wires up the Back button. Runs even while the game is paused.</summary>
	public override void _Ready()
	{
		ProcessMode = ProcessModeEnum.Always;

		scoresContainer = GetNode<VBoxContainer>("HighScores_UI/Scores_Container");
		backButton = GetNode<Button>("HighScores_UI/Back_Button");
		highScoresLabel = GetNode<Label>("HighScores_UI/Label_HighScores");

		backButton.Pressed += GoBack;
		highScoresLabel.PivotOffset = highScoresLabel.Size / 2f;
		highScoresLabel.Resized += () => highScoresLabel.PivotOffset = highScoresLabel.Size / 2f;
	}

	/// <summary>Shows the screen with a freshly rebuilt scores list and starts the title's pulse animation.</summary>
	public void ShowScores()
	{
		Visible = true;
		RefreshDisplay();
		StartPulse();
	}

	/// <summary>Loops a gentle scale pulse on the title label. Pivot is recomputed here (not just in _Ready) since by now layout has definitely settled.</summary>
	private void StartPulse()
	{
		pulseTween?.Kill();
		highScoresLabel.PivotOffset = highScoresLabel.Size / 2f;
		pulseTween = highScoresLabel.CreateTween();
		pulseTween.SetLoops();
		pulseTween.TweenProperty(highScoresLabel, "scale", new Vector2(1.15f, 1.15f), 0.6f)
			.SetTrans(Tween.TransitionType.Sine)
			.SetEase(Tween.EaseType.InOut);
		pulseTween.TweenProperty(highScoresLabel, "scale", Vector2.One, 0.6f)
			.SetTrans(Tween.TransitionType.Sine)
			.SetEase(Tween.EaseType.InOut);
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
		pulseTween?.Kill();
		Visible = false;

		var gameOverLayer = GetNode<CanvasLayer>("../CanvasLayer_GameOver");
		gameOverLayer.Visible = true;
	}
}
