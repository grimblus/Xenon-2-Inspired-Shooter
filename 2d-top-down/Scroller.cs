using Godot;
using System;

public partial class Scroller : Node2D
{
	[Export] public float ScrollSpeed =80f;
	[Export] public float BottomDeadzone = 150f;   // player can move this close to the bottom edge before camera follows
	[Export] public float MaxCameraLeeway = 150f;  // max distance the camera is allowed to shift down
	[Export] public float CameraFollowSpeed = 5f;  // how quickly the camera catches up

	private Player player;
	private Camera2D camera;
	private float baseCameraY;
	private float screenHeight;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		player = GetNode<Player>("Player");
		camera = GetNode<Camera2D>("Camera2D");
		baseCameraY = camera.Position.Y;
		screenHeight = GetViewportRect().Size.Y;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Position += new Vector2(0, -ScrollSpeed * (float)delta);

		float overflow = player.Position.Y - (baseCameraY + screenHeight - BottomDeadzone);
		float targetOffset = Mathf.Clamp(overflow, 0f, MaxCameraLeeway);
		float currentOffset = camera.Position.Y - baseCameraY;
		float newOffset = Mathf.Lerp(currentOffset, targetOffset, (float)delta * CameraFollowSpeed);

		camera.Position = new Vector2(camera.Position.X, baseCameraY + newOffset);
	}
}
