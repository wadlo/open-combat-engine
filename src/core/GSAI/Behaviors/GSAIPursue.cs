using Godot;
using System;

namespace GodotSteeringAI
{
    /// <summary>
    /// Calculates an acceleration to make an agent intercept another based on the
    /// target agent's movement.
    /// @category - Individual behaviors
    /// </summary>
    class GSAIPursue : GSAISteeringBehavior
    {

        /// <summary>
        /// The target agent that the behavior is trying to intercept.
        /// </summary>
        public GSAISteeringAgent Target { get; set; }

        /// <summary>
        /// The maximum amount of time in the future the behavior predicts the target's location.
        /// </summary>
        public float PredictTimeMax { get; set; }


        public GSAIPursue(GSAISteeringAgent agent, GSAISteeringAgent target, float predict_time_max = 1)
            :base(agent)
        {
            Target = target;
            PredictTimeMax = predict_time_max;
        }


        protected override void _CalculateSteering(GSAITargetAcceleration acceleration)
        {
            var target_position = Target.Position;
            var distance_squared = (target_position - Agent.Position).LengthSquared();

            var speed_squared = Agent.LinearVelocity.LengthSquared();
            var predict_time = PredictTimeMax;

            if (speed_squared > 0)
            {
                var predict_time_squared = distance_squared / speed_squared;
                if (predict_time_squared < PredictTimeMax * PredictTimeMax)
                    predict_time = Mathf.Sqrt(predict_time_squared);
            }

            acceleration.Linear = (target_position + (Target.LinearVelocity * predict_time) - Agent.Position).Normalized();
            acceleration.Linear *= _GetModifiedAcceleration();

            acceleration.Angular = 0; // There may be a problem here.
        }

        protected virtual float _GetModifiedAcceleration()
        {
            return Agent.LinearAccelerationMax;
        }
    }
}
