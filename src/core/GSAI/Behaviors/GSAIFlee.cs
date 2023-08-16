using Godot;
using System;

namespace GodotSteeringAI
{
    /// <summary>
    /// Calculates acceleration to take an agent directly away from a target agent.
    /// @category - Individual behaviors
    /// </summary>
    class GSAIFlee : GSAISeek
    {
        public GSAIFlee(GSAISteeringAgent agent, GSAIAgentLocation target)
            : base(agent, target) { }

        protected override void _CalculateSteering(GSAITargetAcceleration acceleration)
        {
            acceleration.Linear = (Agent.Position - Target.Position).Normalized() * Agent.LinearAccelerationMax;
            acceleration.Angular = 0;
        }
    }
}
