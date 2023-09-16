using Godot;
using System;

namespace GodotSteeringAI
{

    /// <summary>
    /// A base class for a specialized steering agent that updates itself every frame
    /// so the user does not have to. All other specialized agents derive from this.
    /// @category - Specialized agents
    /// @tags - abstract
    /// </summary>
    public abstract partial class GSAISpecializedAgent : GSAISteeringAgent
    {
        /// <summary>
        /// If `true`, calculates linear and angular velocities based on the previous
        /// frame. When `false`, the user must keep those values updated.
        /// </summary>
        public bool CalculateVelocities { get; set; } = true;

        /// <summary>
        /// If `true`, interpolates the current linear velocity towards 0 by the `linear_drag_percentage` value.
        /// Does not apply to `RigidBody` and `RigidBody2D` nodes.
        /// </summary>
        public bool ApplyLinearDrag { get; set; } = true;

        /// <summary>
        /// If `true`, interpolates the current angular velocity towards 0 by the `angular_drag_percentage` value.
        /// Does not apply to `RigidBody` and `RigidBody2D` nodes.
        /// </summary>
        public bool ApplyAngularDrag { get; set; } = true;

        /// <summary>
        /// The percentage between the current linear velocity and 0 to interpolate by if `apply_linear_drag` is true.
        /// Does not apply to `RigidBody` and `RigidBody2D` nodes.
        /// </summary>
        public float LinearDragPercentage { get; set; } = 0;

        /// <summary>
        /// The percentage between the current angular velocity and 0 to interpolate by if `apply_angular_drag` is true.
        /// Does not apply to `RigidBody` and `RigidBody2D` nodes.
        /// </summary>
        public float AngularDragPercentage { get; set; } = 0;

        protected float _last_orientation;
        protected bool _applied_steering = false;

        /// <summary>
        /// Moves the agent's body by target `acceleration`.
        /// @tags - virtual
        /// </summary>
        /// <param name="acceleration"></param>
        /// <param name="delta"></param>
        public virtual void _ApplySteering(GSAITargetAcceleration acceleration, float delta)
        {

        }
    }
}
