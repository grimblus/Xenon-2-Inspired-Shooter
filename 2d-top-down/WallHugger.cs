using Godot;
using System;

public partial class WallHugger : RigidBody2D
{
	// Called when the node enters the scene tree for the first time.
	private bool canShoot = false;
	private Timer cooldown;
	
	public override void _Ready()
	{
		var animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		string[] mobTypes = animatedSprite2D.SpriteFrames.GetAnimationNames();
		animatedSprite2D.Play(mobTypes[GD.Randi() % mobTypes.Length]);
		
		cooldown = GetNode<Timer>("ShootCooldown");
	}
	
	private void OnVisibleOnScreenNotifier2DScreenExited()
	{
		QueueFree();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		//while (animatedSprite2D.frame == 11 && cooldown) {
			//canShoot = true;
		//}
	}
	
	public void Shoot() 
	{
		if (!canShoot) {
			return;
		}
		
		cooldown.Start();
		
	}
}
