using Godot;
using System;

public partial class Main : Node
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		NewGame();
	}
	
	[Export]
	public PackedScene MobScene { get; set; }
	
	//How many are spawned before another spawn point is chosen
	[Export]
	public int SpawnTotal = 5; 

	//spawn increment for spawn total
	private int spawnCount = 0;
	private float currentProgressRatio = 0f;

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	public void GameOver()
	{
		GetNode<Timer>("MobTimer").Stop();
		GetNode<Timer>("MobBatchTimer").Stop();
	}

	public void NewGame()
	{
		var player = GetNode<Player>("Scroller/Player");

		GetNode<Timer>("StartTimer").Start();
	}
	
	private void OnStartTimerTimeout()
	{
		GetNode<Timer>("MobBatchTimer").Start();
	}
	
	//Made so mobs can spawn in batches rather than continuous spawning
	private void OnMobBatchTimerTimeout()
	{
		// Choose random location on spawn2D, done here so that the batch spawns there
		var mobSpawnLocation = GetNode<PathFollow2D>("Scroller/MobPath/MobSpawnLocation");
		currentProgressRatio = GD.Randf();

		// Start spawning, mobtimer starts
		spawnCount = 0;
		GetNode<Timer>("MobTimer").Start();
	}
	
	// We also specified this function name in PascalCase in the editor's connection window.
	private void OnMobTimerTimeout()
	{
		var mobSpawnLocation = GetNode<PathFollow2D>("Scroller/MobPath/MobSpawnLocation");
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
