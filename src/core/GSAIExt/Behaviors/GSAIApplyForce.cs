using Godot;
using System;

namespace GodotSteeringAI
{
    /// <summary>
    /// Applies a force that decays over time
    /// This will add a listener to the physics frame of the node's attached tree.
    /// </summary>
    public partial class GSAIApplyForce : GSAISteeringBehavior
    {
        Vector3 force;
        private float strength;
        private float duration;
        private WeakRef nodeRef;

        public GSAIApplyForce(
            Node nodeToAttachTo,
            GSAISteeringAgent agent,
            Vector3 initialForce,
            float duration = 1.0f
        )
            : base(agent)
        {
            nodeRef = WeakRef(nodeToAttachTo);
            ApplyForce(initialForce);
            nodeToAttachTo.GetTree().PhysicsFrame += ApplyDecay;
            this.duration = duration;
        }

        protected override void _CalculateSteering(GSAITargetAcceleration acceleration)
        {
            acceleration.Linear = strength * force;
        }

        private void ApplyDecay()
        {
            float delta = (float)nodeRef.GetRef().As<Node>().GetPhysicsProcessDeltaTime();
            // TODO fix this to be exponential decay.
            strength = Mathf.Max(strength - delta / duration, 0.0f);
        }

        public void ApplyForce(Vector3 newForce)
        {
            force = (strength * force + newForce) / 2.0f;
            strength = 1.0f;
        }
    }
}
