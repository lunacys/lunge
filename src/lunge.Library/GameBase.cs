using System;
using lunge.Library.Assets;
using lunge.Library.Debugging.Logging;
using lunge.Library.DI;
using lunge.Library.GameTimers;
using lunge.Library.Input;
using lunge.Library.Resources;
using lunge.Library.Screens;
using lunge.Library.Settings;
using lunge.Library.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Screens.Transitions;
using MonoGame.Extended.ViewportAdapters;

namespace lunge.Library
{
    public abstract class GameBase : Game, IGame
    {
        // protected GraphicsDeviceManager Graphics { get; set; }
        protected SpriteBatch SpriteBatch { get; set; }
        // protected GameSettings GameSettings { get; set; }
        protected ResourceManager ResourceManager { get; set; }

        private ViewportAdapter _viewportAdapter;
        public ViewportAdapter ViewportAdapter
        {
            get
            {
                if (_viewportAdapter == null)
                {
                    _viewportAdapter = new DefaultViewportAdapter(GraphicsDevice);
                }

                return _viewportAdapter;
            }
            set { _viewportAdapter = value; }
        }

        public IAssetManager AssetManager
        {
            get => _assetManager;
            protected set
            {
                _assetManager?.Dispose();
                _assetManager = value;
            }
        }

        // protected GameSettingsGameComponent GameSettingsComponent { get; private set; }
        public IScreenManager ScreenManager { get; private set; }

        public GraphicsDeviceManager Graphics => Worker.Graphics;

        private IAssetManager _assetManager;

        public GameBase Game => this;
        
        public GameBase(IAssetManager assetManager, IScreenManager screenManager)
        {
            // Graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            if (assetManager == null)
                assetManager = new ContentAssetManager(Content);
            AssetManager = assetManager;
            ScreenManager = screenManager;

            IsMouseVisible = true;

            // TODO: Fix GameSettings loading/saving/handling
            // GameSettings = gameSettings;

            // GameSettingsComponent = new GameSettingsGameComponent(this, GameSettings);
            // if (!GameSettingsComponent.TryLoad())
            //    GameSettings.LoadDefaults();

            //Graphics.PreferredBackBufferWidth = Convert.ToInt32(GameSettings["WindowWidth"]);
            //Graphics.PreferredBackBufferHeight = Convert.ToInt32(GameSettings["WindowHeight"]);
            //IsMouseVisible = (bool)GameSettings["IsMouseVisible"];
            //Graphics.IsFullScreen = (bool) GameSettings["IsFullScreen"];

            //GameSettings = GameSettingsComponent.GameSettings;
        }

        protected override void Initialize()
        {
            TextureStream.GraphicsDevice = GraphicsDevice;
            
            // Services.AddService(AssetManager);
            // Services.AddService(GameSettings);
            // Services.AddService(ResourceManager);
            
            ScreenManager.Initialize();

            base.Initialize();
            // Components.Add(GameSettingsComponent);
        }

        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);
            
            base.LoadContent();
        }

        protected override void UnloadContent()
        {
            //GameSettingsComponent.SerializeToFile();

            base.UnloadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            InputManager.Update(gameTime);
            GameTimerManager.Update(gameTime);
            ScreenManager.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            ScreenManager.Draw(gameTime);

            base.Draw(gameTime);
        }
    }
}