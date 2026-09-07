using Godot;

/// <summary>
/// Plays the startup logo sequence: fades each BootPage's logos in, holds, fades out, then
/// advances to the next page (or the main menu once all pages are done). Any key/click
/// press skips straight to the fade-out of the current page.
/// </summary>
public partial class BootScreen : Control
{
	[Export] public BootPage[] Pages;
	[Export] public string NextScenePath = "res://scenes/ui/main_menu.tscn";

	[Export] public float FadeTime = 0.5f;
	[Export] public float HoldTime = 2.0f;

	private enum State { FadeIn, Hold, FadeOut }
	private State state;
	private float timer = 0f;
	private int index = 0;

	private VBoxContainer logoRow;

	/// <summary>Caches the logo row container and starts the sequence at the first page.</summary>
	public override void _Ready()
	{
		logoRow = GetNode<VBoxContainer>("LogoRow");
		ShowPage(0);
	}

	/// <summary>Builds the logo row for the given page (sized from each logo's own aspect ratio) and starts its fade-in.</summary>
	private void ShowPage(int i)
	{
		index = i;

		foreach (Node child in logoRow.GetChildren())
		{
			child.QueueFree();
		}

		foreach (BootLogo logo in Pages[index].Logos)
		{
			var rect = new TextureRect();
			rect.Texture = logo.Texture;
			rect.ExpandMode = TextureRect.ExpandModeEnum.IgnoreSize;
			rect.StretchMode = TextureRect.StretchModeEnum.KeepAspectCentered;

			float aspect = logo.Texture.GetWidth() / (float)logo.Texture.GetHeight();
			rect.CustomMinimumSize = new Vector2(logo.MaxHeight * aspect, logo.MaxHeight);

			logoRow.AddChild(rect);
		}

		logoRow.Modulate = new Color(1, 1, 1, 0);
		state = State.FadeIn;
		timer = 0f;
	}

	/// <summary>Advances the current page's fade-in/hold/fade-out state machine each frame.</summary>
	public override void _Process(double delta)
	{
		float dt = (float)delta;

		switch (state)
		{
			case State.FadeIn:
				timer += dt;
				float fadeInAlpha = Mathf.Clamp(timer / FadeTime, 0f, 1f);
				logoRow.Modulate = new Color(1, 1, 1, fadeInAlpha);
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
				logoRow.Modulate = new Color(1, 1, 1, fadeOutAlpha);
				if (timer >= FadeTime)
				{
					timer = 0f;
					AdvanceToNext();
				}
				break;
		}
	}

	/// <summary>Lets the player skip the current page's hold time with any key press or click.</summary>
	public override void _UnhandledInput(InputEvent @event)
	{
		bool skipPressed = (@event is InputEventKey key && key.Pressed && !key.Echo)
			|| (@event is InputEventMouseButton mouse && mouse.Pressed);

		if (skipPressed)
		{
			SkipCurrent();
		}
	}

	/// <summary>Jumps straight to the fade-out state, skipping the rest of the current page's hold time.</summary>
	private void SkipCurrent()
	{
		if (state == State.FadeOut) return; // already leaving, let it finish naturally
		state = State.FadeOut;
		timer = 0f;
	}

	/// <summary>Moves to the next page, or transitions to NextScenePath once all pages have played.</summary>
	private void AdvanceToNext()
	{
		int next = index + 1;
		if (next >= Pages.Length)
		{
			GetTree().ChangeSceneToFile(NextScenePath);
		}
		else
		{
			ShowPage(next);
		}
	}
}
