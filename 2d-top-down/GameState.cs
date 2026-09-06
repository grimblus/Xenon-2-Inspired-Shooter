using Godot;

public partial class GameState : Node
{
	public static GameState Instance { get; private set; }

	public int Score { get; private set; } = 0;

	public bool MissileBuy = false;
	public bool DroneBuy = false;
	public bool DodgeBuy = false;

	public override void _Ready()
	{
		Instance = this;
	}

	public void AddScore(int amount)
	{
		Score += amount;
	}

	public bool TrySpend(int amount)
	{
		if (Score < amount)
			return false;

		Score -= amount;
		return true;
	}
}
