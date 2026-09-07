using Godot;
using System;

/// <summary>
/// A pickup orb that grants the player a power-up on contact: either a temporary in-level
/// boost (triple-shot, speed, laser, shield, invincibility) or a permanent shop-style
/// upgrade flag (missile/drone/dodge). Appearance and effect are driven by Type.
/// </summary>
public partial class PowerUp : Area2D
{
	public enum PowerType {
		TripleShot,
		SpeedUp,
		LaserUp,
		ShieldUp,
		InvincibleUp,
		MissileBuy,
		DroneBuy,
		DodgeBuy
	}
	
	[Export] public PowerType Type;
	
	private AnimatedSprite2D orb;
	
	/// <summary>Caches the orb sprite and sets its appearance based on Type.</summary>
	public override void _Ready() {
		orb = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		SetAppearance();
	}

	/// <summary>Plays the animation matching this power-up's Type.</summary>
	private void SetAppearance() {
		switch (Type) {
			case PowerType.TripleShot:
				orb.Animation = "tripleShot";
				break;
				
			case PowerType.SpeedUp:
				orb.Animation = "speedUp";
				break;
				
			case PowerType.LaserUp:
				orb.Animation = "laserUp";
				break;
				
			case PowerType.ShieldUp:
				orb.Animation = "shieldUp";
				break;
				
			case PowerType.InvincibleUp:
				orb.Animation = "invincibleUp";
				break;
				
			case PowerType.MissileBuy:
				orb.Animation = "missileBuy";
				break;
				
			case PowerType.DroneBuy:
				orb.Animation = "droneBuy";
				break;
				
			case PowerType.DodgeBuy:
				orb.Animation = "dodgeBuy";
				break;
		}
		
		orb.Play();
	}
	
	/// <summary>Applies this power-up to the player on contact, then removes the orb.</summary>
	private void _on_body_entered(Node2D body) {
		if (body is Player player) {
			ApplyPowerUp(player);
			QueueFree();
		}
	}

	/// <summary>Grants the player the effect matching this power-up's Type.</summary>
	private void ApplyPowerUp(Player player) {
		switch (Type) {
			case PowerType.TripleShot:
				player.ActivateTripleShot();
				break;
				
			case PowerType.SpeedUp:
				player.ActivateSpeedUp();
				break;
				
			case PowerType.LaserUp:
				player.ActivateLaser();
				break;
				
			case PowerType.ShieldUp:
				player.shieldUp = true;
				break;
				
			case PowerType.InvincibleUp:
				player.ActivateInvincible();
				break;
				
			case PowerType.MissileBuy:
				player.missileBuy = true;
				break;
				
			case PowerType.DroneBuy:
				player.droneBuy = true;
				break;

			case PowerType.DodgeBuy:
				player.dodgeBuy = true;
				break;
		}
	}
}
