using Godot;
using System;

public partial class BossEnemy : Area2D
{
	private int maxHealth = 1000;
	private int currentHealth = 1000;
	private Camera2D camera;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		camera = GetNode<Camera2D>("/root/Level2/Scroller/Camera2D");
		
		//so I dont have to readd it to enemies just in case
		AddToGroup("enemies");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	public void TakeDamage(int amount) 
	{		
		currentHealth = Mathf.Clamp(currentHealth - amount, 0, maxHealth);
		if (currentHealth <= 0) {
			Die();
		}
	}
	
	private void Die()
	{
		QueueFree();
	}
}
