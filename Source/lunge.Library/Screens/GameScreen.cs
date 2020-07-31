using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace lunge.Library.Screens
{
    public abstract class GameScreen : IGameScreen
    {
        public IGame Game { get; }

        public ContentManager Content => Game.Content;
        public GraphicsDevice GraphicsDevice => Game.GraphicsDevice;
        public GameServiceContainer Services => Game.Services;

        IScreenManager IGameScreen.ScreenManager { get; set; }

        protected GameScreen(IGame game)
        {
            Game = game;
        }

        public virtual void Initialize() { }

        public virtual void LoadContent() { }

        public virtual void UnloadContent() { }

        public abstract void Update(GameTime gameTime);

        public abstract void Draw(GameTime gameTime);

        public virtual void Dispose() { }
    }
}