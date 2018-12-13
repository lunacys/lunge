using System;

namespace lunge.Library.Entities.ECS.Systems
{
    public interface ISystem : IDisposable
    {
        void Initialize(World world);
    }
}