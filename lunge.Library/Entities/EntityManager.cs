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

        private Dictionary<int, Entity> _entities;
        private List<Entity> _addedEntities;
        private List<Entity> _removedEntities;

        private int _nextId;

        public EntityManager(World world)
        {
            World = world;

            _entities = new Dictionary<int, Entity>();
            _addedEntities = new List<Entity>();
            _removedEntities = new List<Entity>();
        }

        public void Add(Entity entity)
        {
            entity.Id = _nextId++;
            entity.EntityManager = this;
            _addedEntities.Add(entity);
        }

        public Entity Get(int entityId)
        {
            return _entities[entityId];
        }

        public void Destroy(Entity entity)
        {
            entity.Destroy();
            _removedEntities.Add(entity);
        }

        public void Destroy(int entityId)
        {
            var entity = _entities[entityId];
            entity.Destroy();
            _removedEntities.Add(entity);
        }

        public void Update(GameTime gameTime)
        {
            foreach (var entity in _addedEntities)
            {
                _entities[entity.Id] = entity;
                entity.Initialize(World);
                ActiveCount++;
                EntityAdded?.Invoke(entity);
            }

            foreach (var entity in _removedEntities)
            {
                if (_entities.ContainsKey(entity.Id))
                    _entities.Remove(entity.Id);
                entity.Deinitialize();
                ActiveCount--;
                EntityRemoved?.Invoke(entity);
            }

            _addedEntities.Clear();
            _removedEntities.Clear();

            foreach (var entity in _entities)
            {
                if (!entity.Value.IsExpired)
                    entity.Value.Update(gameTime);
                else 
                    _removedEntities.Add(entity.Value);
            }
        }

        public void Draw(GameTime gameTime)
        {
            foreach (var entity in _entities)
            {
                entity.Value.Draw(gameTime);
            }
        }
    }
}