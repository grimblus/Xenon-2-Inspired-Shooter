using Godot;

/// <summary>
/// Autoload singleton holding the player's persistent run state: current score
/// (spent as currency in the shop) and which upgrades have been purchased.
/// Survives scene changes, unlike per-instance fields on Player.
/// </summary>
public partial class GameState : Node
{
	public static GameState Instance { get; private set; }

	public int Score { get; private set; } = 0;

	public bool MissileBuy = false;
	public bool DroneBuy = false;
	public bool DodgeBuy = false;

	/// <summary>Registers this node as the globally-accessible singleton instance.</summary>
	public override void _Ready()
	{
		Instance = this;
	}

	/// <summary>Adds to the player's score (e.g. from kills or level completion).</summary>
	public void AddScore(int amount)
	{
		Score += amount;
	}

	/// <summary>Attempts to spend score as currency. Returns false without deducting if funds are insufficient.</summary>
	public bool TrySpend(int amount)
	{
		if (Score < amount)
			return false;

		Score -= amount;
		return true;
	}
}
