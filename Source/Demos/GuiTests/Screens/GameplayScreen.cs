using System;
using lunge.Library;
using lunge.Library.Debugging.Logging;
using lunge.Library.Entities;
using lunge.Library.Gui;
using lunge.Library.Gui.Controls;
using lunge.Library.Input;
using lunge.Library.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.ViewportAdapters;

namespace GuiTests.Screens
{
    public class GameplayScreen : Screen
    {
        private readonly LogHelper Logger = LogHelper.GetLogger(nameof(GameplayScreen), LogTarget.Console);

        private SpriteBatch _spriteBatch;

        private World _world;
        private OrthographicCamera _camera;
        private InputHandler _inputHandler;

        private Canvas _mainCanvas;

        public GameplayScreen(GameBase game) 
            : base(game)
        {
            Logger.Log("Constructing GameplayScreen..");

            GameRoot.ViewportAdapter = new WindowViewportAdapter(GameRoot.Window, GameRoot.GraphicsDevice);

            Logger.Log("Done constructing GameplayScreen");
        }

        public override void Initialize()
        {
            Logger.Log("Initializing..");

            _world = new WorldBuilder(GameRoot).Build();

            _camera = new OrthographicCamera(GameRoot.ViewportAdapter);

            _inputHandler = new InputHandler(GameRoot, _camera);

            _mainCanvas = new Canvas(GameRoot, "MainCanvas", Vector2.Zero, new Size2(800, 600));
            

            Logger.Log("Initialization done. Calling base.Initialize()");

            base.Initialize();

            Logger.Log("Done Calling base.Initialize()");
        }

        public override void LoadContent()
        {
            Logger.Log("Loading Content");

            _spriteBatch = new SpriteBatch(GameRoot.GraphicsDevice);
            
            Assets.Load(GameRoot.Content);
            InitializeGuiControls();

            Logger.Log("Finished Loading Content. Calling base.Initialize()");

            base.LoadContent();

            Logger.Log("Done Calling base.LoadContent()");
        }

        public override void Update(GameTime gameTime)
        {
            _inputHandler.Update(gameTime);

            if (_inputHandler.IsKeyDown(Keys.A))
            {
                _camera.Move(-Vector2.UnitX);
            }
            if (_inputHandler.IsKeyDown(Keys.D))
            {
                _camera.Move(Vector2.UnitX);
            }
            if (_inputHandler.IsKeyDown(Keys.W))
            {
                _camera.Move(-Vector2.UnitY);
            }
            if (_inputHandler.IsKeyDown(Keys.S))
            {
                _camera.Move(Vector2.UnitY);
            }

            _mainCanvas.Update(gameTime);
            _world.Update(gameTime);

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin(transformMatrix: _camera.GetViewMatrix());
            {
                _world.Draw(gameTime);
                _spriteBatch.DrawRectangle(_camera.BoundingRectangle, Color.LightGray, 5f);
                
                DrawDebugInformation();
            }
            _spriteBatch.End();
            //_mainCanvas.SpriteBatchSettings.TransformMatrix = _camera.GetViewMatrix();
            _mainCanvas.Draw(gameTime);

            //_spriteBatch.Begin();
            //{
            //}
            //_spriteBatch.End();

            base.Draw(gameTime);
        }

        private void DrawDebugInformation()
        {
            var fpsCounterComponent = GameRoot.Services.GetService<FramesPerSecondCounterComponent>();

            var basePosition = _camera.ScreenToWorld(Vector2.One * 16);
            var debugString =
                $"Use WASD keys to move camera around\n" +
                $"Camera Position: {_camera.Position}";

            foreach (var entity in _world.GetAllEntities())
            {
                debugString += $"{entity.Id} ({entity.GetType().Name}) ";
            }

            debugString += "\n";

            _spriteBatch.DrawString(Assets.MainFont, debugString, basePosition, Color.Black);
        }

        private void InitializeGuiControls()
        {
            var btnExit = new Button("ButtonExit", Vector2.One, 48, 20, Assets.MainFont)
            {
                Text = "Exit",
                DrawDepth = 0.0f
            };
            btnExit.Clicked += (sender, args) => GameRoot.Exit();
            _mainCanvas.AddControl(btnExit);

            var btnExitTooltip = new Tooltip("TooltipButtonExit", btnExit, Assets.MainFont)
            {
                Delay = 0.5,
                Offset = new Vector2(15, 10),
                Text = "Quit game completely",
                Size = new Size2(160, 20)
            };
            _mainCanvas.AddControl(btnExitTooltip);

            var panel1 = new Panel("Panel1")
            {
                Position = new Vector2(100, 10),
                Size = new Size2(400, 400),
                BorderWidth = 2f,
                IsMoveable = true
            };
            _mainCanvas.AddControl(panel1);

            var gridTest = new Grid("Grid1", new Size2(128, 256), new Size2(64, 64))
            {
                Position = new Vector2(16, 16),
                BorderWidth = 3f,
                ParentControl = panel1
            };

            var label1 = new Label("Label1", Assets.MainFont, new Vector2(188, 32), "Hello!", Color.Black,
                TextAlignment.Center)
            {
                ParentControl = panel1
            };
            _mainCanvas.AddControl(label1);

            var gridButton = new Button("GridButton1", Vector2.Zero, 1, 1, Assets.MainFont)
            {
                ParentControl = gridTest
            };
            gridButton.Clicked += (sender, args) => Console.WriteLine("CLICKED");
            _mainCanvas.AddControl(gridButton);

            gridTest.GetCellAt(0, 0).AttachedControl = gridButton;
            gridTest.ForEach((cell, x, y) =>
            {
                cell.Font = Assets.MainFont;
                cell.Text = "Hi!!";
                cell.ForegroundColor = Color.Red;
                cell.Image = new Image($"Img_{x}_{y}", new Sprite(Assets.HeroTexture), gridTest);
            });

            _mainCanvas.AddControl(gridTest);

            var btnPanel = new Button("BtnPnl", new Vector2(400, 16), 64, 20, Assets.MainFont)
            {
                ParentControl = panel1,
                Text = "Hello!"
            };
            _mainCanvas.AddControl(btnPanel);

            _mainCanvas.Visualize();
        }
    }
}