using Godot;
using System;

namespace GodotSteeringAI
{
    /// <summary>
    /// Calculates an acceleration to take an agent to a target agent's position directly.
    /// @category - Individual behaviors
    /// </summary>
    class GSAISeek : GSAISteeringBehavior
    {
        /// <summary>
        /// The target that the behavior aims to move the agent to.
        /// </summary>
        public GSAIAgentLocation Target { get; set; }


        public GSAISeek(GSAISteeringAgent agent, GSAIAgentLocation target)
            :base(agent)
        {
            Target = target;
        }

        protected override void _CalculateSteering(GSAITargetAcceleration acceleration)
        {
            acceleration.Linear = (Target.Position - Agent.Position).Normalized() * Agent.LinearAccelerationMax;
            acceleration.Angular = 0; // There may be a problem here.
        }
    }
}
