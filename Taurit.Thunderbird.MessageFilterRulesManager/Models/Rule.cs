using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using Taurit.Thunderbird.MessageFilterRulesManager.Models;

namespace Taurit.Thunderbird.MessageFilterRulesManager
{
    [DebuggerDisplay("{" + nameof(Name) + "}")]
    public class Rule
    {
        public RuleAction LastAction;

        public Rule(string name)
        {
            Name = name;
        }


        public string Name { get; }

        public string Enabled { get; private set; }
        public string Type { get; private set; }

        public List<Condition> Conditions;

        public List<RuleAction> Actions { get; set; } = new List<RuleAction>();

        public string Condition {
            // serialized: eg "AND (subject,contains,GTA) AND (subject,contains,Grand Theft Auto)"
            get
            {
                if (Conditions.Count == 0) return "ALL";

                StringBuilder result = new StringBuilder();
                bool first = true;

                foreach (var condition in Conditions)
                {
                    if (first)
                    {
                        first = false;
                    }
                    else
                    {
                        result.Append(" ");
                    }

                    result.Append($"OR ({condition.Field},{condition.Relation},{condition.Value})");
                }

                return result.ToString();
            }
        }

        public void AddProperty(string key, string value)
        {
            if (key == "enabled")
            {
                Enabled = value;
            }
            else if (key == "type")
            {
                Type = value;
            }
            else if (key == "condition")
            {
                Conditions = ParseConditions(value);
            }
            else if (key == "action")
            {
                var ruleAction = new RuleAction {Action = value};
                LastAction = ruleAction;
                Actions.Add(ruleAction);
            }
            else if (key == "actionValue")
            {
                LastAction.ActionValue = value;
            }
            else
            {
                throw new InvalidOperationException($"Unexpected key for rule: `{key}`");
            }

        }

        // serialized: eg "AND (subject,contains,GTA) AND (subject,contains,Grand Theft Auto)"
        private List<Condition> ParseConditions(string serializedConditions)
        {
            if (serializedConditions == "ALL") return new List<Condition>();
            if (serializedConditions == null) throw new ArgumentNullException(nameof(serializedConditions));

            List<Condition> conditions = new List<Condition>();
            var split = serializedConditions.Split(new[] { " OR ", "OR " }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var condition in split)
            {
                var unwrapped = Unwrap(condition);
                var conditionSplit = unwrapped.Split(',', 3);
                var field = conditionSplit[0];
                var relation = conditionSplit[1];
                var value = conditionSplit[2];
                conditions.Add(new Condition(field, relation, value));
            }

            return conditions;
        }

        private string Unwrap(string quotedString)
        {
            if (String.IsNullOrEmpty(quotedString)) throw new ArgumentNullException(nameof(quotedString));
            if (quotedString[0] != '(') throw new ArgumentException(quotedString);
            if (quotedString[quotedString.Length - 1] != ')') throw new ArgumentException(quotedString);

            return quotedString.Substring(1, quotedString.Length - 2);
        }
    }
}