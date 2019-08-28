using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace Taurit.Thunderbird.MessageFilterRulesManager
{
    /// <summary>
    ///     Class that is able to parse Thunderbird's MsgFilterRules file
    /// </summary>
    public class FileParser
    {
        public RulesFile Parse(string fileName)
        {
            var lines = File.ReadAllLines(fileName);

            var numberOfNameLinesEncountered = 0;
            var metadata = new Dictionary<string, string>();
            var rules = new List<Rule>();
            Rule rule = null;
            foreach (var line in lines)
            {
                (string key, string value) kv = ParseLine(line);
                if (kv.key == "name")
                {
                    numberOfNameLinesEncountered++;

                    if (rule != null) rules.Add(rule);
                    rule = new Rule(kv.value);
                }
                else
                {
                    if (numberOfNameLinesEncountered == 0)
                        metadata[kv.key] = kv.value;
                    else if (kv.key != "name")
                        rule.AddProperty(kv.key, kv.value);
                }
            }

            if (rule != null)
                // the last rule will not be added in a loop
                rules.Add(rule);

            return new RulesFile(metadata, rules);
        }

        private (string, string) ParseLine(string line)
        {
            var splitLine = line.Split("=", 2);
            var key = splitLine[0];

            var value = Unquote(splitLine[1]);
            var unescapedValue = Regex.Unescape(value);


            return (key, unescapedValue);
        }

        private string Unquote(string quotedString)
        {
            if (quotedString[0] != '\"') throw new ArgumentException(quotedString);
            if (quotedString[quotedString.Length - 1] != '\"') throw new ArgumentException(quotedString);

            return quotedString.Substring(1, quotedString.Length - 2);
        }
    }
}