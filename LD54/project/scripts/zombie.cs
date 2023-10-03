using Godot;

public partial class zombie : AnimatedSprite2D
{
	float count = 0.0f;
	bool on = false;

	[Export]
	float walkSpeed = 0.0f;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		this.Play("default");
		SetLit(false);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		var pos = this.Position;
		pos.Y += this.walkSpeed * (float)delta;

		this.Position = pos;

		//float fDelta = (float)delta;
		//count += fDelta;

		//if (count >= 1.0f)
		//{
		//	count = 0.0f;

		//	on = !on;

		//	SetLit(on);
		//}
	}

	public void SetLit(bool isLit)
	{
        var mat = this.Material as CanvasItemMaterial;
        if (mat != null)
        {
            mat.LightMode = isLit ? CanvasItemMaterial.LightModeEnum.Unshaded : CanvasItemMaterial.LightModeEnum.Normal;
        }
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
