using Godot;

public partial class MainMenu : Control
{
	[Export] public string StartGamePath = "res://main.tscn";

	private Button startGameButton;
	private Button controlsButton;
	private Button exitButton;
	private Control controlsPanel;
	private Button backButton;

	public override void _Ready()
	{
		startGameButton = GetNode<Button>("VBoxContainer/StartGame_Button");
		controlsButton = GetNode<Button>("VBoxContainer/Controls_Button");
		exitButton = GetNode<Button>("VBoxContainer/Exit_Button");
		controlsPanel = GetNode<Control>("ControlsPanel");
		backButton = GetNode<Button>("ControlsPanel/PanelContainer/VBoxContainer/Back_Button");

		startGameButton.Pressed += StartGame;
		controlsButton.Pressed += ShowControls;
		exitButton.Pressed += Exit;
		backButton.Pressed += HideControls;
	}

	private void StartGame()
	{
		GetTree().ChangeSceneToFile(StartGamePath);
	}

	private void ShowControls()
	{
		controlsPanel.Visible = true;
	}

	private void HideControls()
	{
		controlsPanel.Visible = false;
	}

	private void Exit()
	{
		GetTree().Quit();
	}
}
