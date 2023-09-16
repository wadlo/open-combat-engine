using Godot;
using GodotSteeringAI;
using System;

namespace GodotSteeringAI
{
    /**
    * Automatically updates the agent's position every physics frame
    */
    public partial class GSAIDistanceFromLocation : GSAIAgentLocation
    {
        private WeakRef thisNodeRef;
        public float preferredRadius = 100.0f;
        private GSAIAgentLocation targetsLocation;

        private Node2D GetBody()
        {
            return thisNodeRef.GetRef().As<Node2D>();
        }

        public GSAIDistanceFromLocation(
            Node2D thisNode,
            GSAIAgentLocation targetsLocation,
            float _preferredRadius
        )
        {
            preferredRadius = _preferredRadius;
            thisNodeRef = WeakRef(thisNode);
            this.targetsLocation = targetsLocation;
            if (!thisNode.IsInsideTree())
            {
                thisNode.Ready += OnReady;
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
            Vector3 diff = (
                GSAIUtils.ToVector3(GetBody()?.Position ?? Vector2.Zero) - targetsLocation.Position
            ).Normalized();
            Position = targetsLocation.Position + diff * preferredRadius;
        }
    }
}
