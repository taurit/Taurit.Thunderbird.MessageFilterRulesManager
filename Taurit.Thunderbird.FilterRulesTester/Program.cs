using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using Taurit.Thunderbird.ExcelToCsvConverter;
using Taurit.Thunderbird.MessageFilterRulesManager.Models;
using Taurit.Thunderbird.MessageFilterRulesManager.Services;

namespace Taurit.Thunderbird.FilterRulesTester
{


    internal class Program
    {
        private readonly RuleTester _ruleTester;

        private Program(string pathToRulesInExcelFormat)
        {
            // read my rules in Excel format
            var reader = new ExcelRulesReader();
            var rules = reader.Read(pathToRulesInExcelFormat);

            // convert to Thunderbird rules model
            var thunderbirdRules = new RulesConverter().Convert(rules);

            this._ruleTester = new RuleTester(thunderbirdRules);
        }

        private static void Main(string[] args)
        {
            var program = new Program(@"d:\ProgramData\ApplicationData\TauritToolkit\ThunderbirdFilteringRules.xlsx");

            program.SimulateFilterExecutionOnRealRssFeedItems(
                @"d:\ProgramData\ApplicationData\Thunderbird\Profiles\eaqnr9ie.default\Mail\Feeds\Archives.sbd\2019",
                @"d:\ProgramData\ApplicationData\Thunderbird\Profiles\eaqnr9ie.default\Mail\Feeds\Archives.sbd\2020"
            );
        }

        private void SimulateFilterExecutionOnRealRssFeedItems(params string[] pathsToRssFeedArchiveFiles)
        {
            int index = 0;
            int nextPauseIndex = 40;
            // read subjects of real news item headers from my RSS feed archive
            foreach (var archiveFilePath in pathsToRssFeedArchiveFiles)
            {
                var lines = File.ReadLines(archiveFilePath);
                foreach (var line in lines)
                {
                    if (TryGetSubjectFromLine(line, out string subject))
                    {
                        _ruleTester.TestFilterOnSingleRssItem(subject);
                        index++;
                    }

                    if (index == nextPauseIndex)
                    {
                        nextPauseIndex += 40;
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