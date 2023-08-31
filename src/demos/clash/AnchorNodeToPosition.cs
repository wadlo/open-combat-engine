using Godot;
using System;

public partial class AnchorNodeToPosition : Node2D
{
    [Export]
    public Node2D node;

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        node.GlobalPosition = GlobalPosition;
    }
}
