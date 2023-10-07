using Godot;
using System;

public partial class Spawner : Node2D
{
	private bool _isOccupied;
	private Zombie _zombie;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void Occupy(Zombie z)
	{
		_zombie = z;
		AddChild(z);
		_isOccupied = true;
	}

	public Zombie GetZombie() { return _zombie; }

	public void Evacuate()
	{
		_isOccupied = false;
	}

	public bool IsOccupied()
	{
		return _isOccupied;
	}
}
