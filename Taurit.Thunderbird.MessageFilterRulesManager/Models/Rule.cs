using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Taurit.Thunderbird.MessageFilterRulesManager
{
    [DebuggerDisplay("{" + nameof(Name) + "}")]
    internal class Rule
    {
        public RuleAction LastAction;

        public Rule(string name)
        {
            Name = name;
        }


        public string Name { get; }

        public string Enabled { get; private set; }
        public string Type { get; private set; }
        public string Condition { get; private set; }

        public List<RuleAction> Actions { get; set; } = new List<RuleAction>();

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
                Condition = value;
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
    }
}