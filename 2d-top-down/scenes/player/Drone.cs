using Godot;
using System;

public partial class Drone : AnimatedSprite2D
{
	[Export] public float OrbitRadius = 50f;
	[Export] public float OrbitSpeed = 3f;
	[Export] public PackedScene BulletScene;
	
	private Timer droneTimer;
	private float angle = 0f;
	private Marker2D droneBulletSpawn;
	
	public override void _Ready() {
		droneTimer = GetNode<Timer>("DroneTimer");
		droneBulletSpawn = GetNode<Marker2D>("DroneBulletSpawn");
		
		droneTimer.Timeout += Shoot;
		droneTimer.Start();
	}
	
	public override void _Process(double delta) {
		angle += OrbitSpeed * (float)delta;
	
		Position = new Vector2(
			Mathf.Cos(angle),
			Mathf.Sin(angle)
		) * OrbitRadius;
	}
	
	private void Shoot() {
		if (BulletScene == null)
			return;
		
		var b = BulletScene.Instantiate<Bullet>();
		b.Shooter = this;
		GetParent().AddChild(b);
		b.Start(droneBulletSpawn.GlobalPosition);
	}
}
