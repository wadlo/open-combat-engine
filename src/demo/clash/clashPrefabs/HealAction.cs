using Godot;
using System;

public partial class HealAction : Node, HasEntityAction
{
    [Export]
    public float healAmount;

    public bool RunAction(Entity entity)
    {
        OpenTDE.Utils.GetChildOfType<Health>((Node)entity).Heal(healAmount);
        return true;
    }
}
