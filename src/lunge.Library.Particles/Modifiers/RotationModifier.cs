using Nez.Persistence;

namespace lunge.Library.Particles.Modifiers
{
    public class RotationModifier : Modifier
    {
        [JsonInclude]
        public float RotationRate { get; set; }

        public override unsafe void Update(float elapsedSeconds, ParticleBuffer.ParticleIterator iterator)
        {
            var rotationRateDelta = RotationRate*elapsedSeconds;

            while (iterator.HasNext)
            {
                var particle = iterator.Next();
                particle->Rotation += rotationRateDelta;
            }
        }
    }
}