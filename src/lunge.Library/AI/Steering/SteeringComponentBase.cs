using System;
using Microsoft.Xna.Framework;
using Nez;

namespace lunge.Library.AI.Steering
{
    public abstract class SteeringComponentBase : Component, ISteeringBehavior
    {
        [Inspectable]
        public ISteeringEntity SteeringEntity { get; set; }
        
        [Inspectable]
        public bool IsAdditive { get; set; }

        public Predicate<ConditionArgs> Condition { get; set; }

        public ISteeringBehavior NestedBehavior { get; set; }

        public override void Initialize()
        {
            base.Initialize();
            
            SteeringEntity = Entity as ISteeringEntity;
        }

        public abstract Vector2 Steer(ISteeringTarget target);
        

        public override void OnRemovedFromEntity()
        {
            SteeringEntity.Reset();
        }

        public override void OnAddedToEntity()
        {
            SteeringEntity.Reset();
        }
    }
}