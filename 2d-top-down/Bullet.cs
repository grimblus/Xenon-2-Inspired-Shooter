using Godot;
using System;

public partial class Bullet : Area2D
{
	[Export] public float Speed = -250f;
	public bool usedByRightHugger = false;
	public bool usedByLeftHugger = false;
	
	public void Start(Vector2 pos) {
		Position = pos;
	}

	public override void _Process(double delta) {
		//Var for shoot direction
		Vector2 shootDirection;

		//Ifs are for when wallhugger uses
		if (usedByRightHugger) {
			shootDirection = new Vector2(Speed, 0);
		} else if (usedByLeftHugger) {
			shootDirection = new Vector2(-Speed, 0);
		} else {
			//Default as player bullet
			shootDirection = new Vector2(0, Speed);
		}
		
		//The rest of the equation
		Position += shootDirection * (float)delta;
		
		// Bullet faces where it moves
		Rotation = shootDirection.Angle() + Mathf.Pi / 2;
	}
	
	private void OnAreaEntered(Area2D area) {
		if (area.IsInGroup("enemies")) {
			//area.Explode();
			QueueFree();
		}
	}
	
	private void OnVisibleOnScreenNotifier2DScreenExited() {
		QueueFree();
	}
}
