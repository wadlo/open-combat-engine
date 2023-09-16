using Godot;
using System;

namespace GodotSteeringAI
{
    /// <summary>
    /// Adds velocity, speed, and size data to `GSAIAgentLocation`.
    /// It is the character's responsibility to keep this information up to date for the steering toolkit to work correctly.
    /// </summary>
    public partial class GSAISteeringAgent : GSAIAgentLocation
    {
        /// <summary>
        /// The amount of velocity to be considered effectively not moving.
        /// </summary>
        public float ZeroLinearSpeedThreshold { get; set; } = 0.01f;

        /// <summary>
        /// The maximum speed at which the agent can move.
        /// </summary>
        public float LinearSpeedMax { get; set; } = 0;

        /// <summary>
        /// The maximum amount of acceleration that any behavior can apply to the agent.
        /// </summary>
        public float LinearAccelerationMax { get; set; } = 0;

        /// <summary>
        /// The maximum amount of angular speed at which the agent can rotate.
        /// </summary>
        public float AngularSpeedMax { get; set; } = 0;

        /// <summary>
        /// The maximum amount of angular acceleration that any behavior can apply to an agent.
        /// </summary>
        public float AngularAccelerationMax { get; set; } = 0;

        /// <summary>
        /// Current velocity of the agent.
        /// </summary>
        public Vector3 LinearVelocity { get; set; } = Vector3.Zero;

        /// <summary>
        /// Current angular velocity of the agent.
        /// </summary>
        public float AngularVelocity { get; set; } = 0;

        /// <summary>
        /// The radius of the sphere that approximates the agent's size in space.
        /// </summary>
        public float BoundingRadius { get; set; } = 0;

        /// <summary>
        /// Used internally by group behaviors and proximities to mark the agent as already considered.
        /// </summary>
        public bool IsTagged { get; set; } = false;
    }
}
