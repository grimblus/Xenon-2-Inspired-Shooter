using Godot;
using System;

/// <summary>
/// A homing-free missile projectile (player's Missile upgrade): travels like a Bullet but
/// deals direct hit damage plus an area-of-effect explosion to nearby enemies on impact.
/// </summary>
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

	/// <summary>Caches the explosion hitbox and keeps it disabled until the missile actually explodes.</summary>
	public override void _Ready() {
		explosion = GetNode<Area2D>("Explosion");
		explosion.Visible = false;
		explosion.Monitoring = false;
	}

	/// <summary>Positions the missile and tints it by who fired it (orange for enemies, blue for the player).</summary>
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
	
	/// <summary>Moves the missile each frame in the direction determined by who fired it, and orients its sprite to face that direction.</summary>
	public override void _Process(double delta) {
		Vector2 shootDirection;

		if (usedByBoss) {
			shootDirection = aimDirection * Mathf.Abs(Speed);
		} else if (usedByRightHugger) {
			shootDirection = new Vector2(Speed, 0);
		} else if (usedByLeftHugger) {
			shootDirection = new Vector2(-Speed, 0);
		} else {
			// Default: player missile, travels straight up (negative Speed).
			shootDirection = new Vector2(0, Speed);
		}

		Position += shootDirection * (float)delta;
		Rotation = shootDirection.Angle() + Mathf.Pi / 2;
	}

	/// <summary>Deals direct hit damage to an overlapped enemy Area2D, then detonates.</summary>
	private void OnAreaEntered(Area2D area) {
		if (area.IsInGroup("enemies") && (usedByEnemy == false)) {
			if (area.HasMethod("TakeDamage"))
			{
				area.Call("TakeDamage", Damage);
			}
			Explode();
		}
	}
	
	/// <summary>Deals direct hit damage to a collided enemy body, then detonates.</summary>
	private void OnBodyEntered(Node2D body) {
		if (body.IsInGroup("enemies") && !usedByEnemy) {
			if (body.HasMethod("TakeDamage"))
			{
				body.Call("TakeDamage", Damage);
			}
			Explode();
		}
	}

	/// <summary>
	/// Stops the missile, enables its explosion hitbox for one physics frame to catch
	/// nearby enemies for ExplosionDamage, plays the explosion animation, then frees the missile.
	/// </summary>
	private async void Explode() {
		SetProcess(false);
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
	
	/// <summary>Frees the missile once it leaves the visible screen, so off-screen shots don't linger forever.</summary>
	private void OnVisibleOnScreenNotifier2DScreenExited() {
		QueueFree();
	}
}
