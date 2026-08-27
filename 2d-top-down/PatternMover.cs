using Godot;
using System;

public partial class PatternMover : RigidBody2D
{
	
	[Export] public Curve2D Pattern;
	[Export] public float PatternSpeed = 200.0f;
	[Export] public Path2D PatternPath;

	private Vector2 spawnPosition;
	private float spawnRotation;
	private float distanceTravelled = 0.0f;
	private float patternLength;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		string[] mobTypes = animatedSprite2D.SpriteFrames.GetAnimationNames();
		animatedSprite2D.Play(mobTypes[GD.Randi() % mobTypes.Length]);
		
		spawnPosition = GlobalPosition;
		spawnRotation = Rotation;

		//Makes the pattern from the Path2D work
		if (Pattern != null)
			patternLength = Pattern.GetBakedLength();
			
		if (PatternPath != null)
		{
			Pattern = PatternPath.Curve;
			patternLength = Pattern.GetBakedLength();
		}
	}
	
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

		// Area2D can just have its position set directly — no physics engine to fight
		GlobalPosition = spawnPosition + rotatedOffset;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	private void OnVisibleOnScreenNotifier2DScreenExited()
	{
		QueueFree();
	}
}
