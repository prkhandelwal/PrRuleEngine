using PrRuleEngine.Models;
using System;
using System.Collections.Generic;
using System.Text;
using PrRuleEngine.Constants;
using PrRuleEngine.Interfaces;

namespace PrRuleEngine
{
    // TO Parse incoming data
    public class Parser
    {
        private const string fieldSignalName = "signal";
        private const string fieldValueName = "value";
        private const string fieldValueTypeName = "value_type";

        public bool TryParse(string input, out List<SignalData> validSignals, out List<SignalData> invalidSignals, bool canStopOnFirstValidationFailure, params IRule[] rules)
        {
            try
            {
                if (string.IsNullOrEmpty(input)) { throw new ArgumentNullException("input"); }

                validSignals = new List<SignalData>();
                invalidSignals = new List<SignalData>();

                foreach (var signal in input.Split(Constants.Constants.ObjectEnd))
                {
                    var elements = signal.Split(new[] { Constants.Constants.ElementSeparator }, StringSplitOptions.RemoveEmptyEntries);
                    if (elements.Length == 3) // Signal, Value and ValueType
                    {
                        string parsedSignal = string.Empty, parsedValue = string.Empty, parsedValueType = string.Empty;

                        foreach (var element in elements)
                        {
                            if (!string.IsNullOrWhiteSpace(element))
                            {
                                int indexOfObjectSeparator = element.IndexOf(Constants.Constants.ObjectSeparator);
                                string fieldName = element.Substring(0, indexOfObjectSeparator);
                                string fieldValue = element.Substring(indexOfObjectSeparator + 1);

                                if (!string.IsNullOrWhiteSpace(fieldName) && !string.IsNullOrWhiteSpace(fieldValue))
                                {
                                    fieldName = fieldName.ToLowerInvariant().Split(Constants.Constants.ObjectWrapper)[1];
                                    fieldValue = fieldValue.Split(Constants.Constants.ObjectWrapper)[1];

                                    switch (fieldName.ToLowerInvariant())
                                    {
                                        case fieldSignalName:
                                            parsedSignal = fieldValue;
                                            continue;

                                        case fieldValueTypeName:
                                            parsedValueType = fieldValue;
                                            continue;

                                        case fieldValueName:
                                            parsedValue = fieldValue;
                                            continue;

                                        default:
                                            break;
                                    }
                                }
                                else { return false; }
                            }
                        }

                        if (string.IsNullOrWhiteSpace(parsedSignal) ||
                            string.IsNullOrWhiteSpace(parsedValue) ||
                            string.IsNullOrWhiteSpace(parsedValueType))
                        {
                            return false;
                        }

                        var signalData = new SignalData(parsedSignal, parsedValue, parsedValueType);
                        bool isValidSignal = true;
                        foreach (var rule in rules)
                        {
                            if (!rule.IsValid(signalData))
                            {
                                isValidSignal = false;
                                invalidSignals.Add(signalData);

                                if (canStopOnFirstValidationFailure) return true;
                                break;
                            }
                        }
                        if (isValidSignal) { validSignals.Add(signalData); }
                    }
                }
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
