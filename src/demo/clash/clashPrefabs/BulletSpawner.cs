using Godot;
using GodotSteeringAI;
using System;

public partial class BulletSpawner : Node2D
{
    [Signal]
    public delegate void BulletSpawnEventHandler(Vector2 directionVector);

    [Export]
    public PackedScene bulletPrefab;

    [Export]
    public Target target;

    private Usable ability;

    public override void _Ready()
    {
        ability = OpenTDE.Utils.GetSiblingOfType<Usable>(this);

        ability.OnFire += () =>
        {
            float angle = (
                GSAIUtils.ToVector2(target.targetLocation.Position)
                - GetParent<Node2D>().GlobalPosition
            ).Angle();
            FireBullet(angle);
        };
    }

    public void FireBullet(float angle)
    {
        KinematicArea2D instantiated = bulletPrefab.Instantiate<KinematicArea2D>();
        GetTree().Root.AddChild(instantiated);

        instantiated.GlobalPosition = GlobalPosition;
        instantiated.LinearVelocity = instantiated.LinearVelocity.Rotated(angle);

        EmitSignal(SignalName.BulletSpawn, instantiated.LinearVelocity.Rotated(Mathf.Pi));

        if (target != null)
        {
            OpenTDE.Utils.GetChildrenOfType<Target>(instantiated)[0].targetGroups.AddRange(
                target.targetGroups
            );
        }
    }
}
