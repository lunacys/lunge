using System;
using System.Collections;
using System.Collections.Generic;

namespace lunge.Library.Input
{
    public class InputCommandMapper<T> : IEnumerable<KeyValuePair<InputEntry<T>, IInputCommand>> where T : Enum
    {
        private readonly Dictionary<InputEntry<T>, IInputCommand> _commandMap;

        public IInputCommand this[InputEntry<T> entry]
        {
            get => Get(entry);
            set => Map(entry, value);
        }

        public InputCommandMapper()
        {
            _commandMap = new Dictionary<InputEntry<T>, IInputCommand>();
        }

        public void Map(T key, Func<T, bool> handler, IInputCommand command)
        {
            Map(new InputEntry<T>(key, handler), command);
        }

        public void Map(InputEntry<T> entry, IInputCommand command)
        {
            if (_commandMap.ContainsKey(entry))
                return;
            
            _commandMap[entry] = command;
        }

        public IInputCommand GetForKeyAndHandler(T key, Func<T, bool> handler)
        {
            return Get(new InputEntry<T>(key, handler));
        }

        public IInputCommand Get(InputEntry<T> entry)
        {
            return _commandMap[entry];
        }

        public void Remove(InputEntry<T> entry)
        {
            _commandMap.Remove(entry);
        }

        public IEnumerator<KeyValuePair<InputEntry<T>, IInputCommand>> GetEnumerator()
        {
            return _commandMap.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public class InputCommandMapper<T, TK> : IEnumerable<KeyValuePair<InputEntry<T>, IInputCommand<TK>>> where T : Enum where TK : IInputHandleable
    {
        private readonly Dictionary<InputEntry<T>, IInputCommand<TK>> _commandMap;

        public IInputCommand<TK> this[InputEntry<T> entry]
        {
            get => Get(entry);
            set => Map(entry, value);
        }

        public InputCommandMapper()
        {
            _commandMap = new Dictionary<InputEntry<T>, IInputCommand<TK>>();
        }

        public void Map(T key, Func<T, bool> handler, IInputCommand<TK> command)
        {
            Map(new InputEntry<T>(key, handler), command);
        }

        public void Map(InputEntry<T> entry, IInputCommand<TK> command)
        {
            if (_commandMap.ContainsKey(entry))
                return;

            _commandMap[entry] = command;
        }

        public IInputCommand<TK> GetForKeyAndHandler(T key, Func<T, bool> handler)
        {
            return Get(new InputEntry<T>(key, handler));
        }

        public IInputCommand<TK> Get(InputEntry<T> entry)
        {
            return _commandMap[entry];
        }

        public void Remove(InputEntry<T> entry)
        {
            _commandMap.Remove(entry);
        }

        public IEnumerator<KeyValuePair<InputEntry<T>, IInputCommand<TK>>> GetEnumerator()
        {
            return _commandMap.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}