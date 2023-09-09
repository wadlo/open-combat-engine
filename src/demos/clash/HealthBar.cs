using Godot;
using System;

public partial class HealthBar : Node2D
{
    [Export]
    Health health;

    [Export]
    Node2D healthSprite;

    private float startingHealth;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        startingHealth = health.health;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        if (health.health < startingHealth)
        {
            Visible = true;
            healthSprite.Scale = new Vector2(health.health / startingHealth, healthSprite.Scale.Y);
        }
        else
        {
            Visible = false;
        }
    }
}
