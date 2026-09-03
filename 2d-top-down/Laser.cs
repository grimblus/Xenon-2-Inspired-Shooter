using Godot;
using System;

public partial class Laser : Area2D
{
	private void _on_laser_area_entered(Area2D area) {
		if (area.IsInGroup("enemies")) {
			//area.Explode();
			QueueFree();
		}
	}
}
