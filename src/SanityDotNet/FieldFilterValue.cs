using System;
using System.Globalization;

namespace SanityDotNet
{
    public abstract class FieldFilterValue
    {
        internal FieldFilterValue()
        {
        }

        public static implicit operator FieldFilterValue(decimal value)
        {
            return Create(value);
        }

        public static implicit operator FieldFilterValue(int value)
        {
            return Create(value);
        }

        public static implicit operator FieldFilterValue(double value)
        {
            return Create(value);
        }

        public static implicit operator FieldFilterValue(float value)
        {
            return Create(value);
        }

        public static implicit operator FieldFilterValue(long value)
        {
            return Create(value);
        }

        public static implicit operator FieldFilterValue(bool value)
        {
            return Create(value);
        }

        public static implicit operator FieldFilterValue(string value)
        {
            return Create(value);
        }

        public static implicit operator FieldFilterValue(DateTime value)
        {
            return Create(value);
        }

        public static implicit operator FieldFilterValue(Enum value)
        {
            return Create(value);
        }

        public static implicit operator FieldFilterValue(Guid value)
        {
            return Create(value);
        }

        public static FieldFilterValue Create(int value)
        {
            return new FieldFilterValue<int>(value);
        }

        public static FieldFilterValue<double> Create(double value)
        {
            return new FieldFilterValue<double>(value);
        }

        public static FieldFilterValue<float> Create(float value)
        {
            return new FieldFilterValue<float>(value);
        }

        public static FieldFilterValue<long> Create(long value)
        {
            return new FieldFilterValue<long>(value);
        }

        public static FieldFilterValue<bool> Create(bool value)
        {
            return new FieldFilterValue<bool>(value);
        }

        public static FieldFilterValue<string> Create(string value)
        {
            return new FieldFilterValue<string>(value);
        }

        public static FieldFilterValue<DateTime> Create(DateTime value)
        {
            return new FieldFilterValue<DateTime>(value);
        }

        public static FieldFilterValue<Enum> Create(Enum value)
        {
            return new FieldFilterValue<Enum>(value);
        }

        public static FieldFilterValue<Guid> Create(Guid value)
        {
            return new FieldFilterValue<Guid>(value);
        }

        public static FieldFilterValue Create(decimal value)
        {
            return new FieldFilterValue<decimal>(value);
        }

        public abstract bool Is<T>();

        public abstract string ToQueryFormat();
    }

    public class FieldFilterValue<T> : FieldFilterValue
    {
        internal FieldFilterValue(T value)
        {
            Value = value;
        }

        public T Value { get; }

        public override string ToString()
        {
            return Value.ToString();
        }

        public static implicit operator T(FieldFilterValue<T> value)
        {
            return value.Value;
        }

        public override bool Equals(object obj)
        {
            return obj is FieldFilterValue<int> value
                ? Value.Equals(value.Value)
                : Value.Equals(obj);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override bool Is<TValueType>()
        {
            return Value is TValueType;
        }

        public override string ToQueryFormat()
        {
            var type = typeof(T);

            if (type == typeof(string)
                || type == typeof(Guid)
                || type == typeof(DateTime)
                || type == typeof(DateTime?)
                || type == typeof(Enum))
            {
                return $"\"{ToString()}\"";
            }

            if (type == typeof(float)
                || type == typeof(decimal)
                || type == typeof(double))
            {
                return ((IFormattable) Value).ToString(null, CultureInfo.InvariantCulture);
            }

            return ToString();
        }
    }
}