using System;
using System.Collections.Generic;
using System.Linq;
using lunge.Library.Debugging.Logging;
using Microsoft.Xna.Framework;

namespace lunge.Library.GameSystems
{
    public class GameSystemComponent : DrawableGameComponent, IGameSystemManager
    {
        private readonly List<GameSystem> _gameSystems;

        /// <summary>
        /// <see cref="GameSystem"/> added
        /// </summary>
        public event EventHandler<GameSystemAddedEventArgs> SystemAdded;

        public GameSystemComponent(Game game, IEnumerable<GameSystem> systems) : this(game)
        {
            foreach (var gameSystem in systems)
                Register(gameSystem);
        }

        public GameSystemComponent(Game game) : base(game) 
        {
            _gameSystems = new List<GameSystem>();
        }

        /// <summary>
        /// Finds and returns <see cref="GameSystem"/> with specified type
        /// </summary>
        /// <typeparam name="T">Type of the system</typeparam>
        /// <returns><see cref="GameSystem"/></returns>
        public T FindSystem<T>() where T : GameSystem
        {
            var system = _gameSystems.OfType<T>().FirstOrDefault();

            if (system == null)
                throw new InvalidOperationException($"{typeof(T).Name} not registered");

            return system;
        }

        /// <summary>
        /// Gets all registered game systems
        /// </summary>
        /// <returns>All registered game systems</returns>
        public IList<GameSystem> GetAllGameSystems()
        {
            return _gameSystems;
        }

        /// <summary>
        /// Registers and returns <see cref="GameSystem"/>
        /// </summary>
        /// <typeparam name="T"><see cref="GameSystem"/></typeparam>
        /// <param name="system"><see cref="GameSystem"/></param>
        /// <returns><see cref="GameSystem"/></returns>
        public T Register<T>(T system) where T : GameSystem
        {
            LogHelper.Log($"GameSystemManager: Registering System {typeof(T)}");
            SystemAdded?.Invoke(this, new GameSystemAddedEventArgs(this, system));

            system.GameSystemManager = this;
            system.IsActive = true;
            _gameSystems.Add(system);

            return system;
        }

        /// <summary>
        /// Resets all game systems
        /// </summary>
        public void Reset()
        {
            foreach (var gameSystem in _gameSystems)
            {
                gameSystem.Reset();
            }
        }

        /// <summary>
        /// Initializes all game systems
        /// </summary>
        public override void Initialize()
        {
            foreach (var gameSystem in _gameSystems)
            {
                LogHelper.Log($"GameSystemManager: Initializing System {gameSystem}");
                gameSystem.Initialize();
                LogHelper.Log($"GameSystemManager: End Initializing System {gameSystem}");
            }
        }

        /// <summary>
        /// Updates all game systems
        /// </summary>
        /// <param name="gameTime"><see cref="GameTime"/></param>
        public override void Update(GameTime gameTime)
        {
            foreach (var gameSystem in _gameSystems.Where(s => s.IsActive))
            {
                gameSystem.Update(gameTime);
            }
        }
        
        /// <summary>
        /// Draws all game systems
        /// </summary>
        /// <param name="gameTime"><see cref="GameTime"/></param>
        public override void Draw(GameTime gameTime)
        {
            foreach (var gameSystem in _gameSystems.Where(s => s.IsActive))
            {
                (gameSystem as DrawableGameSystem)?.Draw(gameTime);
            }
        }
    }
}
