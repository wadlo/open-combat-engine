using Godot;
using System;

public partial class ClashButtons : VBoxContainer
{
    [Export]
    public ClashSpawnPoint spawnPoint;

    public override void _Ready()
    {
        Button spawnArcher = GetChild<Button>(0);
        spawnArcher.Pressed += () =>
        {
            spawnPoint.SpawnArcher();
        };

        Button spawnSwordsman = GetChild<Button>(1);
        spawnSwordsman.Pressed += () =>
        {
            spawnPoint.SpawnSwordsman();
        };
    }
}
