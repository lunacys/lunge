using System.Collections.Generic;
using lunge.Library.Entities.Systems;
using Microsoft.Xna.Framework;

namespace lunge.Library.Entities
{
    public class WorldBuilder
    {
        private readonly IGame _game;

        private readonly List<ISystem> _systems;

        public WorldBuilder(IGame game)
        {
            _game = game;
            _systems = new List<ISystem>();
        }

        public WorldBuilder AddSystem(ISystem system)
        {
            _systems.Add(system);
            return this;
        }

        public World Build()
        {
            var world = new World(_game);

            foreach (var system in _systems)
            {
                world.RegisterSystem(system);
            }

            return world;
        }
    }
}