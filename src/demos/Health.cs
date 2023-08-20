using System;
using Godot;

public partial class Health : Node
{
    [Export]
    public float health = 100.0f;

    public void Damage(float damage)
    {
        health = Mathf.Max(0.0f, health - damage);
        if (health <= 0.0f)
        {
            TriggerDeath();
        }
    }

    public void TriggerDeath()
    {
        this.GetParent().QueueFree();
    }
}
