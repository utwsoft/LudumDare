using Godot;
using System;
public partial class Root : Node2D
{
    private RandomNumberGenerator _rng;
    private PackedScene _zombieScene;
    private PackedScene _spotlightScene;

    [Export]
    public Label Timer;

    [Export]
    public Label KillCount;

    [Export]
    public Label FinalCount;

    private int _countDown;

    private float _secondCounter;

    private int _killCount;

    private bool _isFinalCountDisplay;

    // Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        _isFinalCountDisplay = false;

        _countDown = 15;

        _killCount = 0;

        _secondCounter = 0.0f;

		_rng = new RandomNumberGenerator();

        _zombieScene = GD.Load<PackedScene>("res://prefabs/Zombie.tscn");
        _spotlightScene = GD.Load<PackedScene>("res://prefabs/spotlight.tscn");

        CreateRandomSpawnZombies();

        UpdateTimerLabel();
    }

    private void CreateRandomSpawnZombies()
    {
        for (int i = 0; i < 10; ++i)
        {
            CreateZombie(_rng.RandiRange(32, 448), _rng.RandiRange(64, 740));
        }
    }

    private Node2D GetMonstersNode()
    {
        return GetNode<Node2D>("world/monsters");
    }

	private void CreateZombie(int x, int y)
	{
        if (_zombieScene == null)
            return;

        var node = _zombieScene.Instantiate();

        node.Name = "zombie";

        Node2D monsters = GetMonstersNode();
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
                z.SetWalkSpeed(10.0f);

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
        _secondCounter += (float)delta;

        if (!_isFinalCountDisplay)
        {
            if (_secondCounter >= 1.0f)
            {
                _secondCounter = 0.0f;

                _countDown--;

                UpdateTimerLabel();


                if (_countDown < 0)
                {
                    KillCount.Visible = false;
                    Timer.Visible = false;
                    FinalCount.Visible = true;

                    _isFinalCountDisplay = true;

                    UpdateFinalCountLabel();
                }
            }
        }
        else
        {
            if (_secondCounter >= 2.0f)
            {
                Reset();
            }
        }
	}

    private void UpdateTimerLabel()
    {
        int minute = _countDown / 60;
        int second = _countDown % 60;

        string counterStr = $"{minute}:{second:00}";

        if (Timer != null)
        {
            Timer.Text = counterStr;
        }
    }

    private void UpdateKillCountLabel()
    {
        string killCountStr = $"{_killCount}";

        if (KillCount != null)
        {
            KillCount.Text = killCountStr;
        }
    }

    private void UpdateFinalCountLabel()
    {
        string killCountStr = $"{_killCount}";

        if (FinalCount != null)
        {
            FinalCount.Text = killCountStr;
        }
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
        Node2D monsters = GetMonstersNode();
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
        if (z != null)
        {
            Node2D spot = _spotlightScene.Instantiate() as Node2D;
            z.AddChild(spot);
            spot.Position = Vector2.Zero;
            z.SetLit(true);
        }
    }

    public void KillZombie(Node2D zombie)
    {
        _killCount++;

        UpdateKillCountLabel();
    }

    private void Reset()
    {
        CreateRandomSpawnZombies();

        _isFinalCountDisplay = false;
        _countDown = 15;

        _killCount = 0;

        var monsters = GetMonstersNode();

        foreach (var monster in monsters.GetChildren())
        {
            monsters.RemoveChild(monster);
            monster.QueueFree();
        }

        if (FinalCount != null)
        {
            KillCount.Visible = true;
            Timer.Visible = true;
            FinalCount.Visible = false;
        }

        UpdateKillCountLabel();
        UpdateTimerLabel();
    }
}
