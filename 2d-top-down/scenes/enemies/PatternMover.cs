using Godot;
using System;

/// <summary>
/// Enemy that follows a fixed movement pattern (a Curve2D, either set directly or baked
/// from a sibling Path2D) relative to its spawn point, freeing itself once the pattern completes.
/// </summary>
public partial class PatternMover : Area2D
{
	[Export] public Curve2D Pattern;
	[Export] public float PatternSpeed = 200.0f;
	[Export] public Path2D PatternPath;

	private Vector2 spawnPosition;
	private float spawnRotation;
	private float distanceTravelled = 0.0f;
	private float patternLength;
	private Player player;

	private int health = 1;

	/// <summary>Records the spawn transform, bakes the movement pattern, and registers as an enemy.</summary>
	public override void _Ready()
	{
		var animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		animatedSprite2D.Play("default");

		spawnPosition = GlobalPosition;
		spawnRotation = Rotation;

		// PatternPath (if assigned) takes priority over a directly-set Pattern curve.
		if (Pattern != null)
			patternLength = Pattern.GetBakedLength();
			
		if (PatternPath != null)
		{
			Pattern = PatternPath.Curve;
			patternLength = Pattern.GetBakedLength();
		}
		
		player = GetNode<Player>("/root/Main/Scroller/Player");
		
		//so I dont have to readd it to enemies just in case
		AddToGroup("enemies");
	}
	
	/// <summary>Advances along the movement pattern each physics frame, freeing this enemy once the pattern completes.</summary>
	public override void _PhysicsProcess(double delta)
	{
		if (Pattern == null || patternLength <= 0)
			return;

		distanceTravelled += PatternSpeed * (float)delta;

		if (distanceTravelled >= patternLength)
		{
			QueueFree();
			return;
		}

		Vector2 localOffset = Pattern.SampleBaked(distanceTravelled);
		Vector2 rotatedOffset = localOffset.Rotated(spawnRotation);

		GlobalPosition = spawnPosition + rotatedOffset;
	}

	/// <summary>Frees this enemy once it scrolls off-screen.</summary>
	private void OnVisibleOnScreenNotifier2DScreenExited()
	{
		QueueFree();
	}

	/// <summary>Applies damage; on death, disables collision, plays the explode animation, and awards score.</summary>
	public void TakeDamage(int amount)
	{
		var animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		health -= amount;
		if (health <= 0)
		{
			var collisionShape = GetNode<CollisionShape2D>("CollisionShape2D");
			collisionShape.SetDeferred("disabled", true);

			animatedSprite2D.Play("explode");
			animatedSprite2D.AnimationFinished += Die;
			player.Call("AddScore", 50);
		}
	}

	/// <summary>Removes this enemy from the scene once its death animation finishes.</summary>
	private void Die()
	{
		QueueFree();
	}
}
