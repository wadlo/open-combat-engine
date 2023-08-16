using Godot;
using System;


namespace GodotSteeringAI
{
    /// <summary>
    /// A specialized steering agent that updates itself every frame so the user does
    /// not have to using a KinematicBody2D.
    /// @category - Specialized agents
    /// </summary>
    public partial class GSAIKinematicBody2DAgent : GSAISpecializedAgent
    {
        /// <summary>
        /// SLIDE uses `move_and_slide`
        /// COLLIDE uses `move_and_collide`
        /// POSITION changes the `global_position` directly
        /// </summary>
        public enum MovementType
        {
            SLIDE, COLLIDE, POSITION
        }

        /// <summary>
        /// The KinematicBody2D to keep track of
        /// </summary>
        public CharacterBody2D Body { get { return _BodyRefToBody(); } set { _SetBody(value); } }

        /// <summary>
        /// The type of movement the body executes
        /// </summary>
        public MovementType MoveType { get; set; }

        private WeakRef _body_ref;
        private Vector2 _last_position;

        private CharacterBody2D _BodyRefToBody()
        {
            return _body_ref.GetRef().As<CharacterBody2D>();
        }

        public GSAIKinematicBody2DAgent(CharacterBody2D body, MovementType moveType = MovementType.SLIDE)
        {
            _body_ref = WeakRef(body);
            MoveType = moveType;
            if (!body.IsInsideTree())
            {
                body.Connect("ready", new Callable(this, nameof(_OnBody_Ready)));
                return;
            }
            _SetBody(body);
            body.GetTree().Connect("physics_frame", new Callable(this, nameof(_onSceneTree_PhysicsFrame)));
        }

        private void _OnBody_Ready()
        {
            _SetBody(_BodyRefToBody());
            _BodyRefToBody().GetTree().Connect("physics_frame", new Callable(this, nameof(_onSceneTree_PhysicsFrame)));
        }

        /// <summary>
        /// Moves the agent's `body` by target `acceleration`.
        /// @tags - virtual
        /// </summary>
        /// <param name="acceleration"></param>
        /// <param name="delta"></param>
        public override void _ApplySteering(GSAITargetAcceleration acceleration, float delta)
        {
            var body = _BodyRefToBody();
            if (body is null || !body.IsInsideTree())
                return;

            _applied_steering = true;
            switch (MoveType)
            {
                case MovementType.SLIDE:
                    _ApplySlidingSteering(body, acceleration.Linear, delta);
                    break;
                case MovementType.COLLIDE:
                    _ApplyCollideSteering(body, acceleration.Linear, delta);
                    break;
                default:
                    _ApplyPositionSteering(body, acceleration.Linear, delta);
                    break;
            }
            _ApplyOrientationSteering(body, acceleration.Angular, delta);
        }

        private void _ApplySlidingSteering(CharacterBody2D body, Vector3 accel, float delta)
        {
            var velocity = GSAIUtils.ToVector2(LinearVelocity + accel * delta).LimitLength(LinearSpeedMax);
            if (ApplyLinearDrag)
                velocity = velocity.Lerp(Vector2.Zero, LinearDragPercentage);

            body.Velocity = velocity;
            body.MoveAndSlide();
            velocity = body.Velocity;
            if (CalculateVelocities)
                LinearVelocity = GSAIUtils.ToVector3(velocity);
        }

        private void _ApplyCollideSteering(CharacterBody2D body, Vector3 accel, float delta)
        {
            var velocity = GSAIUtils.ClampedV3(LinearVelocity + accel * delta, LinearSpeedMax);
            if (ApplyLinearDrag)
                velocity = velocity.Lerp(Vector3.Zero, LinearDragPercentage);
            body.MoveAndCollide(GSAIUtils.ToVector2(velocity) * delta);
            if (CalculateVelocities)
                LinearVelocity = velocity;
        }

        private void _ApplyPositionSteering(CharacterBody2D body, Vector3 accel, float delta)
        {
            var velocity = GSAIUtils.ClampedV3(LinearVelocity + accel * delta, LinearSpeedMax);
            if (ApplyLinearDrag)
                velocity = velocity.Lerp(Vector3.Zero, LinearDragPercentage);
            body.GlobalPosition += GSAIUtils.ToVector2(velocity) * delta;
            if (CalculateVelocities)
                LinearVelocity = velocity;
        }

        private void _ApplyOrientationSteering(CharacterBody2D body, float angular_acceleration, float delta)
        {
            var velocity = Mathf.Clamp(AngularVelocity + angular_acceleration * delta, -AngularSpeedMax, AngularSpeedMax);
            if (ApplyAngularDrag)
                velocity = Mathf.Lerp(velocity, 0, AngularDragPercentage);
            body.Rotation += velocity * delta;
            if (CalculateVelocities)
                AngularVelocity = velocity;
        }

        private void _SetBody(CharacterBody2D body)
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
            var body = _BodyRefToBody();
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
                    LinearVelocity = GSAIUtils.ClampedV3(GSAIUtils.ToVector3(current_position - _last_position), LinearSpeedMax);
                    if (ApplyLinearDrag)
                        LinearVelocity = LinearVelocity.Lerp(Vector3.Zero, LinearDragPercentage);

                    AngularVelocity = Mathf.Clamp(_last_orientation - current_orientation, -AngularSpeedMax, AngularSpeedMax);

                    if (ApplyAngularDrag)
                        AngularVelocity = Mathf.Lerp(AngularVelocity, 0, AngularDragPercentage);

                    _last_position = current_position;
                    _last_orientation = current_orientation;
                }
            }
        }
    }
}
