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

	public override void _Ready()
	{
		//Input.SetMouseMode(Input.MouseMode.Captured);

		this.horizontal = GetNode<Spatial>("H");
		this.vertical = GetNode<Spatial>("H/V");
		this.clippedCamera = GetNode<ClippedCamera>("H/V/Camera");

		this.clippedCamera.AddException(GetParent());
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{
		this.cameraRootV = Mathf.Clamp(this.cameraRootV, this.verticalMin, this.verticalMax);

		Vector3 rotationHorizontal = this.horizontal.RotationDegrees;
		rotationHorizontal.y = Mathf.Lerp(rotationHorizontal.y, this.cameraRootH, delta * this.horizontalAcceleration);
		this.horizontal.RotationDegrees = rotationHorizontal;

		Vector3 rotationVertical = this.vertical.RotationDegrees;
		rotationVertical.x = Mathf.Lerp(rotationVertical.x, this.cameraRootV, delta * this.verticalAcceleration);
		this.vertical.RotationDegrees = rotationVertical;
	}

	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseMotion eventMouseMotion)
		{
			this.cameraRootH -= eventMouseMotion.Relative.x * horizontalSensitivity;
			this.cameraRootV += eventMouseMotion.Relative.y * verticalSensitivity;
		}
	}
}
