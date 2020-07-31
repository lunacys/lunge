using Microsoft.Xna.Framework;
using MonoGame.Extended.Screens.Transitions;

namespace lunge.Library.Screens
{
    public interface IScreenManager
    {
        void LoadScreen(IGameScreen screen, Transition transition);
        void LoadScreen(IGameScreen screen);

        void Initialize();
        void Update(GameTime gameTime);
        void Draw(GameTime gameTime);
    }
}