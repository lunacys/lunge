using System;
using System.IO;
using lunge.Library.Debugging.Logging;
using lunge.Library.GameAssets;
using lunge.Library.GameTimers;
using lunge.Library.Graphs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using lunge.Library.Input;
using lunge.Library.Serialization;
using lunge.Library.Settings;
using MonoGame.Extended;
using Newtonsoft.Json;
using FramesPerSecondCounterComponent = lunge.Library.Debugging.FramesPerSecondCounterComponent;

namespace Nightly
{
    public class GameRoot : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private InputHandler _input;
        private int _testCount = 0;
        private AssetManager _assetManager;
        private GraphCanvas _fpsCanvas;

        private Texture2D _testTexture;

        [GameSettingsEntry("MusicVolume", 1.0)]
        public double MusicVolume { get; set; }

        [GameSettingsEntry("SfxVolume", 1.0)]
        public double SfxVolume { get; set; }

        private GameSettingsGameComponent _gameSettings;

        private FramesPerSecondCounterComponent _fpsCounter;
		

        public GameRoot()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            IsMouseVisible = true;

            LogHelper.Target = LogTarget.Console | LogTarget.File | LogTarget.Database;
        }

        protected override void Initialize()
        {
            _input = new InputHandler(this);
            _input.KeyboardHandler = _input.IsKeyDown;
            _input[Keys.Space] = () =>
            {
                _testCount++;
                LogHelper.LogAsync($"Space Key Pressed. Test count: {_testCount}").Wait();
            };

            GameTimer gt = new GameTimer(1.5, true);
            gt.OnTimeElapsed += (sender, args) =>
            {
                LogHelper.Log($"SfxVolume: {SfxVolume}, MusicVolume: {MusicVolume}");
            };
            GameTimerManager.Add(gt);

            LogHelper.LogAsync("TEST ERROR!", LogLevel.Error).Wait();

            _gameSettings = new GameSettingsGameComponent(this);

            _fpsCanvas = new GraphCanvas(this)
            {
                Position = new Vector2(16, 128),
                Size = new Size2(512, 128),
                MaxValue = 60,
                CellSize = new Size2(16, 16),
                MinValue = 0,
                ShouldDrawBars = false
            };
            

            _fpsCounter = new FramesPerSecondCounterComponent(this);
            _fpsCounter.OnFpsUpdate += (sender, args) =>
            {
                _fpsCanvas.PushValue(_fpsCounter.FramesPerSecond);
            };

            //Components.Add(_fpsCanvas);
            Components.Add(_fpsCounter);
            Components.Add(_gameSettings);
            Components.Add(_input);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _assetManager = new AssetManager(GraphicsDevice, "Assets");

            // TODO: use this.Content to load your game content here
            try
            {
                _testTexture = _assetManager.Load<Texture2D>("Test.png");
            }
            catch (Exception e)
            {
                LogHelper.Log($"Error: {e.Message}");
            }
            

            SfxVolume = _gameSettings.Get<double>("SfxVolume");
            MusicVolume = _gameSettings.Get<double>("MusicVolume");
        }

        protected override void UnloadContent()
        {
            base.UnloadContent();

            string str = JsonConvert.SerializeObject(_gameSettings.GameSettings, Formatting.Indented, new GameSettingsConverter());

            using (StreamWriter sw = new StreamWriter("Settings.json"))
            {
                sw.WriteLine(str);
            }
        }

        protected override void Update(GameTime gameTime)
        {
            if (_input.IsKeyDown(Keys.Escape))
                Exit();
            
            GameTimerManager.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            
            _spriteBatch.Begin();
            if (_testTexture != null)
                _spriteBatch.Draw(_testTexture, new Vector2(64, 64), null, Color.White);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
