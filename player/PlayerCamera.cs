using Godot;
using System;

public class PlayerCamera : Spatial
{
	public float cameraRootH, cameraRootV = 0;

	private Spatial horizontal, vertical;
	private ClippedCamera clippedCamera;

	[Export]
	public float verticalMin = -40;
	[Export]
	public float verticalMax = 10;
	[Export]
	public float verticalSensitivity = 0.1f;
	[Export]
	public float horizontalSensitivity = 0.1f;
	[Export]
	public float verticalAcceleration = 10;
	[Export]
	public float horizontalAcceleration = 10;

	private Joystick joystick;

	public override void _Ready()
	{
		this.horizontal = GetNode<Spatial>("H");
		this.vertical = GetNode<Spatial>("H/V");
		this.clippedCamera = GetNode<ClippedCamera>("H/V/Camera");

		this.clippedCamera.AddException(GetParent());

		this.joystick = GetTree().CurrentScene.GetNode("LayerGUI").GetNode<Joystick>("Control/JoystickCamera");
	}

	public override void _Process(float delta)
	{
		if (this.joystick != null && this.joystick.isWorking)
		{
			this.cameraRootH -= this.joystick.output.x * horizontalSensitivity;
			this.cameraRootV += this.joystick.output.y * verticalSensitivity;

			this.cameraRootV = Mathf.Clamp(this.cameraRootV, this.verticalMin, this.verticalMax);

			Vector3 rotationHorizontal = this.horizontal.RotationDegrees;
			rotationHorizontal.y = Mathf.Lerp(rotationHorizontal.y, this.cameraRootH, delta * this.horizontalAcceleration);
			this.horizontal.RotationDegrees = rotationHorizontal;

			Vector3 rotationVertical = this.vertical.RotationDegrees;
			rotationVertical.x = Mathf.Lerp(rotationVertical.x, this.cameraRootV, delta * this.verticalAcceleration);
			this.vertical.RotationDegrees = rotationVertical;

			Player x = GetTree().CurrentScene.GetNode("Player") as Player;
			Vector2 vector2 = new Vector2(x.Transform.origin.x, x.Transform.origin.z);
			Vector2 vector = new Vector2(rotationVertical.x, rotationHorizontal.y);

			//GD.Print(Mathf.Rad2Deg(vector.AngleToPoint(vector2)));
			GD.Print(this.clippedCamera.GlobalTransform);
		}
	}
}
