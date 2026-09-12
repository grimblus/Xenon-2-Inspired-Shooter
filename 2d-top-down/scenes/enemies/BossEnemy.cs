using Godot;
using System;

/// <summary>
/// Level 2's boss: a multi-eyed enemy that follows an optional movement pattern, shoots
/// at the player, and shows a health bar. On death, awards a large score bonus and
/// triggers the victory (Congrats) screen.
/// </summary>
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
	private ProgressBar healthBar;

	/// <summary>
	/// Randomizes each body part's idle animation, bakes the movement pattern, caches node
	/// references, initializes the health bar, and registers as an enemy.
	/// </summary>
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
		
		var expAnimatedSprite2D = GetNode<AnimatedSprite2D>("ExplodeAnimatedSprite2D");
		expAnimatedSprite2D.Visible = false;
		
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

		healthBar = GetNode<ProgressBar>("HealthBar");
		healthBar.MaxValue = maxHealth;
		healthBar.Value = currentHealth;

		//so I dont have to read it to enemies just in case
		AddToGroup("enemies");
	}

	/// <summary>Advances along the movement pattern each frame, looping back to the start once it completes.</summary>
	public override void _Process(double delta)
	{
		if (Pattern == null || patternLength <= 0)
		return;

		distanceTravelled += PatternSpeed * (float)delta;

		if (distanceTravelled >= patternLength)
		{
			distanceTravelled = 0f;
			return;
		}

		Vector2 localOffset = Pattern.SampleBaked(distanceTravelled);
		Vector2 rotatedOffset = localOffset.Rotated(spawnRotation);
		Position = spawnPosition + rotatedOffset;
	}
	
	/// <summary>Fires at the player, called on the shoot cooldown timer.</summary>
	private void OnShootTimerTimeout()
	{
		if (player == null) {
			canShoot = false;
		}
		Shoot();
	}

	/// <summary>Fires a bullet aimed directly at the player's current position.</summary>
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
	
	/// <summary>Applies damage to the boss's health bar; on death, hides all body parts, plays the explosion, and disables collision/shooting.</summary>
	public void TakeDamage(int amount)
	{
		var bodyAnimatedSprite2D = GetNode<AnimatedSprite2D>("MainBodyAnimatedSprite2D");
		var rEyeAnimatedSprite2D = GetNode<AnimatedSprite2D>("RightEyeAnimatedSprite2D");
		var rEye2AnimatedSprite2D = GetNode<AnimatedSprite2D>("RightEye2AnimatedSprite2D");
		var rEye3AnimatedSprite2D = GetNode<AnimatedSprite2D>("RightEye3AnimatedSprite2D");
		var lEyeAnimatedSprite2D = GetNode<AnimatedSprite2D>("LeftEyeAnimatedSprite2D");	
		var lEye2AnimatedSprite2D = GetNode<AnimatedSprite2D>("LeftEye2AnimatedSprite2D");		
		var lEye3AnimatedSprite2D = GetNode<AnimatedSprite2D>("LeftEye3AnimatedSprite2D");
		var expAnimatedSprite2D = GetNode<AnimatedSprite2D>("ExplodeAnimatedSprite2D");
		var collisionShape = GetNode<CollisionShape2D>("CollisionShape2D");
		
		currentHealth = Mathf.Clamp(currentHealth - amount, 0, maxHealth);
		healthBar.Value = currentHealth;
		if (currentHealth <= 0) {
			bodyAnimatedSprite2D.Visible = false;
			rEyeAnimatedSprite2D.Visible = false;
			rEye2AnimatedSprite2D.Visible = false;
			rEye3AnimatedSprite2D.Visible = false;
			lEyeAnimatedSprite2D.Visible = false;
			lEye2AnimatedSprite2D.Visible = false;
			lEye3AnimatedSprite2D.Visible = false;
			
			collisionShape.SetDeferred("disabled", true);
			canShoot = false;
			
			expAnimatedSprite2D.Visible = true;
			expAnimatedSprite2D.Play("default");
			expAnimatedSprite2D.AnimationFinished += Die;	
		}
	}
	
	/// <summary>Awards the boss-kill score bonus (a flat 12,000: the original 10,000 kill bonus plus a 2,000 replacement for the old per-hit scoring), shows the victory screen, and pauses the game before freeing the boss.</summary>
	private void Die()
	{
		player.Call("AddScore", 12000);

		var congratsLayer = GetNode<CongratsScreen>("/root/Main/CanvasLayer_Congrats");
		congratsLayer.ShowCongrats();
		GetTree().Paused = true;

		QueueFree();
	}
}
