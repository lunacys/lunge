using System;

namespace lunge.Library.Bindables
{
    public class Bindable<T> : IDisposable
    {
        public EventHandler<BindableValueChangeEvent<T>> ValueChanged;
        
        public string Name { get; }
        public T Default { get; set; }

        public T Value
        {
            get => _value;
            set
            {
                var oldVal = _value;
                _value = value;
                ValueChanged?.Invoke(this, new BindableValueChangeEvent<T>(value, oldVal));
            }
        }

        private T _value;

        public Bindable(T defaultVal, EventHandler<BindableValueChangeEvent<T>> action = null)
        {
            if (action != null)
                ValueChanged += action;

            Default = defaultVal;
            Value = defaultVal;
        }

        public Bindable(string name, T defaultVal, EventHandler<BindableValueChangeEvent<T>> action = null)
        {
            if (action != null)
                ValueChanged += action;

            Name = name;
            Default = defaultVal;
            Value = Default;
        }
        
        public void UnHookEventHandlers() => ValueChanged = null;
        public override string ToString() => Value.ToString();
        public void Dispose() => UnHookEventHandlers();
        public void TriggerChange() => ValueChanged?.Invoke(this, new BindableValueChangeEvent<T>(Value, Value));
        public void ChangeWithoutTrigger(T val) => _value = val;
    }
}