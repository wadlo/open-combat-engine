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
        face = new GSAIFace(steeringAgent, target.targetLocation);
        targetAcceleration = new GSAITargetAcceleration();
        GlobalRotation = GSAIUtils.Vector2ToAngle(new Vector2(1.0f, 0.0f));

        face.DecelerationRadius = 20f;
        face.AlignmentTolerance = 0.0f;
        steeringAgent.AngularAccelerationMax = 200.0f;
        steeringAgent.AngularSpeedMax = 250.0f;
    }

    public override void _PhysicsProcess(double _delta)
    {
        float delta = (float)_delta;
        steeringAgent.AngularVelocity *= 0.85f;
        steeringAgent.Orientation = GlobalRotation + Mathf.Pi / 2.0f;
        steeringAgent.Position = GSAIUtils.ToVector3(GlobalPosition);

        face?.CalculateSteering(targetAcceleration);

        steeringAgent.AngularVelocity += delta * targetAcceleration.Angular;
        Rotate(delta * steeringAgent.AngularVelocity);
    }
}
