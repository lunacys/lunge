using lunge.Library.Assets;
using lunge.Library.GameTimers;
using lunge.Library.Input;
using lunge.Library.Resources;
using lunge.Library.Screens;
using lunge.Library.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.ViewportAdapters;

namespace lunge.Library
{
    public abstract class GameBase : Game, IGame
    {
        // protected GraphicsDeviceManager Graphics { get; set; }
        protected SpriteBatch SpriteBatch { get; set; }
        // protected GameSettings GameSettings { get; set; }
        protected ResourceManager ResourceManager { get; set; }
        protected bool UseAssetsHotReload { get; set; }

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
            get
            {
                return UseAssetsHotReload ? _hotReloadAssetManager : _contentAssetManager;
            }
            protected set
            {
                if (UseAssetsHotReload)
                {
                    _hotReloadAssetManager.Dispose();
                    _hotReloadAssetManager = value;
                }
                else
                {
                    _contentAssetManager.Dispose();
                    _contentAssetManager = value;
                }
            }
        }

        // protected GameSettingsGameComponent GameSettingsComponent { get; private set; }
        public IScreenManager ScreenManager { get; private set; }

        public GraphicsDeviceManager Graphics { get; }

        private IAssetManager _contentAssetManager;
        private IAssetManager _hotReloadAssetManager;

        public GameBase Game => this;
        
        public GameBase(bool useAssetsHotReload = false)
        {
            UseAssetsHotReload = useAssetsHotReload;
            Graphics = new GraphicsDeviceManager(this);
            
            _contentAssetManager = new ContentAssetManager(this, Content);
            _contentAssetManager.RootDirectory = "Content";
            _hotReloadAssetManager = new HotReloadAssetManager(this);
            _hotReloadAssetManager.RootDirectory = "Content";

            IsMouseVisible = true;
            
            ScreenManager = new ScreenManager();

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