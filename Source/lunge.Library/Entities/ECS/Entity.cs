using System;

namespace lunge.Library.Entities.ECS
{
    public class Entity : IEquatable<Entity>
    {
        private readonly EntityManager _entityManager;
        private readonly ComponentManager _componentManager;

        public int Id { get; }

        internal Entity(int id, EntityManager entityManager, ComponentManager componentManager)
        {
            Id = id;
            _entityManager = entityManager;
            _componentManager = componentManager;
        }

        public void Destroy()
        {
            _entityManager.Destroy(Id);
        }

        public bool Equals(Entity other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Id == other.Id;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Entity) obj);
        }

        public override int GetHashCode()
        {
            return Id;
        }

        public static bool operator ==(Entity left, Entity right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Entity left, Entity right)
        {
            return !Equals(left, right);
        }
    }
}