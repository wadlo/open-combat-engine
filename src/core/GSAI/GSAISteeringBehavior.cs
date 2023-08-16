using Godot;
using System;

namespace GodotSteeringAI
{
    /// <summary>
    /// Base class for all steering behaviors.
    /// 
    /// Steering behaviors calculate the linear and the angular acceleration to be to the agent that owns them.
    /// 
    /// The `CalculateSteering` function is the entry point for all behaviors.
    /// 
    /// Individual steering behaviors encapsulate the steering logic.
    /// </summary>
    abstract class GSAISteeringBehavior
    {
        /// <summary>
        /// If `false`, all calculations return zero amounts of acceleration.
        /// </summary>
        public bool IsEnabled { get; set; } = true;

        /// <summary>
        /// The AI agent on which the steering behavior bases its calculations.
        /// </summary>
        public GSAISteeringAgent Agent { get; set; }


        public GSAISteeringBehavior(GSAISteeringAgent agent) {
            Agent = agent;
        }

        /// <summary>
        /// Sets the `acceleration` with the behavior's desired amount of acceleration.
        /// </summary>
        /// <param name="acceleration"></param>
        public void CalculateSteering(GSAITargetAcceleration acceleration)
        {
            if (IsEnabled)
                _CalculateSteering(acceleration);
            else
                acceleration.SetZero();
        }


        protected virtual void _CalculateSteering(GSAITargetAcceleration acceleration)
        {
            acceleration.SetZero();
        }
    }
}
