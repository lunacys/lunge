using System;
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
            Register(entry.Key, entry.Func, command);
        }

        public void Register(T key, Func<T, bool> handler, IInputCommand<TK> command)
        {
            _commandMapper.Map(key, handler, command);
        }

        public void Remove(InputEntry<T> entry)
        {
            _commandMapper.Remove(entry);
        }

        /// <summary>
        /// Handles all the registered actions and returns all the actions that
        /// satisfy the handler condition. Note: because of yield return this method
        /// may be relatively slow and CPU-dependent.
        /// </summary>
        /// <returns><see cref="IEnumerable{T}"/> of <see cref="IInputCommand{T}"/></returns>
        public IEnumerable<IInputCommand<TK>> Handle()
        {
            foreach (var command in _commandMapper)
            {
                if (command.Key.Func(command.Key.Key))
                    yield return command.Value;
            }

            yield return NullCommand;
        }

        public void Handle(TK handleable)
        {
            foreach (var cmd in _commandMapper)
            {
                if (cmd.Key.Func(cmd.Key.Key))
                    cmd.Value.Execute(handleable);
            }
        }
    }
}