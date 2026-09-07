using Godot;

/// <summary>
/// Parallax background decoration: plays a meteor's idle animation on loop.
/// Purely visual, no gameplay behaviour.
/// </summary>
public partial class MeteorDefault : AnimatedSprite2D
{
	/// <summary>Starts the meteor's default animation as soon as it enters the scene.</summary>
	public override void _Ready()
	{
		Play("default");
	}
}
