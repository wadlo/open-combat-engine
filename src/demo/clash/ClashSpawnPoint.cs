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
    public Texture2D archerSprite;

    [Export]
    public PackedScene swordsmanPrefab;

    [Export]
    public Texture2D swordsmanSprite;

    public float spawnRadius = 2000.0f;

    [Export]
    public Color unitColor;

    public override void _Ready()
    {
        // To make each run behave exactly the same
        GD.Seed(100);
        CallDeferred("SpawnArcher");
        CallDeferred("SpawnArcher");
        CallDeferred("SpawnArcher");
        CallDeferred("SpawnSwordsman");
        CallDeferred("SpawnSwordsman");
        CallDeferred("SpawnSwordsman");
        CallDeferred("SpawnArcher");
        CallDeferred("SpawnArcher");
        CallDeferred("SpawnArcher");
        CallDeferred("SpawnSwordsman");
        CallDeferred("SpawnSwordsman");
        CallDeferred("SpawnSwordsman");
        CallDeferred("SpawnArcher");
        CallDeferred("SpawnArcher");
        CallDeferred("SpawnArcher");
        CallDeferred("SpawnSwordsman");
        CallDeferred("SpawnSwordsman");
        CallDeferred("SpawnSwordsman");
    }

    public void SpawnArcher()
    {
        CharacterBody2D instantiated = archerPrefab.Instantiate<CharacterBody2D>();
        GetTree().Root.AddChild(instantiated);

        RandomlyPositionUnit(instantiated);
        ColorUnit(instantiated, archerSprite);
    }

    public void SpawnSwordsman()
    {
        CharacterBody2D instantiated = swordsmanPrefab.Instantiate<CharacterBody2D>();
        GetTree().Root.AddChild(instantiated);

        RandomlyPositionUnit(instantiated);
        ColorUnit(instantiated, swordsmanSprite);
    }

    public void ColorUnit(CharacterBody2D instantiated, Texture2D newImage)
    {
        (instantiated.GetChild<FaceTarget>(0).Material as ShaderMaterial).CallDeferred(
            "set_shader_parameter",
            "color",
            unitColor
        );

        instantiated.GetChild<FaceTarget>(0).GetChild<Sprite2D>(0).Texture = newImage;
    }

    public void RandomlyPositionUnit(CharacterBody2D instantiated)
    {
        int maxIterations = 500;
        for (int i = 100; i < maxIterations; i++)
        {
            instantiated.GlobalPosition =
                GlobalPosition
                + spawnRadius * (1.0f * i / maxIterations) * new Vector2(GD.Randf(), GD.Randf());
            if (instantiated.MoveAndCollide(Vector2.Zero, true) == null)
            {
                break;
            }
        }

        // instantiated.GetChild<Node2D>(0).Modulate = unitColor;
        instantiated.AddToGroup(group);
        OpenTDE.Utils.GetChildrenOfType<Target>(instantiated)[0].targetGroups.Add(attackGroup);
    }
}
