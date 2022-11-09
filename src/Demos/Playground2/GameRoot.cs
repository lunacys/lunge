using System;
using System.Collections.Generic;
using lunge.Library.Debugging.Logging;
using lunge.Library.Debugging.Logging.Loggers;
using lunge.Library.Debugging.Profiling;
using Microsoft.Xna.Framework;
using Nez;
using Nez.ImGuiTools;
using Nez.Persistence.Binary;
using Playground2.Scenes;

namespace Playground2
{
    public class GameRoot : Core
    {
        private readonly ILogger _logger = LoggerFactory.GetLogger("GameRoot");
        private FileDataStore _dataStore;

        public static float DeltaTimeMultiplier { get; private set; }

        public GameRoot()
        {
            LoggerFactory.DefaultLoggers = new List<ILoggerFrontend>
            {
                new ConsoleLogger(),
                new FileLogger(),
                new DiagnosticsLogger()
            };
        }

        protected override void Initialize()
        {
            _logger.Debug("Started initializing...");
            
            base.Initialize();

            _dataStore = new FileDataStore("Saves", FileDataStore.FileFormat.Text);
            Services.AddService(_dataStore);

            _dataStore.Save("save.sav", new TestData());

            var imGuiManager = new ImGuiManager();
            Core.RegisterGlobalManager(imGuiManager);

            _logger.Debug("Setting Spatial hash cell size");
            Physics.SpatialHashCellSize = 40;
            
            _logger.Debug("Setting up scene");
            try
            {
                Scene = new SteeringBehaviorsScene();  //new ModTestingScene(); // new TestScene(Core.GraphicsDevice);
            }
            catch (Exception e)
            {
                _logger.Log("FATAL: " + e.Message, LogLevel.Error);
                throw;
            }
            
            DebugRenderEnabled = true;

            DeltaTimeMultiplier = (float)Math.Round(1/TargetElapsedTime.TotalSeconds);

            _logger.Debug("Ended initializing");
        }

        protected override void Update(GameTime gameTime)
        {
            GlobalTimeManager.TimeAction(() =>
            {
                base.Update(gameTime);
            }, "GameRoot.Update");
        }

        protected override void Draw(GameTime gameTime)
        {
            GlobalTimeManager.TimeAction(() =>
            {
                base.Draw(gameTime);
            }, "GameRoot.Draw");
        }

        protected override void UnloadContent()
        {
            base.UnloadContent();

            //KeyValueDataStore.Default.Flush(_dataStore);
        }
    }

    class TestData : IPersistable
    {
        public int Integer { get; set; } = 1337;
        public string String { get; set; } = "hello!";
        public bool Boolean { get; set; } = true;

        public void Recover(IPersistableReader reader)
        {
            Integer = reader.ReadInt();
            String = reader.ReadString();
            Boolean = reader.ReadBool();
        }

        public void Persist(IPersistableWriter writer)
        {
            writer.Write(Integer);
            writer.Write(String);
            writer.Write(Boolean);
        }
    }
}