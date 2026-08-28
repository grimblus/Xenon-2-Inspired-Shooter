using Godot;
using System;

public partial class WallHugger : RigidBody2D
{
	
	[Export] public bool FacingRight = true;
	[Export] public PackedScene BulletScene;
	[Export] public float ShootInterval = 1.5f;
	[Export] public float BulletSpeed = 250f;
	
	// Called when the node enters the scene tree for the first time.
	private Marker2D bulletSpawn;
	private Timer cooldown;
	private bool canShoot = true;
	private Player player = null;
	private Vector2 direction;
	
	public override void _Ready()
	{
		var animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		if (FacingRight) {
			animatedSprite2D.Play("facingright");
		} else {
			animatedSprite2D.Play("facingleft");
		}
		
		bulletSpawn = GetNode<Marker2D>("BulletSpawn");
		cooldown = GetNode<Timer>("ShootCooldown");
		cooldown.WaitTime = ShootInterval;
		cooldown.Start();
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
	
	private void OnShootTimerTimeout()
	{
		Shoot();
	}
	
	public void Shoot() 
	{
		if (!canShoot) {
			return;
		}
		
		var bullet = BulletScene.Instantiate<Bullet>();
		GetParent().AddChild(bullet);

		if (FacingRight) {
			bullet.usedByRightHugger = true;
		} else {
			bullet.usedByLeftHugger = true;
		}
		bullet.Start(bulletSpawn.GlobalPosition);
		bullet.Speed = BulletSpeed;
		
	}
}
