using PrRuleEngine.Interfaces;
using PrRuleEngine.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PrRuleEngine
{
    public class Rule : IRule
    {
        private static List<Rule> _rules;
        public string Signal { get; private set; }
        public Comparison ComparisonType { get; private set; }
        public dynamic Value { get; private set; }
        public string ValueType { get; private set; }

        public Rule(string signal, dynamic value, Comparison comparisonType)
        {
            if (string.IsNullOrWhiteSpace(signal)) { throw new ArgumentNullException("signal"); }
            if (value == null) { throw new ArgumentNullException("value"); }

            Signal = signal;
            Value = value;
            ComparisonType = comparisonType;
            ValueType = value.GetType().ToString().Split('.')[1];
        }

        public bool IsValid(SignalData input)
        {
            bool result = true;

            if (input != null)
            {
                if (Signal == input.Signal && ValueType.ToLowerInvariant() == input.ValueType.ToLowerInvariant() && Value.GetType() == input.Value.GetType())
                {
                    switch (ComparisonType)
                    {
                        case Comparison.Equal:
                            result = !(Signal == input.Signal && input.Value == Value);
                            break;

                        case Comparison.NotEqual:
                            result = !(Signal == input.Signal && input.Value != Value);
                            break;

                        case Comparison.GreaterThan:
                            result = !(Signal == input.Signal && input.Value > Value);
                            break;

                        case Comparison.GreaterThanOrEqual:
                            result = !(Signal == input.Signal && input.Value >= Value);
                            break;

                        case Comparison.LessThan:
                            result = !(Signal == input.Signal && input.Value < Value);
                            break;

                        case Comparison.LessThanOrEqual:
                            result = !(Signal == input.Signal && input.Value <= Value);
                            break;

                        default:
                            return result;
                    }
                }
            }

            return result;
        }

        public static Rule[] GetRules()
        {
            if (_rules == null)
            {
                LoadRulesFromCsv();
            }

            return _rules.ToArray();
        }

        public void AddRule(Rule rule)
        {
            if (rule == null) { throw new ArgumentNullException("rule"); }

            if (_rules == null)
            {
                LoadRulesFromCsv();
            }

            _rules.Add(rule);
        }

        private static void LoadRulesFromCsv(string filename = "Rules.csv")
        {
            try
            {
                if (_rules == null) _rules = new List<Rule>();
                if (File.Exists(filename))
                {
                    var rules = File.ReadAllLines(filename);
                    if (rules != null)
                    {
                        bool isHeader = true;
                        foreach (var rule in rules)
                        {
                            if (isHeader) { isHeader = false; continue; }
                            var data = rule.Split(Constants.Constants.ElementSeparator);

                            var signal = data[0].Split(Constants.Constants.ObjectWrapper)[1];
                            Enum.TryParse(data[1].Split(Constants.Constants.ObjectWrapper)[1], out Comparison comparisonType);
                            var valueType = data[3].Split(Constants.Constants.ObjectWrapper)[1];

                            var typeInfo = Type.GetType($"System.{valueType}", false, true);
                            dynamic value = null;
                            if (typeInfo != null)
                            {
                                value = Convert.ChangeType(data[2].Split(Constants.Constants.ObjectWrapper)[1], typeInfo);
                            }
                            else
                            {
                                value = data[2].Split('"')[1];
                            }
                            _rules.Add(new Rule(signal, value, comparisonType));
                        }
                    }
                }
            }
            catch (Exception)
            {
                //TODO: Log Error
                throw;
            }
        }
    }
}
