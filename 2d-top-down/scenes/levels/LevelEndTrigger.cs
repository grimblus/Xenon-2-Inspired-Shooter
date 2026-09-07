using Godot;

public partial class LevelEndTrigger : Area2D
{
	[Export] public string NextScenePath = "res://scenes/ui/shop_screen.tscn";

	public override void _Ready()
	{
		BodyEntered += OnBodyEntered;
	}

	private void OnBodyEntered(Node2D body)
	{
		if (body is Player)
		{
			GetTree().ChangeSceneToFile(NextScenePath);
		}
	}
}
