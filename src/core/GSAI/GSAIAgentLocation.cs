using Godot;
using System;

namespace GodotSteeringAI
{
    /// <summary>
    /// Represents an agent with only a location and an orientation.
    /// </summary>
    public partial class GSAIAgentLocation : GodotObject
    {
        /// <summary>
        /// The agent's position in space.
        /// </summary>
        public Vector3 Position { get; set; } = Vector3.Zero;

        /// <summary>
        /// The agent's orientation on its Y axis rotation.
        /// </summary>
        public float Orientation { get; set; } = 0;
    }
}
