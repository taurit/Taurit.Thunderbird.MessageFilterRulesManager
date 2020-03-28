using System;
using System.Collections.Generic;
using Taurit.Thunderbird.ExcelToCsvConverter;
using Taurit.Thunderbird.MessageFilterRulesManager.Models;
using Taurit.Thunderbird.MessageFilterRulesManager.Services;

namespace Taurit.Thunderbird.FilterRulesTester
{
    internal class RuleTester
    {
        private readonly string _pathToRulesInExcelFormat;
        private readonly StatsCollector _statsCollector;
        private List<Rule> _thunderbirdRules;

        
        public RuleTester(string pathToRulesInExcelFormat, StatsCollector statsCollector)
        {
            _pathToRulesInExcelFormat = pathToRulesInExcelFormat;
            _statsCollector = statsCollector;
            UpdateRules();
        }


        public void TestFilterOnSingleRssItem(string subject)
        {
            foreach (var rule in _thunderbirdRules) // eg "Gaming" rule
            {
                var isWhitelistRule = rule.Name == "Whitelist";

                foreach (var condition in rule.Conditions) // eg "Tibia" condition
                {
                    if (condition.Relation != "contains")
                        throw new ArgumentOutOfRangeException(nameof(condition.Relation),
                            $"Unexpected and currently unexpected relation type '{condition.Relation}'");

                    if (condition.Field != "subject")
                        throw new ArgumentOutOfRangeException(nameof(condition.Field),
                            $"Unexpected and currently unexpected field type '{condition.Field}'");

                    if (subject.Contains(condition.Value, StringComparison.InvariantCultureIgnoreCase))
                    {
                        // filter would work.
                        if (isWhitelistRule)
                        {
                            // ReSharper disable once ConditionIsAlwaysTrueOrFalse - temporarily hiding blocked rules
                            Console.ForegroundColor = isWhitelistRule ? ConsoleColor.Green : ConsoleColor.Red;
                            Console.Write(subject);
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.WriteLine($" ({rule.Name}, subject contains '{condition.Value}')");
                        }

                        _statsCollector.RecordUseOfARule(rule, condition);
                        

                        return;
                    }
                }
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(subject);
        }

        /// <summary>
        ///     Since when using this tool I'm adjusting the rules in Excel on a second monitor, it makes sense to continue
        ///     simulation with updated rules to have an instant feedback about my changes.
        /// </summary>
        public void UpdateRules()
        {
            // read my rules in Excel format
            var reader = new ExcelRulesReader();
            var rules = reader.Read(_pathToRulesInExcelFormat);

            // convert to Thunderbird rules model
            _thunderbirdRules = new RulesConverter().Convert(rules);
        }
    }
}