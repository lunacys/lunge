using System.IO;
using lunge.Library.Debugging.Logging;
using lunge.Library.GameAssets;
using lunge.Library.GameTimers;
using lunge.Library.Resources;
using lunge.Library.Screens;
using lunge.Library.Serialization;
using lunge.Library.Settings;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;

namespace lunge.Library
{
    public class GameBase : Game
    {
        protected GraphicsDeviceManager Graphics { get; set; }
        protected SpriteBatch SpriteBatch { get; set; }
        protected GameSettings GameSettings { get; set; }
        protected ResourceManager ResourceManager { get; set; }
        protected IAssetManager AssetManager { get; set; }

        protected GameSettingsGameComponent GameSettingsComponent { get; private set; }
        protected ScreenGameComponent ScreenManagerComponent { get; private set; }
        

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

            LogHelper.Target = LogTarget.Console | LogTarget.File;

            if (assetManager == null)
                assetManager = new ContentAssetManager(Content);
            AssetManager = assetManager;

            if (gameSettings == null)
            {
                gameSettings = new GameSettings();
            }
                
            GameSettings = gameSettings;

            Graphics.PreferredBackBufferWidth = (int)GameSettings["WindowWidth"];
            Graphics.PreferredBackBufferHeight = (int)GameSettings["WindowHeight"];
            IsMouseVisible = (bool)GameSettings["IsMouseVisible"];
            Graphics.IsFullScreen = (bool) GameSettings["IsFullScreen"];

            ResourceManager = new ResourceManager();

            ScreenManagerComponent = new ScreenGameComponent(this);
            GameSettingsComponent = new GameSettingsGameComponent(this, GameSettings);

            Components.Add(ScreenManagerComponent);
            Components.Add(GameSettingsComponent);
        }

        protected override void Initialize()
        {
            

            Services.AddService(AssetManager);
            Services.AddService(GameSettings);
            Services.AddService(ResourceManager);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);
            
            base.LoadContent();
        }

        protected override void UnloadContent()
        {
            string str = JsonConvert.SerializeObject(GameSettingsComponent.GameSettings, Formatting.Indented, new GameSettingsConverter());

            using (StreamWriter sw = new StreamWriter("Settings.json"))
            {
                sw.WriteLine(str);
            }

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
            ScreenManagerComponent.Register(screen);
            if (showImmediately)
                screen.Show<T>();
        }
    }
}