using Godot;
using System;

public partial class BossEnemy : Area2D
{
	[Export] public Curve2D Pattern;
	[Export] public float PatternSpeed = 200.0f;
	[Export] public Path2D PatternPath;
	
	[Export] public PackedScene BulletScene;
	[Export] public float ShootInterval = 1.5f;
	[Export] public float BulletSpeed = 250f;
	
	//health
	private int maxHealth = 1000;
	private int currentHealth = 1000;
	private float patternLength;
	
	//for path pattern
	private Vector2 spawnPosition;
	private float spawnRotation;
	private float distanceTravelled = 0.0f;
	
	private Player player;
	private bool canShoot = true;
	private Marker2D bulletSpawn;
	private Timer cooldown;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var bodyAnimatedSprite2D = GetNode<AnimatedSprite2D>("MainBodyAnimatedSprite2D");
		string[] bodyMobTypes = bodyAnimatedSprite2D.SpriteFrames.GetAnimationNames();
		bodyAnimatedSprite2D.Play(bodyMobTypes[GD.Randi() % bodyMobTypes.Length]);
		
		var rEyeAnimatedSprite2D = GetNode<AnimatedSprite2D>("RightEyeAnimatedSprite2D");
		string[] rEyeMobTypes = rEyeAnimatedSprite2D.SpriteFrames.GetAnimationNames();
		rEyeAnimatedSprite2D.Play(rEyeMobTypes[GD.Randi() % rEyeMobTypes.Length]);
		
		var rEye2AnimatedSprite2D = GetNode<AnimatedSprite2D>("RightEye2AnimatedSprite2D");
		string[] rEye2MobTypes = rEye2AnimatedSprite2D.SpriteFrames.GetAnimationNames();
		rEye2AnimatedSprite2D.Play(rEye2MobTypes[GD.Randi() % rEye2MobTypes.Length]);
		
		var rEye3AnimatedSprite2D = GetNode<AnimatedSprite2D>("RightEye3AnimatedSprite2D");
		string[] rEye3MobTypes = rEye3AnimatedSprite2D.SpriteFrames.GetAnimationNames();
		rEye3AnimatedSprite2D.Play(rEye3MobTypes[GD.Randi() % rEye3MobTypes.Length]);
		
		var lEyeAnimatedSprite2D = GetNode<AnimatedSprite2D>("LeftEyeAnimatedSprite2D");
		string[] lEyeMobTypes = lEyeAnimatedSprite2D.SpriteFrames.GetAnimationNames();
		lEyeAnimatedSprite2D.Play(lEyeMobTypes[GD.Randi() % lEyeMobTypes.Length]);
		
		var lEye2AnimatedSprite2D = GetNode<AnimatedSprite2D>("LeftEye2AnimatedSprite2D");
		string[] lEye2MobTypes = lEye2AnimatedSprite2D.SpriteFrames.GetAnimationNames();
		lEye2AnimatedSprite2D.Play(lEye2MobTypes[GD.Randi() % lEye2MobTypes.Length]);
		
		var lEye3AnimatedSprite2D = GetNode<AnimatedSprite2D>("LeftEye3AnimatedSprite2D");
		string[] lEye3MobTypes = lEye3AnimatedSprite2D.SpriteFrames.GetAnimationNames();
		lEye3AnimatedSprite2D.Play(lEye3MobTypes[GD.Randi() % lEye3MobTypes.Length]);
		
		//Makes it so that the boss actually starts where I put it in the scene
		spawnPosition = Position;
		spawnRotation = Rotation;
		
		if (Pattern != null)
			patternLength = Pattern.GetBakedLength();
			
		if (PatternPath != null)
		{
			Pattern = PatternPath.Curve;
			patternLength = Pattern.GetBakedLength();
		}
		
		bulletSpawn = GetNode<Marker2D>("BulletSpawn");
		cooldown = GetNode<Timer>("ShootCooldown");
		cooldown.WaitTime = ShootInterval;
		cooldown.Start();
		
		player = GetNode<Player>("/root/Main/Scroller/Player");
		
		//so I dont have to read it to enemies just in case
		AddToGroup("enemies");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Pattern == null || patternLength <= 0)
		return;

		distanceTravelled += PatternSpeed * (float)delta;

		//if pattern finish, loop
		if (distanceTravelled >= patternLength)
		{
			distanceTravelled = 0f;
			return;
		}

		Vector2 localOffset = Pattern.SampleBaked(distanceTravelled);
		Vector2 rotatedOffset = localOffset.Rotated(spawnRotation);
		Position = spawnPosition + rotatedOffset;
	}
	
	private void OnShootTimerTimeout()
	{
		if (player == null) {
			canShoot = false;
		}
		Shoot();
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

		bullet.Start(bulletSpawn.GlobalPosition);
		bullet.Speed = BulletSpeed;
		
		Vector2 direction = (player.GlobalPosition - bulletSpawn.GlobalPosition).Normalized();
		bullet.usedByBoss = true;
		bullet.aimDirection = direction;
	}
	
	public void TakeDamage(int amount) 
	{		
		currentHealth = Mathf.Clamp(currentHealth - amount, 0, maxHealth);
		if (currentHealth <= 0) {
			Die();
		}
	}
	
	private void Die()
	{
		QueueFree();
	}
}
