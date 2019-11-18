using PrRuleEngine.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrRuleEngine.Interfaces
{
    public interface IRule
    {
        string Signal { get; }

        dynamic Value { get; }

        Comparison ComparisonType { get; }

        bool IsValid(SignalData input);
    }

    public enum Comparison : short
    {
        Equal,
        NotEqual,
        GreaterThan,
        GreaterThanOrEqual,
        LessThan,
        LessThanOrEqual,
    }
}
