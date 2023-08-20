using Godot;
using System;

public partial class CollisionDamage : Node
{
    public void BodyEntered(Node2D node)
    {
        GD.Print(node);
        (node as Knockbackable)
            .GetKnockbackForce()
            .ApplyForce(GodotSteeringAI.GSAIUtils.ToVector3(new Vector2(100.0f, 0.0f)));

        // node.GetChildre
    }
}
