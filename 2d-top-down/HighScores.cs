using Godot;

public partial class HighScores : CanvasLayer
{
	private VBoxContainer scoresContainer;
	private Button backButton;

	public override void _Ready()
	{
		ProcessMode = ProcessModeEnum.Always;

		scoresContainer = GetNode<VBoxContainer>("HighScores_UI/Scores_Container");
		backButton = GetNode<Button>("HighScores_UI/Back_Button");

		backButton.Pressed += GoBack;
	}

	public void ShowScores()
	{
		Visible = true;
		RefreshDisplay();
	}

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
		label.AddThemeFontOverride("font", GD.Load<FontFile>("res://fonts/pixel_font.png")); // add this line
		scoresContainer.AddChild(label);
	}
}

	private void GoBack()
	{
		Visible = false;

		var gameOverLayer = GetNode<CanvasLayer>("../CanvasLayer_GameOver");
		gameOverLayer.Visible = true;
	}
}
