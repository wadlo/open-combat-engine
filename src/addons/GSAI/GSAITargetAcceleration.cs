using Godot;
using System;

namespace GodotSteeringAI
{
    /// <summary>
    /// A desired linear and angular amount of acceleration requested by the steering system.
    /// @category - Base types
    /// </summary>
    public partial class GSAITargetAcceleration
    {
        /// <summary>
        /// Linear acceleration
        /// </summary>
        public Vector3 Linear { get; set; } = Vector3.Zero;

        /// <summary>
        /// Angular acceleration
        /// </summary>
        public float Angular { get; set; } = 0;

        /// <summary>
        /// Sets the linear and angular components to 0.
        /// </summary>
        public void SetZero()
        {
            Linear = Vector3.Zero;
            Angular = 0;
        }

        /// <summary>
        /// Adds `accel`'s components, multiplied by `scalar`, to this one.
        /// </summary>
        /// <param name="accel"></param>
        /// <param name="scalar"></param>
        public void AddScaledAccel(GSAITargetAcceleration accel, float scalar)
        {
            Linear += accel.Linear * scalar;
            Angular += accel.Angular * scalar;
        }

        /// <summary>
        /// Returns the squared magnitude of the linear and angular components.
        /// </summary>
        /// <returns></returns>
        public float GetMagnitudeSquared()
        {
            return Linear.LengthSquared() + Angular * Angular;
        }

        /// <summary>
        /// Returns the magnitude of the linear and angular components.
        /// </summary>
        /// <returns></returns>
        public float GetMagnitude()
        {
            return Mathf.Sqrt(GetMagnitudeSquared());
        }
    }
}
