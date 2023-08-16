using Godot;
using System;

public partial class BulletSpawner : Node2D
{
    [Signal]
    public delegate void BulletSpawnEventHandler(Vector2 directionVector);

    [Export]
    public PackedScene bulletPrefab;

    [Export]
    public float bulletAngleVariance = 0.0f;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready() { }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta) { }

    public void SpawnBullet()
    {
        RigidBody2D instantiated = bulletPrefab.Instantiate<RigidBody2D>();
        GetTree().Root.AddChild(instantiated);

        instantiated.GlobalPosition = GlobalPosition;

        float angle = GlobalRotation + (GD.Randf() * 2 - 1) * Mathf.DegToRad(bulletAngleVariance);
        instantiated.GlobalRotation = angle;
        instantiated.LinearVelocity = instantiated.LinearVelocity.Rotated(angle);
        EmitSignal(SignalName.BulletSpawn, instantiated.LinearVelocity.Rotated(Mathf.Pi));
    }
}
