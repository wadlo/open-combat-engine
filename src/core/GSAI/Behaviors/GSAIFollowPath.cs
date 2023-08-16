using Godot;
using System;

namespace GodotSteeringAI
{

    /// <summary>
    /// Produces a linear acceleration that moves the agent along the specified path.
    /// @category - Individual behaviors
    /// </summary>
    class GSAIFollowPath : GSAIArrive
    {
        /// <summary>
        /// The path to follow and travel along.
        /// </summary>
        public GSAIPath Path3D { get; set; }

        /// <summary>
        /// The distance along the path to generate the next target position.
        /// </summary>
        public float PathOffset { get; set; } = 0;

        /// <summary>
        /// Whether to use `GSAIArrive` behavior on an open path.
        /// </summary>
        public bool IsArriveEnabled { get; set; } = true;

        /// <summary>
        /// The amount of time in the future to predict the owning agent's position along
        /// the path. Setting it to 0.0 will force non-predictive path following.
        /// </summary>
        public float PredictionTime { get; set; } = 0;


        public GSAIFollowPath(GSAISteeringAgent agent, GSAIPath path, float path_offset = 0, float prediction_time = 0)
            :base(agent, null)
        {
            Path3D = path;
            PathOffset = path_offset;
            PredictionTime = prediction_time;
        }

        protected override void _CalculateSteering(GSAITargetAcceleration acceleration)
        {
            var location = PredictionTime == 0 ?
                Agent.Position :
                Agent.Position + (Agent.LinearVelocity * PredictionTime);

            var distance = Path3D.CalculateDistance(location);
            var target_distance = distance + PathOffset;

            if (PredictionTime > 0 && Path3D.IsOpen)
            {
                if (target_distance < Path3D.CalculateDistance(Agent.Position))
                {
                    target_distance = Path3D.Length;
                }
            }

            var target_position = Path3D.CalculateTargetPosition(target_distance);
            if (IsArriveEnabled && Path3D.IsOpen)
            {
                if (PathOffset >= 0)
                {
                    if (target_distance > Path3D.Length - DecelerationRadius)
                    {
                        _Arrive(acceleration, target_position);
                        return;
                    }
                }
                else if (target_distance < DecelerationRadius)
                {
                    _Arrive(acceleration, target_position);
                    return;
                }
            }

            acceleration.Linear = (target_position - Agent.Position).Normalized();
            acceleration.Linear *= Agent.LinearAccelerationMax;
            acceleration.Angular = 0; // There may be a problem here.
        }
    }
}
