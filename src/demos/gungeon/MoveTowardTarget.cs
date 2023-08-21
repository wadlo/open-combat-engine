using Godot;
using GodotSteeringAI;
using System;

public partial class MoveTowardTarget : CharacterBody2D, Knockbackable
{
    [Export]
    public float maxSpeed = 200.0f;

    [Export]
    public float maxAcceleration = 200.0f;
    private Target target;

    // These variables are essentially a representation of velocity and acceleration inside the steering model.
    private GSAITargetAcceleration acceleration = new GSAITargetAcceleration();

    private GSAISteeringAgent agent;
    private GSAIArrive steering;
    private GSAIApplyForce force;

    public override void _Ready()
    {
        target = OpenTDE.Utils.GetChildrenOfType<Target>(this)[0];
        agent = new GSAIKinematicBody2DAgent(this);

        steering = new GSAIArrive(agent, target.targetLocation);

        agent.LinearSpeedMax = maxSpeed;
        agent.LinearAccelerationMax = maxAcceleration;
        //agent.CalculateVelocities = false;
        force = new GSAIApplyForce(this, agent, Vector3.Zero);
    }

    public override void _PhysicsProcess(double _delta)
    {
        float delta = (float)_delta;

        force.CalculateSteering(acceleration);
        Vector2 lastVelocity = Velocity;
        Velocity = GSAIUtils.ToVector2(this.acceleration.Linear);
        MoveAndSlide();
        Velocity = lastVelocity;

        steering.CalculateSteering(acceleration);

        Velocity += delta * GSAIUtils.ToVector2(acceleration.Linear);
        Velocity = Velocity.LimitLength(maxSpeed);

        MoveAndSlide();
        agent.LinearVelocity = GSAIUtils.ToVector3(Velocity);
    }

    public GSAIApplyForce GetKnockbackForce()
    {
        return this.force;
    }
}
