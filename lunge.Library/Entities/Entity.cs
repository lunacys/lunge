using lunge.Library.Entities.Components;
using Microsoft.Xna.Framework;

namespace lunge.Library.Entities
{
    public abstract class Entity
    {
        public ComponentContainer Components { get; private set; }

        public int Id { get; internal set; }
        public EntityManager EntityManager { get; internal set; }

        public bool IsExpired { get; private set; }

        protected Entity()
        {
            Components = new ComponentContainer();
        }

        public void AddComponent(IComponent component)
        {
            Components.Add(component);
        }

        public T GetComponent<T>() where T : IComponent
        {
            return Components.Get<T>();
        }

        public void Destroy()
        {
            IsExpired = true;
        }

        public virtual void Initialize(World world)
        {
            foreach (var component in Components)
                component.Initialize(world);
        }

        public virtual void Deinitialize() { }

        public virtual void Update(GameTime gameTime)
        {
            foreach (var component in Components)
                component.Update(gameTime);
        }

        public virtual void Draw(GameTime gameTime)
        {
            foreach (var component in Components)
                component.Draw(gameTime);
        }
    }
}