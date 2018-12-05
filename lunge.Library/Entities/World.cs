using System;
using System.Collections.Generic;
using lunge.Library.Entities.Systems;
using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace lunge.Library.Entities
{
    public class World : SimpleDrawableGameComponent
    {
        private List<IUpdateSystem> _updateSystems;
        private List<IDrawSystem> _drawSystems;

        internal EntityManager EntityManager { get; }
        internal ComponentManager ComponentManager { get; }

        public int EntityCount => EntityManager.ActiveCount;

        internal World()
        {
            _updateSystems = new List<IUpdateSystem>();
            _drawSystems = new List<IDrawSystem>();

            RegisterSystem(ComponentManager = new ComponentManager());
            RegisterSystem(EntityManager = new EntityManager(ComponentManager));
        }

        public void RegisterSystem(ISystem system)
        {
            if (system is IUpdateSystem updateSystem)
                _updateSystems.Add(updateSystem);
            else if (system is IDrawSystem drawSystem)
                _drawSystems.Add(drawSystem);
            
            system.Initialize(this);
        }

        public Entity CreateEntity()
        {
            return EntityManager.Create();
        }

        public Entity GetEntity(int entityId)
        {
            return EntityManager.Get(entityId);
        }

        public void DestroyEntity(int entityId)
        {
            EntityManager.Destroy(entityId);
        }

        public void DestroyEntity(Entity entity)
        {
            EntityManager.Destroy(entity);
        }

        public override void Dispose()
        {
            foreach (var updateSystem in _updateSystems)
                updateSystem.Dispose();

            foreach (var drawSystem in _drawSystems)
                drawSystem.Dispose();

            base.Dispose();
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var system in _updateSystems)
                system.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            foreach (var system in _drawSystems)
                system.Draw(gameTime);
        }
    }
}