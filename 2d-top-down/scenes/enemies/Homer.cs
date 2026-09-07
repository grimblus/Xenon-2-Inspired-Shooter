using Godot;
using System;

/// <summary>
/// Simple enemy that plays a random idle animation on spawn. Not currently instanced
/// anywhere in the game (no takeDamage/shoot behaviour implemented yet).
/// </summary>
public partial class Homer : Area2D
{
	/// <summary>Plays a randomly chosen animation from this enemy's sprite frames.</summary>
	public override void _Ready()
	{
		var animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		string[] mobTypes = animatedSprite2D.SpriteFrames.GetAnimationNames();
		animatedSprite2D.Play(mobTypes[GD.Randi() % mobTypes.Length]);
	}

	/// <summary>Frees this enemy once it scrolls off-screen.</summary>
	private void OnVisibleOnScreenNotifier2DScreenExited()
	{
		QueueFree();
	}
}
