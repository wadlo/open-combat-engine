using Godot;
using System;

public partial class FaceMouse : Node2D
{
    [Export]
    public bool stayUpright = false;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready() { }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        PointToMouse();
    }

    private void PointToMouse()
    {
        Vector2 mousePos = GetGlobalMousePosition();
        float xDiff = mousePos.X - GlobalPosition.X;
        Rotation = Mathf.Atan2(mousePos.Y - GlobalPosition.Y, xDiff);
        if (stayUpright)
        {
            if (xDiff < 0)
            {
                Scale = new Vector2(Scale.X, -1.0f);
            }
            else
            {
                Scale = new Vector2(Scale.X, 1.0f);
            }
        }
    }
}
