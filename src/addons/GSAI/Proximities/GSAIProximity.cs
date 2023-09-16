using Godot;
using System;
using System.Collections.Generic;

namespace GodotSteeringAI
{
    /// <summary>
    /// Base container type that stores data to find the neighbors of an agent.
    /// @category - Proximities
    /// @tags - abstract
    /// </summary>
    public abstract partial class GSAIProximity : RefCounted
    {
        /// <summary>
        /// The owning agent whose neighbors are found in the group
        /// </summary>
        public GSAISteeringAgent Agent { get; private set; }

        /// <summary>
        /// The agents who are part of this group and could be potential neighbors
        /// </summary>
        public List<GSAISteeringAgent> Agents { get; set; }


        public GSAIProximity(GSAISteeringAgent agent, List<GSAISteeringAgent> agents)
        {
            Agent = agent; Agents = agents;
        }

        public delegate bool AgentCallback(GSAISteeringAgent agent);

        /// <summary>
        /// Returns a number of neighbors based on a `callback` function.
        /// 
        /// `_FindNeighbors` calls `callback` for each agent in the `Agents` array and
        /// adds one to the count if its `callback` returns true.
        /// </summary>
        /// <param name="callback"></param>
        /// <returns>adds one to the count if its `callback` returns true.</returns>
        public virtual int _FindNeighbors(AgentCallback callback)
        {
            return 0;
        }
    }
}
