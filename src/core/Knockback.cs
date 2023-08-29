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

    CharacterBody2D parent;

    public override void _Ready()
    {
        steeringAgent = new GSAISteeringAgent();
        knockbackAcceleration = new GSAITargetAcceleration();
        knockback = new GSAIApplyForce(this, steeringAgent, Vector3.Zero, knockbackDuration);
        parent = GetParent<CharacterBody2D>();
    }

    public override void _PhysicsProcess(double delta)
    {
        knockback.CalculateSteering(knockbackAcceleration);
        knockbackVelocity = GSAIUtils.ToVector2(knockbackAcceleration.Linear);

        // This is a small performance improvement. We don't need to MoveAndSlide unless there's actually some velocity here.
        if (knockbackVelocity.LengthSquared() > 0.1f)
        {
            Vector2 currVelocity = parent.Velocity;
            parent.Velocity = knockbackVelocity;
            parent.MoveAndSlide();
            parent.Velocity = currVelocity;
        }
    }
}
