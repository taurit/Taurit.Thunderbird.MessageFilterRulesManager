using System;
using System.IO;
using System.Runtime.InteropServices;
using Taurit.Thunderbird.ExcelToCsvConverter;
using Taurit.Thunderbird.MessageFilterRulesManager.Services;

namespace Taurit.Thunderbird.FilterRulesTester
{


    internal class Program
    {
        private static void Main(string[] args)
        {
            var program = new Program();
            program.SimulateFilterExecutionOnRealRssFeedItems(
                @"d:\ProgramData\ApplicationData\TauritToolkit\ThunderbirdFilteringRules.xlsx",
                @"d:\ProgramData\ApplicationData\Thunderbird\Profiles\eaqnr9ie.default\Mail\Feeds\Archives.sbd\2019",
                @"d:\ProgramData\ApplicationData\Thunderbird\Profiles\eaqnr9ie.default\Mail\Feeds\Archives.sbd\2020"
            );

        }

        private void SimulateFilterExecutionOnRealRssFeedItems(string pathToRulesInExcelFormat, params string[] pathsToRssFeedArchiveFiles)
        {
            
            // read my rules in Excel format
            var reader = new ExcelRulesReader();
            var rules = reader.Read(pathToRulesInExcelFormat);

            // convert to Thunderbird rules model
            var tbRules = new RulesConverter().Convert(rules);

            int index = 0;
            int nextPauseIndex = 20;
            // read subjects of real news item headers from my RSS feed archive
            foreach (var archiveFilePath in pathsToRssFeedArchiveFiles)
            {
                var lines = File.ReadLines(archiveFilePath);
                foreach (var line in lines)
                {
                    if (TryGetSubjectFromLine(line, out string subject))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(subject);

                        index++;
                    }

                    if (index == nextPauseIndex)
                    {
                        nextPauseIndex += 20;
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine("Press key to see more items...");
                        Console.ReadKey();
                    }
                }

            }
        }

        private bool TryGetSubjectFromLine(string line, out string subject)
        {
            if (line.StartsWith("Subject: "))
            {
                subject = line.Substring(9);
                return true;
            }

            subject = null;
            return false;
        }
    }
}