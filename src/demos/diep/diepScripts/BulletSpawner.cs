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

    public void FireBullet(float angle)
    {
        KinematicArea2D instantiated = bulletPrefab.Instantiate<KinematicArea2D>();
        GetTree().Root.AddChild(instantiated);

        instantiated.GlobalPosition = GlobalPosition;

        float randomAngle = (GD.Randf() * 2 - 1) * Mathf.DegToRad(bulletAngleVariance) + angle;
        instantiated.GlobalRotation = randomAngle;
        instantiated.LinearVelocity = instantiated.LinearVelocity.Rotated(randomAngle);
        EmitSignal(SignalName.BulletSpawn, instantiated.LinearVelocity.Rotated(Mathf.Pi));
    }
}
