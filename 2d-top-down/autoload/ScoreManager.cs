using Godot;
using System.Collections.Generic;

/// <summary>
/// Autoload singleton tracking the top all-time high scores across playthroughs,
/// independent of GameState's per-run score/currency.
/// </summary>
public partial class ScoreManager : Node
{
	public static ScoreManager Instance { get; private set; }

	public List<int> HighScores { get; private set; } = new List<int>();
	private const int MaxEntries = 3;

	/// <summary>Registers this node as the globally-accessible singleton instance.</summary>
	public override void _Ready()
	{
		Instance = this;
	}

	/// <summary>Records a finished run's score into the leaderboard, keeping only the top MaxEntries.</summary>
	public void SubmitScore(int score)
	{
		HighScores.Add(score);
		HighScores.Sort((a, b) => b.CompareTo(a)); // descending, highest first

		if (HighScores.Count > MaxEntries)
		{
			HighScores.RemoveRange(MaxEntries, HighScores.Count - MaxEntries);
		}
	}
}
