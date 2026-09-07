using Godot;
using System;

public partial class Laser : Area2D
{
	public Player player;
	
	private void OnLaserAreaEntered(Area2D area) {
		if (area.IsInGroup("enemies")) {
			if (area.HasMethod("TakeDamage"))  {
				area.Call("TakeDamage", 10);
			}
		}
	}
	
	private void OnLaserBodyEntered(Node2D body) {
		if (body.IsInGroup("enemies")) {
			if (body.HasMethod("TakeDamage")) {
				body.Call("TakeDamage", 10);
			} 
		}
	}
}
