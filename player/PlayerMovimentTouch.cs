using Godot;
public class PlayerMovimentTouch : PlayerMovement
{
	public PlayerControl playerControl;

	public PlayerMovimentTouch(Player player, PlayerControl playerControl) : base(player)
	{
		this.playerControl = playerControl;
	}

	public new void OnMoviment(float delta)
	{
		string strDebug = "";

		if (this.playerControl._ongoing_drag == 0)
		{
			Vector3 direction = new Vector3(this.playerControl.GetMoviment().x, 0, this.playerControl.GetMoviment().y);
			Vector3 newVelocity = this.CalculateNewVelocity(direction, delta);

			_velocity.x = newVelocity.x;
			_velocity.z = newVelocity.z;

			_velocity = this.Player.MoveAndSlide(_velocity, Vector3.Up);

			if (this.Player.IsOnFloor() && this.Jump)
			{
				_velocity.y = this.JumpSpeed;
			}

			Vector3 bodyRotation = this.Player.GetNode<Spatial>("Body").Rotation;
			bodyRotation.y = Mathf.LerpAngle(bodyRotation.y, Mathf.Atan2(direction.x * -1, direction.z * -1), delta * 10);

			this.Player.GetNode<Spatial>("Body").Rotation = bodyRotation;
			this.Player.playerAnimation.updateBasicController(direction.Length());

			this.Player.playerObserver.OnPlayerMove(direction);

			strDebug = "Direction: " + direction.ToString();
			strDebug += "\nDirection Length: " + direction.Length().ToString();

			strDebug += "\n_velocity: " + _velocity.ToString();
			strDebug += "\n_velocity Length: " + _velocity.Length().ToString();

			strDebug += "\nbodyRotation: " + bodyRotation.ToString();
			strDebug += "\nbodyRotation Length: " + bodyRotation.Length().ToString();

			strDebug += "\nOn Going Drag: " + this.playerControl._ongoing_drag;

			this.playerControl.GetNode<Label>("Label").Text = strDebug;
		}
		else
		{
			this.Player.playerAnimation.updateBasicController(0.0f);
		}
	}
}
