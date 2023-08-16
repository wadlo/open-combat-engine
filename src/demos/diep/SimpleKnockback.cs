using Godot;
using System;

public partial class SimpleKnockback : Node2D
{
    [Export]
    public CharacterBody2D characterBody2D;

    [Export]
    public float forceMultiplier = 0.04f;

    public void ApplyKnockback(Vector2 force)
    {
        characterBody2D.Velocity += forceMultiplier * force;
    }
}
