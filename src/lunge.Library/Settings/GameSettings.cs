using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Nez.Persistence;

namespace lunge.Library.Settings
{
    public class GameSettings : IEnumerable<KeyValuePair<string, object>>
    {
        private readonly Dictionary<string, object> _gameSettings;

        public object this[string name] => Get(name);

        [JsonExclude]
        public DefaultGameSettings DefaultGameSettings { get; }

        public GameSettings()
        {
            _gameSettings = new Dictionary<string, object>();

            DefaultGameSettings = GetDefaultGameSettings();
        }

        public void Add(string name, object value)
        {
            if (_gameSettings.Count != 0 && _gameSettings.ContainsKey(name))
                return;

            _gameSettings[name] = value;
        }

        public object Get(string name)
        {
            if (!_gameSettings.ContainsKey(name))
            {
                throw new ArgumentException("Cannot find setting with this name", name);
            }

            return _gameSettings[name];
        }

        public void LoadDefaults()
        {
            GameSettingsGameComponent.ScanAssembly(Assembly.GetCallingAssembly(), this);
            //FillDefaultGameSettings(DefaultGameSettings);
        }

        private void FillDefaultGameSettings(DefaultGameSettings defaultGameSettings)
        {
            Add(nameof(defaultGameSettings.IsFullScreen), defaultGameSettings.IsFullScreen);
            Add(nameof(defaultGameSettings.WindowWidth), defaultGameSettings.WindowWidth);
            Add(nameof(defaultGameSettings.WindowHeight), defaultGameSettings.WindowHeight);
            Add(nameof(defaultGameSettings.IsMouseVisible), defaultGameSettings.IsMouseVisible);
        }

        public static DefaultGameSettings GetDefaultGameSettings()
        {
            return new DefaultGameSettings();
        }

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return _gameSettings.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}