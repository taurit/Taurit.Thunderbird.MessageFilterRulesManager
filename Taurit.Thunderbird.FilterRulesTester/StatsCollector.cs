using System;
using System.Collections.Generic;
using System.Linq;
using Taurit.Thunderbird.MessageFilterRulesManager.Models;

namespace Taurit.Thunderbird.FilterRulesTester
{
    internal class StatsCollector
    {
        private readonly Dictionary<string, int> _rulesStats = new Dictionary<string, int>();

        public void RecordUseOfARule(Rule rule, Condition condition)
        {
            var statsKey = $"{rule.Name}: {condition.Value}";

            if (!_rulesStats.ContainsKey(statsKey))
                _rulesStats.Add(statsKey, 0);

            _rulesStats[statsKey]++;
        }

        public void DisplayMostUsedRules()
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Most used rules");

            Console.ForegroundColor = ConsoleColor.DarkGreen;
            var top = _rulesStats.OrderByDescending(x => x.Value).Take(30);
            foreach (var rule in top) Console.WriteLine($"{rule.Value,-6} {rule.Key}");
        }

        public void DisplayLeastUsedRules()
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Least used rules");

            Console.ForegroundColor = ConsoleColor.DarkRed;
            var bottom = _rulesStats.OrderBy(x => x.Value).Take(30);
            foreach (var rule in bottom) Console.WriteLine($"{rule.Value,-6} {rule.Key}");
        }
    }
}