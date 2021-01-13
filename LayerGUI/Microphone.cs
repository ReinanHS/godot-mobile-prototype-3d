using Godot;
using System;

public class Microphone : TouchScreenButton
{
	private AudioEffectRecord audioEffect;
	private AudioStreamPlayer audioStreamPlayer, audioStreamRecord;
	private AudioStreamSample recording;

	public override void _Ready()
	{
		this.audioStreamPlayer = GetNode<AudioStreamPlayer>("AudioStreamPlayer");
		this.audioStreamRecord = GetNode<AudioStreamPlayer>("AudioStreamRecord");

		int idx = AudioServer.GetBusIndex("Record");

		this.audioEffect = AudioServer.GetBusEffect(idx, 0) as AudioEffectRecord;

		this.Connect("pressed", this, "pressed");
		this.Connect("released", this, "released");
	}

	public void pressed()
	{
		if (!this.audioEffect.IsRecordingActive())
		{
			recording = this.audioEffect.GetRecording();
			this.audioEffect.SetRecordingActive(true);
		}
	}

	public void released()
	{
		this.audioStreamPlayer.Stream = recording;
		this.audioStreamPlayer.Play();

		this.audioEffect.SetRecordingActive(false);
	}
}
