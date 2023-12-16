namespace lunge.Library.Particles.Modifiers.Interpolators
{
    public class RotationInterpolator : Interpolator<float>
    {
        public override unsafe void Update(float amount, Particle* particle)
        {
            particle->Rotation = (EndValue - StartValue) * amount + StartValue;
        }
    }
}