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

        MonsterControl node = GetMonstersNode() as MonsterControl;
        node.Init(_rng);
        node.OnMarkZombie += MarkZombie;

        UpdateTimerLabel();
    }

    private Node2D GetMonstersNode()
    {
        return GetNode<Node2D>("world/monsters");
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

            //zombie z = SelectRandomZombie();
            //MarkZombie(z);
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

    private void MarkZombie(zombie z, bool active)
    {
        if (z != null)
        {
            if (active && !z.IsLit())
            {
                Node2D spot = _spotlightScene.Instantiate() as Node2D;
                spot.Name = "spot";
                z.AddChild(spot);
                spot.Position = Vector2.Zero;
                z.SetLit(active);
            }
            else if (!active && z.IsLit())
            {
                var light = z.GetNode("./spot");
                z.RemoveChild(light);
                light.QueueFree();
                z.SetLit(active);
            }

            
        }
    }

    public void KillZombie(Node2D zombie)
    {
        _killCount++;

        UpdateKillCountLabel();
    }

    private void Reset()
    {
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
