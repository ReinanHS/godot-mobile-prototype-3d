using Godot;
using System;

public class PlayerControl : Control
{
	private TouchScreenButton MovimentTouch;
	private Sprite ControlButtonSprite;

	private Vector2 _radius = new Vector2(48, 48);
	private float _button_size;

	public int _ongoing_drag = -1;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		this.ControlButtonSprite = this.GetNode<Sprite>("ControlMoveButton");
		this.MovimentTouch = this.GetNode<TouchScreenButton>("ControlMoveButton/TouchControl");

		this._button_size = this.ControlButtonSprite.Texture.GetSize().x - 75;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{
		if (_ongoing_drag == -1)
		{
			Vector2 posDifference = (new Vector2(-115,-115) - _radius) - MovimentTouch.Position;
			MovimentTouch.Position = posDifference * 20 * delta;
		}
	}


	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventScreenDrag eventScreenDrag) 
		{
			Vector2 eventDistFromCentre = eventScreenDrag.Position - ControlButtonSprite.GlobalPosition;

			if (eventDistFromCentre.Length() <= _button_size * MovimentTouch.GlobalScale.x || eventScreenDrag.Index == _ongoing_drag)
			{
				MovimentTouch.GlobalPosition = eventScreenDrag.Position - this._radius;

				if (GetButtonPos().Length() > _button_size)
				{
					MovimentTouch.Position = GetButtonPos().Normalized() * _button_size - _radius;
				}

				_ongoing_drag = eventScreenDrag.Index;
			}
		}

		if (@event is InputEventScreenTouch eventScreenTouch)
		{
			if (eventScreenTouch.Index == _ongoing_drag && !@event.IsPressed())
			{
				_ongoing_drag = -1;
			}
		}
	}

	public Vector2 GetButtonPos()
	{
		return this.MovimentTouch.Position + this._radius;
	}

	public Vector2 GetMoviment()
	{
		if (GetButtonPos().Length() > 15)
		{
			return GetButtonPos().Normalized();
		}

		return new Vector2(0,0);
	}
}
