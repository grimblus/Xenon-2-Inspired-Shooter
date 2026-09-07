using Godot;
using System;

/// <summary>
/// Drives the auto-scroll effect by continuously moving the whole level (Scroller and its
/// children: Camera2D, Player) upward, and gives the camera limited "leeway" to follow the
/// player down when they approach the bottom of the screen, so the ship can never scroll
/// off the bottom edge.
/// </summary>
public partial class Scroller : Node2D
{
	[Export] public float ScrollSpeed =80f;
	[Export] public float BottomDeadzone = 50f;   // player can move this close to the bottom edge before camera follows
	[Export] public float CameraFollowSpeed = 5f; // how quickly the camera catches up

	private Player player;
	private Camera2D camera;
	private float baseCameraY;
	private float screenHeight;

	/// <summary>Caches the player/camera references and the camera's starting Y position.</summary>
	public override void _Ready()
	{
		player = GetNode<Player>("Player");
		camera = GetNode<Camera2D>("Camera2D");
		baseCameraY = camera.Position.Y;
		screenHeight = GetViewportRect().Size.Y;
	}

	/// <summary>Scrolls the level upward each frame, and smoothly pulls the camera down to keep the player in view once they cross the bottom deadzone.</summary>
	public override void _Process(double delta)
	{
		Position += new Vector2(0, -ScrollSpeed * (float)delta);

		float overflow = player.Position.Y - (baseCameraY + screenHeight - BottomDeadzone);
		float targetOffset = Mathf.Max(overflow, 0f);
		float currentOffset = camera.Position.Y - baseCameraY;
		float newOffset = Mathf.Lerp(currentOffset, targetOffset, (float)delta * CameraFollowSpeed);

		camera.Position = new Vector2(camera.Position.X, baseCameraY + newOffset);
	}
}
