using Godot;
using System;

public partial class CollisionDamage : Node
{
    [Export]
    public float damage = 10.0f;

    public void BodyEntered(Node2D other)
    {
        GD.Print(other);
        (other as Knockbackable)
            .GetKnockbackForce()
            .ApplyForce(GodotSteeringAI.GSAIUtils.ToVector3(new Vector2(100.0f, 0.0f)));

        foreach (Health health in OpenTDE.Utils.GetChildrenOfType<Health>(other))
        {
            health.Damage(damage);
        }
    }
}
