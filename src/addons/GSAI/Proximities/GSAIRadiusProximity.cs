using Godot;
using System;
using System.Collections.Generic;

namespace GodotSteeringAI
{
    /// <summary>
    /// Determines any agent that is in the specified list as being neighbors with the owner agent if
    /// they lie within the specified radius.
    /// @category - Proximities
    /// </summary>
    public partial class GSAIRadiusProximity : GSAIProximity
    {
        /// <summary>
        /// The radius around the owning agent to find neighbors in
        /// </summary>
        public float Radius { get; set; }


        private long _last_frame = 0;
        private SceneTree _scene_tree;

        public GSAIRadiusProximity(GSAISteeringAgent agent, List<GSAISteeringAgent> agents, float radius)
            : base(agent, agents)
        {
            Radius = radius;
            _scene_tree = Engine.GetMainLoop() as SceneTree;
        }

        /// <summary>
        /// Returns a number of neighbors based on a `callback` function.
        /// `_FindNeighbors` calls `callback` for each agent in the `agents` array that lie within
        /// the radius around the owning agent and adds one to the count if its `callback` returns true.
        /// </summary>
        /// <param name="callback"></param>
        /// <returns></returns>
        public override int _FindNeighbors(AgentCallback callback)
        {
            var neighbor_count = 0;
            var current_frame = _scene_tree != null ? _scene_tree.GetFrame() : -_last_frame;
            if (current_frame != _last_frame)
            {
                _last_frame = current_frame;
                var owner_position = Agent.Position;

                foreach (var cur_agent in Agents)
                {
                    if (cur_agent != Agent)
                    {
                        var distance_squared = owner_position.DistanceSquaredTo(cur_agent.Position);
                        var range_to = Radius + cur_agent.BoundingRadius;
                        if (distance_squared < range_to * range_to)
                        {
                            if (callback(cur_agent))
                            {
                                cur_agent.IsTagged = true;
                                neighbor_count++;
                                continue;
                            }
                        }
                    }
                    cur_agent.IsTagged = false;
                }
            }
            else
            {
                foreach (var cur_agent in Agents)
                {
                    if (cur_agent != Agent && cur_agent.IsTagged)
                    {
                        if (callback(cur_agent))
                            neighbor_count++;
                    }
                }
            }

            return neighbor_count;
        }

    }
}
