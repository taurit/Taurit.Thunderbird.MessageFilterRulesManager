using System;
using System.Collections.Generic;
using System.Text;

namespace Taurit.Thunderbird.MessageFilterRulesManager.Models
{
    class Condition
    {
        public string Field { get; }
        public string Relation { get; }
        public string Value { get; }

        public Condition(string field, string relation, string value)
        {
            Field = field;
            Relation = relation;
            Value = value;
        }
    }
}
