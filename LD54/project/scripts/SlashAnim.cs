using Godot;
using System;

public partial class SlashAnim : AnimatedSprite2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Play();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void OnSlashAnimComplete()
	{
		this.GetParent().QueueFree();
	}
}