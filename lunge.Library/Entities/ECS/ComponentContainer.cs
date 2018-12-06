using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using lunge.Library.Entities.ECS.Components;

namespace lunge.Library.Entities
{
    public class ComponentContainer : IList<IComponent>
    {
        public int ActiveCount => _components.Count(c => c.IsActive);
        public int Count => _components.Count;
        public bool IsReadOnly => false;

        public event Action<IComponent> ComponentAdded;
        public event Action<IComponent> ComponentRemoved;

        private readonly List<IComponent> _components;

        public ComponentContainer()
        {
            _components = new List<IComponent>();
        }

        public IEnumerator<IComponent> GetEnumerator()
        {
            return _components.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(IComponent item)
        {
            _components.Add(item);
            ComponentAdded?.Invoke(item);
        }

        public T Add<T>() where T : IComponent, new()
        {
            var n = new T();
            _components.Add(n);
            return n;
        }

        public T Get<T>() where T : IComponent
        {
            return (T)_components.Find(c => c is T);
        }

        public void Clear()
        {
            foreach (var component in _components)
                Remove(component);
        }

        public bool Contains(IComponent item)
        {
            return _components.Contains(item);
        }

        public void CopyTo(IComponent[] array, int arrayIndex)
        {
            _components.CopyTo(array, arrayIndex);
        }

        public bool Remove(IComponent item)
        {
            var removedComponent = item;
            var result = _components.Remove(item);
            ComponentRemoved?.Invoke(removedComponent);
            return result;
        }

        public int IndexOf(IComponent item)
        {
            return _components.IndexOf(item);
        }

        public void Insert(int index, IComponent item)
        {
            _components.Insert(index, item);
            ComponentAdded?.Invoke(item);
        }

        public void RemoveAt(int index)
        {
            _components.RemoveAt(index);
        }

        public IComponent this[int index]
        {
            get => _components[index];
            set => _components[index] = value;
        }
    }
}