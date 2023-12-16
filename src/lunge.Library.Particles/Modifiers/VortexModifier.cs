using Microsoft.Xna.Framework;
using Nez.Persistence;

namespace lunge.Library.Particles.Modifiers
{
    public unsafe class VortexModifier : Modifier
    {
        // Note: not the real-life one
        private const float _gravConst = 100000f;

        [JsonInclude]
        public Vector2 Position { get; set; }
        [JsonInclude]
        public float Mass { get; set; }
        [JsonInclude]
        public float MaxSpeed { get; set; }

        public override void Update(float elapsedSeconds, ParticleBuffer.ParticleIterator iterator)
        {
            while (iterator.HasNext)
            {
                var particle = iterator.Next();
                var diff = Position + particle->TriggerPos - particle->Position;

                var distance2 = diff.LengthSquared();

                var speedGain = _gravConst*Mass/distance2*elapsedSeconds;
                // normalize distances and multiply by speedGain
                diff.Normalize();
                particle->Velocity += diff*speedGain;
            }
        }
    }
}