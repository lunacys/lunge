using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.Screens.Transitions;

namespace lunge.Library.Screens
{
    public class ScreenManager : SimpleDrawableGameComponent, IScreenManager
    {
        private IGameScreen _activeScreen;
        private Transition _activeTransition;
        
        public void LoadScreen(IGameScreen screen, Transition transition)
        {
            if (_activeTransition != null)
                return;

            _activeTransition = transition;
            _activeTransition.StateChanged += (sender, args) => LoadScreen(screen);
            _activeTransition.Completed += (sender, args) =>
            {
                _activeTransition.Dispose();
                _activeTransition = null;
            };
        }

        public void LoadScreen(IGameScreen screen)
        {
            _activeScreen?.UnloadContent();
            _activeScreen?.Dispose();

            screen.ScreenManager = this;
            screen.Initialize();
            screen.LoadContent();
            _activeScreen = screen;
        }

        public override void Initialize()
        {
            base.Initialize();

            _activeScreen?.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            _activeScreen?.LoadContent();
        }

        protected override void UnloadContent()
        {
            base.UnloadContent();

            _activeScreen?.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            _activeScreen?.Update(gameTime);
            _activeTransition?.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            _activeScreen?.Draw(gameTime);
            _activeTransition?.Draw(gameTime);
        }
    }
}