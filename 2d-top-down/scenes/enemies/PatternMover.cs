using Godot;
using System;

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
	
	// health
	private int health = 1;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		string[] mobTypes = animatedSprite2D.SpriteFrames.GetAnimationNames();
		animatedSprite2D.Play("default");
		
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
		
		player = GetNode<Player>("/root/Main/Scroller/Player");
		
		//so I dont have to readd it to enemies just in case
		AddToGroup("enemies");
	}
	
	public override void _PhysicsProcess(double delta)
	{
		//if no pattern do nothing
		if (Pattern == null || patternLength <= 0)
			return;

		distanceTravelled += PatternSpeed * (float)delta;

		//Once the pattern has been travelled through, queuefree
		if (distanceTravelled >= patternLength)
		{
			QueueFree();
			return;
		}

		Vector2 localOffset = Pattern.SampleBaked(distanceTravelled);
		Vector2 rotatedOffset = localOffset.Rotated(spawnRotation);

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
	
	public void TakeDamage(int amount) 
	{
		var animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		health -= amount;
		if (health <= 0) 
		{
			//turns off collision 
			var collisionShape = GetNode<CollisionShape2D>("CollisionShape2D");
			collisionShape.SetDeferred("disabled", true);
			
			animatedSprite2D.Play("explode");
			animatedSprite2D.AnimationFinished += Die;						
			player.Call("AddScore", 50);
		}
	}
	
	private void Die() 
	{
		QueueFree();
	}
}
