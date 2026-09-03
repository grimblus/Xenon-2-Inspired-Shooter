using Godot;
using System;

public partial class Bullet : Area2D
{
	[Export] public float Speed = -250f;
	
	public void Start(Vector2 pos) {
		Position = pos;
	}

	public override void _Process(double delta) {
		Position += new Vector2(0, Speed * (float)delta);
	}
	
	private void _on_area_entered(Area2D area) {
		if (area.IsInGroup("enemies")) {
			//area.Explode();
			QueueFree();
		}
	}
	
	private void OnVisibleOnScreenNotifier2DScreenExited() {
		QueueFree();
	}
}
