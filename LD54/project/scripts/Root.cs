using Godot;
using System;
public partial class Root : Node2D
{
    private RandomNumberGenerator _rng;
    private PackedScene _zombieScene;
    private PackedScene _spotlightScene;

    // Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_rng = new RandomNumberGenerator();

        _zombieScene = GD.Load<PackedScene>("res://prefabs/Zombie.tscn");
        _spotlightScene = GD.Load<PackedScene>("res://prefabs/spotlight.tscn");

        for (int i = 0; i < 10; ++i)
        {
            CreateZombie(_rng.RandiRange(32, 448), _rng.RandiRange(64, 740));
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

            zombie z = SelectRandomZombie();
            MarkZombie(z);
        }
    }

    private zombie SelectRandomZombie()
    {
        Node2D monsters = GetNode<Node2D>("monsters");
        if (monsters != null)
        {
            int count = monsters.GetChildCount();
            int index = _rng.RandiRange(0, count);

            GD.Print($"index: {index}");

            return monsters.GetChild(index) as zombie;
        }

        return null;
    }

    private void MarkZombie(zombie z)
    {
        Node2D spot = _spotlightScene.Instantiate() as Node2D;
        this.AddChild(spot);
        spot.Position = z.Position;
    }
}
