using Microsoft.Xna.Framework;
using Nez.Persistence;

namespace lunge.Library.Particles.Profiles
{
    public class LineProfile : Profile
    {
        [JsonInclude]
        public Vector2 Axis { get; set; }
        [JsonInclude]
        public float Length { get; set; }

        public override void GetOffsetAndHeading(out Vector2 offset, out Vector2 heading)
        {
            var vect = Axis*Random.NextSingle(Length*-0.5f, Length*0.5f);
            offset = new Vector2(vect.X, vect.Y);
            Random.NextUnitVector(out heading);
        }
    }
}