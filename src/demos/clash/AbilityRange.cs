using Godot;
using GodotSteeringAI;
using System;

public partial class AbilityRange : Node
{
    [Export]
    public float abilityRange = 100.0f;

    [Export]
    private Target target;

    public override void _Ready() { }

    public bool isWithinRange()
    {
        return (
                GSAIUtils.ToVector2(target.targetLocation.Position)
                - GetParent<Node2D>().GlobalPosition
            ).LengthSquared() <= Mathf.Pow(abilityRange, 2.0f);
    }
}
