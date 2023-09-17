using Godot;
using System;

public partial class DestroyTimer : Node
{
    // Called when the node enters the scene tree for the first time.
    [Export]
    float destroyTimer = 3.0f;

    public override void _Ready() { }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        destroyTimer -= (float)delta;
        if (destroyTimer <= 0.0f)
        {
            this.GetParent().QueueFree();
        }
    }
}
