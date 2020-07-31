using System;
using System.Globalization;

namespace lunge.Library.Bindables
{
    public class BindableDouble : Bindable<double>
    {
        public double MinValue { get; set; }
        public double MaxValue { get; set; }

        private double _value;

        public new double Value
        {
            get => _value;
            set
            {
                var prevVal = _value;

                if (value <= MinValue)
                    _value = MinValue;
                else if (value >= MaxValue)
                    _value = MaxValue;
                else
                    _value = value;
                
                if (_value != prevVal)
                    ValueChanged?.Invoke(this, new BindableValueChangeEvent<double>(_value,prevVal));
            }
        }

        public BindableDouble(double defaultVal, double min, double max,
            EventHandler<BindableValueChangeEvent<double>> action = null) : base(defaultVal, action)
        {
            MinValue = min;
            MaxValue = max;
            Value = defaultVal;
        }

        public BindableDouble(string name, double defaultVal, double min, double max,
            EventHandler<BindableValueChangeEvent<double>> action = null) : base(name, defaultVal, action)
        {
            MinValue = min;
            MaxValue = max;
            Value = defaultVal;
        }
        
        public override string ToString() => Value.ToString(CultureInfo.InvariantCulture);
    }
}