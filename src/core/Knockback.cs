using Godot;
using System;
using GodotSteeringAI;

public partial class Knockback : Node
{
    [Export]
    public float knockbackDuration = 1.0f;

    public GSAIApplyForce knockback;
    private GSAISteeringAgent steeringAgent;
    private GSAITargetAcceleration knockbackAcceleration;
    private Vector2 knockbackVelocity = Vector2.Zero;

    public override void _Ready()
    {
        steeringAgent = new GSAISteeringAgent();
        knockbackAcceleration = new GSAITargetAcceleration();
        knockback = new GSAIApplyForce(this, steeringAgent, Vector3.Zero, knockbackDuration);
    }

    public override void _PhysicsProcess(double delta)
    {
        knockback.CalculateSteering(knockbackAcceleration);
        knockbackVelocity = GSAIUtils.ToVector2(knockbackAcceleration.Linear);
        CharacterBody2D parent = GetParent<CharacterBody2D>();

        Vector2 currVelocity = parent.Velocity;
        parent.Velocity = knockbackVelocity;
        parent.MoveAndSlide();
        parent.Velocity = currVelocity;
    }
}
