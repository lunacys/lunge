using System;
using System.IO;
using System.Linq;
using System.Reflection;
using lunge.Library.Debugging.Logging;
using lunge.Library.Serialization;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;

namespace lunge.Library.Settings
{
    public class GameSettingsGameComponent : DrawableGameComponent
    {
        public GameSettings GameSettings { get; private set; }

        public string SettingsFileName { get; set; }

        private bool _doScanAssembly;

        public object this[string name] => GameSettings.Get(name);

        private static readonly LogHelper Logger = LoggerFactory.GetLogger("GameSettingsGameComponent");

        public GameSettingsGameComponent(Game game, GameSettings gameSettings = null, bool doScanAssembly = true)
            : base(game)
        {
            GameSettings = gameSettings ?? new GameSettings();
            SettingsFileName = "Settings.json";
            _doScanAssembly = doScanAssembly;
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            if (_doScanAssembly)
                Scan(Assembly.GetEntryAssembly());

            if (!TryLoad())
            {
                GameSettings.LoadDefaults();
            }

            base.LoadContent();
        }

        public bool TryLoad()
        {
            return TryLoad(GameSettings, SettingsFileName);
        }

        public static bool TryLoad(GameSettings gameSettings, string fileName)
        {
            if (File.Exists(fileName))
            {
                string str;
                using (StreamReader sr = new StreamReader(fileName))
                {
                    str = sr.ReadToEnd();
                }

                var tmp = JsonConvert.DeserializeObject<GameSettings>(str);
                foreach (var gameSetting in tmp)
                {
                    gameSettings.Add(gameSetting.Key, gameSetting.Value);
                }

                return true;
            }

            return false;
        }

        protected override void UnloadContent()
        {
            // SerializeToFile();

            base.UnloadContent();
        }

        public void SerializeToFile()
        {
            string str = JsonConvert.SerializeObject(GameSettings, Formatting.Indented);

            using (StreamWriter sw = new StreamWriter(SettingsFileName))
            {
                sw.WriteLine(str);
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public T Get<T>(string name)
        {
            var obj = GameSettings.Get(name);

            if (!(obj is T))
                throw new InvalidCastException();

            return (T)obj;
        }

        private void Scan(Assembly assembly)
        {
            ScanAssembly(assembly, GameSettings);
        }

        public static void ScanAssembly(Assembly assembly, GameSettings gameSettings)
        {
            Logger.Log($"Scanning assembly: {assembly.FullName}");

            var types = assembly.GetTypes();
            foreach (var type in types)
            {
                var props = type.GetProperties()
                    .Where(prop => prop.IsDefined(typeof(GameSettingsEntryAttribute), false));

                //List<GameSettingsEntryAttribute> attributes = new List<GameSettingsEntryAttribute>();

                foreach (var prop in props)
                {
                    Logger.Log($"Found setting property: {prop.Name}");

                    var attr = (GameSettingsEntryAttribute)prop.GetCustomAttribute(typeof(GameSettingsEntryAttribute), false);

                    //attributes.Add(attr);
                    string name = !string.IsNullOrEmpty(attr.SettingName) ? attr.SettingName : prop.Name;
                    object value = attr.DefaultValue;

                    gameSettings.Add(name, value);
                }
            }
        }
    }
}