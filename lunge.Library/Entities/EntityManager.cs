using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using lunge.Library.Entities.Systems;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Collections;

namespace lunge.Library.Entities
{
    public class EntityManager : UpdateSystem
    {
        public int ActiveCount { get; private set; }

        public int Capacity => _entities.Capacity;
        public IEnumerable<int> Entities => _entities.Where(e => e != null).Select(e => e.Id);

        public event Action<int> EntityAdded;
        public event Action<int> EntityRemoved;
        public event Action<int> EntityChanged;

        private const int DefaultListSize = 128;

        private List<Entity> _entities;
        private List<int> _addedEntities;
        private List<int> _removedEntities;
        private List<int> _updatedEntities;

        private readonly ComponentManager _componentManager;

        private int _nextId;

        public EntityManager(ComponentManager componentManager)
        {
            _componentManager = componentManager;
            _entities = new List<Entity>(DefaultListSize);
            _addedEntities = new List<int>(DefaultListSize);
            _removedEntities = new List<int>(DefaultListSize);
            _updatedEntities = new List<int>(DefaultListSize);

            _componentManager.ComponentsChanged += OnComponentsChanged;
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var entityId in _addedEntities)
            {
                _entityToComponentBits[entityId] = _componentManager.CreateComponentBits(entityId);
                ActiveCount++;
                EntityAdded?.Invoke(entityId);
            }

            foreach (var entityId in _updatedEntities)
            {
                _entityToComponentBits[entityId] = _componentManager.CreateComponentBits(entityId);
                EntityChanged?.Invoke(entityId);
            }

            foreach (var entityId in _removedEntities)
            {
                var entity = _entities[entityId];
                _entities.Remove(entity);
                _componentManager.Destroy(entityId);
                ActiveCount--;
                
                EntityRemoved?.Invoke(entityId);
            }

            _addedEntities.Clear();
            _removedEntities.Clear();
            _updatedEntities.Clear();
        }

        public Entity Create()
        {
            var entity = new Entity(_nextId++, this, _componentManager);
            var id = entity.Id;
            _entities.Add(entity);
            _addedEntities.Add(id);
            return entity;
        }

        public Entity Get(int entityId)
        {
            return _entities[entityId];
        }

        public void Destroy(int entityId)
        {
            Debug.Assert(!_removedEntities.Contains(entityId));
            _removedEntities.Add(entityId);
        }

        public void Destroy(Entity entity)
        {
            Destroy(entity.Id);
        }

        private void OnComponentsChanged(int entityId)
        {
            _updatedEntities.Add(entityId);
        }
    }
}