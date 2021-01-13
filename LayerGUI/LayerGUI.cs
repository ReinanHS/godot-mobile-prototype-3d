using Godot;
using System;

public class LayerGUI : CanvasLayer
{
	[Export]
	public NodePath PanelPath, joystickPath, joystickCameraPath;

	public ProgressBar fomeBar, sedeBar, vidaBar;

	public Joystick joystick, joystickCamera;

	public override void _Ready()
	{
		this.joystick = GetNode<Joystick>(this.joystickPath);
		this.joystickCamera = GetNode<Joystick>(this.joystickCameraPath);

		this.fomeBar = GetNode(this.PanelPath).GetNode<ProgressBar>("Fome/ProgressBar");
		this.sedeBar = GetNode(this.PanelPath).GetNode<ProgressBar>("Sede/ProgressBar");
		this.vidaBar = GetNode(this.PanelPath).GetNode<ProgressBar>("Vida/ProgressBar");
	}
}
