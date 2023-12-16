using Microsoft.Xna.Framework;
using lunge.Library;
using Nez.Persistence;

namespace lunge.Library.Particles
{
    public class ParticleReleaseParameters
    {
        public ParticleReleaseParameters()
        {
            Quantity = 1;
            Speed = new Range<float>(-1f, 1f);
            Color = new Range<HslColor>(Microsoft.Xna.Framework.Color.White.ToHsl(), Microsoft.Xna.Framework.Color.White.ToHsl());
            Opacity = new Range<float>(0f, 1f);
            Scale = new Range<float>(1f, 1f);
            Rotation = new Range<float>(-MathHelper.Pi, MathHelper.Pi);
            Mass = 1f;
            MaintainAspectRatioOnScale = true;
            ScaleX = new Range<float>(1f, 1f);
            ScaleY = new Range<float>(1f, 1f);
        }

        [JsonInclude]
        public Range<int> Quantity { get; set; }
        [JsonInclude]
        public Range<float> Speed { get; set; }
        [JsonInclude]
        public Range<HslColor> Color { get; set; }
        [JsonInclude]
        public Range<float> Opacity { get; set; }
        [JsonInclude]
        public Range<float> Scale { get; set; }
        [JsonInclude]
        public Range<float> Rotation { get; set; }
        [JsonInclude]
        public Range<float> Mass { get; set; }
        [JsonInclude]
        public bool MaintainAspectRatioOnScale { get; set; }
        [JsonInclude]
        public Range<float> ScaleX { get; set; }
        [JsonInclude]
        public Range<float> ScaleY { get; set; }

    }
}
