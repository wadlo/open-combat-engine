using Godot;
using System;

namespace GodotSteeringAI
{
    /// <summary>
    /// Steers the agent to avoid obstacles in its path. Approximates obstacles as spheres.
    /// @category - Group behaviors
    /// </summary>
    class GSAIAvoidCollisions : GSAIGroupBehavior
    {
        private GSAISteeringAgent _first_neighbor;
        private float _shortest_time;
        private float _first_minimum_separation;
        private float _first_distance;
        private Vector3 _first_relative_position;
        private Vector3 _first_relative_velocity;

        public GSAIAvoidCollisions(GSAISteeringAgent agent, GSAIProximity proximity)
            : base(agent, proximity) { }


        protected override void _CalculateSteering(GSAITargetAcceleration acceleration)
        {
            _shortest_time = float.PositiveInfinity;
            _first_neighbor = null;
            _first_minimum_separation = 0;
            _first_distance = 0;

            var neighbor_count = Proximity._FindNeighbors(_callback);

            if (neighbor_count == 0 || _first_neighbor is null)
                acceleration.SetZero();
            else
            {
                if (_first_minimum_separation <= 0 || _first_distance < Agent.BoundingRadius + _first_neighbor.BoundingRadius)
                {
                    acceleration.Linear = _first_neighbor.Position - Agent.Position;
                }
                else
                {
                    acceleration.Linear = _first_relative_position + _first_relative_velocity * _shortest_time;
                }
            }

            acceleration.Linear = acceleration.Linear.Normalized() * -Agent.LinearAccelerationMax;
            acceleration.Angular = 0; // There may be a problem here.
        }

        /// <summary>
        /// Callback for the proximity to call when finding neighbors. Keeps track of every `neighbor`
        /// that was found but only keeps the one the owning agent will most likely collide with.
        /// @tags - virtual
        /// </summary>
        /// <param name="neighbor"></param>
        /// <returns></returns>
        protected override bool _ReportNeighbor(GSAISteeringAgent neighbor)
        {
            var relative_position = neighbor.Position - Agent.Position;
            var relative_velocity = neighbor.LinearVelocity - Agent.LinearVelocity;
            var relative_speed_squared = relative_velocity.LengthSquared();

            if (relative_speed_squared == 0)
                return false;
            else
            {
                var time_to_collision = -relative_position.Dot(relative_velocity) / relative_speed_squared;

                if (time_to_collision <= 0 || time_to_collision >= _shortest_time)
                    return false;

                var distance = relative_position.Length();
                var minimum_separation = distance - Mathf.Sqrt(relative_speed_squared) * time_to_collision;
                
                if (minimum_separation > Agent.BoundingRadius + neighbor.BoundingRadius)
                    return false;

                _shortest_time = time_to_collision;
                _first_neighbor = neighbor;
                _first_minimum_separation = minimum_separation;
                _first_distance = distance;
                _first_relative_position = relative_position;
                _first_relative_velocity = relative_velocity;
                return true;
            }
        }
    }
}
