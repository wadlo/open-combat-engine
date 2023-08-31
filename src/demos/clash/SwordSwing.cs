using Godot;
using System;

public partial class SwordSwing : Node2D
{
    [Export]
    public Usable usable;

    [Export]
    public float startSwingRotation = -45.0f;

    [Export]
    public float endSwingRotation = 45.0f;

    [Export]
    public float idleSwingRotation = 20.0f;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready() { }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        float animationEndRotation;
        float animationStartRotation;

        if (usable.IsCharging())
        {
            animationStartRotation = idleSwingRotation;
            animationEndRotation = startSwingRotation;
        }
        else if (usable.IsFiring())
        {
            animationStartRotation = startSwingRotation;
            animationEndRotation = endSwingRotation;
        }
        else if (usable.IsReloading())
        {
            animationStartRotation = endSwingRotation;
            animationEndRotation = idleSwingRotation;
        }
        else
        {
            animationStartRotation = idleSwingRotation;
            animationEndRotation = idleSwingRotation;
        }

        RotationDegrees =
            usable.GetCurrentStatePercent() * (animationEndRotation - animationStartRotation)
            + animationStartRotation;
    }
}
