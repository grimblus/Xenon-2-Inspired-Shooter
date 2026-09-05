using Godot;
using System.Collections.Generic;

public partial class ScoreManager : Node
{
	public static ScoreManager Instance { get; private set; }

	public List<int> HighScores { get; private set; } = new List<int>();
	private const int MaxEntries = 3;

	public override void _Ready()
	{
		Instance = this;
	}

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
