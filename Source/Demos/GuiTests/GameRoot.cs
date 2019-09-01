using GuiTests.Screens;
using lunge.Library;
using lunge.Library.Debugging;
using lunge.Library.Debugging.Logging;
using lunge.Library.GameTimers;
using lunge.Library.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GuiTests
{
    public class GameRoot : GameBase
    {
        private SpriteBatch _spriteBatch;
        private ScreenGameComponent _screenComponent;
        private GameplayScreen _gameplayScreen;
        private FramesPerSecondCounterComponent _fpsCounter;

        private readonly LogHelper Logger = LogHelper.GetLogger(nameof(GameRoot), LogTarget.Console);

        public GameRoot()
        {
            Logger.Log("Constructing..");

            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            Graphics.PreferredBackBufferWidth = 1600;
            Graphics.PreferredBackBufferHeight = 900;

            Window.AllowUserResizing = true;

            Logger.Log("Constructing done!");
        }

        protected override void Initialize()
        {
            Logger.Log("Initializing..");

            Logger.Log("Creating a new screen: GameplayScreen");
            _gameplayScreen = new GameplayScreen(this);

            Logger.Log("Registering new screen: GameplayScreen");
            _screenComponent = new ScreenGameComponent(this);
            _screenComponent.Register(_gameplayScreen);
            Components.Add(_screenComponent);

            Logger.Log("Creating FramesPerSecondCounterComponent");
            _fpsCounter = new FramesPerSecondCounterComponent(this);
            Components.Add(_fpsCounter);
            Logger.Log("Adding FramesPerSecondCounterComponent as Service");
            Services.AddService(_fpsCounter);

            var fpsCounter = Services.GetService<FramesPerSecondCounterComponent>();
            Logger.Log("Service: " + fpsCounter);

            Logger.Log("Initialization done. Calling base.Initialize()");

            base.Initialize();

            Logger.Log("Done Calling base.Initialize()");
        }

        protected override void LoadContent()
        {
            Logger.Log("Loading Content..");

            _spriteBatch = new SpriteBatch(GraphicsDevice);

            //Assets.Load(Content);

            Logger.Log("Done loading content");

            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            GameTimerManager.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            base.Draw(gameTime);
        }
    }
}