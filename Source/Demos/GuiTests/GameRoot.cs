using GuiTests.Screens;
using lunge.Library;
using lunge.Library.Debugging;
using lunge.Library.Debugging.Logging;
using lunge.Library.GameTimers;
using lunge.Library.Input;
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

        private readonly LogHelper _logger = LoggerFactory.GetLogger(nameof(GameRoot));

        public GameRoot()
        {
            _logger.Log("Constructing..");

            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            Graphics.PreferredBackBufferWidth = 1600;
            Graphics.PreferredBackBufferHeight = 900;

            Window.AllowUserResizing = true;

            _logger.Log("Constructing done!");
        }

        protected override void Initialize()
        {
            _logger.Log("Initializing..");

            _logger.Log("Creating a new screen: GameplayScreen");
            _gameplayScreen = new GameplayScreen(this);

            _logger.Log("Registering new screen: GameplayScreen");
            _screenComponent = new ScreenGameComponent();
            _screenComponent.LoadScreen(_gameplayScreen);
            Components.Add(_screenComponent);

            _logger.Log("Creating FramesPerSecondCounterComponent");
            _fpsCounter = new FramesPerSecondCounterComponent(this);
            Components.Add(_fpsCounter);
            _logger.Log("Adding FramesPerSecondCounterComponent as Service");
            Services.AddService(_fpsCounter);

            var fpsCounter = Services.GetService<FramesPerSecondCounterComponent>();
            _logger.Log("Service: " + fpsCounter);

            _logger.Log("Initialization done. Calling base.Initialize()");

            base.Initialize();

            _logger.Log("Done Calling base.Initialize()");
        }

        protected override void LoadContent()
        {
            _logger.Log("Loading Content..");

            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _logger.Log("Done loading content");

            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            InputManager.Update(gameTime);

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