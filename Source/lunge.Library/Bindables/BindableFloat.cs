using System;
using System.Globalization;
using Microsoft.Xna.Framework;

namespace lunge.Library.Bindables
{
    public class BindableFloat : Bindable<float>
    {
        public float MinValue { get; }
        public float MaxValue { get; }

        private float _value;

        public new float Value
        {
            get => _value;
            set
            {
                var previousVal = _value;
                _value = MathHelper.Clamp(value, MinValue, MaxValue);

                // ReSharper disable once CompareOfFloatsByEqualityOperator
                if (_value != previousVal)
                    ValueChanged?.Invoke(this, new BindableValueChangeEvent<float>(_value, previousVal));
            }
        }

        public BindableFloat(float defaultVal, float min, float max,
            EventHandler<BindableValueChangeEvent<float>> action = null)
            : base(defaultVal, action)
        {
            MinValue = min;
            MaxValue = max;
            Value = defaultVal;
        }

        /// <inheritdoc />
        /// <summary>
        /// </summary>
        /// <param name="name"></param>
        /// <param name="defaultVal"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="action"></param>
        public BindableFloat(string name, float defaultVal, float min, float max,
            EventHandler<BindableValueChangeEvent<float>> action = null)
            : base(name, defaultVal, action)
        {
            MinValue = min;
            MaxValue = max;
            Value = defaultVal;
        }

        /// <inheritdoc />
        /// <summary>
        ///     Prints a stringified value of the Bindable.
        /// </summary>
        /// <returns></returns>
        public override string ToString() => Value.ToString(CultureInfo.InvariantCulture);
    }
}