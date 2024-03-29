﻿using Microsoft.Xna.Framework;
using Nez.Persistence;

namespace lunge.Library.Particles.Modifiers
{
    public class DragModifier : Modifier
    {
        [JsonInclude]
        public float DragCoefficient { get; set; } = 0.47f;
        [JsonInclude]
        public float Density { get; set; } = .5f;

        public override unsafe void Update(float elapsedSeconds, ParticleBuffer.ParticleIterator iterator)
        {
            while (iterator.HasNext)
            {
                var particle = iterator.Next();
                var drag = -DragCoefficient*Density*particle->Mass*elapsedSeconds;

                particle->Velocity = new Vector2(
                    particle->Velocity.X + particle->Velocity.X*drag,
                    particle->Velocity.Y + particle->Velocity.Y*drag);
            }
        }
    }
}