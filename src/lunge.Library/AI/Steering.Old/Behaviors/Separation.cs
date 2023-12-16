using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Nez;

namespace lunge.Library.AI.Steering.Old.Behaviors
{

    public class Separation : SteeringComponentBase
    {
        public Func<Separation, IEnumerable<ISteeringEntity>> CheckNearestFunc { get; set; }
        [Inspectable]
        public float SeparationRadius { get; set; }
        [Inspectable]
        public float MaxSeparation { get; set; }

        public Separation(Func<Separation, IEnumerable<ISteeringEntity>> checkNearestFunc, float separationRadius, float maxSeparation)
        {
            CheckNearestFunc = checkNearestFunc;
            SeparationRadius = separationRadius;
            MaxSeparation = maxSeparation;
        }

        public override Vector2 Steer(ISteeringTarget target)
        {
            var force = Vector2.Zero;
            var neighborCount = 0;

            var result = CheckNearestFunc.Invoke(this);
            
            foreach (var entity in result)
            {
                force += entity.Position - SteeringEntity.Position;
                neighborCount++;
            }

            if (neighborCount != 0)
            {
                force /= neighborCount;
                force *= -1;
            }

            if (force != Vector2.Zero)
                force.Normalize();
            force *= MaxSeparation;

            return force;
        }
    }
}