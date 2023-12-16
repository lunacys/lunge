using lunge.Library.Particles.Modifiers.Interpolators;
using Nez.Persistence;

namespace lunge.Library.Particles.Modifiers
{
    public class VelocityModifier : Modifier
    {
        [JsonInclude]
        public List<Interpolator> Interpolators { get; set; } = new List<Interpolator>();

        [JsonInclude]
        public float VelocityThreshold { get; set; }

        public override unsafe void Update(float elapsedSeconds, ParticleBuffer.ParticleIterator iterator)
        {
            var velocityThreshold2 = VelocityThreshold*VelocityThreshold;
            var n = Interpolators.Count;

            while (iterator.HasNext)
            {
                var particle = iterator.Next();
                var velocity2 = particle->Velocity.LengthSquared();

                if (velocity2 >= velocityThreshold2)
                {
                    for (var i = 0; i < n; i++)
                    {
                        var interpolator = Interpolators[i];
                        interpolator.Update(1, particle);
                    }
                }
                else
                {
                    var t = (float) Math.Sqrt(velocity2)/VelocityThreshold;
                    for (var i = 0; i < n; i++)
                    {
                        var interpolator = Interpolators[i];
                        interpolator.Update(t, particle);
                    }
                }
            }
        }
    }
}