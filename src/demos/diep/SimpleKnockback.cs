using Godot;
using System;

public partial class SimpleKnockback : Node2D
{
    [Export]
    NodePath characterBody2DRef;
    CharacterBody2D characterBody2D;

    [Export]
    float forceMultiplier = 0.04f;

    public void ApplyKnockback(Vector2 force)
    {
        characterBody2D = GetNode<CharacterBody2D>(characterBody2DRef);
        characterBody2D.Velocity += forceMultiplier * force;
    }
}
