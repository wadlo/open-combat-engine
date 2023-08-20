using Godot;
using System;

public partial class KinematicArea2D : Area2D
{
    [Export]
    public Vector2 LinearVelocity = Vector2.Zero;

    public override void _PhysicsProcess(double delta)
    {
        Position += (float)delta * LinearVelocity;
    }
}
