using PrRuleEngine;
using PrRuleEngine.Interfaces;
using PrRuleEngine.Models;
using System;
using System.Collections.Generic;
using System.IO;

namespace RuleEngineApp
{
    class Program
    {
        private static readonly string _inputData;
        static Program()
        {
            _inputData = File.ReadAllText("raw_data.json");
        }
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            int input = 1;
            while(input != 0)
            {
                Console.WriteLine("Welcome to Rule Engine! Select any one option or press 0 to exit!");
                Console.WriteLine("1. Test Data Without Rules");
                Console.WriteLine("2. Test Data With Rules from excel file");
                Console.WriteLine("3. Test Data With custom Rules");

                int.TryParse(Console.ReadLine(), out input);

                switch (input)
                {
                    case 1:
                        TestDataWithoutRules();
                        break;
                    case 2:
                        TestDataWithRulesFromFile();
                        break;
                    case 3:
                        TestDataWithCustomRules();
                        break;
                    default:
                        break;
                }
                Console.WriteLine("Press a key to continue");
                Console.ReadKey();

            }

        }

        private static void TestDataWithoutRules()
        {
            List<SignalData> expectedInvalidSignals = new List<SignalData>();

            var parser = new Parser();

            if (parser.TryParse(_inputData, out List<SignalData> actualValidSignals, out List<SignalData> actualInvalidSignals, false))
            {
                Console.WriteLine($"Number of invalid signals {actualInvalidSignals.Count}");
                Console.WriteLine($"Number of valid signals {actualValidSignals.Count}");
            }
            else
            {
                Console.WriteLine("Unable to parse the data");
            }
        }

        private static void TestDataWithRulesFromFile()
        {
            var expectedInvalidSignals = new List<SignalData>
            {
                new SignalData("ATL2", "LOW", "String"),
                new SignalData("ATL2", "LOW", "String"),
                new SignalData("ATL2", "LOW", "String"),
                new SignalData("ATL2", "LOW", "String")
            };

            var rules = Rule.GetRules();

            var parser = new Parser();
            if (parser.TryParse(_inputData, out List<SignalData> actualValidSignals, out List<SignalData> actualInvalidSignals, false, rules))
            {
                Console.WriteLine($"Number of invalid signals {actualInvalidSignals.Count}");
                Console.WriteLine($"Number of valid signals {actualValidSignals.Count}");
            }
            else
            {
                Console.WriteLine("Unable to parse incoming data.");
            }
        }

        private static void TestDataWithCustomRules()
        {
            var rules = new List<Rule>
            {
                new Rule("ATL9", new DateTime(2017, 6, 14), Comparison.LessThan),
                new Rule("ATL3", "LOW", Comparison.Equal),
                new Rule("ATL10", new DateTime(2017, 7, 15), Comparison.GreaterThan),
                new Rule("ATL7", "HIGH", Comparison.NotEqual)
            }.ToArray();

            var parser = new Parser();
            if (parser.TryParse(_inputData, out List<SignalData> actualValidSignals, out List<SignalData> actualInvalidSignals, false, rules))
            {
                Console.WriteLine("Applied custom rules!");
                foreach (var item in rules)
                {
                    Console.WriteLine($"Signal: {item.Signal}, Comparision Type: {item.ComparisonType}, Value: {item.Value}, Value Type: {item.ValueType}");
                }

                Console.WriteLine($"Number of invalid signals {actualInvalidSignals.Count}");
                Console.WriteLine("Invalid Signals:");
                foreach (var item in actualInvalidSignals)
                {
                    Console.WriteLine($"Signal: {item.Signal}, Value: {item.Value}, Value Type: {item.ValueType}");
                }

                Console.WriteLine($"Number of valid signals {actualInvalidSignals.Count}");
            }
            else
            {
                Console.WriteLine("Unable to parse incoming data.");
            }
        }
    }
}
