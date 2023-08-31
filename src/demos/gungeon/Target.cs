using Godot;
using GodotSteeringAI;
using Godot.Collections;

public partial class Target : Node
{
    [Export]
    public Array<string> targetGroups = new Array<string>();
    public GSAIAgentLocation targetLocation = new GSAIAgentLocation();

    [Export]
    public bool shouldCalculateTarget = false;

    [Export]
    private Node2D target;

    public override void _PhysicsProcess(double delta)
    {
        if (shouldCalculateTarget)
        {
            if (target != null && IsInstanceValid(target))
            {
                targetLocation.Position = GSAIUtils.ToVector3(target.Position);
            }
            else if (targetGroups.Count > 0)
            {
                target = OpenTDE.Utils.GetClosestNodeInGroup(
                    GetTree(),
                    GetParent<Node2D>().Position,
                    targetGroups[0]
                );
            }
        }
    }
}
