using lunge.Library.Entities;

namespace lunge.Library.Input
{
    public abstract class CommandMapper
    {
        protected CommandMapper()
        {
            
        }

        public void MapTo<T>(IInputCommand command) where T : Entity
        {
            
        }
    }
}