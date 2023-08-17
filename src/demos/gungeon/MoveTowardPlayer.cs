using Godot;
using GodotSteeringAI;
using System;

public partial class MoveTowardPlayer : CharacterBody2D
{
    [Export]
    public QuickPlayerMovement player;

    [Export]
    public float maxSpeed = 200.0f;

    [Export]
    public float maxAcceleration = 200.0f;

    // These variables are essentially a representation of velocity and acceleration inside the steering model.
    private GSAITargetAcceleration acceleration = new GSAITargetAcceleration();

    private GSAIKinematicBody2DAgent agent;
    private GSAIArrive steering;

    public override void _Ready()
    {
        agent = new GSAIKinematicBody2DAgent(this);
        steering = new GSAIArrive(agent, player.gSAIAgentLocation);
        agent.LinearSpeedMax = maxSpeed;
        agent.LinearAccelerationMax = maxAcceleration;
        agent.CalculateVelocities = false;
        steering.DecelerationRadius = 300.0f;
    }

    public override void _PhysicsProcess(double _delta)
    {
        float delta = (float)_delta;
        steering.CalculateSteering(acceleration);

        Velocity += delta * GSAIUtils.ToVector2(acceleration.Linear);

        MoveAndSlide();
        agent.LinearVelocity = GSAIUtils.ToVector3(Velocity);
    }
}
