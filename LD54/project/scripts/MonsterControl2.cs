using Godot;
using System;
using System.Collections.Generic;

public partial class MonsterControl2 : Node2D
{
    private PackedScene _zombieScene;

    [Export]
    private SpawnSystem _spawner;

    [Signal]
    public delegate void OnMarkZombieEventHandler(Zombie z, bool active);

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

        Init(new RandomNumberGenerator());
    }

	public void Init(RandomNumberGenerator rng)
	{
		_rng = rng;

        _spawnTime = 0.5f;
        _lightTime = _rng.RandfRange(0.25f, 0.75f);

        int initialSpawnCount = 16;

        while (initialSpawnCount > 0)
        {
            int index = _rng.RandiRange(0, (_spawner.NumCols * _spawner.NumRows) - 1);
            Spawner spawner = _spawner.GetChild(index) as Spawner;

            if (!spawner.IsOccupied())
            {
                Zombie z = CreateZombie();
                z.Position = Vector2.Zero;
                spawner.Occupy(z);
                initialSpawnCount--;
            }
        }
    }

    private List<Spawner> GetUnoccupiedSpawners()
    {
        List<Spawner> ret = new List<Spawner>();
        foreach (Spawner spawner in _spawner.GetChildren())
        {
            if (!spawner.IsOccupied())
                ret.Add(spawner);
        }
        return ret;
    }

    private List<Spawner> GetOccupiedSpawners()
    {
        List<Spawner> ret = new List<Spawner>();
        foreach (Spawner spawner in _spawner.GetChildren())
        {
            if (spawner.IsOccupied())
                ret.Add(spawner);
        }
        return ret;
    }

    private void TrySpawnZombieAtRandom()
    {
        List<Spawner> slots = GetUnoccupiedSpawners();

        if (slots.Count == 0)
            return;

        int index = _rng.RandiRange(0, slots.Count);

        Zombie z = CreateZombie();
        slots[index].Occupy(z);
        z.Position = Vector2.Zero;
    }

    private Zombie CreateZombie()
    {
        if (_zombieScene == null)
            return null;

        var node = _zombieScene.Instantiate();

        node.Name = "zombie";

        AnimatedSprite2D sprite = node as AnimatedSprite2D;
        if (sprite != null)
        {
            sprite.Position = Vector2.Zero;
        }

        Zombie z = node as Zombie;
        if (z != null)
        {
            z.SetLit(false);
        }

        return z;
    }

    private void UpdateLights(float delta)
    {
        _lightTimer += delta;

        if (_lightTimer >= _lightTime)
        {
            _lightsOn = !_lightsOn;

            List<Spawner> occupiedSpawners = GetOccupiedSpawners();

            if (_lightsOn)
            {
                HashSet<int> indices = new HashSet<int>();
                int onCount = _rng.RandiRange(4, 8);
                while (onCount > 0)
                {
                    indices.Add(_rng.RandiRange(0, occupiedSpawners.Count));
                    onCount--;
                }

                GD.Print($"on: {indices.Count}");

                foreach (var i in indices)
                {
                    Spawner spawner = occupiedSpawners[i];

                    Zombie z = spawner.GetZombie();
                    EmitSignal(SignalName.OnMarkZombie, z, true);
                }
            }
            else
            {
                foreach (Spawner child in occupiedSpawners)
                {
                    Zombie z = child.GetZombie();
                    EmitSignal(SignalName.OnMarkZombie, z, false);
                }
            }

            _lightTimer = 0.0f;

            _lightTime = _rng.RandfRange(2f, 3f);
        }
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
        _spawnTimer += (float)delta;

        if (_spawnTimer >= _spawnTime)
        {
            _spawnTime = _rng.RandfRange(0.5f, 1.0f);

            _spawnTimer = 0.0f;

            TrySpawnZombieAtRandom();
        }

        UpdateLights((float)delta);
	}

    public void DisableAllColliders()
    {
        List<Spawner> occupied = GetOccupiedSpawners();
        foreach (Spawner spawner in occupied)
        {
            Zombie z = spawner.GetZombie();
            z.ActivateCollision(false);
        }
    }
}
