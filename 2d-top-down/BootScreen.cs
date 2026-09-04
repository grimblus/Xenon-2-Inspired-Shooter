using Godot;

public partial class BootScreen : Control
{
	// assign in order: bblogo, pcflogo, xenon banner, xenon 3d logo
	[Export] public Texture2D[] Logos; 
	[Export] public string NextScenePath = "res://main.tscn";

	[Export] public float FadeTime = 0.5f;
	[Export] public float HoldTime = 2.0f;

	private enum State { FadeIn, Hold, FadeOut }
	private State state;
	private float timer = 0f;
	private int index = 0;

	private TextureRect display;

	public override void _Ready()
	{
		display = GetNode<TextureRect>("Display");
		ShowLogo(0);
	}

	private void ShowLogo(int i)
	{
		index = i;
		display.Texture = Logos[index];
		display.Modulate = new Color(1, 1, 1, 0);
		state = State.FadeIn;
		timer = 0f;
	}

	public override void _Process(double delta)
	{
		float dt = (float)delta;

		switch (state)
		{
			case State.FadeIn:
				timer += dt;
				float fadeInAlpha = Mathf.Clamp(timer / FadeTime, 0f, 1f);
				display.Modulate = new Color(1, 1, 1, fadeInAlpha);
				if (timer >= FadeTime)
				{
					timer = 0f;
					state = State.Hold;
				}
				break;

			case State.Hold:
				timer += dt;
				if (timer >= HoldTime)
				{
					timer = 0f;
					state = State.FadeOut;
				}
				break;

			case State.FadeOut:
				timer += dt;
				float fadeOutAlpha = Mathf.Clamp(1f - (timer / FadeTime), 0f, 1f);
				display.Modulate = new Color(1, 1, 1, fadeOutAlpha);
				if (timer >= FadeTime)
				{
					timer = 0f;
					AdvanceToNext();
				}
				break;
		}
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		bool skipPressed = (@event is InputEventKey key && key.Pressed && !key.Echo)
			|| (@event is InputEventMouseButton mouse && mouse.Pressed);

		if (skipPressed)
		{
			SkipCurrent();
		}
	}

	private void SkipCurrent()
	{
		if (state == State.FadeOut) return; // already leaving, let it finish naturally
		state = State.FadeOut;
		timer = 0f;
	}

	private void AdvanceToNext()
	{
		int next = index + 1;
		if (next >= Logos.Length)
		{
			GetTree().ChangeSceneToFile(NextScenePath);
		}
		else
		{
			ShowLogo(next);
		}
	}
}
