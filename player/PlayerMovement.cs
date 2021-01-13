using Godot;
public class PlayerMovement
{
	protected Player Player { get; }
	protected Vector3 velocity = Vector3.Zero;

	public PlayerMovement(Player player)
	{
		this.Player = player;
	}

	public void onMoviment(float delta, Vector3 direction)
	{
		Vector3 newVelocity = this.calculateNewVelocity(direction, delta);
		this.velocity.x = newVelocity.x;
		this.velocity.z = newVelocity.z;

		this.velocity = this.Player.MoveAndSlide(this.velocity, Vector3.Up);
		this.Player.playerAnimation.updateBasicController(direction.Length());

		if (this.Player.IsOnFloor() && Input.IsActionPressed("jump"))
		{
			this.velocity.y = this.Player.JumpSpeed;
		}
	}

	private Vector3 calculateNewVelocity(Vector3 direction, float delta)
	{
		this.velocity.y += delta * this.Player.Gravity;
		Vector3 newVelocity = this.velocity;

		newVelocity.y = 0;

		float speed = this.Player.WalkingSpeed;

		if (Input.IsActionPressed("run"))
		{
			speed = this.Player.RunningSpeed;
		}

		Vector3 target = direction * speed;

		float acceleration = direction.Dot(newVelocity) > 0 ? this.Player.Acceleration : this.Player.Deacceleration;
		newVelocity = newVelocity.LinearInterpolate(target, acceleration * delta);

		return newVelocity;
	}
}