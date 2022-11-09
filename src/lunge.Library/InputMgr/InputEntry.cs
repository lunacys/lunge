using System;
using System.Collections.Generic;

namespace lunge.Library.InputMgr
{
    public struct InputEntry<T> : IEquatable<InputEntry<T>> where T : Enum
    {
        public T Key { get; }
        public Func<T, bool> Func { get; }

        public InputEntry(T key, Func<T, bool> func)
        {
            Key = key;
            Func = func;
        }

        public bool Equals(InputEntry<T> other)
        {
            return EqualityComparer<T>.Default.Equals(Key, other.Key) && Func.Equals(other.Func);
        }

        public override bool Equals(object obj)
        {
            return obj is InputEntry<T> other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Key, Func);
        }
    }
}