using Godot;
using System;

public partial class WallHugger : RigidBody2D
{
	
	[Export] public bool FacingRight = true;
	[Export] public PackedScene BulletScene;
	[Export] public float ShootInterval = 1.5f;
	[Export] public float BulletSpeed = 250f;
	[Export] public float ShootRange = 800f;
	
	// Called when the node enters the scene tree for the first time.
	private Marker2D bulletSpawn;
	private Timer cooldown;
	private bool canShoot = true;
	private Player player;
	private Vector2 direction;
	
	//health
	private int health = 1;
	
	public override void _Ready()
	{
		var animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		if (FacingRight) {
			animatedSprite2D.Play("facingright");
			bulletSpawn = GetNode<Marker2D>("RightBulletSpawn");
		} else {
			animatedSprite2D.Play("facingleft");
			bulletSpawn = GetNode<Marker2D>("LeftBulletSpawn");
		}
		
		cooldown = GetNode<Timer>("ShootCooldown");
		cooldown.WaitTime = ShootInterval;
		cooldown.Start();
		
		player = GetNode<Player>("/root/Main/Scroller/Player");
		
		//so I dont have to readd it to enemies just in case
		AddToGroup("enemies");
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
		if (!this.Visible) {
			canShoot = false;
		}
	}
	
	private void OnShootTimerTimeout()
	{
		if (player == null) {
			canShoot = false;
		}
		
		float playerDistance = GlobalPosition.DistanceTo(player.GlobalPosition);
		
		if (playerDistance <= ShootRange) {
			Shoot();
		}
	}
	
	public void Shoot() 
	{
		if (!canShoot) {
			return;
		}
		
		var bullet = BulletScene.Instantiate<Bullet>();
		bullet.Shooter = this;
		bullet.usedByEnemy = true;
		GetParent().AddChild(bullet);

		if (FacingRight) {
			bullet.usedByRightHugger = true;
		} else {
			bullet.usedByLeftHugger = true;
		}
		bullet.Start(bulletSpawn.GlobalPosition);
		bullet.Speed = BulletSpeed;
		
	}
	
	public void TakeDamage(int amount) 
	{
		health -= amount;
		if (health <= 0) 
		{
			QueueFree();
			player.Call("AddScore", 100);
		}
	}
}
