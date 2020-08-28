using lunge.Library.Assets;
using lunge.Library.Debugging.Logging;
using lunge.Library.GameTimers;
using lunge.Library.Graphics;
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

        private LogHelper _logger => LoggerFactory.GetLogger("GameBase");

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

        private ImGuiRenderer _imGuiRenderer;

        public GameBase Game => this;
        
        public GameBase(bool useAssetsHotReload = false)
        {
            _logger.Log("Constructing");

            UseAssetsHotReload = useAssetsHotReload;
            Graphics = new GraphicsDeviceManager(this);
            
            _contentAssetManager = new ContentAssetManager(this, Content);
            _contentAssetManager.RootDirectory = "Content";
            _hotReloadAssetManager = new HotReloadAssetManager(this);
            _hotReloadAssetManager.RootDirectory = "Content";

            IsMouseVisible = true;
            
            ScreenManager = new ScreenManager();

            _logger.Log("End Constructing");

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
            _logger.Log("Initializing");

            TextureStream.GraphicsDevice = GraphicsDevice;
            
            // Services.AddService(AssetManager);
            // Services.AddService(GameSettings);
            // Services.AddService(ResourceManager);

            _imGuiRenderer = new ImGuiRenderer(this);
            
            ScreenManager.Initialize();

            base.Initialize();
            // Components.Add(GameSettingsComponent);

            _logger.Log("End Initializing");
        }

        protected override void LoadContent()
        {
            _logger.Log("Loading Content");

            SpriteBatch = new SpriteBatch(GraphicsDevice);

            _imGuiRenderer.RebuildFontAtlas();
            
            base.LoadContent();

            _logger.Log("End Loading Content");
        }

        protected override void UnloadContent()
        {
            _logger.Log("Unloading Content");
            //GameSettingsComponent.SerializeToFile();

            base.UnloadContent();

            _logger.Log("End Unloading Content");
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
            _imGuiRenderer.BeforeLayout(gameTime);
            ScreenManager.Draw(gameTime);
            _imGuiRenderer.AfterLayout();

            base.Draw(gameTime);
        }
    }
}