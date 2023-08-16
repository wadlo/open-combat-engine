using Godot;
using System;

namespace GodotSteeringAI
{
    /// <summary>
    /// Calculates acceleration to take an agent away from where a target agent is moving.
    /// @category - Individual behaviors
    /// </summary>
    class GSAIEvade : GSAIPursue
    {
        public GSAIEvade(GSAISteeringAgent agent, GSAISteeringAgent target, float predict_time_max = 1)
            : base(agent, target, predict_time_max) { }

        protected override float _GetModifiedAcceleration()
        {
            return -Agent.LinearAccelerationMax;
        }
    }
}
