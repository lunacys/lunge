using Microsoft.Xna.Framework;
using Nez.Tweens;

namespace lunge.Library.Particles.Modifiers.Interpolators;

public sealed class ColorRgbInterpolator : Interpolator<Color>
{
    public override unsafe void Update(float amount, Particle* particle)
    {
        particle->Color = Lerps.Lerp(StartValue, EndValue, amount).ToHsl();
    }
}