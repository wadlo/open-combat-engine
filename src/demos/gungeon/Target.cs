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
            if (!SetTargetLocationIfPossible() && targetGroups.Count > 0)
            {
                target = OpenTDE.Utils.GetClosestNodeInGroup(
                    GetTree(),
                    GetParent<Node2D>().Position,
                    targetGroups[0]
                );
                SetTargetLocationIfPossible();
            }
        }
    }

    // Returns true if target position was set successfully, false otherwise.
    private bool SetTargetLocationIfPossible()
    {
        if (target != null && IsInstanceValid(target))
        {
            targetLocation.Position = GSAIUtils.ToVector3(target.Position);
            return true;
        }
        return false;
    }

    public Node2D GetTargetObject()
    {
        return target;
    }
}
