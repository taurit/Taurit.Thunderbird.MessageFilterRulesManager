using System;
using System.Collections.Generic;
using System.Linq;
using Taurit.Thunderbird.ExcelToCsvConverter;
using Taurit.Thunderbird.MessageFilterRulesManager.Models;

namespace Taurit.Thunderbird.MessageFilterRulesManager.Services
{
    /// <summary>
    ///     Allows to convert excel rules into thunderbird's rules
    /// </summary>
    internal class RulesConverter
    {
        private const string WhitelistKeyword = "whitelist";

        public List<Rule> Convert(IReadOnlyList<ExcelRule> excelRules)
        {
            var result = new List<Rule>();
            foreach (var category in excelRules.GroupBy(x => x.Category))
                result.Add(
                    category.Key.Contains(WhitelistKeyword, StringComparison.InvariantCultureIgnoreCase)
                        ? GetWhitelistRule(category)
                        : GetBlacklistRule(category)
                );


            result.Add(GetMarkReadRule());

            var whitelist = result.Find(IsWhitelist);
            result.Remove(whitelist);
            result.Insert(0, whitelist);

            // ensure whitelist is first on the list
            if (!IsWhitelist(result.First()))
                throw new InvalidOperationException("The whitelist needs to be a first rule in a list to be effective");

            return result;
        }

        private static bool IsWhitelist(Rule x)
        {
            return x.Name.Contains(WhitelistKeyword, StringComparison.InvariantCultureIgnoreCase);
        }

        private Rule GetBlacklistRule(IGrouping<string, ExcelRule> category)
        {
            var rule = new Rule(category.Key);
            rule.Type = "17"; // not sure why, but TB sets it always to 17. Must be documented somewhere.
            rule.Enabled = "yes"; // todo - configurable in excel
            rule.Actions = new List<RuleAction>
            {
                new RuleAction
                {
                    Action = "Move to folder",
                    ActionValue = "mailbox://nobody@Feeds/Archives/2020"
                },
                new RuleAction
                {
                    Action = "Mark read"
                }
            };
            foreach (var condition in category)
            {
                var tbConditions = CreateTbConditions(condition);
                rule.Conditions.AddRange(tbConditions);
            }

            return rule;
        }

        private static List<Condition> CreateTbConditions(ExcelRule condition)
        {
            var conditions = new List<Condition>();
            if (condition.WholeWordsOnly == false)
            {
                conditions.Add(new Condition("subject", "contains", condition.Text));
            }
            else
            {
                conditions.Add(new Condition("subject", "contains", $" {condition.Text} "));
                conditions.Add(new Condition("subject", "contains", $" {condition.Text},"));
                conditions.Add(new Condition("subject", "contains", $" {condition.Text}."));
                conditions.Add(new Condition("subject", "contains", $" {condition.Text}?"));
                conditions.Add(new Condition("subject", "contains", $" {condition.Text}!"));
                conditions.Add(new Condition("subject", "contains", $" {condition.Text}-"));
                conditions.Add(new Condition("subject", "contains", $" {condition.Text}:"));
            }

            return conditions;
        }

        private Rule GetWhitelistRule(IGrouping<string, ExcelRule> category)
        {
            var rule = new Rule(category.Key);
            rule.Type = "17"; // not sure why, but TB sets it always to 17. Must be documented somewhere.
            rule.Enabled = "yes";
            rule.Actions = new List<RuleAction>
            {
                new RuleAction
                {
                    Action = "Stop execution"
                }
            };
            foreach (var condition in category)
            {
                var tbConditions = CreateTbConditions(condition);
                rule.Conditions.AddRange(tbConditions);
            }

            return rule;
        }

        private Rule GetMarkReadRule()
        {
            return new Rule("Mark as read while archiving")
            {
                Actions = new List<RuleAction>
                {
                    new RuleAction
                    {
                        Action = "Mark read"
                    }
                },
                Conditions = new List<Condition>(), // all
                Type = "128", // the only one, rest have "17"
                Enabled = "yes"
            };
        }
    }
}