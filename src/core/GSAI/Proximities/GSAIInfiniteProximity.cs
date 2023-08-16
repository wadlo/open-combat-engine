using Godot;
using System;
using System.Collections.Generic;

namespace GodotSteeringAI
{
    /// <summary>
    /// Determines any agent that is in the specified list as being neighbors with the
    /// owner agent, regardless of distance.
    /// @category - Proximities
    /// </summary>
    public partial class GSAIInfiniteProximity : GSAIProximity
    {
        public GSAIInfiniteProximity(GSAISteeringAgent agent, List<GSAISteeringAgent> agents)
            : base(agent, agents) { }


        /// <summary>
        /// Returns a number of neighbors based on a `callback` function.
        /// `_FindNeighbors` calls `callback` for each agent in the `agents` array and
        /// adds one to the count if its `callback` returns true.
        /// </summary>
        /// <param name="callback"></param>
        /// <returns></returns>
        public override int _FindNeighbors(AgentCallback callback)
        {
            var neighbor_count = 0;
            foreach (var cur_agent in Agents)
            {
                if (cur_agent != Agent)
                {
                    if (callback(cur_agent))
                        neighbor_count++;
                }
            }
            return neighbor_count;
        }
    }
}
