using Microsoft.Xna.Framework;

namespace lunge.Library.AI.Steering
{
    public interface ISteeringTarget
    {
        Vector2 Position { get; set; }
        bool IsActual { get; }
    }
}