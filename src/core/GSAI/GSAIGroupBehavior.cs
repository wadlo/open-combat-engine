using Godot;
using System;

namespace GodotSteeringAI
{
    /// <summary>
    /// Base type for group-based steering behaviors.
    /// @category - Base types
    /// </summary>
    abstract class GSAIGroupBehavior : GSAISteeringBehavior
    {
        /// <summary>
        /// Container to find neighbors of the agent and calculate group behavior.
        /// </summary>
        public GSAIProximity Proximity { get; private set; }

        protected GSAIProximity.AgentCallback _callback;

        public GSAIGroupBehavior(GSAISteeringAgent agent, GSAIProximity proximity)
            : base(agent)
        {
            Proximity = proximity;
            _callback = _ReportNeighbor;
        }

        /// <summary>
        /// Internal callback for the behavior to define whether or not a member is relevant.
        /// @tags - virtual
        /// </summary>
        /// <param name="_neighbor"></param>
        /// <returns></returns>
        protected virtual bool _ReportNeighbor(GSAISteeringAgent neighbor)
        {
            return false;
        }
    }
}
