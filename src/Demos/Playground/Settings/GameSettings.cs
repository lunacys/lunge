using Microsoft.Xna.Framework.Input;
using Nez.Persistence;
using Nez.Persistence.Binary;

namespace Playground.Settings;

public class GameSettings : IPersistable
{
    private static readonly GameSettings _default = new GameSettings
    {
        FullScreen = false,
        ExitKey = Keys.Escape,
        WindowWidth = 1920,
        WindowHeight = 1080
    };

    public static GameSettings Default => _default;

    [JsonInclude]
    public bool FullScreen { get; set; } = false;
    [JsonInclude]
    public Keys ExitKey { get; set; } = Keys.Escape;
    [JsonInclude]
    public int WindowWidth { get; set; } = 1920;
    [JsonInclude]
    public int WindowHeight { get; set; } = 1080;

    public void Recover(IPersistableReader reader)
    {
        FullScreen = reader.ReadBool();
        ExitKey = (Keys)reader.ReadInt();
        WindowWidth = reader.ReadInt();
        WindowHeight = reader.ReadInt();
    }

    public void Persist(IPersistableWriter writer)
    {
        writer.Write(FullScreen);
        writer.Write((int)ExitKey);
        writer.Write(WindowWidth);
        writer.Write(WindowHeight);
    }
}