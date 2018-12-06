using System;
using System.Collections.Generic;
using System.Linq;
using lunge.Library.Debugging.Logging;
using Microsoft.Xna.Framework;

namespace lunge.Library.Entities.Systems
{
    public class SystemManager : ISystemManager
    {
        private readonly List<ISystem> _gameSystems;

        public World World { get; }

        /// <summary>
        /// <see cref="UpdateSystem"/> added
        /// </summary>
        public event EventHandler<SystemAddedEventArgs> SystemAdded;

        public SystemManager(World world, IEnumerable<ISystem> systems) 
            : this(world)
        {
            foreach (var gameSystem in systems)
                Register(gameSystem);
        }

        public SystemManager(World world)
        {
            World = world;
            _gameSystems = new List<ISystem>();
        }

        /// <summary>
        /// Finds and returns <see cref="UpdateSystem"/> with specified type
        /// </summary>
        /// <typeparam name="T">Type of the system</typeparam>
        /// <returns><see cref="UpdateSystem"/></returns>
        public T FindSystem<T>() where T : ISystem
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
        public IList<ISystem> GetAllGameSystems()
        {
            return _gameSystems;
        }

        /// <summary>
        /// Registers and returns <see cref="UpdateSystem"/>
        /// </summary>
        /// <typeparam name="T"><see cref="UpdateSystem"/></typeparam>
        /// <param name="system"><see cref="UpdateSystem"/></param>
        /// <returns><see cref="UpdateSystem"/></returns>
        public T Register<T>(T system) where T : ISystem
        {
            LogHelper.Log($"GameSystemManager: Registering System {typeof(T)}");
            SystemAdded?.Invoke(this, new SystemAddedEventArgs(this, system));

            system.SystemManager = this;
            system.IsActive = true;
            _gameSystems.Add(system);

            return system;
        }

        /// <summary>
        /// Initializes all game systems
        /// </summary>
        public void Initialize()
        {
            Console.WriteLine("Intitialize()");
            foreach (var gameSystem in _gameSystems)
            {
                LogHelper.Log($"GameSystemManager: Initializing System {gameSystem}");
                gameSystem.Initialize(World);
                LogHelper.Log($"GameSystemManager: End Initializing System {gameSystem}");
            }
        }

        /// <summary>
        /// Updates all game systems
        /// </summary>
        /// <param name="gameTime"><see cref="GameTime"/></param>
        public void Update(GameTime gameTime)
        {
            foreach (var gameSystem in _gameSystems.Where(s => s.IsActive))
                (gameSystem as IUpdateSystem)?.Update(gameTime);
        }
        
        /// <summary>
        /// Draws all game systems
        /// </summary>
        /// <param name="gameTime"><see cref="GameTime"/></param>
        public void Draw(GameTime gameTime)
        {
            foreach (var gameSystem in _gameSystems.Where(s => s.IsActive))
                (gameSystem as IDrawSystem)?.Draw(gameTime);
        }
    }
}
