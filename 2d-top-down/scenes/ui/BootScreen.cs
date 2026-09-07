using Godot;

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

	public override void _Ready()
	{
		logoRow = GetNode<VBoxContainer>("LogoRow");
		ShowPage(0);
	}

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
