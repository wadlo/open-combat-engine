using Godot;
using System;

namespace GodotSteeringAI
{
    /// <summary>
    /// Calculates acceleration to take an agent to its target's location. The
    /// calculation attempts to arrive with zero remaining velocity.
    /// @category - Individual behaviors
    /// </summary>
    class GSAIArrive : GSAISteeringBehavior
    {
        /// <summary>
        /// Target agent to arrive to.
        /// </summary>
        private GSAIAgentLocation target;

        /// <summary>
        /// Distance from the target for the agent to be considered successfully arrived.
        /// </summary>
        public float ArrivalTolerance { get; set; }

        /// <summary>
        /// Distance from the target for the agent to begin slowing down.
        /// </summary>
        public float DecelerationRadius { get; set; }

        /// <summary>
        /// Represents the time it takes to change acceleration.
        /// </summary>
        public float TimeToReach { get; set; } = 0.1f;


        public GSAIArrive(GSAISteeringAgent agent, GSAIAgentLocation target)
            : base(agent)
        {
            this.target = target;
        }

        protected virtual void _Arrive(GSAITargetAcceleration acceleration, Vector3 target_position)
        {
            var to_target = target_position - Agent.Position;
            var distance = to_target.Length();

            if (distance <= ArrivalTolerance)
                acceleration.SetZero();
            else
            {
                var desired_speed = Agent.LinearSpeedMax;
                if (distance <= DecelerationRadius)
                {
                    desired_speed *= distance / DecelerationRadius;
                }

                var desired_velocity = to_target * desired_speed / distance;
                desired_velocity = (desired_velocity - Agent.LinearVelocity) * 1.0f / TimeToReach;

                acceleration.Linear = GSAIUtils.ClampedV3(desired_velocity, Agent.LinearAccelerationMax);
                acceleration.Angular = 0;
            }
        }

        protected override void _CalculateSteering(GSAITargetAcceleration acceleration)
        {
            _Arrive(acceleration, target.Position);
        }

    }
}
