using Godot;
using GodotSteeringAI;
using System;

public partial class QuickPlayerMovement : CharacterBody2D, GSAITargetable, Knockbackable
{
    [Export]
    public float maxSpeed = 200.0f;

    [Export]
    public float userAcceleration = 400.0f;

    [Export]
    public float maxAcceleration = 250.0f;

    [Export]
    public float friction = 175.0f;

    [Export]
    public Usable weapon;

    [Export]
    public float knockbackDuration = 0.1f;

    public GSAIAgentLocation gSAIAgentLocation;
    private GSAIApplyForce knockback;
    private GSAITargetAcceleration knockbackAcceleration = new GSAITargetAcceleration();
    private GSAISteeringAgent steeringAgent;
    private Vector2 knockbackVelocity = Vector2.Zero;

    public override void _Ready()
    {
        gSAIAgentLocation = new GSAINodeAgentLocation(this);
        knockback = new GSAIApplyForce(this, steeringAgent, Vector3.Zero, knockbackDuration);
        UsableKnockback.ApplyKnockbackAfterUse(weapon, this);
    }

    public override void _PhysicsProcess(double delta)
    {
        knockback.CalculateSteering(knockbackAcceleration);
        knockbackVelocity = GSAIUtils.ToVector2(knockbackAcceleration.Linear);

        Vector2 currVelocity = Velocity;
        Velocity = knockbackVelocity;
        MoveAndSlide();
        Velocity = currVelocity;
        Velocity = Velocity.Lerp(Vector2.Zero, friction * (float)delta);

        Vector2 userInputAcceleration =
            (float)delta
            * this.userAcceleration
            * new Vector2(
                Input.GetActionStrength("ui_right") - Input.GetActionStrength("ui_left"),
                Input.GetActionStrength("ui_down") - Input.GetActionStrength("ui_up")
            ).LimitLength(1.0f);

        Vector2 acceleration = userInputAcceleration;
        Velocity += acceleration;
        Velocity = Velocity.LimitLength(maxSpeed);
        MoveAndSlide();
    }

    public GSAIApplyForce GetKnockbackForce()
    {
        return this.knockback;
    }

    public GSAIAgentLocation GetAgentLocation()
    {
        return this.gSAIAgentLocation;
    }
}
