using Godot;
using System;

/// <summary>
/// The player's laser weapon beam (Laser power-up). A persistent hitbox that continuously
/// damages enemies it overlaps, rather than a one-shot projectile like Bullet. Damage is
/// applied both immediately on contact (area_entered/body_entered, so a fast-moving enemy
/// that only clips the beam for a moment still takes a hit) and on a repeating tick against
/// whatever's still overlapping (so a target that lingers, like the boss, keeps taking
/// damage instead of just the one hit on first contact).
/// </summary>
public partial class Laser : Area2D
{
	[Export] public int DamagePerTick = 10;

	public Player player;
	private Timer damageTickTimer;

	/// <summary>Caches the damage-tick timer and wires it up.</summary>
	public override void _Ready() {
		damageTickTimer = GetNode<Timer>("DamageTickTimer");
		damageTickTimer.Timeout += DamageOverlapping;
	}

	/// <summary>Damages an enemy Area2D the instant the beam starts overlapping it.</summary>
	private void OnLaserAreaEntered(Area2D area) {
		DamageIfEnemy(area);
	}

	/// <summary>Damages an enemy body the instant the beam starts overlapping it.</summary>
	private void OnLaserBodyEntered(Node body) {
		DamageIfEnemy(body);
	}

	/// <summary>Damages every enemy area/body still overlapping the beam, called on the damage-tick timer.</summary>
	private void DamageOverlapping() {
		foreach (Area2D area in GetOverlappingAreas()) {
			DamageIfEnemy(area);
		}

		foreach (Node body in GetOverlappingBodies()) {
			DamageIfEnemy(body);
		}
	}

	/// <summary>Applies DamagePerTick to the given node if it's an enemy that can take damage.</summary>
	private void DamageIfEnemy(Node node) {
		if (node.IsInGroup("enemies") && node.HasMethod("TakeDamage")) {
			node.Call("TakeDamage", DamagePerTick);
		}
	}
}
