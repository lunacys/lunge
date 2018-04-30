using System;
using System.Collections.Generic;
using LunarisGE.Library;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;

namespace TestGame.DesktopGL
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private FpsCounterAdvanced _fpsCounterAdvanced;
        private FpsCounterAverage _fpsCounterAverage;

        private SpriteFont _mainFont;

        private List<Rectangle> _rects = new List<Rectangle>();

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            Test.PrintHello();

            graphics.SynchronizeWithVerticalRetrace = false;
            TargetElapsedTime = TimeSpan.FromSeconds(1.0f / 120.0f);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            _fpsCounterAverage = new FpsCounterAverage(this);
            _fpsCounterAdvanced = new FpsCounterAdvanced(this);

            Components.Add(_fpsCounterAverage);
            Components.Add(_fpsCounterAdvanced);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            _mainFont = Content.Load<SpriteFont>("MainFont");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            if (Mouse.GetState(Window).LeftButton == ButtonState.Pressed)
                for (int i = 0; i < 30; i++)
                    _rects.Add(new Rectangle(Mouse.GetState(Window).X + i, Mouse.GetState(Window).Y + i, 8, 8));
                

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            foreach (var rect in _rects)
            {
                spriteBatch.DrawRectangle(rect, Color.Red, 2f);
            }
            spriteBatch.End();

            base.Draw(gameTime);

            spriteBatch.Begin();
            spriteBatch.DrawString(_mainFont, _fpsCounterAdvanced.ToString(), new Vector2(16, 16), Color.Black);
            spriteBatch.DrawString(_mainFont, $"{_fpsCounterAverage}", new Vector2(16, 40), Color.Black);
            spriteBatch.End();

        }
    }
}
