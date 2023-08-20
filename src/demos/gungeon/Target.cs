using Godot;
using GodotSteeringAI;
using System;

public partial class Target : Node
{
    public GSAIAgentLocation targetLocation = new GSAIAgentLocation();

    [Export]
    public Node2D target;

    public override void _PhysicsProcess(double delta)
    {
        targetLocation.Position = GSAIUtils.ToVector3(target.Position);
    }
}
