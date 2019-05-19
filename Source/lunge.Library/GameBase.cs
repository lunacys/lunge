using lunge.Library.GameAssets;
using lunge.Library.GameTimers;
using lunge.Library.Resources;
using lunge.Library.Screens;
using lunge.Library.Settings;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace lunge.Library
{
    public class GameBase : Game
    {
        protected GraphicsDeviceManager Graphics { get; set; }
        protected SpriteBatch SpriteBatch { get; set; }
        protected GameSettings GameSettings { get; set; }
        protected ResourceManager ResourceManager { get; set; }
        protected IAssetManager AssetManager { get; set; }

        private ScreenGameComponent _screenGameComponent;
        

        public GameBase()
            : this(null, null)
        {
            
        }

        public GameBase(GameSettings gameSettings)
            : this(null, gameSettings)
        {
            
        }

        public GameBase(IAssetManager assetManager, GameSettings gameSettings)
        {
            Graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            Graphics.PreferredBackBufferWidth = 1600;
            Graphics.PreferredBackBufferHeight = 900;

            if (assetManager == null)
                assetManager = new ContentAssetManager(Content);
            AssetManager = assetManager;

            if (gameSettings == null)
                gameSettings = new GameSettings();
            GameSettings = gameSettings;

            ResourceManager = new ResourceManager();
        }

        protected override void Initialize()
        {
            _screenGameComponent = new ScreenGameComponent(this);

            Services.AddService(AssetManager);
            Services.AddService(GameSettings);
            Services.AddService(ResourceManager);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);

            Components.Add(_screenGameComponent);

            base.LoadContent();
        }

        protected override void UnloadContent()
        {
            base.UnloadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            GameTimerManager.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }

        public void AddScreen<T>(T screen, bool showImmediately = true) where T : Screen
        {
            _screenGameComponent.Register(screen);
            if (showImmediately)
                screen.Show<T>();
        }
    }
}