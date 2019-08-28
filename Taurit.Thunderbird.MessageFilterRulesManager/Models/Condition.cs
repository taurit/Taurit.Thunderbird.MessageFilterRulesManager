using System.Diagnostics;

namespace Taurit.Thunderbird.MessageFilterRulesManager.Models
{
    [DebuggerDisplay("{Field},{Relation},{Value}")]
    public class Condition
    {
        public Condition(string field, string relation, string value)
        {
            Field = field;
            Relation = relation;
            Value = value;
        }

        public string Field { get; }
        public string Relation { get; }
        public string Value { get; }
    }
}