using System.Diagnostics;

namespace Taurit.Thunderbird.MessageFilterRulesManager.Models
{
    [DebuggerDisplay("{" + nameof(Action) + "} {" + nameof(ActionValue) + "}")]
    public class RuleAction
    {
        public string Action { get; set; }
        public string ActionValue { get; set; }
    }
}