using Godot;
using GodotSteeringAI;
using System;

public partial class SwordSwing : Node2D
{
    [Export]
    public Target target;

    [Export]
    public Usable swordItem;

    [Export]
    public float swingRadius = 2.0f;

    public override void _Ready() { }

    public override void _PhysicsProcess(double _delta)
    {
        Rotation = Mathf.Pi / 2.0f + (-0.5f) * swingRadius;

        if (swordItem.IsFiring())
        {
            Rotation = Mathf.Pi / 2.0f + (swordItem.GetFireStatePercent() - 0.5f) * swingRadius;
        }
    }
}
