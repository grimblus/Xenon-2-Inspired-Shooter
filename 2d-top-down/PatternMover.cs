using Godot;
using System;

public partial class PatternMover : RigidBody2D
{
	
	[Export] public Curve2D Pattern;
	[Export] public float PatternSpeed = 200.0f;
	[Export] public Path2D PatternPath;

	private Vector2 _spawnPosition;
	private float _spawnRotation;
	private float _distanceTravelled = 0.0f;
	private float _patternLength;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		string[] mobTypes = animatedSprite2D.SpriteFrames.GetAnimationNames();
		animatedSprite2D.Play(mobTypes[GD.Randi() % mobTypes.Length]);
		
		_spawnPosition = GlobalPosition;
		_spawnRotation = Rotation;

		if (Pattern != null)
			_patternLength = Pattern.GetBakedLength();
			
		if (PatternPath != null)
		{
			Pattern = PatternPath.Curve;
			_patternLength = Pattern.GetBakedLength();
		}
	}
	
	public override void _IntegrateForces(PhysicsDirectBodyState2D state)
	{
		if (Pattern == null || _patternLength <= 0)
			return;

		_distanceTravelled += PatternSpeed * (float)state.Step;

		if (_distanceTravelled >= _patternLength)
		{
			QueueFree();
			return;
		}

		Vector2 localOffset = Pattern.SampleBaked(_distanceTravelled);

		Vector2 rotatedOffset = localOffset.Rotated(_spawnRotation);

		Vector2 targetPosition = _spawnPosition + rotatedOffset;

		state.LinearVelocity = (targetPosition - state.Transform.Origin) / (float)state.Step;
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
