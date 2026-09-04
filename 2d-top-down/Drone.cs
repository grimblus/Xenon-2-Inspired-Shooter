using Godot;
using System;

public partial class Drone : AnimatedSprite2D
{
	[Export] public float OrbitRadius = 50f;
	[Export] public float OrbitSpeed = 3f;
	[Export] public PackedScene BulletScene;
	
	private Timer droneTimer;
	private float angle = 0f;
	
	public override void _Ready() {
		droneTimer = GetNode<Timer>("DroneTimer");
		
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
		
		Node2D bullet = BulletScene.Instantiate<Node2D>();
		
		GetTree().CurrentScene.AddChild(bullet);
		
		bullet.GlobalPosition = GlobalPosition;
	}
}
