using System;
using lunge.Library.Debugging.Logging;
using lunge.Library.Debugging.Profiling;
using Microsoft.Xna.Framework;
using Nez;
using Nez.ImGuiTools;
using Nez.Persistence;
using Nez.Persistence.Binary;
using Nez.UI;
using Playground.Scenes;
using Playground.Settings;

namespace Playground;


public class TestObject
{
    [JsonInclude]
    public bool ShouldEncode { get; set; } = true;
    public int ShouldNotEncode { get; set; } = 11;

}
public class GameRoot : Core
{
    private readonly ILogger _logger = LoggerFactory.GetLogger("GameRoot");
    private FileDataStore _dataStore;
    private GameSettingsService _gameSettings;

    public static float DeltaTimeMultiplier { get; private set; }

    public GameRoot()
    {
    }

    protected override void Initialize()
    {
        _logger.Debug("Started initializing...");

        var test = Json.ToJson(new TestObject());

        base.Initialize();

        _dataStore = new FileDataStore(Storage.GetStorageRoot(), FileDataStore.FileFormat.Binary);
        Services.AddService(_dataStore);

        _gameSettings = Services.GetOrAddService<GameSettingsService>();
        _gameSettings.Load();
        //_dataStore.Load("GameSettings.bin", _gameSettings);
        
        var imGuiManager = new ImGuiManager();
        RegisterGlobalManager(imGuiManager);

        Services.AddService(Skin.CreateDefaultSkin());

        _logger.Debug("Setting Spatial hash cell size");
        Physics.SpatialHashCellSize = 40;

        _logger.Debug("Setting up scene");
        try
        {
            Scene = new WorldGenerationScene();
        }
        catch (Exception e)
        {
            _logger.Log("FATAL: " + e.Message, LogLevel.Error);
            throw;
        }

        DebugRenderEnabled = false;

        DeltaTimeMultiplier = (float)Math.Round(1 / TargetElapsedTime.TotalSeconds);

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

        //_dataStore.Save("GameSettings.bin", _gameSettings);
        _gameSettings.Save();
    }
}