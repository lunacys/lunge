using System;

namespace lunge.Library.GameSystems
{
    public abstract class GameSystem : IDisposable
    {


        protected virtual void Dispose(bool disposing)
        {
            
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}