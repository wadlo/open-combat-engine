using Godot;
using System;

namespace GodotSteeringAI
{
    /// <summary>
    /// Calculates an angular acceleration to match an agent's orientation to that of
    /// its target. Attempts to make the agent arrive with zero remaining angular velocity.
    /// @category - Individual behaviors
    /// </summary>
    class GSAIMatchOrientation : GSAISteeringBehavior
    {
        /// <summary>
        /// The target orientation for the behavior to try and match rotations to.
        /// </summary>
        public GSAIAgentLocation Target { get; set; }

        /// <summary>
        /// The amount of distance in radians for the behavior to consider itself close
        /// enough to be matching the target agent's rotation.
        /// </summary>
        public float AlignmentTolerance { get; set; }

        /// <summary>
        /// The amount of distance in radians from the goal to start slowing down.
        /// </summary>
        public float DecelerationRadius { get; set; }

        /// <summary>
        /// The amount of time to reach the target velocity
        /// </summary>
        public float TimeToReach { get; set; } = 0.1f;

        /// <summary>
        /// Whether to use the X and Z components instead of X and Y components when
        /// determining angles. X and Z should be used in 3D.
        /// </summary>
        public bool UseZ { get; set; }


        public GSAIMatchOrientation(GSAISteeringAgent agent, GSAIAgentLocation target, bool use_z = false)
            : base(agent)
        {
            UseZ = use_z;
            Target = target;
        }


        protected virtual void _MatchOrientation(GSAITargetAcceleration acceleration, float desired_orientation)
        {
            var rotation = Mathf.Wrap(desired_orientation - Agent.Orientation, -Mathf.Pi, Mathf.Pi);

            var rotation_size = Mathf.Abs(rotation);

            if (rotation_size <= AlignmentTolerance)
                acceleration.SetZero();
            else
            {
                var desired_rotation = Agent.AngularSpeedMax;
                if (rotation_size <= DecelerationRadius)
                    desired_rotation *= rotation_size / DecelerationRadius;

                desired_rotation *= rotation / rotation_size;

                acceleration.Angular = (desired_rotation - Agent.AngularVelocity) / TimeToReach;

                var limited_acceleration = Mathf.Abs(acceleration.Angular);
                if (limited_acceleration > Agent.AngularAccelerationMax)
                    acceleration.Angular *= Agent.AngularAccelerationMax / limited_acceleration;
            }

            acceleration.Linear = Vector3.Zero; // There may be a problem here.
        }

        protected override void _CalculateSteering(GSAITargetAcceleration acceleration)
        {
            _MatchOrientation(acceleration, Target.Orientation);
        }
    }
}
