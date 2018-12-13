using System;
using System.Collections.Generic;
using System.Linq;
using lunge.Library.Entities.Systems;
using Microsoft.Xna.Framework;

namespace lunge.Library.Entities
{
    public class World : DrawableGameComponent, ISystemManager
    {
        public Game GameRoot { get; }

        internal EntityManager EntityManager { get; }
        
        private List<IUpdateSystem> _updateSystems;
        private List<IDrawSystem> _drawSystems;

        public int EntityCount => EntityManager.ActiveCount;

        public event EventHandler<SystemAddedEventArgs> SystemAdded;

        internal World(Game game)
            : base(game)
        {
            GameRoot = game;

            _updateSystems = new List<IUpdateSystem>();
            _drawSystems = new List<IDrawSystem>();

            EntityManager = new EntityManager(this);
        }

        public void RegisterSystem<T>(T system) where T : ISystem
        {
            system.SystemManager = this;
            system.IsActive = true;

            if (system is IUpdateSystem updateSystem)
                _updateSystems.Add(updateSystem);
            if (system is IDrawSystem drawSystem)
                _drawSystems.Add(drawSystem);

            SystemAdded?.Invoke(this, new SystemAddedEventArgs(this, system));
            system.Initialize(this);
        }

        public T FindSystem<T>() where T : ISystem
        {
            var system = _updateSystems.OfType<T>().First();

            if (system == null)
                system = _drawSystems.OfType<T>().First();

            if (system == null)
                throw new InvalidOperationException($"{typeof(T).Name} not registered");

            return system;
        }

        public void AddEntity(Entity entity)
        {
            EntityManager.Add(entity);
        }

        public Entity GetEntity(int entityId)
        {
            return EntityManager.Get(entityId);
        }

        public T GetEntity<T>() where T : Entity
        {
            return EntityManager.Get<T>();
        }

        public void DestroyEntity(int entityId)
        {
            EntityManager.Destroy(entityId);
        }

        public void DestroyEntity(Entity entity)
        {
            EntityManager.Destroy(entity);
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var system in _updateSystems)
                system.Update(gameTime);

            EntityManager.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            foreach (var system in _drawSystems)
                system.Draw(gameTime);

            EntityManager.Draw(gameTime);
        }
    }
}