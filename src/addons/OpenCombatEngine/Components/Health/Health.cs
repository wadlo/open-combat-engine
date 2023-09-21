using System;
using Godot;

public partial class Health : Node
{
    [Export]
    public float health = 100.0f;

    private float maxHealth;

    public override void _Ready()
    {
        maxHealth = health;
    }

    public void Damage(float damage)
    {
        health = Mathf.Max(0.0f, health - damage);
        if (health <= 0.0f)
        {
            TriggerDeath();
        }
    }

    public void Heal(float amount)
    {
        health = Mathf.Min(maxHealth, health + amount);
    }

    public void TriggerDeath()
    {
        this.GetParent().QueueFree();
    }
}
