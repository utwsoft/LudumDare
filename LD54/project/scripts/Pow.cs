using Godot;
using System;

public partial class Pow : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Timer timer = new Timer();
        AddChild(timer);
        timer.Autostart = true;
		timer.OneShot = true;
        timer.Timeout += _timer_Timeout;
		timer.Start(0.1);

        
    }

    private void _timer_Timeout()
    {
		QueueFree();
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
	}
}
