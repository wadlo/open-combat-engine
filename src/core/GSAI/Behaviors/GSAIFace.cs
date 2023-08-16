using Godot;
using System;

namespace GodotSteeringAI
{
    /// <summary>
    /// Calculates angular acceleration to rotate a target to face its target's
    /// position. The behavior attemps to arrive with zero remaining angular velocity.
    /// @category - Individual behaviors
    /// </summary>
    class GSAIFace : GSAIMatchOrientation
    {
        public GSAIFace(GSAISteeringAgent agent, GSAIAgentLocation target, bool use_z = false)
            : base(agent, target, use_z) { }


        protected virtual void _Face(GSAITargetAcceleration acceleration, Vector3 target_position)
        {
            var to_target = target_position - Agent.Position;
            var distance_squared = to_target.LengthSquared();

            if (distance_squared < Agent.ZeroLinearSpeedThreshold)
            {
                acceleration.SetZero();
            }
            else
            {
                var orientation = UseZ ?
                    GSAIUtils.Vector3ToAngle(to_target) :
                    GSAIUtils.Vector2ToAngle(GSAIUtils.ToVector2(to_target));
                
                _MatchOrientation(acceleration, orientation);
            }
        }

        protected override void _CalculateSteering(GSAITargetAcceleration acceleration)
        {
            _Face(acceleration, Target.Position);
        }
    }
}
