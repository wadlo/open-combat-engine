using Godot;
using System;

public partial class ClashSpawnPoint : Node2D
{
    [Export]
    public string group;

    [Export]
    public string attackGroup;

    [Export]
    public PackedScene archerPrefab;

    [Export]
    public float spawnRadius = 200.0f;

    [Export]
    public Color unitColor;

    public void SpawnArcher()
    {
        CharacterBody2D instantiated = archerPrefab.Instantiate<CharacterBody2D>();
        // Add random because physics won't separate objects with the exact same position.
        GetTree().Root.AddChild(instantiated);

        int maxIterations = 100;
        for (int i = 0; i < maxIterations; i++)
        {
            instantiated.GlobalPosition =
                GlobalPosition
                + spawnRadius * (1.0f * i / maxIterations) * new Vector2(GD.Randf(), GD.Randf());
            if (instantiated.MoveAndCollide(Vector2.Zero, true) == null)
            {
                break;
            }
        }

        instantiated.GetChild<Node2D>(0).Modulate = unitColor;
        instantiated.AddToGroup(group);
        OpenTDE.Utils.GetChildrenOfType<Target>(instantiated)[0].targetGroups.Add(attackGroup);
    }
}
