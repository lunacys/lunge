using System;
using Microsoft.Xna.Framework;

namespace lunge.Library.Bindables
{
    public class BindableVector2 : Bindable<Vector2>
    {

        private Vector2 _value;

        public new Vector2 Value
        {
            get => _value;
            set
            {
                var prevVal = _value;

                _value = value;

                if (_value != prevVal)
                    ValueChanged?.Invoke(this, new BindableValueChangeEvent<Vector2>(_value, prevVal));
            }
        }
        
        public BindableVector2(Vector2 defaultVal, EventHandler<BindableValueChangeEvent<Vector2>> action = null) :
            base(defaultVal, action)
        {
        }

        public BindableVector2(string name, Vector2 defaultVal,
            EventHandler<BindableValueChangeEvent<Vector2>> action = null) : base(name, defaultVal, action)
        {
        }

        public override string ToString() => Value.ToString();
    }
}