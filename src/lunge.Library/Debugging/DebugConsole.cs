using System;
using System.IO;
using ImGuiNET;
using lunge.Library.Graphics;
using lunge.Library.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Input.InputListeners;
using SpriteFontPlus;

namespace lunge.Library.Debugging
{
    public class DebugConsole : DrawableGameComponent
    {
        public new IGame Game { get; }
        public float Width { get; private set; }
        public float Height { get; private set; }
        public bool IsFocused { get; set; }
        public bool IsVisible { get; set; }

        private KeyboardListener _keyboardListener;
        private string _typedString;
        private string _outputString;
        private SpriteBatch _spriteBatch;
        private SpriteFont _debugFont;
        private float _drawingHeight;
        private ImGuiRenderer _imGuiRenderer;

        public DebugConsole(IGame game)
            : base(game.Game)
        {
            Game = game;
            Width = game.GraphicsDevice.Viewport.Width;
            Height = 300;
            _drawingHeight = 0;
            IsFocused = false;
            IsVisible = false;

            _keyboardListener = new KeyboardListener();
            _keyboardListener.KeyTyped += KeyboardListenerOnKeyTyped;

            _typedString = "";
            _outputString = "";

            _spriteBatch = new SpriteBatch(Game.GraphicsDevice);

            var fontBakeResult = TtfFontBaker.Bake(
                File.ReadAllBytes("Content/Fonts/DebugConsoleFont.ttf"),
                14, 1024, 1024, new[]
                {
                    CharacterRange.BasicLatin
                }
            );
            _debugFont = fontBakeResult.CreateSpriteFont(Game.GraphicsDevice);

            _imGuiRenderer = new ImGuiRenderer(Game);
            _imGuiRenderer.RebuildFontAtlas();
        }

        public override void Update(GameTime gameTime)
        {
            _keyboardListener.Update(gameTime);

            if (InputManager.WasMouseButtonPressed(MouseButton.Left))
            {
                if (new RectangleF(0, _drawingHeight - 16, Width, 16).Contains(InputManager.MousePosition.ToPoint()))
                {
                    IsFocused = true;
                }
                else
                {
                    IsFocused = false;
                }
            }

            if (_drawingHeight < Height && IsVisible)
            {
                _drawingHeight += 0.5f * (float) gameTime.ElapsedGameTime.TotalMilliseconds;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            if (!IsVisible)
                return;

            _spriteBatch.Begin();

            _spriteBatch.FillRectangle(0, 0, Width, _drawingHeight, Color.Black);
            _spriteBatch.DrawRectangle(0, _drawingHeight - 16, Width, 16, IsFocused ? Color.LightGray : Color.Gray, 2f);
            _spriteBatch.DrawString(_debugFont, _outputString, Vector2.One, Color.White);
            _spriteBatch.DrawString(_debugFont, _typedString, new Vector2(3, _drawingHeight - 16 - 1), Color.White);

            if (IsFocused)
            {
                var xOffset = _debugFont.MeasureString(_typedString);
                _spriteBatch.DrawString(_debugFont, "|", new Vector2(xOffset.X, _drawingHeight - 16 - 1), Color.LightGray);
            }

            _spriteBatch.End();
        }

        public void PushText(string text)
        {

        }

        private void Submit()
        {
            _typedString = _typedString.TrimStart('\r', '\n', '\n').TrimEnd('\r', '\n', ' ');
            Console.WriteLine($"Submitted: {_typedString}");
            _outputString += $"Submitted: {_typedString}" + "\n";
        }

        private void KeyboardListenerOnKeyTyped(object sender, KeyboardEventArgs e)
        {
            if (e.Key == Keys.OemTilde)
            {
                if (!IsVisible)
                {
                    IsVisible = true;
                }
                else
                {
                    IsVisible = false;
                    _drawingHeight = 0;
                }
            }

            if (!IsFocused)
                return;

            if (e.Key == Keys.Enter)
            {
                Submit();
                _typedString = "";
            }
            else if (e.Key == Keys.Back && _typedString.Length > 0)
            {
                _typedString = _typedString.Remove(_typedString.Length - 1, 1);
            }
            else if (e.Character != null)
            {
                _typedString += e.Character;
            }
        }
    }
}