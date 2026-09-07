using Godot;
using System;

/// <summary>
/// Level root: manages the enemy wave spawner (batches of PatternMover mobs along a path)
/// and exposes a debug hotkey to jump straight to the shop screen.
/// </summary>
public partial class Main : Node
{
	/// <summary>Kicks off the level's spawn sequence as soon as it enters the scene tree.</summary>
	public override void _Ready()
	{
		NewGame();
	}

	[Export]
	public PackedScene MobScene { get; set; }

	//How many are spawned before another spawn point is chosen
	[Export]
	public int SpawnTotal = 5;

	[Export]
	public string ShopScreenPath = "res://scenes/ui/shop_screen.tscn";

	//spawn increment for spawn total
	private int spawnCount = 0;
	private float currentProgressRatio = 0f;

	/// <summary>Checks the ~ debug hotkey each frame to jump straight to the shop screen for testing.</summary>
	public override void _Process(double delta)
	{
		if (Input.IsKeyPressed(Key.Quoteleft))
		{
			GetTree().ChangeSceneToFile(ShopScreenPath);
		}
	}

	/// <summary>Stops mob spawning, called when the player dies.</summary>
	public void GameOver()
	{
		GetNode<Timer>("MobTimer").Stop();
		GetNode<Timer>("MobBatchTimer").Stop();
	}

	/// <summary>Starts the level's intro timer, which kicks off the first wave of enemy spawns.</summary>
	public void NewGame()
	{
		GetNode<Timer>("StartTimer").Start();
	}

	/// <summary>Starts spawning the first batch of mobs, called once the level's intro timer elapses.</summary>
	private void OnStartTimerTimeout()
	{
		GetNode<Timer>("MobBatchTimer").Start();
	}

	/// <summary>Picks a random spawn point along the mob path and starts a new batch, called on the batch interval timer.</summary>
	private void OnMobBatchTimerTimeout()
	{
		// Choose random location on spawn2D, done here so that the batch spawns there
		var mobSpawnLocation = GetNode<PathFollow2D>("Scroller/Camera2D/MobPath/MobSpawnLocation");
		currentProgressRatio = GD.Randf();

		// Start spawning, mobtimer starts
		spawnCount = 0;
		GetNode<Timer>("MobTimer").Start();
	}
	
	/// <summary>Spawns one mob at the current batch's chosen path position, called on the mob spawn-interval timer.</summary>
	// We also specified this function name in PascalCase in the editor's connection window.
	private void OnMobTimerTimeout()
	{
		var mobSpawnLocation = GetNode<PathFollow2D>("Scroller/Camera2D/MobPath/MobSpawnLocation");
		mobSpawnLocation.ProgressRatio = currentProgressRatio;

		//Create new instance of mob
		PatternMover mob = MobScene.Instantiate<PatternMover>();
		mob.Position = mobSpawnLocation.GlobalPosition;
		mob.Rotation = 0f; // so mobs only face downward

		AddChild(mob);

		//Increment spawn count so it knows when the batch is finished
		spawnCount++;
		if (spawnCount >= SpawnTotal)
		{
			GetNode<Timer>("MobTimer").Stop(); // batch done, wait for next MobBatchTimer
		}
	}
}
