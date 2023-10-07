using Godot;
using System;
using System.Collections.Generic;

public partial class MonsterControl : Node2D
{
    private PackedScene _zombieScene;

    [Signal]
    public delegate void OnMarkZombieEventHandler(Zombie z, bool active);

    [Export]
	private int TotalDistributionWidth;

	[Export]
	private int TotalDistributionHeight;


	[Export]
	private int NumRows;

	[Export]
	private int NumCols;

	private RandomNumberGenerator _rng;

    private float _spawnTimer;

    private float _spawnTime;

    private float _lightTimer;

    private float _lightTime;

    private bool _lightsOn;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        _lightsOn = false;

        _spawnTime = 0.5f;

        _lightTime = 1.0f;

        _zombieScene = GD.Load<PackedScene>("res://prefabs/Zombie.tscn");
    }

	public void Init(RandomNumberGenerator rng)
	{
		_rng = rng;

        _spawnTime = 0.5f;
        _lightTime = _rng.RandfRange(0.25f, 1.0f);

        for (int i = 0; i < 16; ++i)
        {
            SpawnZombie();
        }
    }

	private void SpawnZombie()
	{
		int index = _rng.RandiRange(0, NumRows * NumCols);

		int colIndex = index % NumCols;
		int rowIndex = index / NumRows;

		float hSpace = TotalDistributionWidth / NumCols;
		float vSpace = TotalDistributionHeight / NumRows;

		Vector2 spawnLoc = new Vector2(rowIndex * hSpace, colIndex * vSpace);

		int randArea = 12;
		spawnLoc.X += _rng.RandfRange(-randArea, randArea);
		spawnLoc.Y += _rng.RandfRange(-randArea, randArea);

        CreateZombie((int)spawnLoc.X, (int)spawnLoc.Y);
	}

    private void CreateZombie(int x, int y)
    {
        if (_zombieScene == null)
            return;

        var node = _zombieScene.Instantiate();

        node.Name = "Zombie";

        AddChild(node);

        AnimatedSprite2D sprite = node as AnimatedSprite2D;
        if (sprite != null)
        {
            sprite.Position = new Vector2(x, y);
        }

        Zombie z = node as Zombie;
        if (z != null)
        {
            z.SetWalkSpeed(10.0f);

            z.SetLit(false);
        }
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
        _spawnTimer += (float)delta;

        return;

        if (_spawnTimer >= _spawnTime)
        {
            _spawnTimer = 0.0f;

            SpawnZombie();
        }

        _lightTimer += (float)delta;

        if (_lightTimer >= _lightTime)
        {
            _lightsOn = !_lightsOn;

            if (_lightsOn)
            {
                HashSet<int> indices = new HashSet<int>();
                int onCount = _rng.RandiRange(4,8);
                while (onCount > 0)
                {
                    indices.Add(_rng.RandiRange(0, GetChildCount()));
                    onCount--;
                }


                GD.Print($"on: {indices.Count}");

                foreach (var i in indices)
                {
                    Zombie z = GetChild(i) as Zombie;
                    EmitSignal(SignalName.OnMarkZombie, z, true);
                }
            }
            else
            {
                int count = GetChildCount();
                foreach (var child in  GetChildren())
                {
                    Zombie z = child as Zombie;
                    EmitSignal(SignalName.OnMarkZombie, z, false);
                }

                GD.Print($"off: {count}");
            }

            _lightTimer = 0.0f;

            _lightTime = _rng.RandfRange(2f, 3f);


        }
	}
}
