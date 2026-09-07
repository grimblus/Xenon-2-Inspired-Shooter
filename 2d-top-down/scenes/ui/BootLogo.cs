using Godot;

/// <summary>
/// A single logo shown during the boot sequence, paired with its own max display height
/// so different logos can be sized independently (see BootPage, BootScreen).
/// </summary>
public partial class BootLogo : Resource
{
	[Export] public Texture2D Texture;
	[Export] public float MaxHeight = 160f;
}
