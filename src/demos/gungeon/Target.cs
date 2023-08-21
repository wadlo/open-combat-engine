using Godot;
using GodotSteeringAI;
using Godot.Collections;

public partial class Target : Node
{
    [Export]
    public Array<string> targetGroups = new Array<string>();
    public GSAIAgentLocation targetLocation = new GSAIAgentLocation();

    [Export]
    public Node2D target;

    public override void _PhysicsProcess(double delta)
    {
        if (target == null || !IsInstanceValid(target))
        {
            target = OpenTDE.Utils.GetClosestNodeInGroup(
                GetTree(),
                GetParent<Node2D>().Position,
                targetGroups[0]
            );
        }
        else
        {
            targetLocation.Position = GSAIUtils.ToVector3(target.Position);
        }
    }
}
