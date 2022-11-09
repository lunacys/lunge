using Microsoft.Xna.Framework;
using Nez;

namespace lunge.Library.AI.Steering
{
    public class MouseEntityComponent : Component, IUpdatable, ISteeringTarget
    {
        Vector2 ISteeringTarget.Position
        {
            get => Entity.Position;
            set => Entity.Position = value;
        }

        public bool IsActual => true;

        public void Update()
        {
            Entity.Position = Nez.Input.MousePosition;
        }
    }
}