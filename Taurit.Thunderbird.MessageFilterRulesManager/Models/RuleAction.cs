using System.Diagnostics;

namespace Taurit.Thunderbird.MessageFilterRulesManager
{
    [DebuggerDisplay("{" + nameof(Action) + "} {" + nameof(ActionValue) + "}")]
    internal class RuleAction
    {
        public string Action { get; set; }
        public string ActionValue { get; set; }
    }
}