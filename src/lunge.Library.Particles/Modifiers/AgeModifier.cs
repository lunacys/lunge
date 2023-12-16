using System.ComponentModel;
using lunge.Library.Particles.Modifiers.Interpolators;
using Nez.Persistence;

namespace lunge.Library.Particles.Modifiers
{
    public class AgeModifier : Modifier
    {
        [EditorBrowsable(EditorBrowsableState.Always)]
        [JsonInclude]
        public List<Interpolator> Interpolators { get; set; } = new List<Interpolator>();

        public override unsafe void Update(float elapsedSeconds, ParticleBuffer.ParticleIterator iterator)
        {
            var n = Interpolators.Count;
            while (iterator.HasNext)
            {
                var particle = iterator.Next();
                for (var i = 0; i < n; i++)
                {
                    var interpolator = Interpolators[i];
                    interpolator.Update(particle->Age, particle);
                }
            }
        }
    }
}