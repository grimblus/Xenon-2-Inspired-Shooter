using Godot;
using System;

public partial class Player : Area2D
{
	[Export] public float Speed = 150f;
	[Export] public float Cooldown = 0.25f;
	[Export] PackedScene BulletScene;
	
	private Sprite2D ship;
	private AnimatedSprite2D boosters;
	private AnimatedSprite2D boosters2;
	private Timer cooldown;
	private Marker2D bulletSpawn;
	private Vector2 screenSize;
	private bool canShoot = true;
	
	public override void _Ready() {
		ship = GetNode<Sprite2D>("Ship");
		boosters = GetNode<AnimatedSprite2D>("Ship/Boosters");
		boosters2 = GetNode<AnimatedSprite2D>("Ship/Boosters2");
		cooldown = GetNode<Timer>("GunCooldown");
		bulletSpawn = GetNode<Marker2D>("BulletSpawn");
		screenSize = GetViewportRect().Size;
		Start();
	}
	
	private void Start() {
		Position = new Vector2(screenSize.X/2, screenSize.Y-64);
		cooldown.WaitTime = Cooldown;
	}
	
	public override void _Process(double delta) {
		Vector2 input = Input.GetVector("left", "right", "up", "down");
		
		if (input.X > 0) {
			ship.Frame= 5;
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
		
		Position += input * Speed * (float)delta;
		Position = Position.Clamp(new Vector2(8, 8), screenSize - new Vector2(8, 8)); //Keeps within screen NOT FINAL
		
		if (Input.IsActionPressed("shoot")) {
			Shoot();
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
}
