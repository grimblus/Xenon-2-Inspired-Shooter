using Godot;
using System;

/// <summary>
/// The player's laser weapon beam (Laser power-up). A persistent hitbox that continuously
/// damages enemies it overlaps, rather than a one-shot projectile like Bullet. Damage is
/// applied on a repeating tick against whatever's currently overlapping (area_entered/
/// body_entered only fire once per overlap, which is why a stationary target like the boss
/// used to take just one hit despite sitting in the beam the whole time).
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

	/// <summary>Damages every enemy area/body currently overlapping the beam, called on the damage-tick timer.</summary>
	private void DamageOverlapping() {
		foreach (Area2D area in GetOverlappingAreas()) {
			if (area.IsInGroup("enemies") && area.HasMethod("TakeDamage")) {
				area.Call("TakeDamage", DamagePerTick);
			}
		}

		foreach (Node body in GetOverlappingBodies()) {
			if (body.IsInGroup("enemies") && body.HasMethod("TakeDamage")) {
				body.Call("TakeDamage", DamagePerTick);
			}
		}
	}
}
