using Godot;
using System;

namespace GodotSteeringAI
{
    /// <summary>
    /// A specialized steering agent that updates itself every frame so the user does
    /// not have to using a RigidBody2D.
    /// @category - Specialized agents
    /// </summary>
    public partial class GSAIRigidBody2DAgent: GSAISpecializedAgent
    {
        /// <summary>
        /// The RigidBody2D to keep track of
        /// </summary>
        public RigidBody2D Body { get { return _BodyRefToBody(); } set { _SetBody(value); } }

        private WeakRef _body_ref;
        private Vector2 _last_position;

        private RigidBody2D _BodyRefToBody()
        {
            return _body_ref.GetRef().As<RigidBody2D>();
        }

        public GSAIRigidBody2DAgent(RigidBody2D body)
        {
            if (!body.IsInsideTree())
            {
                body.Ready += _OnBody_Ready;
                return;
            }
            _SetBody(body);
            body.GetTree().PhysicsFrame += _onSceneTree_PhysicsFrame;
        }

        private void _OnBody_Ready()
        {
            _SetBody(_BodyRefToBody());
            _BodyRefToBody().GetTree().PhysicsFrame += _onSceneTree_PhysicsFrame;
        }

        /// <summary>
        /// Moves the agent's `body` by target `acceleration`.
        /// @tags - virtual
        /// </summary>
        /// <param name="acceleration"></param>
        /// <param name="delta"></param>
        public override void _ApplySteering(GSAITargetAcceleration acceleration, float delta)
        {
            var body = Body;
            if (body is null)
                return;

            _applied_steering = true;
            body.ApplyCentralImpulse(GSAIUtils.ToVector2(acceleration.Linear));
            body.ApplyTorqueImpulse(acceleration.Angular);
            if (CalculateVelocities)
            {
                LinearVelocity = GSAIUtils.ToVector3(body.LinearVelocity);
                AngularVelocity = body.AngularVelocity;
            }
        }

        private void _SetBody(RigidBody2D body)
        {
            if (body is null)
                return;
            _body_ref = WeakRef(body);
            _last_position = body.GlobalPosition;
            _last_orientation = body.Rotation;

            Position = GSAIUtils.ToVector3(_last_position);
            Orientation = _last_orientation;
        }

        private void _onSceneTree_PhysicsFrame()
        {
            var body = Body;
            if (body is null || !body.IsInsideTree())
                return;

            var current_position = body.GlobalPosition;
            var current_orientation = body.Rotation;

            Position = GSAIUtils.ToVector3(current_position);
            Orientation = current_orientation;

            if (CalculateVelocities)
            {
                if (_applied_steering)
                    _applied_steering = false;
                else
                {
                    LinearVelocity = GSAIUtils.ToVector3(body.LinearVelocity);
                    AngularVelocity = body.AngularVelocity;
                }
            }
        }
    }
}
