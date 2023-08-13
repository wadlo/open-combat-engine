using Godot;
using System;

public partial class BulletSpawner : Node2D
{
	[Export]
	public PackedScene bulletPrefab;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	public void SpawnBullet() {
		RigidBody2D instantiated = bulletPrefab.Instantiate<RigidBody2D>();
		GetTree().Root.AddChild(instantiated);
		instantiated.GlobalPosition = GlobalPosition;
		instantiated.GlobalRotation = GlobalRotation;
		instantiated.LinearVelocity = instantiated.LinearVelocity.Rotated(GlobalRotation);
	}
}



