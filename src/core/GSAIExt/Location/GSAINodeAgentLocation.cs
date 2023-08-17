using Godot;
using GodotSteeringAI;
using System;

namespace GodotSteeringAI
{
    /**
    * Automatically updates the agent's position every physics frame
    */
    public partial class GSAINodeAgentLocation : GSAIAgentLocation
    {
        private WeakRef bodyRef;

        private Node2D GetBody()
        {
            return bodyRef.GetRef().As<Node2D>();
        }

        public GSAINodeAgentLocation(Node2D body)
        {
            bodyRef = WeakRef(body);
            if (!body.IsInsideTree())
            {
                body.Ready += OnReady;
                return;
            }
            else
            {
                OnReady();
            }
        }

        private void OnReady()
        {
            _UpdateLocation();
            GetBody().GetTree().PhysicsFrame += _UpdateLocation;
        }

        private void _UpdateLocation()
        {
            Position = GSAIUtils.ToVector3(GetBody().Position);
        }
    }
}
