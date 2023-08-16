using Godot;
using System;


namespace GodotSteeringAI
{
    /// <summary>
    /// Calculates an acceleration that repels the agent from its neighbors in the given `GSAIProximity`.
    /// The acceleration is an average based on all neighbors, multiplied by a strength decreasing by the inverse square law in relation to distance, and it accumulates.
    /// @category - Group behaviors
    /// </summary>
    class GSAISeparation : GSAIGroupBehavior
    {
        /// <summary>
        /// The coefficient to calculate how fast the separation strength decays with distance.
        /// </summary>
        public float DecayCoefficient { get; set; } = 1;


        private GSAITargetAcceleration _acceleration;

        public GSAISeparation(GSAISteeringAgent agent, GSAIProximity proximity)
            : base(agent, proximity) { }


        protected override void _CalculateSteering(GSAITargetAcceleration acceleration)
        {
            acceleration.SetZero();
            _acceleration = acceleration;

            Proximity._FindNeighbors(_callback);
        }

        /// <summary>
        /// Callback for the proximity to call when finding neighbors. Determines the amount of
        /// acceleration that `neighbor` imposes based on its distance from the owner agent.
        /// @tags - virtual
        /// </summary>
        /// <param name="_neighbor"></param>
        /// <returns></returns>
        protected override bool _ReportNeighbor(GSAISteeringAgent neighbor)
        {
            var to_agent = Agent.Position - neighbor.Position;

            var distance_squared = to_agent.LengthSquared();
            var acceleration_max = Agent.LinearAccelerationMax;

            var strength = DecayCoefficient / distance_squared;
            if (strength > acceleration_max)
                strength = acceleration_max;

            _acceleration.Linear += to_agent * (strength / Mathf.Sqrt(distance_squared));

            return true;
        }
    }
}
