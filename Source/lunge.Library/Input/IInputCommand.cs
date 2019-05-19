using lunge.Library.Entities;

namespace lunge.Library.Input
{
    public interface IInputCommand
    {
        void Execute(Entity entity);
    }
}