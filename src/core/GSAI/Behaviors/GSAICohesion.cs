using Godot;
using System;

namespace GodotSteeringAI
{
    /// <summary>
    /// Calculates an acceleration that attempts to move the agent towards the center
    /// of mass of the agents in the area defined by the `GSAIProximity`.
    /// @category - Group behaviors
    /// </summary>
    class GSAICohesion : GSAIGroupBehavior
    {
        private Vector3 _center_of_mass;

        public GSAICohesion(GSAISteeringAgent agent, GSAIProximity proximity)
            : base(agent, proximity) { }

        protected override void _CalculateSteering(GSAITargetAcceleration acceleration)
        {
            acceleration.SetZero();
            _center_of_mass = Vector3.Zero;

            var neighbor_count = Proximity._FindNeighbors(_callback);

            if (neighbor_count > 0)
            {
                _center_of_mass *= 1.0f / neighbor_count;
                acceleration.Linear = (_center_of_mass - Agent.Position).Normalized() * Agent.LinearAccelerationMax;
            }
        }

        /// <summary>
        /// Callback for the proximity to call when finding neighbors. Adds `neighbor`'s position
        /// to the center of mass of the group.
        /// @tags - virtual
        /// </summary>
        /// <param name="neighbor"></param>
        /// <returns></returns>
        protected override bool _ReportNeighbor(GSAISteeringAgent neighbor)
        {
            _center_of_mass += neighbor.Position;
            return true;
        }
    }
}
