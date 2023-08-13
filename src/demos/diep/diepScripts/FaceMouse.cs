using Godot;
using System;

public partial class FaceMouse : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
		PointToMouse();
	}

	private void PointToMouse() {
		Vector2 mousePos = GetGlobalMousePosition();
		Rotation = Mathf.Atan2(mousePos.Y - Position.Y, mousePos.X - Position.X);
	}
}
