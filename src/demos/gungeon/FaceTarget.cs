using Godot;
using GodotSteeringAI;
using System;

public partial class FaceTarget : Node2D
{
    [Export]
    public Target target;

    public GSAITargetAcceleration targetAcceleration;
    public GSAISteeringAgent steeringAgent;
    private GSAIFace face;

    public override void _Ready()
    {
        steeringAgent = new GSAISteeringAgent();
        face = new GSAIFace(steeringAgent, target?.targetLocation ?? new GSAIAgentLocation());
        targetAcceleration = new GSAITargetAcceleration();
        GlobalRotation = GSAIUtils.Vector2ToAngle(
            GSAIUtils.ToVector2(target?.targetLocation.Position ?? GSAIUtils.ToVector3(Position))
                - Position
        );

        steeringAgent.AngularAccelerationMax = 10.0f;
        steeringAgent.AngularSpeedMax = 100.0f;
    }

    public override void _PhysicsProcess(double _delta)
    {
        float delta = (float)_delta;
        steeringAgent.AngularVelocity *= 0.95f;
        steeringAgent.Orientation = GlobalRotation + Mathf.Pi / 2.0f;
        steeringAgent.Position = GSAIUtils.ToVector3(GlobalPosition);

        face?.CalculateSteering(targetAcceleration);

        steeringAgent.AngularVelocity += delta * targetAcceleration.Angular;
        Rotate(delta * steeringAgent.AngularVelocity);
    }
}
