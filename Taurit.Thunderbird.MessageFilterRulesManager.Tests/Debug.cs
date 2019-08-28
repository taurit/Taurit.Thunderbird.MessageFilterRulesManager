using System;
using System.Collections.Generic;
using System.IO;
using CsvHelper;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Taurit.Thunderbird.MessageFilterRulesManager.Tests
{
    [TestClass]
    public class Debug
    {
        [TestMethod]
        public void ImportRules()
        {
            // import rules
            var fileName =
                "d:\\ProgramData\\ApplicationData\\Thunderbird\\Profiles\\eaqnr9ie.default\\Mail\\Feeds\\msgFilterRules.dat";
            var file = new FileParser().Parse(fileName);

            // aggregate
            var exportedRules = new List<ExportedRule>();
            var ruleToCategory = new Dictionary<string, string>();
            foreach (var rule in file.Rules)
            foreach (var condition in rule.Conditions)
            {
                if (ruleToCategory.ContainsKey(condition.Value))
                {
                    Console.WriteLine($"Duplicate rule for {condition.Value}");
                }
                ruleToCategory[condition.Value] = rule.Name;
            }
                
            foreach (var kvp in ruleToCategory)
            {
                var exportedRule = new ExportedRule(kvp.Key, kvp.Value);
                exportedRules.Add(exportedRule);
            }

            // export to excel file
            using (var writer = new StreamWriter("d:\\rules-exported.csv"))
            using (var csv = new CsvWriter(writer))
            {    
                csv.WriteRecords(exportedRules);
            }
        }
    }
}