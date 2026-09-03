using Godot;
using System;

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
	
	public override void _Ready() {
		orb = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		SetAppearance();
	}
	
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
	
	private void _on_body_entered(Node2D body) {
		if (body is Player player) {
			ApplyPowerUp(player);
			QueueFree();
		}
	}
	
	private void ApplyPowerUp(Player player) {
		switch (Type) {
			case PowerType.TripleShot:
				player.tripleShot = true;
				break;
				
			case PowerType.SpeedUp:
				player.speedUp = true;
				break;
				
			case PowerType.LaserUp:
				player.laserUp = true;
				break;
				
			case PowerType.ShieldUp:
				player.shieldUp = true;
				break;
				
			case PowerType.InvincibleUp:
				player.invincibleUp = true;
				break;
				
			case PowerType.MissileBuy:
				player.missileBuy = true;
				break;
		}
	}
}
