﻿using System;
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
using MonoGame.Extended.ViewportAdapters;
using Newtonsoft.Json;

namespace lunge.Library
{
    public class GameBase : Game
    {
        protected GraphicsDeviceManager Graphics { get; set; }
        protected SpriteBatch SpriteBatch { get; set; }
        protected GameSettings GameSettings { get; set; }
        protected ResourceManager ResourceManager { get; set; }

        private ViewportAdapter _viewportAdapter;
        public ViewportAdapter ViewportAdapter
        {
            get
            {
                if (_viewportAdapter == null)
                {
                    _viewportAdapter = new DefaultViewportAdapter(GraphicsDevice);
                }

                return _viewportAdapter;
            }
            set { _viewportAdapter = value; }
        }

        public IAssetManager AssetManager
        {
            get => _assetManager;
            protected set
            {
                _assetManager?.Dispose();
                _assetManager = value;
            }
        }

        protected GameSettingsGameComponent GameSettingsComponent { get; private set; }
        protected ScreenGameComponent ScreenManagerComponent { get; private set; }

        private IAssetManager _assetManager;
        
        public GameBase(IAssetManager assetManager = null, GameSettings gameSettings = null)
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

            GameSettingsComponent = new GameSettingsGameComponent(this, GameSettings);
            if (!GameSettingsComponent.TryLoad())
                GameSettings.LoadDefaults();

            Graphics.PreferredBackBufferWidth = Convert.ToInt32(GameSettings["WindowWidth"]);
            Graphics.PreferredBackBufferHeight = Convert.ToInt32(GameSettings["WindowHeight"]);
            IsMouseVisible = (bool)GameSettings["IsMouseVisible"];
            Graphics.IsFullScreen = (bool) GameSettings["IsFullScreen"];

            ResourceManager = new ResourceManager();

            ScreenManagerComponent = new ScreenGameComponent(this);
            Components.Add(ScreenManagerComponent);
            GameSettings = GameSettingsComponent.GameSettings;
        }

        protected override void Initialize()
        {
            Services.AddService(AssetManager);
            Services.AddService(GameSettings);
            Services.AddService(ResourceManager);

            

            

            base.Initialize();

            
            Components.Add(GameSettingsComponent);
        }

        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);
            
            base.LoadContent();
        }

        protected override void UnloadContent()
        {
            //GameSettingsComponent.DeserializeToFile();

            base.UnloadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            GameTimerManager.Update(gameTime);

            ScreenManagerComponent.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            ScreenManagerComponent.Draw(gameTime);

            base.Draw(gameTime);
        }

        public void AddScreen<T>(T screen, bool showImmediately = true) where T : Screen
        {
            ScreenManagerComponent.Register(screen);
            //if (showImmediately)
            //    screen.Show<T>();
        }
    }
}