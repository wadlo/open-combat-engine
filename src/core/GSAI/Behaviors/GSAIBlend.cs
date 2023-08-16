using Godot;
using System.Collections.Generic;

namespace GodotSteeringAI
{
    /// <summary>
    /// Blends multiple steering behaviors into one, and returns a weighted
    /// acceleration from their calculations.
    /// Stores the behaviors internally as dictionaries of the form
    /// {
    ///     behavior : GSAISteeringBehavior,
    ///     weight : float
    /// }
    /// @category - Combination behaviors
    /// </summary>
    class GSAIBlend : GSAISteeringBehavior
    {
        public partial class SteeringBehaviorPair
        {
            public GSAISteeringBehavior behavior;
            public float weight;

            public SteeringBehaviorPair(GSAISteeringBehavior behavior, float weight)
            {
                this.behavior = behavior;
                this.weight = weight;
            }
        }


        private List<SteeringBehaviorPair> _behaviors = new List<SteeringBehaviorPair>();
        private GSAITargetAcceleration _accel = new GSAITargetAcceleration();

        public GSAIBlend(GSAISteeringAgent agent) : base(agent) { }

        /// <summary>
        /// Appends a behavior to the internal array along with its `weight`.
        /// </summary>
        /// <param name="behavior"></param>
        /// <param name="weight"></param>
        public void Add (GSAISteeringBehavior behavior, float weight)
        {
            behavior.Agent = Agent;
            _behaviors.Add(new SteeringBehaviorPair(behavior, weight));
        }

        /// <summary>
        /// Returns the behavior at the specified `index`, or a `null` ifnone was found.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public SteeringBehaviorPair GetBehaviorAt(int index)
        {
            if (index < _behaviors.Count)
                return _behaviors[index];
            GD.PrintErr("Tried to get index " + index + " in array of size " + _behaviors.Count);
            return null;
        }

        protected override void _CalculateSteering(GSAITargetAcceleration blended_accel)
        {
            blended_accel.SetZero();
            foreach (var bw in _behaviors) 
            {
                bw.behavior.CalculateSteering(_accel);
                blended_accel.AddScaledAccel(_accel, bw.weight);
            }
            blended_accel.Linear = GSAIUtils.ClampedV3(blended_accel.Linear, Agent.LinearAccelerationMax);
            blended_accel.Angular = Mathf.Clamp(blended_accel.Angular, -Agent.AngularAccelerationMax, Agent.AngularAccelerationMax);

        }
    }
}
