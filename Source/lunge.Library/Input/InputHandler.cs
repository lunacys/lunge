using System.Collections.Generic;

namespace lunge.Library.Input
{
    public class InputHandler<T> where T : System.Enum
    {
        private readonly InputCommandMapper<T> _commandMapper;

        public InputHandler()
        {
            _commandMapper = new InputCommandMapper<T>();
        }

        public void Register(InputEntry<T> entry, IInputCommand command)
        {
            _commandMapper.Map(entry, command);
        }

        public void Remove(InputEntry<T> entry)
        {
            _commandMapper.Remove(entry);
        }

        public void Handle()
        {
            foreach (var command in _commandMapper)
            {
                if (command.Key.Func(command.Key.Key))
                    command.Value.Execute();
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T">Key enum to use, for example Keys or MouseButton</typeparam>
    /// <typeparam name="TK">Class for which input handling will be used</typeparam>
    public class InputHandler<T, TK> where T : System.Enum where TK : IInputHandleable
    {
        public static readonly IInputCommand<TK> NullCommand = new NullCommand<TK>();

        private readonly InputCommandMapper<T, TK> _commandMapper;

        public InputHandler()
        {
            _commandMapper = new InputCommandMapper<T, TK>();
        }

        public void Register(InputEntry<T> entry, IInputCommand<TK> command)
        {
            _commandMapper.Map(entry, command);
        }

        public void Remove(InputEntry<T> entry)
        {
            _commandMapper.Remove(entry);
        }

        public IEnumerable<IInputCommand<TK>> Handle()
        {
            foreach (var command in _commandMapper)
            {
                if (command.Key.Func(command.Key.Key))
                    yield return command.Value;
            }

            yield return NullCommand;
        }
    }
}