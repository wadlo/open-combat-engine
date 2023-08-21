using Godot;
using System;

public partial class ClashSpawnPoint : Node2D
{
    [Export]
    public PackedScene archerPrefab;

    [Export]
    public float spawnRadius = 200.0f;

    public void SpawnArcher()
    {
        CharacterBody2D instantiated = archerPrefab.Instantiate<CharacterBody2D>();
        GetTree().Root.AddChild(instantiated);
        // Add random because physics won't separate objects with the exact same position.

        int maxIterations = 100;
        for (int i = 0; i < maxIterations; i++)
        {
            instantiated.GlobalPosition =
                GlobalPosition + new Vector2(spawnRadius * GD.Randf(), spawnRadius * GD.Randf());
            if (instantiated.MoveAndCollide(Vector2.Zero, true) == null)
            {
                break;
            }
        }
    }
}
