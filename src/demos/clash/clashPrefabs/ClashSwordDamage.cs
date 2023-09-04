using Godot;
using OpenTDE;
using System;

public partial class ClashSwordDamage : Node2D
{
    [Export]
    public float range = 110.0f;

    [Export]
    public float damage = 10.0f;

    [Export]
    public float knockback = 300.0f;

    [Export]
    public Target target;

    [Export]
    public Usable weapon;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        weapon.OnFire += (float _direction) =>
        {
            OnAttack();
        };
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta) { }

    private void OnAttack()
    {
        if (
            (
                GodotSteeringAI.GSAIUtils.ToVector2(target.targetLocation.Position) - GlobalPosition
            ).LengthSquared() <= Mathf.Pow(range, 2.0f)
        )
        {
            DamageTarget();
            KnockbackTarget();
        }
    }

    private void DamageTarget()
    {
        var targetObject = target.GetTargetObject();
        if (targetObject != null)
        {
            Utils.GetChildOfType<Health>(targetObject)?.Damage(damage);
        }
    }

    private void KnockbackTarget()
    {
        var targetObject = target.GetTargetObject();
        if (targetObject != null)
        {
            Utils
                .GetChildOfType<Knockback>(targetObject)
                ?.ApplyForce(
                    (
                        knockback
                        * (
                            target.targetLocation.Position
                            - GodotSteeringAI.GSAIUtils.ToVector3(GlobalPosition)
                        ).Normalized()
                    )
                );
        }
    }
}
