using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace lunge.Library.Screens
{
    public interface IGameScreen : IDisposable
    {
        ContentManager Content { get; }
        GraphicsDevice GraphicsDevice { get; }
        GameServiceContainer Services { get; }
        IScreenManager ScreenManager { get; internal set; }
        IGame Game { get; }
        
        void Initialize();
        void LoadContent();
        void UnloadContent();
        void Update(GameTime gameTime);
        void Draw(GameTime gameTime);
    }
}