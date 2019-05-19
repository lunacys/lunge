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

            TryLoad();

            base.LoadContent();
        }

        public void TryLoad()
        {
            TryLoad(GameSettings, SettingsFileName);
        }

        public static void TryLoad(GameSettings gameSettings, string fileName)
        {
            if (File.Exists(fileName))
            {
                string str;
                using (StreamReader sr = new StreamReader(fileName))
                {
                    str = sr.ReadToEnd();
                }

                var tmp = JsonConvert.DeserializeObject<GameSettings>(str, new GameSettingsConverter());
                foreach (var gameSetting in tmp)
                {
                    gameSettings.Add(gameSetting.Key, gameSetting.Value);
                }
            }
        }

        protected override void UnloadContent()
        {
            string str = JsonConvert.SerializeObject(GameSettings, Formatting.Indented);

            using (StreamWriter sw = new StreamWriter(SettingsFileName))
            {
                sw.WriteLine(str);
            }

            base.UnloadContent();
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
            LogHelper.Log($"Scanning assembly: {assembly.FullName}");

            var types = assembly.GetTypes();
            foreach (var type in types)
            {
                var props = type.GetProperties()
                    .Where(prop => prop.IsDefined(typeof(GameSettingsEntryAttribute), false));

                //List<GameSettingsEntryAttribute> attributes = new List<GameSettingsEntryAttribute>();

                foreach (var prop in props)
                {
                    LogHelper.Log($"{nameof(GameSettingsGameComponent)}: Found setting property: {prop.Name}");

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