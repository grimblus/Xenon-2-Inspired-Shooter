using Godot;
using System;

/// <summary>
/// The player's companion drone (Drone power-up): orbits the player at a fixed radius
/// and periodically fires its own bullets alongside the player's weapons.
/// </summary>
public partial class Drone : AnimatedSprite2D
{
	[Export] public float OrbitRadius = 50f;
	[Export] public float OrbitSpeed = 3f;
	[Export] public PackedScene BulletScene;

	private Timer droneTimer;
	private float angle = 0f;
	private Marker2D droneBulletSpawn;

	/// <summary>Caches child node references and starts the auto-fire timer.</summary>
	public override void _Ready() {
		droneTimer = GetNode<Timer>("DroneTimer");
		droneBulletSpawn = GetNode<Marker2D>("DroneBulletSpawn");

		droneTimer.Timeout += Shoot;
		droneTimer.Start();
	}

	/// <summary>Advances the drone's position along its circular orbit each frame.</summary>
	public override void _Process(double delta) {
		angle += OrbitSpeed * (float)delta;

		Position = new Vector2(
			Mathf.Cos(angle),
			Mathf.Sin(angle)
		) * OrbitRadius;
	}

	/// <summary>Fires a bullet from the drone's spawn point, called on the drone's own timer.</summary>
	private void Shoot() {
		if (BulletScene == null)
			return;
		
		var b = BulletScene.Instantiate<Bullet>();
		b.Shooter = this;
		GetParent().AddChild(b);
		b.Start(droneBulletSpawn.GlobalPosition);
	}
}
