using System;
using lunge.Library.GameSystems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace lunge.Library.Screens
{
    public abstract class Screen : IDisposable
    {
        protected Game GameRoot { get; }
        protected GameSystemComponent GameSystemComponent { get; }
        protected SpriteBatch SpriteBatch { get; private set; }

        protected Screen(Game game)
        {
            GameRoot = game;
            GameSystemComponent = new GameSystemComponent(GameRoot);
            GameRoot.Components.Add(GameSystemComponent);
        }

        public IScreenManager ScreenManager { get; internal set; }
        public bool IsInitialized { get; private set; }
        public bool IsVisible { get; private set; }

        public T FindScreen<T>() where T : Screen
        {
            return ScreenManager?.FindScreen<T>();
        }

        public void Show<T>(bool hideThis) where T : Screen
        {
            var screen = FindScreen<T>();
            screen.Show();

            if (hideThis)
                Hide();
        }

        public void Show<T>() where T : Screen
        {
            Show<T>(true);
        }

        public void Show()
        {
            if (!IsInitialized)
                Initialize();

            IsVisible = true;

            foreach (var gameSystem in GameSystemComponent.GetAllGameSystems())
                gameSystem.IsActive = true;
        }

        public void Hide()
        {
            IsVisible = false;

            foreach (var gameSystem in GameSystemComponent.GetAllGameSystems())
                gameSystem.IsActive = false;
        }

        public virtual void Initialize()
        {
            IsInitialized = true;
        }

        public virtual void LoadContent()
        {
            SpriteBatch = new SpriteBatch(GameRoot.GraphicsDevice);
        }

        public virtual void UnloadContent()
        {
        }

        public virtual void Update(GameTime gameTime)
        {
        }

        public virtual void Draw(GameTime gameTime)
        {
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                GameRoot.Components.Remove(GameSystemComponent);
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}