using Godot;
using System;

public class PlayerAnimation : Spatial
{
	public AnimationTree animationTree;
	public AnimationNodeStateMachinePlayback animationStateMachine;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		this.animationTree = GetNode<AnimationTree>("AnimationTree");
		this.animationStateMachine = (AnimationNodeStateMachinePlayback) this.animationTree.Get("parameters/StateMachine/playback");

		this.updateBasicController(0.0f);
	}

	public void updateBasicController(float position)
	{
		this.animationTree.Set("parameters/StateMachine/BasicController/blend_position", position);
	}
}
