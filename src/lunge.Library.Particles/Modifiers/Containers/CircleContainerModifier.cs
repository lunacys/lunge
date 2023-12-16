using Microsoft.Xna.Framework;
using Nez.Persistence;

namespace lunge.Library.Particles.Modifiers.Containers
{
    public class CircleContainerModifier : Modifier
    {
        [JsonInclude]
        public float Radius { get; set; }
        [JsonInclude]
        public bool Inside { get; set; } = true;
        [JsonInclude]
        public float RestitutionCoefficient { get; set; } = 1;

        public override unsafe void Update(float elapsedSeconds, ParticleBuffer.ParticleIterator iterator)
        {
            var radiusSq = Radius*Radius;
            while (iterator.HasNext)
            {
                var particle = iterator.Next();
                var localPos = particle->Position - particle->TriggerPos;

                var distSq = localPos.LengthSquared();
                var normal = localPos;
                normal.Normalize();

                if (Inside)
                {
                    if (distSq < radiusSq) continue;

                    SetReflected(distSq, particle, normal);
                }
                else
                {
                    if (distSq > radiusSq) continue;

                    SetReflected(distSq, particle, -normal);
                }
            }
        }

        private unsafe void SetReflected(float distSq, Particle* particle, Vector2 normal)
        {
            var dist = (float) Math.Sqrt(distSq);
            var d = dist - Radius; // how far outside the circle is the particle

            var twoRestDot = 2*RestitutionCoefficient*
                             Vector2.Dot(particle->Velocity, normal);
            particle->Velocity -= twoRestDot*normal;

            // exact computation requires sqrt or goniometrics
            particle->Position -= normal*d;
        }
    }
}