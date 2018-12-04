using System;

namespace lunge.Library.Entities.Systems
{
    public interface ISystem : IDisposable
    {
        void Initialize(World world);
    }
}