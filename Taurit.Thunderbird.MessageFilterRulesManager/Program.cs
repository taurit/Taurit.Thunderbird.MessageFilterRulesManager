using System;
using System.Collections.Generic;
using System.IO;
using Taurit.Thunderbird.ExcelToCsvConverter;
using Taurit.Thunderbird.MessageFilterRulesManager.Services;

namespace Taurit.Thunderbird.MessageFilterRulesManager
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                TransformExcelFiltersToThunderbirdFilters(args);
            }
            catch (Exception e)
            {
                Logger.Log($"{e}");
                Console.ReadLine();
                throw;
            }
        }

        private static void TransformExcelFiltersToThunderbirdFilters(string[] args)
        {
            // read and validate arguments
            if (args.Length != 2)
            {
                Logger.Log("You have not provided input arguments");
                Console.ReadLine();
                return;
            }

            var excelRulesFileName = args[0];
            var thunderbirdRulesFileName = args[1];
            if (!File.Exists(excelRulesFileName))
            {
                Logger.Log("Excel file does not exist");
                Console.ReadLine();
                return;
            }
            if (!File.Exists(thunderbirdRulesFileName))
            {
                Logger.Log("Thunderbird file does not exist");
                Console.ReadLine();
                return;
            }

            // make sure that thunderbird has not upgraded the format of the config file (from version 9)
            var msgFilterRulesFile = new FileParser().Parse(thunderbirdRulesFileName);
            var version = msgFilterRulesFile.Version;
            if (version != "9")
            {
                Logger.Log($"Thunderbird seems to have upgraded its config file version from 9 to `{version}`");
                Console.ReadLine();
                return;
            }

            // read my current filtering rules from excel
            ExcelRulesReader reader = new ExcelRulesReader();
            IReadOnlyList<ExcelRule> rules = reader.Read(excelRulesFileName);

            // convert to Thunderbird's model
            List<Rule> tbRules = new RulesConverter().Convert(rules);

            var metadata = new Dictionary<string, string>();
            metadata.Add("version", "9");
            metadata.Add("logging", "no");
            var tbModel = new RulesFile(metadata, tbRules);

            // save & overwrite thunderbird's config
            FileWriter fileWriter = new FileWriter();
            var serializedModel = fileWriter.SerializeModel(tbModel);
            File.WriteAllText(thunderbirdRulesFileName, serializedModel);

            Console.WriteLine("Done!");
        }
    }
}
