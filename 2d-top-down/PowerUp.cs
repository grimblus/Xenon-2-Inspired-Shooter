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
		MissileUp
	}
	
	[Export] public PowerType Type;
	
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
				
			case PowerType.MissileUp:
				player.missileUp = true;
				break;
				
			case PowerType.TripleShot:
				player.tripleShot = true;
				break;
		}
	}
}
