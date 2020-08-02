using System;
using System.Collections.Generic;

namespace lunge.Library.Resources
{
    public class ResourceManager : IResourceManager
    {
        private readonly List<object> _resources = new List<object>();

        public void Insert<T>(T provider)
        {
            if (TryFetch(out object _))
                throw new Exception("Resource already exists");

            _resources.Add(provider);
        }

        public bool FetchOrAdd<T>(out T obj)
        {
            obj = default(T);
            if (!TryFetch(out obj))
            {
                Insert(obj);
                return false;
            }

            return true;
        }

        public bool TryFetch<T>(out T obj)
        {
            obj = default(T);

            var res = _resources.Find(o => o.GetType() == typeof(T));
            if (res == null)
                return false;
            obj = (T) res;

            return true;
        }

        public void Delete<T>()
        {
            object obj;
            if (!TryFetch(out obj))
                throw new Exception($"There's no resource of type {typeof(T)}.");

            _resources.Remove(obj);
        }

        public void Clear()
        {
            _resources.Clear();
        }
    }
}