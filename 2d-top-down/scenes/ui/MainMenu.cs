using Godot;

/// <summary>
/// The main menu, reached after the boot sequence: Start Game, a toggleable Controls
/// panel, and Exit.
/// </summary>
public partial class MainMenu : Control
{
	[Export] public string StartGamePath = "res://scenes/levels/main.tscn";

	private Button startGameButton;
	private Button controlsButton;
	private Button exitButton;
	private Control controlsPanel;
	private Button backButton;

	/// <summary>Caches UI node references and wires up all the button handlers.</summary>
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

	/// <summary>Starts a new run by loading the first level.</summary>
	private void StartGame()
	{
		GetTree().ChangeSceneToFile(StartGamePath);
	}

	/// <summary>Opens the controls panel overlay.</summary>
	private void ShowControls()
	{
		controlsPanel.Visible = true;
	}

	/// <summary>Closes the controls panel overlay.</summary>
	private void HideControls()
	{
		controlsPanel.Visible = false;
	}

	/// <summary>Quits the game.</summary>
	private void Exit()
	{
		GetTree().Quit();
	}
}
