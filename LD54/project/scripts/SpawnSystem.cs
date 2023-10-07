using Godot;
using System;

public partial class SpawnSystem : Node2D
{
    private PackedScene _spawnerScene;

    [Export]
    private int TotalDistributionWidth;

    [Export]
    private int TotalDistributionHeight;


    [Export]
    public int NumRows;

    [Export]
    public int NumCols;

	[Export]
	private float ScrollDownSpeed;

	private float _startX;
	private float _startY;
    
	// Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        _spawnerScene = GD.Load<PackedScene>("res://prefabs/spawner.tscn");

		Reset();
    }

	public void Reset()
	{
		foreach (var child in GetChildren())
		{
			RemoveChild(child);
			child.QueueFree();
		}

		float horizSpacing = TotalDistributionWidth / (NumCols + 1);
		float vertSpacing = TotalDistributionHeight / (NumRows);

		_startX = horizSpacing;
		_startY = vertSpacing;

		float curX = _startX;
		float curY = 0.0f;

		for (int y = 0; y < NumRows; ++y)
		{
			for (int x = 0; x < NumCols; ++x)
			{
				Node2D spawner = _spawnerScene.Instantiate<Node2D>();
				
				spawner.Position = new Vector2(curX, curY);

				this.AddChild(spawner);

				curX += horizSpacing;
			}

			curX = _startX;
			curY += vertSpacing;
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		foreach (Node2D child in GetChildren())
		{
			var pos = child.Position;

            pos.Y += (float)delta * ScrollDownSpeed;

            Rect2 vp = GetViewportRect();

            if (child.Position.Y > vp.Size.Y)
            {
				pos.Y = 0.0f;
            }

			child.Position = pos;
		}
	}
}
