using System;
using System.Collections.Generic;
using System.Text;

namespace PrRuleEngine.Models
{
    public class SignalData
    {
        
        private const string _valueTypeInteger = "integer";
        private const string _valueTypeDatetime = "datetime";

        public SignalData(string signal, string value, string valueType)
        {
            if (string.IsNullOrWhiteSpace(signal)) { throw new ArgumentNullException("signal"); }
            if (string.IsNullOrWhiteSpace(value)) { throw new ArgumentNullException("value"); }
            if (string.IsNullOrWhiteSpace(valueType)) { throw new ArgumentNullException("valueType"); }

            Signal = signal;
            ValueType = valueType;

            switch (valueType.ToLowerInvariant())
            {
                case _valueTypeInteger:
                    if (double.TryParse(value, out double resultInteger)) { Value = resultInteger; }
                    else { Value = value; }
                    break;

                case _valueTypeDatetime:
                    if (DateTime.TryParse(value, out DateTime resultDateTime)) { Value = resultDateTime; }
                    else { Value = value; }
                    break;

                default:
                    Value = value;
                    break;
            }
        }

        public string Signal { get; }
        public dynamic Value { get; }
        public string ValueType { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is SignalData)
            {
                var signal = obj as SignalData;
                return
                    signal.Signal == Signal &&
                    signal.Value == Value &&
                    signal.ValueType == ValueType;
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((string.IsNullOrWhiteSpace(Signal) ? 0 : Signal.GetHashCode() + Signal.Length) +
                (string.IsNullOrWhiteSpace(ValueType) ? 0 : ValueType.GetHashCode() + ValueType.Length) +
                (Value == null ? 0 : this.Value.GetHashCode())) * 7;
            }
        }
    }
}
