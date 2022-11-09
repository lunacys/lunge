namespace lunge.Library.InputMgr
{
    public interface IInputCommand
    {
        void Execute();
    }

    public interface IInputCommand<in T> where T : IInputHandleable
    {
        void Execute(T entity);
    }

    public class NullCommand : IInputCommand
    {
        public void Execute() { }
    }

    public class NullCommand<T> : IInputCommand<T> where T : IInputHandleable
    {
        public void Execute(T entity) { }
    }
}