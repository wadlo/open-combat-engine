using Godot;
using System;

public partial class AbilityConfig : Resource
{
    [Export]
    public float maxUses = 10.0f;

    // State times
    [Export]
    public float recoilTime = 0.4f;

    [Export]
    public float reloadTimePerUnit = 0.8f;

    [Export]
    public float fireDuration = 0.0f;

    [Export]
    public float chargeDuration = 0.0f;

    [Export]
    public bool autoReload = false;
}
