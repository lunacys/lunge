using Microsoft.Xna.Framework;
using MonoGame.Extended.Screens;

namespace lunge.Library.Screens
{
    public abstract class Screen : GameScreen
    {
        protected Screen(Game game) 
            : base(game)
        { }
    }
}