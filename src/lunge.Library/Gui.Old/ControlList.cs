using System;
using System.Collections;
using System.Collections.Generic;

namespace lunge.Library.Gui.Old
{
    /// <summary>
    /// Represents an internal list of <see cref="IControl"/>.
    /// The only publicly accessible members are <see cref="Count"/> and <see cref="Capacity"/>
    /// </summary>
    [Obsolete("This GUI system is obsolete, please use the new one from the lunge.Library.Gui namespace")]
    public class ControlList : IEnumerable<IControl>
    {
        private readonly List<IControl> _controls;

        public int Count => _controls.Count;
        public int Capacity => _controls.Capacity;

        internal ControlList()
        {
            _controls = new List<IControl>();
        }

        internal void Add(IControl control)
        {
            _controls.Add(control);
        }

        internal void Remove(IControl control)
        {
            _controls.Remove(control);
        }

        internal IControl this[int index]
        {
            get => _controls[index];
            set => _controls[index] = value;
        }

        public IEnumerator<IControl> GetEnumerator()
        {
            return _controls.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _controls.GetEnumerator();
        }
    }
}