using Godot;
using System;

public partial class Player : CharacterBody2D
{
	[Export] public float Speed = 150f;
	[Export] public float Cooldown = 0.25f;
	[Export] PackedScene BulletScene;
	[Export] public float BounceStrength = 300f;
	[Export] public float KnockbackDecay = 800f;
	
	/// Power Ups
	[Export] public bool tripleShot = false;
	[Export] public bool speedUp = false;
	[Export] public bool laserUp = false;
	[Export] public bool shieldUp = false;
	[Export] public bool invincibleUp = false;
	[Export] public bool missileUp = false;
	
	/// Health Variables
	private ProgressBar healthBar;
	private int maxHealth = 100;
	private int currentHealth = 100;
	
	private Sprite2D ship;
	private AnimatedSprite2D boosters;
	private AnimatedSprite2D boosters2;
	private Timer cooldown;
	private Marker2D bulletSpawn;
	private Marker2D tripleBulletSpawn1;
	private Marker2D tripleBulletSpawn2;
	private Area2D laser;
	private Sprite2D shield;///Find circle sprite
	private Vector2 screenSize;
	private bool canShoot = true;
	private Vector2 knockback = Vector2.Zero;
	
	public override void _Ready() {
		ship = GetNode<Sprite2D>("Ship");
		boosters = GetNode<AnimatedSprite2D>("Ship/Boosters");
		boosters2 = GetNode<AnimatedSprite2D>("Ship/Boosters2");
		cooldown = GetNode<Timer>("GunCooldown");
		bulletSpawn = GetNode<Marker2D>("BulletSpawn");
		tripleBulletSpawn1 = GetNode<Marker2D>("TripleBulletSpawn1");
		tripleBulletSpawn2 = GetNode<Marker2D>("TripleBulletSpawn2");
		laser = GetNode<Area2D>("Laser");
		shield = GetNode<Sprite2D>("Shield");
		screenSize = GetViewportRect().Size;
		
		/// Health Bar Functioning
		healthBar = GetNode<ProgressBar>("../../CanvasLayer_HealthBar/Health_Bar");		healthBar.MaxValue = maxHealth;
		healthBar.Value = currentHealth;
		
		// For enemy collision
		var hurtbox = GetNode<Area2D>("Hurtbox");
		hurtbox.AreaEntered += HurtboxAreaHit;
		hurtbox.BodyEntered += HurtboxBodyHit;

		Start();
	}
	
	private void Start() {
		Position = new Vector2(screenSize.X/2, screenSize.Y-64);
		cooldown.WaitTime = Cooldown;
	}
	
	public override void _PhysicsProcess(double delta) {
		if (speedUp) {
			Speed = 300f;
		} else {
			Speed = 150f;
		}
		
		if (laserUp) {
			laser.Visible = true;
			laser.ProcessMode = Node.ProcessModeEnum.Inherit;
			laser.SetDeferred("monitoring", true);
		}
		else {
			laser.Visible = false;
			laser.ProcessMode = Node.ProcessModeEnum.Disabled;
			laser.SetDeferred("monitoring", false);
		}
		
		if (shieldUp) {
			shield.Visible = true;
		}
		else {
			shield.Visible = false;
		}
		
		Vector2 input = Input.GetVector("left", "right", "up", "down");
		
		if (input.X > 0) {
			ship.Frame = 5;
			boosters.Animation = "right";
			boosters2.Animation = "right";
		}
		else if (input.X < 0) {
			ship.Frame = 1;
			boosters.Animation = "left";
			boosters2.Animation = "left";
		}
		else {
			ship.Frame = 3;
			boosters.Animation = "forward";
			boosters2.Animation = "forward";
		}
		
		Velocity = input * Speed + knockback;
		MoveAndSlide();
		
		for (int i = 0; i < GetSlideCollisionCount(); i++) {
			var collision = GetSlideCollision(i);
			knockback = collision.GetNormal() * BounceStrength;
		}
		
		knockback = knockback.MoveToward(Vector2.Zero, KnockbackDecay * (float)delta);
		
		if (Input.IsActionPressed("shoot")) {
			Shoot();
		}
		
		/// Testing damage (Remove Later)
		if (Input.IsKeyPressed(Key.H)) {
			TakeDamage(20);
		}
	
	}

	
	public void Shoot() {
		if (!canShoot) {
			return;
		}
		
		canShoot = false;
		
		cooldown.Start();
		
		if (!laserUp) {
			var b = BulletScene.Instantiate<Bullet>();
			b.Shooter = this;
			GetParent().AddChild(b);
			//new so I can change colour
			b.Start(bulletSpawn.GlobalPosition);
			//old
			//b.GlobalPosition = bulletSpawn.GlobalPosition;
		}
		
		if (tripleShot == true) {
			var b1 = BulletScene.Instantiate<Bullet>();
			GetParent().AddChild(b1);
			b1.GlobalPosition = tripleBulletSpawn1.GlobalPosition;
			
			var b2 = BulletScene.Instantiate<Bullet>();
			GetParent().AddChild(b2);
			b2.GlobalPosition = tripleBulletSpawn2.GlobalPosition;
		}
	}
	
	private void OnGunCooldownTimeout() {
		canShoot = true;
	}
	
	public void TakeDamage(int amount) {
		if (!invincibleUp) {	
			if (!shieldUp) {
				currentHealth = Mathf.Clamp(currentHealth - amount, 0, maxHealth);
				UpdateHealthBar();
				if (currentHealth <= 0) {
					Die();
				}
			} 
			else {
				shieldUp = false;
			}
		}
	}
	
	public void Heal(int amount) {
		currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
		UpdateHealthBar();
	}
	
	private void UpdateHealthBar() {
		healthBar.Value = currentHealth;
		
		var fillStyle = new StyleBoxFlat();
		float percent = (float)currentHealth / maxHealth;
		
		if (percent > 0.5f)
			fillStyle.BgColor = Colors.Green;
		else if (percent > 0.2f)
			fillStyle.BgColor = Colors.Orange;
		else
			fillStyle.BgColor = Colors.Red;
		
		healthBar.AddThemeStyleboxOverride("fill", fillStyle);
	}
	
	private void Die() {
		GD.Print("Player died");
	}

	//For area2ds
	private void HurtboxAreaHit(Area2D area)
	{
		if (area.IsInGroup("enemies")) {
			TakeDamage(10);
		} else if (area is Bullet bullet && bullet.usedByEnemy) {
			TakeDamage(10);
			bullet.QueueFree();
		}
	}
	
	//For rigidbodys, charbodys etc
	private void HurtboxBodyHit(Node2D body)
	{
		if (body.IsInGroup("enemies"))
		{
			TakeDamage(10);
		} else if (body is Bullet bullet && bullet.usedByEnemy) {
			TakeDamage(10);
			bullet.QueueFree();
		}
	}
	
}
