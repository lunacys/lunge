using System;
using System.Diagnostics;
using lunge.Library;
using lunge.Library.Assets;
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
        private AssetWatcher _assetWatcher;

        public GameRoot()
        {
            IsMouseVisible = true;
            
            SetupDefaultAssetManager();
            SetupHotReloadAssetManager();
        }

        protected override void Initialize()
        {
            _assetWatcher = new AssetWatcher(AssetManager);
            _assetWatcher.AssetChanged += (sender, args) =>
            {
                Console.WriteLine($"Asset Changed: {args.AssetName}");
                _testTexture = (Texture2D) args.NewAsset;
            };
            
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _testTexture = AssetManager.Load<Texture2D>("Images/Test_1", "Image");
            _assetWatcher.Watch("Images/Test_1", "Image");
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

        [Conditional("RELEASE")]
        private void SetupDefaultAssetManager()
        {
            UseAssetsHotReload = false;
        }

        [Conditional("HOTRELOAD")]
        private void SetupHotReloadAssetManager()
        {
            UseAssetsHotReload = true;
            AssetManager.RootDirectory = "../../../Content";
        }
    }
}
