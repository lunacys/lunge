using Microsoft.Xna.Framework;

namespace LunarisGE.Library
{
    public abstract class FpsCounter : DrawableGameComponent
    {
        public abstract float FramesPerSecond { get; protected set; }

        protected FpsCounter(Game game)
            : base(game)
        { }

        public override void Update(GameTime gameTime) { }

        public override void Draw(GameTime gameTime) { }
    }
}