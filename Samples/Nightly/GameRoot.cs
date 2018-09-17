using System;
using lunge.Library.Debugging.Logging;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using lunge.Library.Input;

namespace Nightly
{
    public class GameRoot : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private InputHandler _input;
        private int _testCount = 0;

        public GameRoot()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            IsMouseVisible = true;

            LogHelper.Target = LogTarget.Console;
        }

        protected override void Initialize()
        {
            base.Initialize();

            _input = new InputHandler(this);
            _input.KeyboardHandler = _input.IsKeyDown;
            _input[Keys.Space] = () =>
            {
                _testCount++;
                LogHelper.Log($"Space Key Pressed. Test count: {_testCount}");
            };

            Components.Add(_input);
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            

        }

        protected override void Update(GameTime gameTime)
        {
            LogHelper.Update();

            if (_input.IsKeyDown(Keys.Escape))
                Exit();
            


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            

            base.Draw(gameTime);
        }
    }
}
