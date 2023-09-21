using Godot;
using System;

public partial class RunActionOnDeath : Node
{
    public Action action;

    public override void _ExitTree()
    {
        action.Invoke();
    }
}
