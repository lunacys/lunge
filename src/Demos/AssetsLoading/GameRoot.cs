using System;
using lunge.Library;
using lunge.Library.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace AssetsLoading
{
    public class GameRoot : GameBase
    {
        private SpriteBatch _spriteBatch;
        private Texture2D _testTexture;
        private string _testText;

        public GameRoot()
        {
            IsMouseVisible = true;
            
#if HOTRELOAD
            UseAssetsHotReload = true;
            // Using a path to the base Content directory which usually contains Content.mgcb
            AssetManager.RootDirectory = "../../../Content";
#else
            UseAssetsHotReload = false;
#endif
        }

        protected override void Initialize()
        {

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _testTexture = AssetManager.Load<Texture2D>("Images/Test_1", "Image");
            _testText = AssetManager.Load<string>("Text/TestTextDoc", "Text");

            Console.WriteLine(_testText);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (InputManager.WasKeyPressed(Keys.Space))
            {
                _testTexture = AssetManager.Reload<Texture2D>("Images/Test_1", "Image");
                _testText = AssetManager.Reload<string>("Text/TestTextDoc", "Text"); 

                Console.WriteLine("New text:");
                Console.WriteLine(_testText);
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();
            _spriteBatch.Draw(_testTexture, Vector2.One * 64, null, Color.White);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
