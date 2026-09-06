using Godot;
using System;

public partial class Missile : Area2D
{
	[Export] public float Speed = -150f;
	[Export] public int Damage = 50;
	[Export] public int ExplosionDamage = 30;
	
	public bool usedByRightHugger = false;
	public bool usedByLeftHugger = false;
	public bool usedByEnemy = false;
	public bool usedByBoss = false;
	public Vector2 aimDirection = Vector2.Down;
	
	public Player player;
	public Node2D Shooter;
	private Area2D explosion;
	
	public override void _Ready() {
		explosion = GetNode<Area2D>("Explosion");
		
		// Keep explosion disabled until the missile explodes
		explosion.Visible = false;
		explosion.Monitoring = false;
	}
	
	public void Start(Vector2 pos) {
		GlobalPosition = pos;
		
		if (usedByEnemy == true) {
			//colour orange if used by enemy
			Modulate = new Color (1.0f, 0.647f, 0.0f);
		} else {
			//default colour as player
			Modulate = new Color (0.53f, 0.81f, 0.98f);
		}
	}
	
	public override void _Process(double delta) {
		//Var for shoot direction
		Vector2 shootDirection;

		//Ifs are for when wallhugger uses
		if (usedByBoss) {
			shootDirection = aimDirection * Mathf.Abs(Speed);
		} else if (usedByRightHugger) {
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
		if (area.IsInGroup("enemies") && (usedByEnemy == false)) {
			//area.Explode();
			if (area.HasMethod("TakeDamage")) 
			{
				area.Call("TakeDamage", Damage);
			}
			Explode();
		}
	}
	
	private void OnBodyEntered(Node2D body) {
		if (body.IsInGroup("enemies") && !usedByEnemy) {
			if (body.HasMethod("TakeDamage"))
			{
				body.Call("TakeDamage", Damage);
			} 
			Explode();
		}
	}
	
	private async void Explode() {
		SetProcess(false);
		
		//Enable explosion
		explosion.Visible = true;
		explosion.Monitoring = true;
		
		await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
		Godot.Collections.Array<Node2D> bodies = explosion.GetOverlappingBodies();
		Godot.Collections.Array<Area2D> areas = explosion.GetOverlappingAreas();
		
		foreach (Node2D body in bodies) {
			if (body.IsInGroup("enemies")) {
				if (body.HasMethod("TakeDamage")) {
					body.Call("TakeDamage", ExplosionDamage);
				}
			}
		}
		
		foreach (Area2D area in areas) {
			if (area.IsInGroup("enemies")) {
				if (area.HasMethod("TakeDamage")) {
					area.Call("TakeDamage", ExplosionDamage);
				}
			}
		}
		
		AnimatedSprite2D explodeAnimation = explosion.GetNode<AnimatedSprite2D>("ExplosionAnim");
		explodeAnimation.Play("explode");
		
		await ToSignal(explodeAnimation, AnimatedSprite2D.SignalName.AnimationFinished);
		QueueFree();
		GD.Print("EXPLOSION");
	}
	
	private void OnVisibleOnScreenNotifier2DScreenExited() {
		QueueFree();
	}
}
