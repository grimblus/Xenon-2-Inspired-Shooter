using Godot;

public partial class ShopScreen : Control
{
	[Export] public string NextLevelPath = "res://level_2.tscn";

	private Button nextLevelButton;
	private Button exitButton;

	public override void _Ready()
	{
		nextLevelButton = GetNode<Button>("VBoxContainer/NextLevel_Button");
		exitButton = GetNode<Button>("VBoxContainer/Exit_Button");

		nextLevelButton.Pressed += GoToNextLevel;
		exitButton.Pressed += Exit;
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
