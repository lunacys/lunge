using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace lunge.Library.Entities
{
    public class EntityManager
    {
        public World World { get; }

        public event Action<Entity> EntityAdded;
        public event Action<Entity> EntityRemoved;

        public int ActiveCount { get; private set; }
        public int Count => _entities.Count;

        private List<Entity> _entities;
        private List<Entity> _addedEntities;
        private List<Entity> _removedEntities;

        private int _nextId;

        public EntityManager(World world)
        {
            World = world;

            _entities = new List<Entity>();
            _addedEntities = new List<Entity>();
            _removedEntities = new List<Entity>();
        }

        public void Add(Entity entity)
        {
            entity.Id = _nextId++;
            entity.EntityManager = this;
            _addedEntities.Add(entity);
        }

        public void Destroy(Entity entity)
        {
            entity.Destroy();
            _removedEntities.Add(entity);
        }

        public void Update(GameTime gameTime)
        {
            foreach (var entity in _addedEntities)
            {
                _entities.Add(entity);
                entity.Initialize(World);
                ActiveCount++;
                EntityAdded?.Invoke(entity);
            }

            foreach (var entity in _removedEntities)
            {
                if (_entities.Contains(entity))
                    _entities.Remove(entity);
                entity.Deinitialize();
                ActiveCount--;
                EntityRemoved?.Invoke(entity);
            }

            _addedEntities.Clear();
            _removedEntities.Clear();

            foreach (var entity in _entities)
            {
                if (!entity.IsExpired)
                    entity.Update(gameTime);
                else 
                    _removedEntities.Add(entity);
            }
        }

        public void Draw(GameTime gameTime)
        {
            foreach (var entity in _entities)
            {
                entity.Draw(gameTime);
            }
        }
    }
}