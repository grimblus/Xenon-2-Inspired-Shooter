using Godot;
using System;

/// <summary>
/// Stationary enemy that clings to a wall facing left or right, shooting at the player
/// whenever they're within range. Dies in one hit.
/// </summary>
public partial class WallHugger : StaticBody2D
{
	[Export] public bool FacingRight = true;
	[Export] public PackedScene BulletScene;
	[Export] public float ShootInterval = 1.5f;
	[Export] public float BulletSpeed = 250f;
	[Export] public float ShootRange = 800f;

	private Marker2D bulletSpawn;
	private Timer cooldown;
	private bool canShoot = true;
	private Player player;
	private Vector2 direction;

	private int health = 1;

	/// <summary>Sets up the facing-direction sprite/bullet spawn, starts the shoot cooldown, and registers as an enemy.</summary>
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
	
	/// <summary>Frees this enemy once it scrolls off-screen.</summary>
	private void OnVisibleOnScreenNotifier2DScreenExited()
	{
		QueueFree();
	}

	/// <summary>Prevents shooting while invisible (e.g. hidden via a level override).</summary>
	public override void _Process(double delta)
	{
		if (!this.Visible) {
			canShoot = false;
		}
	}

	/// <summary>Fires at the player if they're within ShootRange, called on the shoot cooldown timer.</summary>
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
	
	/// <summary>Fires a bullet toward the direction this WallHugger is facing.</summary>
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
	
	/// <summary>Applies damage; on death, disables collision/shooting, plays the explode animation, and awards score.</summary>
	public void TakeDamage(int amount)
	{
		var animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		health -= amount;
		if (health <= 0)
		{
			var collisionShape = GetNode<CollisionShape2D>("CollisionShape2D");
			collisionShape.SetDeferred("disabled", true);
			canShoot = false;
			
			animatedSprite2D.Play("explode");
			animatedSprite2D.AnimationFinished += Die;			
			player.Call("AddScore", 100);
		}
	}
	
	/// <summary>Removes this enemy from the scene once its death animation finishes.</summary>
	public void Die()
	{
		QueueFree();
	}
}
