using Microsoft.Xna.Framework;

namespace lunge.Library.AI.Steering.Old
{
    public interface ISteeringTarget
    {
        Vector2 Position { get; set; }
        bool IsActual { get; }
    }
}