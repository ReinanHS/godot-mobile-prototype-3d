using Godot;

public class Player : KinematicBody
{
	public PlayerMovement PlayerMovement;
	public PlayerMovimentTouch playerMovimentTouch;
	public PlayerControl playerControl;
	public PlayerAnimation playerAnimation;
	public PlayerObserver playerObserver;

	[Export]
	public float Gravity = -9.8f;
	[Export]
	public float MaxSpeed = 5.0f;
	[Export]
	public float JumpSpeed = 7.0f;
	[Export]
	public float Acceleration = 2.0f;
	[Export]
	public float Deacceleration = 4.0f;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		this.playerObserver = new PlayerObserver(this);
		this.playerControl = this.GetParent().GetNode("LayerGUI/ButtonsControl") as PlayerControl;
		this.playerAnimation = this.GetNode("Body/Model") as PlayerAnimation;

		this.PlayerMovement = new PlayerMovement(this);
		this.playerMovimentTouch = new PlayerMovimentTouch(this, this.playerControl);

		this.playerObserver.OnPlayerReady();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{
		this.playerMovimentTouch.OnMoviment(delta);
		//this.PlayerMovement.OnMoviment(delta);
	}

}
