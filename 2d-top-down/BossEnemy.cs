using Godot;
using System;

public partial class BossEnemy : Area2D
{
	private int maxHealth = 1000;
	private int currentHealth = 1000;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var bodyAnimatedSprite2D = GetNode<AnimatedSprite2D>("MainBodyAnimatedSprite2D");
		string[] bodyMobTypes = bodyAnimatedSprite2D.SpriteFrames.GetAnimationNames();
		bodyAnimatedSprite2D.Play(bodyMobTypes[GD.Randi() % bodyMobTypes.Length]);
		
		var rEyeAnimatedSprite2D = GetNode<AnimatedSprite2D>("RightEyeAnimatedSprite2D");
		string[] rEyeMobTypes = rEyeAnimatedSprite2D.SpriteFrames.GetAnimationNames();
		rEyeAnimatedSprite2D.Play(rEyeMobTypes[GD.Randi() % rEyeMobTypes.Length]);
		
		var rEye2AnimatedSprite2D = GetNode<AnimatedSprite2D>("RightEye2AnimatedSprite2D");
		string[] rEye2MobTypes = rEye2AnimatedSprite2D.SpriteFrames.GetAnimationNames();
		rEye2AnimatedSprite2D.Play(rEye2MobTypes[GD.Randi() % rEye2MobTypes.Length]);
		
		var rEye3AnimatedSprite2D = GetNode<AnimatedSprite2D>("RightEye3AnimatedSprite2D");
		string[] rEye3MobTypes = rEye3AnimatedSprite2D.SpriteFrames.GetAnimationNames();
		rEye3AnimatedSprite2D.Play(rEye3MobTypes[GD.Randi() % rEye3MobTypes.Length]);
		
		var lEyeAnimatedSprite2D = GetNode<AnimatedSprite2D>("LeftEyeAnimatedSprite2D");
		string[] lEyeMobTypes = lEyeAnimatedSprite2D.SpriteFrames.GetAnimationNames();
		lEyeAnimatedSprite2D.Play(lEyeMobTypes[GD.Randi() % lEyeMobTypes.Length]);
		
		var lEye2AnimatedSprite2D = GetNode<AnimatedSprite2D>("LeftEye2AnimatedSprite2D");
		string[] lEye2MobTypes = lEye2AnimatedSprite2D.SpriteFrames.GetAnimationNames();
		lEye2AnimatedSprite2D.Play(lEye2MobTypes[GD.Randi() % lEye2MobTypes.Length]);
		
		var lEye3AnimatedSprite2D = GetNode<AnimatedSprite2D>("LeftEye3AnimatedSprite2D");
		string[] lEye3MobTypes = lEye3AnimatedSprite2D.SpriteFrames.GetAnimationNames();
		lEye3AnimatedSprite2D.Play(lEye3MobTypes[GD.Randi() % lEye3MobTypes.Length]);
		
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
