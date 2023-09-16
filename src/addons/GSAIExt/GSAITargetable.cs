using Godot;
using GodotSteeringAI;
using System;

namespace GodotSteeringAI
{
    public interface GSAITargetable
    {
        public abstract GSAIAgentLocation GetAgentLocation();
    }
}
