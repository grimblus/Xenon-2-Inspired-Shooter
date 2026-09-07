using Godot;

/// <summary>
/// One page of the boot sequence: one or more BootLogos shown together, faded in/out as
/// a group by BootScreen before advancing to the next page.
/// </summary>
public partial class BootPage : Resource
{
	[Export] public BootLogo[] Logos = System.Array.Empty<BootLogo>();
}
