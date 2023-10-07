using Godot;
using System;
public partial class Root : Node2D
{
    private RandomNumberGenerator _rng;
    private PackedScene _zombieScene;
    private PackedScene _spotlightScene;
    private PackedScene _powScene;
    private PackedScene _slashScene;


    [Export]
    public Label Timer;

    [Export]
    public Label KillCount;

    [Export]
    public Label FinalCount;

    [Export]
    private SpawnSystem _spawner;

    [Export]
    private MonsterControl2 _monsterControl;

    private int _countDown;

    private float _secondCounter;

    private int _killCount;

    private bool _isFinalCountDisplay;

    private AudioStream _powSound;
    private AudioStream _wooshSound;
    private AudioStream _bellSound;

    private AudioStreamPlayer _powPlayer;
    private AudioStreamPlayer _wooshPlayer;
    private AudioStreamPlayer _bellPlayer;

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
        _powScene = GD.Load<PackedScene>("res://prefabs/pow.tscn");
        _slashScene = GD.Load<PackedScene>("res://prefabs/slash.tscn");


        MonsterControl2 node = GetMonstersNode() as MonsterControl2;
        //node.Init(_rng);
        node.OnMarkZombie += MarkZombie;

        UpdateTimerLabel();

        _powSound = GD.Load<AudioStream>("res://sounds/563356__nicholasdaryl__cartoonpunch.wav");
        _wooshSound = GD.Load<AudioStream>("res://sounds/420668__sypherzent__basic-melee-swing-miss-whoosh.wav");
        _bellSound = GD.Load<AudioStream>("res://sounds/530340__wesleyextreme_gamer__scary-bell-sound.wav");
        _wooshPlayer = new AudioStreamPlayer();
        _wooshPlayer.Stream = _wooshSound;
        AddChild(_wooshPlayer);
        _powPlayer = new AudioStreamPlayer();
        _powPlayer.Stream = _powSound;
        AddChild(_powPlayer);

        _bellPlayer = new AudioStreamPlayer();
        _bellPlayer.Stream = _bellSound;
        AddChild(_bellPlayer);

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
                    _bellPlayer.Play();

                    _monsterControl.DisableAllColliders();

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

            Node2D slash = _slashScene.Instantiate() as Node2D;
            AddChild(slash);

            uint bin = _rng.Randi() % 2;

            float hScale = (bin == 0) ? 1.0f : -1.0f;

            slash.Scale = new Vector2(hScale, 1.0f);

            slash.Position = mouseEvt.Position;

            _wooshPlayer.Play();
        }
    }

    private Zombie SelectRandomZombie()
    {
        Node2D monsters = GetMonstersNode();
        if (monsters != null)
        {
            int count = monsters.GetChildCount();
            int index = _rng.RandiRange(0, count);

            GD.Print($"index: {index}");

            return monsters.GetChild(index) as Zombie;
        }

        return null;
    }

    private void MarkZombie(Zombie z, bool active)
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
        _powPlayer.Play();

        Node2D pow = _powScene.Instantiate() as Node2D;

        Vector2 mousePos = GetViewport().GetMousePosition();

        if ( pow != null )
        {
            this.AddChild(pow);
            pow.Position = mousePos;
        }

        _killCount++;

        UpdateKillCountLabel();
    }

    private void Reset()
    {
        _isFinalCountDisplay = false;
        _countDown = 15;

        _killCount = 0;

        _spawner.Reset();
        _monsterControl.Init(_rng);

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
