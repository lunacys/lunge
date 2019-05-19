namespace lunge.Library.Settings
{
    public class DefaultGameSettings
    {
        [GameSettingsEntry("IsMouseVisible", true)]
        public bool IsMouseVisible { get; set; }

        [GameSettingsEntry("WindowWidth", 800)]
        public int WindowWidth { get; set; }

        [GameSettingsEntry("WindowHeight", 600)]
        public int WindowHeight { get; set; }

        [GameSettingsEntry("IsFullScreen", false)]
        public bool IsFullScreen { get; set; }

        internal DefaultGameSettings()
        {
            
        }
    }
}