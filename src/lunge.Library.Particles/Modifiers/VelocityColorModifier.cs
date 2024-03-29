﻿using Nez.Persistence;

namespace lunge.Library.Particles.Modifiers
{
    public class VelocityColorModifier : Modifier
    {
        [JsonInclude]
        public HslColor StationaryColor { get; set; }
        [JsonInclude]
        public HslColor VelocityColor { get; set; }
        [JsonInclude]
        public float VelocityThreshold { get; set; }

        public override unsafe void Update(float elapsedSeconds, ParticleBuffer.ParticleIterator iterator)
        {
            var velocityThreshold2 = VelocityThreshold*VelocityThreshold;

            while (iterator.HasNext)
            {
                var particle = iterator.Next();
                var velocity2 = particle->Velocity.X*particle->Velocity.X +
                                particle->Velocity.Y*particle->Velocity.Y;
                var deltaColor = VelocityColor - StationaryColor;

                if (velocity2 >= velocityThreshold2)
                    VelocityColor.CopyTo(out particle->Color);
                else
                {
                    var t = (float) Math.Sqrt(velocity2)/VelocityThreshold;

                    particle->Color = new HslColor(
                        deltaColor.H*t + StationaryColor.H,
                        deltaColor.S*t + StationaryColor.S,
                        deltaColor.L*t + StationaryColor.L);
                }
            }
        }
    }
}