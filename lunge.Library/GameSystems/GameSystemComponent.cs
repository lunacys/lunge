using System;
using System.Collections.Generic;
using System.Linq;
using lunge.Library.Debugging.Logging;
using Microsoft.Xna.Framework;

namespace lunge.Library.GameSystems
{
    public class GameSystemComponent : DrawableGameComponent, IGameSystemManager
    {
        private readonly List<DrawableGameSystem> _gameSystems;

        /// <summary>
        /// <see cref="DrawableGameSystem"/> added
        /// </summary>
        public event EventHandler<GameSystemAddedEventArgs> SystemAdded;

        public GameSystemComponent(Game game, IEnumerable<DrawableGameSystem> systems) : this(game)
        {
            foreach (var gameSystem in systems)
                Register(gameSystem);
        }

        public GameSystemComponent(Game game) : base(game) 
        {
            _gameSystems = new List<DrawableGameSystem>();
        }

        /// <summary>
        /// Finds and returns <see cref="DrawableGameSystem"/> with specified type
        /// </summary>
        /// <typeparam name="T">Type of the system</typeparam>
        /// <returns><see cref="DrawableGameSystem"/></returns>
        public T FindSystem<T>() where T : DrawableGameSystem
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
        public IList<DrawableGameSystem> GetAllGameSystems()
        {
            return _gameSystems;
        }

        /// <summary>
        /// Registers and returns <see cref="DrawableGameSystem"/>
        /// </summary>
        /// <typeparam name="T"><see cref="DrawableGameSystem"/></typeparam>
        /// <param name="system"><see cref="DrawableGameSystem"/></param>
        /// <returns><see cref="DrawableGameSystem"/></returns>
        public T Register<T>(T system) where T : DrawableGameSystem
        {
            LogHelper.Log($"GameSystemManager: Registering System {typeof(T)}");
            SystemAdded?.Invoke(this, new GameSystemAddedEventArgs(this, system));

            system.GameSystemManager = this;
            system.IsWorking = true;
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
            foreach (var gameSystem in _gameSystems.Where(s => s.IsWorking))
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
            foreach (var gameSystem in _gameSystems.Where(s => s.IsWorking))
            {
                gameSystem.Draw(gameTime);
            }
        }
    }
}
