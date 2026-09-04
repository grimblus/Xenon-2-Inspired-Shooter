using Godot;
using System;

public partial class Bullet : Area2D
{
	[Export] public float Speed = -250f;
	public bool usedByRightHugger = false;
	public bool usedByLeftHugger = false;
	public bool usedByEnemy = false;
	public Player player;
	public Node2D Shooter;
	
	public void Start(Vector2 pos) {
		Position = pos;
		
		if (usedByEnemy == true) {
			Modulate = new Color (1.0f, 0.647f, 0.0f);
		}
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
	
	//pls merge :)
	
	
	
	private void OnAreaEntered(Area2D area) {
		if (area == Shooter) {
			return;
		}
		
		if (area.IsInGroup("enemies") && (usedByEnemy == false)) {
			//area.Explode();
			if (area.HasMethod("TakeDamage")) 
			{
				area.Call("TakeDamage", 10);
			}
			QueueFree();
		} else if (player != null && usedByEnemy == true) {
			if (player.HasMethod("TakeDamage")) 
			{
				player.Call("TakeDamage",10);
				QueueFree();
			}
		}
	}
	
	private void OnBodyEntered(Node2D body) {
		if (body == Shooter) {
			return;
		}
		
		if (body.IsInGroup("enemies") && !usedByEnemy) {
			if (body.HasMethod("TakeDamage"))
			{
				body.Call("TakeDamage", 10);
			} 
			QueueFree();
		} else if (usedByEnemy == true) {
			if (player != null && player.HasMethod("TakeDamage")) 
			{
				player.Call("TakeDamage",10);
				QueueFree();
			}
		}
	}
	
	private void OnVisibleOnScreenNotifier2DScreenExited() {
		QueueFree();
	}
}
