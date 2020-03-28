using System;
using System.Collections.Generic;
using Taurit.Thunderbird.MessageFilterRulesManager.Models;

namespace Taurit.Thunderbird.FilterRulesTester
{
    internal class RuleTester
    {
        private readonly List<Rule> _thunderbirdRules;

        public RuleTester(List<Rule> thunderbirdRules)
        {
            _thunderbirdRules = thunderbirdRules;
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

                    if (subject.Contains(condition.Value))
                    {
                        // filter would work.
                        Console.ForegroundColor = isWhitelistRule ? ConsoleColor.Green : ConsoleColor.Red;
                        Console.Write(subject);
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine($" ({rule.Name}, subject contains '{condition.Value}')");
                        return;
                    }
                }
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(subject);
        }
    }
}