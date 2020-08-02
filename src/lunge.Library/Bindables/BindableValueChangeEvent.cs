using System;

namespace lunge.Library.Bindables
{
    public class BindableValueChangeEvent<T>: EventArgs
    {
        public T Value { get; set; }
        public T OldValue { get; set; }
        
        public BindableValueChangeEvent(T value, T oldValue)
        {
            Value = value;
            OldValue = oldValue;
        }
    }
}