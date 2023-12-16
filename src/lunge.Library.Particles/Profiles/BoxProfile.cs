using Microsoft.Xna.Framework;
using Nez.Persistence;

namespace lunge.Library.Particles.Profiles
{
    public class BoxProfile : Profile
    {
        [JsonInclude]
        public float Width { get; set; }
        [JsonInclude]
        public float Height { get; set; }

        public override void GetOffsetAndHeading(out Vector2 offset, out Vector2 heading)
        {
            switch (Random.Next(3))
            {
                case 0: // Left
                    offset = new Vector2(Width*-0.5f, Random.NextSingle(Height*-0.5f, Height*0.5f));
                    break;
                case 1: // Top
                    offset = new Vector2(Random.NextSingle(Width*-0.5f, Width*0.5f), Height*-0.5f);
                    break;
                case 2: // Right
                    offset = new Vector2(Width*0.5f, Random.NextSingle(Height*-0.5f, Height*0.5f));
                    break;
                default: // Bottom
                    offset = new Vector2(Random.NextSingle(Width*-0.5f, Width*0.5f), Height*0.5f);
                    break;
            }

            Random.NextUnitVector(out heading);
        }
    }
}