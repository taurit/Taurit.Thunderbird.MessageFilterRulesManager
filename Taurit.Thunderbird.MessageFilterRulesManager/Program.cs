using System.Collections.Generic;
using Taurit.Thunderbird.ExcelToCsvConverter;

namespace Taurit.Thunderbird.MessageFilterRulesManager
{
    class Program
    {
        static void Main(string[] args)
        {
            // read and validate arguments
            var excelRulesFileName = args[0];
            var thunderbirdRulesFileName = args[1];
            
            // make sure that thunderbird has not upgraded the format of the config file (from version 9)
            var msgFilterRulesFile = new FileParser().Parse(thunderbirdRulesFileName);
            var version = msgFilterRulesFile.Version;
            if (version != "9")
            {
                Logger.Log($"Thunderbird seems to have upgraded its config file version from 9 to {version}");
                return;
            }

            // read my current filtering rules from excel
            ExcelRulesReader reader = new ExcelRulesReader();
            IReadOnlyList<ExcelRule> rules = reader.Read(excelRulesFileName);

            // convert to Thunderbird's model
            //var tbModel = new RulesFile();

            // save & overwrite thunderbird's config
            // todo
        }


    }
}
