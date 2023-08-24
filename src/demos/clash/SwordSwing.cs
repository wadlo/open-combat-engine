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
        float defaultArmLength = 25.0f;

        float armLength = defaultArmLength;
        float armRotation = 0.0f;

        // Arm rotation
        float swingStartArmRotation = -swingRadius / 2.0f;
        float swingEndArmRotation = swingRadius / 2.0f;
        float idleArmRotation = Mathf.Pi / 8.0f;

        // Arm Length
        var swingSwordStartRotation = () => armRotation + Mathf.Pi / 2.0f - Mathf.Pi / 4.0f;
        var swingSwordEndRotation = () => armRotation + Mathf.Pi / 2.0f;
        var idleSwordRotation = () => Mathf.Pi / 4.0f;

        if (swordItem.IsFiring())
        {
            float animate = swordItem.GetCurrentStatePercent();
            armRotation = OpenTDE.Utils.MapFloat(
                0,
                1,
                swingStartArmRotation,
                swingEndArmRotation,
                animate
            );
            armLength =
                defaultArmLength
                + OpenTDE.Utils.MapFloat(0.25f, 0, 0, 25.0f, Mathf.Pow(animate - 0.5f, 2.0f));
            Rotation = OpenTDE.Utils.MapFloat(
                0,
                1,
                swingSwordStartRotation(),
                swingSwordEndRotation(),
                animate
            );
        }
        else if (swordItem.IsReloading())
        {
            float animate = swordItem.GetCurrentStatePercent();
            armRotation = OpenTDE.Utils.MapFloat(
                0,
                1,
                swingEndArmRotation,
                idleArmRotation,
                animate
            );
            armLength = 25.0f;
            Rotation = OpenTDE.Utils.MapFloat(
                0,
                1,
                swingSwordEndRotation(),
                idleSwordRotation(),
                animate
            );
        }
        else if (swordItem.IsCharging())
        {
            float animate = swordItem.GetCurrentStatePercent();
            armRotation = OpenTDE.Utils.MapFloat(
                0,
                1,
                idleArmRotation,
                swingStartArmRotation,
                animate
            );
            armLength = 25.0f;
            Rotation = OpenTDE.Utils.MapFloat(
                0,
                1,
                idleSwordRotation(),
                swingSwordStartRotation(),
                animate
            );
        }
        else
        {
            return;
        }

        Position = new Vector2(
            armLength * Mathf.Cos(armRotation),
            armLength * Mathf.Sin(armRotation)
        );
    }
}
