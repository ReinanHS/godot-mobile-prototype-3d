using Godot;
public class PlayerMovement: PlayerMovimentInterface
{
	protected Player Player { get; }

	public float Gravity { get; set;}
	public float MaxSpeed { get; set; }
	public float JumpSpeed { get; set; }
	public float Acceleration { get; set; }
	public float Deacceleration { get; set; }

	protected Vector3 _velocity;

	/**
	 * Movimentação para esquerda
	 */
	public bool MoveLeft { get; set; }
	/**
	 * Movimentação para direita
	 */
	public bool MoveRight { get; set; }
	/**
	 * Movimentação para frente
	 */
	public bool MoveForward { get; set; }
	/**
	 * Movimentação para para trás
	 */
	public bool MoveBackWards { get; set; }
	/**
	 * Pulando
	 */
	public bool Jump { get; set; }

	public PlayerMovement(Player player)
	{
		this.Player = player;

		this.Gravity = player.Gravity;
		this.MaxSpeed = player.MaxSpeed;
		this.JumpSpeed = player.JumpSpeed;
		this.Acceleration = player.Acceleration;
		this.Deacceleration = player.Deacceleration;

		this._velocity = new Vector3();
	}

	public void OnMoviment(float delta)
	{
		this.ListenToInput();

		Vector3 direction = this.CalculateDirectionBasedOnInput();

		direction.y = 0;
		direction = direction.Normalized();

		Vector3 newVelocity = this.CalculateNewVelocity(direction, delta);
		_velocity.x = newVelocity.x;
		_velocity.z = newVelocity.z;

		_velocity = this.Player.MoveAndSlide(_velocity, Vector3.Up);

		if (this.Player.IsOnFloor() && this.Jump)
		{
			_velocity.y = this.JumpSpeed;
		}
	}

	public Vector3 CalculateDirectionBasedOnInput()
	{
		Vector3 direction = new Vector3();
		Transform playerTransform = this.Player.GlobalTransform;

		if (this.MoveForward)
		{
			direction += -playerTransform.basis[2];
		}
		if (this.MoveBackWards)
		{
			direction += playerTransform.basis[2];
		}

		if (this.MoveLeft)
		{
			direction += -playerTransform.basis[0];
		}
		if (this.MoveRight)
		{
			direction += playerTransform.basis[0];
		}

		return direction;
	}

	public Vector3 CalculateNewVelocity(Vector3 direction, float delta)
	{
		this._velocity.y += delta * this.Gravity;
		Vector3 newVelocity = this._velocity;

		newVelocity.y = 0;
	
		Vector3 target = direction * this.MaxSpeed;

		float acceleration = direction.Dot(newVelocity) > 0 ? this.Acceleration : this.Deacceleration;
		newVelocity = newVelocity.LinearInterpolate(target, acceleration * delta);

		return newVelocity;
	}

	public void ListenToInput()
	{
		this.MoveLeft = Input.IsActionPressed("move_left");
		this.MoveRight = Input.IsActionPressed("move_right");
		this.MoveForward = Input.IsActionPressed("move_forward");
		this.MoveBackWards = Input.IsActionPressed("move_backwards");
		this.Jump = Input.IsActionPressed("jump");
	}
}
