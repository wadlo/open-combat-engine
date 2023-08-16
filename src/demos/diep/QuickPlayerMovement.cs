using Godot;
using GodotSteeringAI;
using System;

public partial class QuickPlayerMovement : CharacterBody2D
{
    [Export]
    public float maxSpeed = 200.0f;

    [Export]
    public float userAcceleration = 400.0f;

    [Export]
    public float maxAcceleration = 250.0f;

    [Export]
    public float friction = 175.0f;

    public override void _PhysicsProcess(double delta)
    {
        Vector2 userInputAcceleration =
            (float)delta
            * this.userAcceleration
            * new Vector2(
                Input.GetActionStrength("ui_right") - Input.GetActionStrength("ui_left"),
                Input.GetActionStrength("ui_down") - Input.GetActionStrength("ui_up")
            ).LimitLength(1.0f);

        Vector2 frictionAcceleration =
            Velocity.MoveToward(Vector2.Zero, friction * (float)delta) - Velocity;

        Vector2 acceleration = (userInputAcceleration + frictionAcceleration).LimitLength(
            this.maxAcceleration * (float)delta
        );

        Velocity += acceleration;
        Velocity = Velocity.LimitLength(maxSpeed);
        MoveAndSlide();
    }
}
