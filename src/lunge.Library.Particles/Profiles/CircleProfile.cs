using Microsoft.Xna.Framework;
using Nez.Persistence;

namespace lunge.Library.Particles.Profiles
{
    public class CircleProfile : Profile
    {
        [JsonInclude]
        public float Radius { get; set; }
        [JsonInclude]
        public CircleRadiation Radiate { get; set; }

        public override void GetOffsetAndHeading(out Vector2 offset, out Vector2 heading)
        {
            var dist = Random.NextSingle(0f, Radius);

            Random.NextUnitVector(out heading);

            offset = Radiate == CircleRadiation.In
                ? new Vector2(-heading.X*dist, -heading.Y*dist)
                : new Vector2(heading.X*dist, heading.Y*dist);

            if (Radiate == CircleRadiation.None)
                Random.NextUnitVector(out heading);
        }
    }
}