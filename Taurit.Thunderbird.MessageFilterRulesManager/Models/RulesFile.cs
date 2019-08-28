using System.Collections.Generic;

namespace Taurit.Thunderbird.MessageFilterRulesManager
{
    internal class RulesFile
    {
        public RulesFile(Dictionary<string, string> metadata, List<Rule> rules)
        {
            Metadata = metadata;
            Rules = rules;
        }

        public Dictionary<string, string> Metadata { get; }
        public List<Rule> Rules { get; }

        public string Version => Metadata["version"];
    }
}