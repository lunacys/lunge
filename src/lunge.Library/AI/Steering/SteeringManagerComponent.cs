using System;
using System.Collections.Generic;
using lunge.Library.Utils;
using Microsoft.Xna.Framework;
using Nez;

namespace lunge.Library.AI.Steering
{
    public class SteeringManagerComponent : Component, IUpdatable
    {
        [Inspectable]
        public Vector2 Velocity
        {
            get => _steeringEntity!.Velocity;
            set => _steeringEntity!.Velocity = value;
        }
        [Inspectable]
        public float MaxVelocity
        {
            get => _steeringEntity!.MaxVelocity;
            set => _steeringEntity!.MaxVelocity = value;
        }
        [Inspectable]
        public float MaxForce
        {
            get => _steeringEntity!.MaxForce;
            set => _steeringEntity!.MaxForce = value;
        }
        [Inspectable]
        public float Mass
        {
            get => _steeringEntity!.Mass;
            set => _steeringEntity!.Mass = value;
        } 

        [Inspectable]
        public Vector2 Steering
        {
            get => _steeringEntity!.Steering;
            set => _steeringEntity!.Steering = value;
        }
        [Inspectable]
        public Vector2 DesiredVelocity
        {
            get => _steeringEntity!.DesiredVelocity;
            set => _steeringEntity!.DesiredVelocity = value;
        }

        public Vector2 Position
        {
            get => Entity.Position;
            set => Entity.Position = value;
        }

        public float Rotation
        {
            get => Entity.Rotation;
            set => Entity.Rotation = value;
        }

        private ISteeringEntity? _steeringEntity;
        private IList<SteeringComponentBase>? _steeringComponents;

        private Collider _collider = null!;

        [Inspectable]
        public ISteeringTarget? SteeringTarget { get; set; }

        public SteeringManagerComponent(ISteeringTarget? steeringTarget = null)
        {
            SteeringTarget = steeringTarget;
        }

        public override void Initialize()
        {
            base.Initialize();
            
            _steeringEntity = Entity as ISteeringEntity;
            if (_steeringEntity == null)
                throw new Exception("Steering Manager only works with entities that implement ISteeringEntity");
            
            UpdateComponents();
            
            if (SteeringTarget == null)
            {
                var mouseEntityComponent =
                    Core.Scene.Entities.FindEntity("mouse-entity").GetComponent<MouseEntityComponent>();
                if (mouseEntityComponent != null)
                    SteeringTarget = mouseEntityComponent;
            }

            Velocity = Vector2.Zero;
            Steering = Vector2.Zero;
            DesiredVelocity = Vector2.Zero;
        }

        public void UpdateComponents()
        {
            if (_steeringComponents != null)
                _steeringComponents.Clear();
            
            if (Entity != null)
                _steeringComponents = Entity.GetComponents<SteeringComponentBase>();
        }

        private readonly float _dtMultiplier = 60;

        public void Update()
        {
            bool anyInvokes = false;

            foreach (var component in _steeringComponents)
            {
                if (component.Condition == null || (component.Condition != null &&
                                                    component.Condition(new ConditionArgs(_steeringEntity,
                                                        SteeringTarget))))
                {
                    anyInvokes = true;
                    if (!component.IsAdditive)
                        Steering = component.Steer(SteeringTarget);
                    else
                        Steering += component.Steer(SteeringTarget);
                }
            }

            if (!anyInvokes)
            {
                Steering = -Velocity;
            }

            Steering = MathUtils.Truncate(Steering, MaxForce);
            Steering /= Mass;

            Velocity += Steering;
            Velocity = MathUtils.Truncate(Velocity, MaxVelocity);
            Position += Velocity * (Time.DeltaTime * _dtMultiplier);

            if (Velocity.X != 0 && Velocity.Y != 0)
                Rotation = MathUtils.TurnToFace(Position, Position + DesiredVelocity, Rotation, 0.1f * (Time.DeltaTime * _dtMultiplier));

            Vector2 motion = Vector2.Zero;
            CollisionResult result;

            if (_collider == null)
                _collider = Entity.GetComponent<Collider>();
            if (_collider != null && _collider.CollidesWithAny(ref motion, out result))
            {
                // Debug.Log("collision result: {0}", result);
            }

            Position += motion;

            Position = Vector2.Clamp(Position, Vector2.Zero, new Vector2(1360, 768));
        }

    }
}