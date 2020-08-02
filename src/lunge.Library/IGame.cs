using System;
using lunge.Library.Assets;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.ViewportAdapters;

namespace lunge.Library
{
    public interface IGame
    {
        GameBase Game { get; }
        
        // include all the Properties/Methods that you'd want to use on your Game class below.
        GameWindow Window { get; }
        ContentManager Content { get; }
        GraphicsDevice GraphicsDevice { get; }
        GameServiceContainer Services { get; }
        ViewportAdapter ViewportAdapter { get; set; }

        event EventHandler<EventArgs> Exiting;

        void Run();
        void Exit();
    }
}