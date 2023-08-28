using Godot;
using GodotSteeringAI;
using System;

public partial class QuickPlayerMovement : CharacterBody2D, GSAITargetable
{
	[Export]
	public float maxSpeed = 200.0f;

	[Export]
	public float userAcceleration = 400.0f;

	[Export]
	public float maxAcceleration = 250.0f;

	[Export]
	public float friction = 175.0f;

	[Export]
	public Usable weapon;

	public GSAIAgentLocation gSAIAgentLocation;
	private GSAIApplyForce knockback;
	private GSAITargetAcceleration knockbackAcceleration = new GSAITargetAcceleration();
	private GSAISteeringAgent steeringAgent;
	private Vector2 knockbackVelocity = Vector2.Zero;

	public override void _Ready()
	{
		gSAIAgentLocation = new GSAINodeAgentLocation(this);
		var knockback = OpenTDE.Utils.GetChildrenOfType<Knockback>(this);
		if (knockback.Count > 0)
		{
			UsableKnockback.ApplyKnockbackAfterUse(weapon, knockback[0]);
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		Velocity = Velocity.Lerp(Vector2.Zero, friction * (float)delta);

		Vector2 userInputAcceleration =
			(float)delta
			* this.userAcceleration
			* new Vector2(
				Input.GetActionStrength("ui_right") - Input.GetActionStrength("ui_left"),
				Input.GetActionStrength("ui_down") - Input.GetActionStrength("ui_up")
			).LimitLength(1.0f);

		Vector2 acceleration = userInputAcceleration;
		Velocity += acceleration;
		Velocity = Velocity.LimitLength(maxSpeed);
		MoveAndSlide();
	}

	public GSAIAgentLocation GetAgentLocation()
	{
		return this.gSAIAgentLocation;
	}
}
