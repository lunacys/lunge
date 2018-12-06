using System.Collections.Generic;
using lunge.Library.Entities.Systems;
using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace lunge.Library.Entities
{
    public class World : DrawableGameComponent
    {
        public Game GameRoot { get; }

        internal EntityManager EntityManager { get; }
        internal SystemManager SystemManager { get; }

        private List<IUpdateSystem> _updateSystems;
        private List<IDrawSystem> _drawSystems;

        public int EntityCount => EntityManager.ActiveCount;

        internal World(Game game)
            : base(game)
        {
            GameRoot = game;

            _updateSystems = new List<IUpdateSystem>();
            _drawSystems = new List<IDrawSystem>();

            EntityManager = new EntityManager(this);
            SystemManager = new SystemManager(this);
        }

        public void RegisterSystem<T>(T system) where T : ISystem
        {
            SystemManager.Register(system);
        }

        public ISystem FindSystem<T>() where T : ISystem
        {
            return SystemManager.FindSystem<T>();
        }

        public void AddEntity(Entity entity)
        {
            EntityManager.Add(entity);
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

        public override void Update(GameTime gameTime)
        {
            SystemManager.Update(gameTime);
            EntityManager.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            SystemManager.Draw(gameTime);
            EntityManager.Draw(gameTime);
        }
    }
}