using Microsoft.Xna.Framework.Graphics;
using Nez;

namespace Playground2.Components.Pathfinding2
{
    public class BlockComponent : RenderableComponent
    {
        public override float Width => 32;
        public override float Height => 32;

        private Texture2D _texture;

        public override void Initialize()
        {
            base.Initialize();

            _texture = Core.Content.Load<Texture2D>("test-block-1");
        }

        public override void Render(Batcher batcher, Camera camera)
        {
            batcher.Draw(_texture, Entity.Position);
        }
    }
}