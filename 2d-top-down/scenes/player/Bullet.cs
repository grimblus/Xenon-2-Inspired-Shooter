using Godot;
using System;

/// <summary>
/// A projectile fired by the player or an enemy. Direction and colour depend on the
/// usedBy* flags set by whoever instantiated it (player, WallHugger, boss, etc.).
/// Deals 10 damage to whatever it hits (that isn't its own shooter) and frees itself.
/// </summary>
public partial class Bullet : Area2D
{
	[Export] public float Speed = -250f;
	public bool usedByRightHugger = false;
	public bool usedByLeftHugger = false;
	public bool usedByEnemy = false;
	public bool usedByBoss = false;
	public Vector2 aimDirection = Vector2.Down;

	public Player player;
	public Node2D Shooter;

	/// <summary>Positions the bullet and tints it by who fired it (orange for enemies, blue for the player).</summary>
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

	/// <summary>Moves the bullet each frame in the direction determined by who fired it, and orients its sprite to face that direction.</summary>
	public override void _Process(double delta) {
		Vector2 shootDirection;

		if (usedByBoss) {
			shootDirection = aimDirection * Mathf.Abs(Speed);
		} else if (usedByRightHugger) {
			shootDirection = new Vector2(Speed, 0);
		} else if (usedByLeftHugger) {
			shootDirection = new Vector2(-Speed, 0);
		} else {
			// Default: player bullet, travels straight up (negative Speed).
			shootDirection = new Vector2(0, Speed);
		}

		Position += shootDirection * (float)delta;
		Rotation = shootDirection.Angle() + Mathf.Pi / 2;
	}

	/// <summary>Damages whatever Area2D this bullet overlaps (an enemy hitbox or the player), unless it's the bullet's own shooter.</summary>
	private void OnAreaEntered(Area2D area) {
		if (area == Shooter) {
			return;
		}
		
		if (area.IsInGroup("enemies") && (usedByEnemy == false)) {
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
	
	/// <summary>Damages whatever physics body this bullet collides with (an enemy or the player), unless it's the bullet's own shooter.</summary>
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
	
	/// <summary>Frees the bullet once it leaves the visible screen, so off-screen shots don't linger forever.</summary>
	private void OnVisibleOnScreenNotifier2DScreenExited() {
		QueueFree();
	}
}
