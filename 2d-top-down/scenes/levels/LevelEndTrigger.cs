using Godot;

/// <summary>
/// Invisible collision trigger placed at the end of a level's terrain. Transitions to the
/// next scene (the shop by default) as soon as the player enters it.
/// </summary>
public partial class LevelEndTrigger : Area2D
{
	[Export] public string NextScenePath = "res://scenes/ui/shop_screen.tscn";

	/// <summary>Subscribes to this Area2D's own collision signal.</summary>
	public override void _Ready()
	{
		BodyEntered += OnBodyEntered;
	}

	/// <summary>Transitions to NextScenePath if the body that entered is the player.</summary>
	private void OnBodyEntered(Node2D body)
	{
		if (body is Player)
		{
			GetTree().ChangeSceneToFile(NextScenePath);
		}
	}
}
