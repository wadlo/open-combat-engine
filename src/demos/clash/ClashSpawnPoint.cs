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
    public PackedScene swordsmanPrefab;

    public float spawnRadius = 2000.0f;

    [Export]
    public Color unitColor;

    public void SpawnArcher()
    {
        CharacterBody2D instantiated = archerPrefab.Instantiate<CharacterBody2D>();
        GetTree().Root.AddChild(instantiated);

        RandomlyPositionUnit(instantiated);
    }

    public void SpawnSwordsman()
    {
        CharacterBody2D instantiated = swordsmanPrefab.Instantiate<CharacterBody2D>();
        GetTree().Root.AddChild(instantiated);

        RandomlyPositionUnit(instantiated);
    }

    public void RandomlyPositionUnit(CharacterBody2D instantiated)
    {
        int maxIterations = 500;
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
