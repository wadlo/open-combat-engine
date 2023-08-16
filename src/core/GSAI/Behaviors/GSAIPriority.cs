using Godot;
using System;
using System.Collections.Generic;

namespace GodotSteeringAI
{
    /// <summary>
    /// Container for multiple behaviors that returns the result of the first child
    /// behavior with non-zero acceleration.
    /// @category - Combination behaviors
    /// </summary>
    class GSAIPriority : GSAISteeringBehavior
    {
        private List<GSAISteeringBehavior> _behaviors = new List<GSAISteeringBehavior>();

        /// <summary>
        /// The index of the last behavior the container prioritized.
        /// </summary>
        public int LastSelectedIndex { get; private set; }

        /// <summary>
        /// If a behavior's acceleration is lower than this threshold, the container
        /// considers it has an acceleration of zero.
        /// </summary>
        public float ZeroThreshold { get; set; }


        public GSAIPriority(GSAISteeringAgent agent, float zero_threshold = 0.001f)
            : base(agent)
        {
            ZeroThreshold = zero_threshold;
        }

        /// <summary>
        /// Appends a steering behavior as a child of this container.
        /// </summary>
        /// <param name="behavior"></param>
        public void Add(GSAISteeringBehavior behavior)
        {
            _behaviors.Add(behavior);
        }

        /// <summary>
        /// Returns the behavior at the position in the pool referred to by `index`, or
        /// `null` if no behavior was found.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public GSAISteeringBehavior GetBehaviorAt(int index)
        {
            if (index < _behaviors.Count)
                return _behaviors[index];
            GD.PrintErr("Tried to get index " + index + " in array of size " + _behaviors.Count);
            return null;
        }


        protected override void _CalculateSteering(GSAITargetAcceleration accel)
        {
            var threshold_squared = ZeroThreshold * ZeroThreshold;

            LastSelectedIndex = -1;

            if (_behaviors.Count > 0)
            {
                int i = 0;
                foreach (var behavior in _behaviors)
                {
                    LastSelectedIndex = i++;
                    behavior.CalculateSteering(accel);

                    if (accel.GetMagnitudeSquared() > threshold_squared)
                        break;
                }
            }
            else
                accel.SetZero();
        }
    }
}
