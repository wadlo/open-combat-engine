using Godot;
using System;

namespace GodotSteeringAI
{
    /// <summary>
    /// Calculates an angular acceleration to match an agent's orientation to its direction of travel.
    /// @category - Individual behaviors
    /// </summary>
    class GSAILookWhereYouGo : GSAIMatchOrientation
    {
        public GSAILookWhereYouGo(GSAISteeringAgent agent, bool use_z = false)
            : base(agent, null, use_z) { }


        protected override void _CalculateSteering(GSAITargetAcceleration accel)
        {
            if (Agent.LinearVelocity.LengthSquared() < Agent.ZeroLinearSpeedThreshold)
                accel.SetZero();
            else
            {
                var orientation = UseZ ?
                    GSAIUtils.Vector3ToAngle(Agent.LinearVelocity) :
                    GSAIUtils.Vector2ToAngle(GSAIUtils.ToVector2(Agent.LinearVelocity));
                _MatchOrientation(accel, orientation);
            }
        }
    }
}
