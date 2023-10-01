using Godot;
using System;
public partial class Root : Node2D
{
    private RandomNumberGenerator _rng;
    private PackedScene _zombieScene;

    // Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_rng = new RandomNumberGenerator();

        _zombieScene = GD.Load<PackedScene>("res://prefabs/Zombie.tscn");

        for (int i = 0; i < 10; ++i)
        {
            CreateZombie(_rng.RandiRange(0, 480), _rng.RandiRange(0, 860));
        }
    }

	private void CreateZombie(int x, int y)
	{
        if (_zombieScene == null)
            return;

        var node = _zombieScene.Instantiate();

        node.Name = "zombie";

        Node2D monsters = GetNode<Node2D>("monsters");
        if (monsters != null)
        {
            monsters.AddChild(node);


            AnimatedSprite2D sprite = node as AnimatedSprite2D;
            if (sprite != null)
            {
                sprite.Position = new Vector2(x, y);
            }

            zombie z = node as zombie;
            if (z != null)
            {
                Material mat = GD.Load<Material>("res://materials/MonsterMat.material");
                if (mat != null)
                {
                    var matInstance = mat.Duplicate();
                    z.Material = matInstance as Material;
                    z.SetLit(false);
                }
            }
        }
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    public void OnArea2DInputEvent(Node viewport, InputEvent evt, int shape_idx)
    {
        var mouseEvt = evt as InputEventMouseButton;
        if (mouseEvt != null && mouseEvt.Pressed && mouseEvt.ButtonIndex == MouseButton.Left)
        {
            GD.Print("Root clicked");
            //this.Play("death");
        }
    }
}
