using Godot;
using GodotSteeringAI;
using System;

public interface Knockbackable
{
    public abstract GSAIApplyForce GetKnockbackForce();
}
