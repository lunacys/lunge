using System;
using System.Collections.Generic;
using System.Linq;
using lunge.Library.Entities.Systems;
using Microsoft.Xna.Framework;

namespace lunge.Library.Entities
{
    public class EntityManager : DrawSystem
    {
        public event Action<Entity> EntityAdded;
        public event Action<Entity> EntityRemoved;

        public int ActiveCount { get; private set; }
        public int Count => _entities.Count;

        private Dictionary<int, Entity> _entities;
        private List<Entity> _addedEntities;
        private List<Entity> _removedEntities;

        private int _nextId;

        private World _world;

        public EntityManager(World world)
        {
            _world = world;

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

        public T Get<T>() where T : Entity
        {
            return (T)_entities.Values.First(entity => entity.GetType() == typeof(T));
        }

        public IEnumerable<T> GetAll<T>() where T : Entity
        {
	        return _entities.Values.ToList().FindAll(entity => entity.GetType() == typeof(T)).Cast<T>();
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
        
        public override void Update(GameTime gameTime)
        {
            foreach (var entity in _addedEntities)
            {
                _entities[entity.Id] = entity;
                entity.Initialize(_world);
                ActiveCount++;
                EntityAdded?.Invoke(entity);
            }

            foreach (var entity in _removedEntities)
            {
                if (_entities.ContainsKey(entity.Id))
                {
                    _entities[entity.Id] = null;
                    _entities.Remove(entity.Id);
                    ActiveCount--;
                    EntityRemoved?.Invoke(entity);
                }
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

        public override void Draw(GameTime gameTime)
        {
            foreach (var entity in _entities)
            {
                entity.Value.Draw(gameTime);
            }
        }
    }
}