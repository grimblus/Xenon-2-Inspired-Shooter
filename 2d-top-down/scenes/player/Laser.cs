using Godot;
using System;

/// <summary>
/// The player's laser weapon beam (Laser power-up). A persistent hitbox that continuously
/// damages enemies it overlaps, rather than a one-shot projectile like Bullet.
/// </summary>
public partial class Laser : Area2D
{
	public Player player;

	/// <summary>Damages an enemy Area2D the beam overlaps.</summary>
	private void OnLaserAreaEntered(Area2D area) {
		if (area.IsInGroup("enemies")) {
			if (area.HasMethod("TakeDamage"))  {
				area.Call("TakeDamage", 10);
			}
		}
	}

	/// <summary>Damages an enemy body the beam overlaps.</summary>
	private void OnLaserBodyEntered(Node2D body) {
		if (body.IsInGroup("enemies")) {
			if (body.HasMethod("TakeDamage")) {
				body.Call("TakeDamage", 10);
			} 
		}
	}
}
