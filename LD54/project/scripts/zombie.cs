using Godot;

public partial class zombie : AnimatedSprite2D
{
	float count = 0.0f;
	bool on = false;

	[Export]
	float walkSpeed = 0.0f;

	bool _isLit = false;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		this.Play("default");

		SelfModulate = new Color(0.0f, 0.0f, 0.0f);

		_isLit = false;
		SetLit(_isLit);
		ActivateCollision(_isLit);
	}

	public void ActivateCollision(bool activated)
	{
		var area = this.GetChild(0) as Area2D;
        if (area != null)
        {
			area.SetProcess(activated);
        }

    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		var pos = this.Position;
		pos.Y += this.walkSpeed * (float)delta;

		this.Position = pos;
	}

	public void SetLit(bool isLit)
	{
		if (_isLit == isLit)
			return;

		_isLit = isLit;

		ActivateCollision(_isLit);

		SelfModulate = _isLit ? new Color(1.0f, 1.0f, 1.0f) : new Color(0.0f, 0.0f, 0.0f);
    }

	public void SetWalkSpeed(float walkSpeed)
	{
		this.walkSpeed = walkSpeed;
	}

	public void OnArea2DInputEvent(Node viewport, InputEvent evt, int shape_idx)
	{
		var mouseEvt = evt as InputEventMouseButton;
		if (mouseEvt != null && mouseEvt.Pressed && mouseEvt.ButtonIndex == MouseButton.Left)
		{
			this.Play("death");

			this.walkSpeed = 0.0f;

			ActivateCollision(false);

			var root = GetTree().Root.GetNode("root");
			if (root != null)
			{
				Root r = root as Root;
				if (r != null)
				{
                    r.KillZombie(this);
                }
			}
		}
	}

	public void OnAnimFinished()
	{
		if (this.Animation == "death")
		{
			GD.Print("Death anim finished");

			QueueFree();
		}
	}
}
