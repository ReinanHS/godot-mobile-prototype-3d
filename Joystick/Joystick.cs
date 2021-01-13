using Godot;
using System;

public class Joystick : Control
{
	public bool isWorking = false;
	public Vector2 output = new Vector2(0,0);
	public enum JoystickMode
	{
		FIXED,
		DYNAMIC,
		FOLLOWING,
	}

	[Export]
	public JoystickMode joystickMode = JoystickMode.FIXED;

	public enum VectorMode
	{
		REAL,
		NORMALIZED,
	}

	[Export]
	public VectorMode vectorMode = VectorMode.REAL;
	[Export]
	public Color pressedColor = Color.ColorN("gray");
	[Export]
	public int directions = 0;
	[Export]
	public int simmetryAngle = 90;
	[Export]
	public float deadZone = 0.2f;
	[Export]
	public float clampZone = 1f;
	public enum VisibilityMode
	{
		ALWAYS,
		TOUCHSCREEN_ONLY,
	}
	[Export]
	public VisibilityMode visibilityMode = VisibilityMode.ALWAYS;

	private TextureRect background, handle;
	private Color originalColor;
	private Vector2 originalPosition;
	private int touchIndex = -1;

	public override void _Ready()
	{
		this.background = GetNode<TextureRect>("Background");
		this.handle = GetNode<TextureRect>("Background/Handle");

		this.originalColor = this.handle.SelfModulate;
		this.originalPosition = this.background.RectPosition;

		if (OS.HasTouchscreenUiHint() && this.visibilityMode == VisibilityMode.TOUCHSCREEN_ONLY)
		{
			Hide();
		}
	}

	public bool _TouchStarted(InputEventScreenTouch @event)
	{
		return @event.IsPressed() && this.touchIndex == -1;
	}

	public bool _TouchEnded(InputEventScreenTouch @event)
	{
		return !@event.Pressed && this.touchIndex == @event.Index;
	}

	public bool isInsideControlRect(Vector2 globalPosition, Control control)
	{
		bool x = globalPosition.x > control.RectGlobalPosition.x && globalPosition.x < control.RectGlobalPosition.x + (control.RectSize.x * control.RectScale.x);
		bool y = globalPosition.y > control.RectGlobalPosition.y && globalPosition.y < control.RectGlobalPosition.y + (control.RectSize.y * control.RectScale.y);
		return x && y;
	}

	public bool isInsideControlCircle(Vector2 globalPosition, Control control)
	{
		float ray = control.RectSize.x * control.RectScale.x / 2;
		Vector2 center = control.RectGlobalPosition + new Vector2(ray, ray);
		Vector2 ray_position = globalPosition - center;
		return ray_position.LengthSquared() < ray * ray;
	}

	public void Following(Vector2 vector)
	{
		float clamp_size = this.clampZone * this.background.RectSize.x / 2;
		if (vector.Length() > clamp_size)
		{
			Vector2 radius = vector.Normalized() * clamp_size;
			Vector2 delta = vector - radius;
			Vector2 newPos = this.background.RectPosition + delta;
			newPos.x = Mathf.Clamp(newPos.x, this.background.RectSize.x / 2, RectSize.x - this.background.RectSize.x / 2);
			newPos.y = Mathf.Clamp(newPos.y, this.background.RectSize.y / 2, RectSize.y - this.background.RectSize.y / 2);
			this.background.RectPosition = newPos;
		}
	}

	public Vector2 DirectionalVector(Vector2 vector, int nDirections, float simmetryAngle = Mathf.Pi/2)
	{
		float angle = (vector.Angle() + simmetryAngle) / (Mathf.Pi / nDirections);
		angle = angle >= 0 ? Mathf.Floor(angle) : Mathf.Ceil(angle);

		if ((int)Mathf.Abs(angle) % 2 == 1)
		{
			angle = angle >= 0 ? angle + 1 : angle - 1;
		}

		angle *= Mathf.Pi / nDirections;
		angle -= simmetryAngle;

		return new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * vector.Length();
	}

	public void _CenterControl(Control control, Vector2 newGlobalPosition)
	{
		control.RectGlobalPosition = newGlobalPosition - (control.RectSize / 2);
	}

	public void ResetHandle()
	{
		this._CenterControl(this.handle, this.background.RectGlobalPosition + (this.background.RectSize / 2));
	}

	public void Reset()
	{
		this.touchIndex = -1;
		this.isWorking = false;
		this.output = new Vector2(0, 0);
		this.handle.SelfModulate = this.originalColor;
		this.background.RectPosition = this.originalPosition;
		this.ResetHandle();
	}

	public override void _Input(InputEvent @event)
	{
		if ( !(@event is InputEventScreenTouch || @event is InputEventScreenDrag) )
		{
			return;
		}

		if (@event is InputEventScreenTouch inputEventScreenTouch)
		{
			if (_TouchStarted(inputEventScreenTouch) && isInsideControlRect(inputEventScreenTouch.Position, this))
			{
				if (this.joystickMode == JoystickMode.DYNAMIC || this.joystickMode == JoystickMode.FOLLOWING)
				{
					_CenterControl(this.background, inputEventScreenTouch.Position);
				}
				if (isInsideControlCircle(inputEventScreenTouch.Position, this.background))
				{
					this.touchIndex = inputEventScreenTouch.Index;
					this.handle.SelfModulate = this.pressedColor;
				}
			}
			else if(_TouchEnded(inputEventScreenTouch))
			{
				Reset();
			}
		}
		else if (@event is InputEventScreenDrag eventScreenDrag)
		{ 
			if (this.touchIndex != eventScreenDrag.Index)
			{
				return;
			}

			float ray = this.background.RectSize.x / 2;
			float deadSize = this.deadZone * ray;
			float clampSize = this.clampZone * ray;

			Vector2 center = this.background.RectGlobalPosition + (this.background.RectSize / 2);
			Vector2 vector = eventScreenDrag.Position - center;

			if (vector.Length() > deadSize)
			{
				if (this.directions > 0)
				{
					vector = DirectionalVector(vector, this.directions, Mathf.Deg2Rad(this.simmetryAngle));
				}

				if (this.vectorMode == VectorMode.NORMALIZED)
				{
					output = vector.Normalized();
					_CenterControl(this.handle, output * clampSize + center);
				}
				else if (this.vectorMode == VectorMode.REAL)
				{
					Vector2 clampedVector = vector.Clamped(clampSize);
					this.output = vector.Normalized() * (clampedVector.Length() - deadSize) / (clampSize - deadSize);
					_CenterControl(this.handle, clampedVector + center);
				}

				this.isWorking = true;

				if (this.joystickMode == JoystickMode.FOLLOWING)
				{
					Following(vector);
				}
			}
			else
			{
				this.isWorking = false;
				this.output = new Vector2(0,0);
				this.ResetHandle();
				return;
			}
		}
	}
}
