using Godot;

public class Player : KinematicBody
{
	public PlayerAnimation playerAnimation;
	public PlayerObserver playerObserver;

	[Export]
	public float Gravity = -9.8f;
	[Export]
	public float RunningSpeed = 6;
	[Export]
	public float WalkingSpeed = 3;
	[Export]
	public float JumpSpeed = 5.0f;
	[Export]
	public float Acceleration = 2.0f;
	[Export]
	public float Deacceleration = 4.0f;
	[Export]
	public NodePath layerPath;

	public LayerGUI layer;
	private PlayerMovement playerMovement;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		this.playerObserver = new PlayerObserver(this);
		this.playerAnimation = this.GetNode<PlayerAnimation>("Body/Model");
		this.playerMovement = new PlayerMovement(this);

		try
		{
			this.layer = this.GetNode<LayerGUI>(this.layerPath);
		}
		catch
		{
			this.layer = this.GetParent().GetNode<LayerGUI>("LayerGUI");
		}

		this.playerObserver.OnPlayerReady();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{
		this._Move(delta);
	}

	public void _Move(float delta)
	{
		Vector3 direction = new Vector3(this.layer.joystick.output.x, 0, this.layer.joystick.output.y);
		direction = direction.Normalized();

		playerMovement.onMoviment(delta, direction);

		if (this.layer.joystick != null && this.layer.joystick.isWorking)
		{
			Vector3 bodyRotation = this.GetNode<Spatial>("Body").Rotation;
			bodyRotation.y = Mathf.LerpAngle(bodyRotation.y, Mathf.Atan2(direction.x * -1, direction.z * -1), delta * 10);

			this.GetNode<Spatial>("Body").Rotation = bodyRotation;
		}
	}
}
