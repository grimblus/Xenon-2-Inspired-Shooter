using Godot;
using System;
public partial class Player : CharacterBody2D
{
	[Export] public float Speed = 150f;
	[Export] public float Cooldown = 0.25f;
	[Export] PackedScene BulletScene;
	[Export] public float BounceStrength = 300f;
	[Export] public float KnockbackDecay = 800f;
	
	/// Health Variables
	private ProgressBar healthBar;
	private int maxHealth = 100;
	private int currentHealth = 100;
	
	/// Score Variables
	private Label scoreLabel;
	private int score = 0;
	
	private bool isDead = false;
	
	private Sprite2D ship;
	private AnimatedSprite2D boosters;
	private AnimatedSprite2D boosters2;
	private Timer cooldown;
	private Marker2D bulletSpawn;
	private Vector2 screenSize;
	private bool canShoot = true;
	private Vector2 knockback = Vector2.Zero;
	
	public override void _Ready() {
		ship = GetNode<Sprite2D>("Ship");
		boosters = GetNode<AnimatedSprite2D>("Ship/Boosters");
		boosters2 = GetNode<AnimatedSprite2D>("Ship/Boosters2");
		cooldown = GetNode<Timer>("GunCooldown");
		bulletSpawn = GetNode<Marker2D>("BulletSpawn");
		screenSize = GetViewportRect().Size;
		
		/// Health Bar Functioning
		healthBar = GetNode<ProgressBar>("../../CanvasLayer_HUD/HUD_Bar/Health_Bar");
		healthBar.MaxValue = maxHealth;
		healthBar.Value = currentHealth;
		
		/// Score Display
		scoreLabel = GetNode<Label>("../../CanvasLayer_HUD/HUD_Bar/Score_Label");
		UpdateScoreLabel();
		
		Start();
	}
	
	private void Start() {
		Position = new Vector2(screenSize.X/2, screenSize.Y-64);
		cooldown.WaitTime = Cooldown;
	}
	
	public override void _PhysicsProcess(double delta) {
		Vector2 input = Input.GetVector("left", "right", "up", "down");
		if (input.X > 0) {
			ship.Frame = 5;
			boosters.Animation = "right";
			boosters2.Animation = "right";
		}
		else if (input.X < 0)
		{
			ship.Frame = 1;
			boosters.Animation = "left";
			boosters2.Animation = "left";
		}
		else
		{
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
	
	public void Shoot(){
		if (!canShoot) {
			return;
		}
		
		canShoot = false;
		cooldown.Start();
		var b = BulletScene.Instantiate<Bullet>();
		GetParent().AddChild(b);
		b.GlobalPosition = bulletSpawn.GlobalPosition;
	}
	
	private void OnGunCooldownTimeout() {
		canShoot = true;
	}
	
	/// Player
	
	public void TakeDamage(int amount)
{
	currentHealth = Mathf.Clamp(currentHealth - amount, 0, maxHealth);
	UpdateHealthBar();
	if (currentHealth <= 0)
	{
		Die();
	}
}
	public void Heal(int amount)
{
	currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
	UpdateHealthBar();
}
	private void UpdateHealthBar()
{
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
	
	public void AddScore(int amount)
{
	score += amount;
	UpdateScoreLabel();
}
	private void UpdateScoreLabel()
{
	scoreLabel.Text = score.ToString("D7"); // pads to 7 digits, e.g. 0041920
}
	
private void Die()
{
	if (isDead) return;
	isDead = true;
	
	GD.Print("Player died");
	
	var gameOverLayer = GetNode<GameOver>("../../CanvasLayer_GameOver");
	gameOverLayer.ShowGameOver();
	
	GetTree().Paused = true;
}
	
}
