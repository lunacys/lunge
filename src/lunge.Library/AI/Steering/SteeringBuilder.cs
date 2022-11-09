using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nez;
using Nez.Sprites;

namespace lunge.Library.AI.Steering
{
    public class SteeringBuilder
    {
        private static int _currId = 0;

        private readonly SteeringEntity _entity;

        private bool _isAnyBehaviorAdded = false;

        public SteeringBuilder() : this(new Vector2(128, 128))
        { }
        
        public SteeringBuilder(Vector2 position, ISteeringTarget target = null)
        {
            _entity = new SteeringEntity
            {
                Name = "steering-" + _currId,
                Position = position,
                Tag = 123
            };

            var smc = _entity.AddComponent(new SteeringManagerComponent(target));
            _entity.ResetEvent += (sender, args) => smc.UpdateComponents();

            _currId++;
        }

        public SteeringBuilder AddBehavior(ISteeringBehavior behavior, Predicate<ConditionArgs> condition = null)
        {
            var cmp = _entity.AddComponent(behavior as SteeringComponentBase);
            if (_isAnyBehaviorAdded)
                cmp.IsAdditive = true;

            if (condition != null)
                cmp.Condition = condition;

            _isAnyBehaviorAdded = true;
            return this;
        }

        public SteeringBuilder AddNestedBehavior(ISteeringBehavior behavior, ISteeringBehavior nestedBehavior, Predicate<ConditionArgs> condition = null)
        {
            behavior.NestedBehavior = nestedBehavior;

            AddBehavior(behavior, condition);

            return this;
        }

        public static bool ApproachCircleCondition(ConditionArgs args)
        {
            return Vector2.Distance(args.Target.Position, args.Entity.Position) < 6f;
        }

        public SteeringBuilder SetPhysicalParams(SteeringPhysicalParams @params)
        {
            if (@params.Mass.HasValue)
                _entity.Mass = @params.Mass.Value;
            if (@params.MaxForce.HasValue)
                _entity.MaxForce = @params.MaxForce.Value;
            if (@params.MaxVelocity.HasValue)
                _entity.MaxVelocity = @params.MaxVelocity.Value;

            return this;
        }
        
        public SteeringBuilder UseDefaultRenderer(Texture2D texture)
        {
            _entity.AddComponent(new SpriteRenderer(texture));
            return this;
        }

        public SteeringBuilder AddCollider(float radius)
        {
            _entity.AddComponent(new CircleCollider(radius)).PhysicsLayer = 1;
            return this;
        }

        public SteeringEntity Build()
        {
            return _entity;
        }
    }
}