using System.IO;
using Nez.Persistence;

namespace Playground.Settings;

public class GameSettingsService
{
    public GameSettings GameSettings { get; private set; } = GameSettings.Default;
    public string Filename { get; set; } = "Settings.json";

    public void Save()
    {
        var jsonString = Json.ToJson(GameSettings, true);
        File.WriteAllText(Filename, jsonString);
    }

    public void Load()
    {
        if (!File.Exists(Filename))
            return;

        var jsonString = File.ReadAllText(Filename);

        if (string.IsNullOrEmpty(jsonString) || jsonString.ToLower() == "null")
            GameSettings = GameSettings.Default;
        
        GameSettings = Json.FromJson<GameSettings>(jsonString);
    }
}