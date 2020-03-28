using System;
using System.IO;

namespace Taurit.Thunderbird.FilterRulesTester
{
    internal class Program
    {
        private readonly RuleTester _ruleTester;
        private readonly StatsCollector _statsCollector;

        private Program(string pathToRulesInExcelFormat)
        {
            _statsCollector = new StatsCollector();
            _ruleTester = new RuleTester(pathToRulesInExcelFormat, _statsCollector);
        }

        private static void Main()
        {
            var program = new Program(@"d:\ProgramData\ApplicationData\TauritToolkit\ThunderbirdFilteringRules.xlsx");

            program.SimulateFilterExecutionOnRealRssFeedItems(
                @"d:\ProgramData\ApplicationData\Thunderbird\Profiles\eaqnr9ie.default\Mail\Feeds\Archives.sbd\2019",
                @"d:\ProgramData\ApplicationData\Thunderbird\Profiles\eaqnr9ie.default\Mail\Feeds\Archives.sbd\2020"
            );
        }

        private void SimulateFilterExecutionOnRealRssFeedItems(params string[] pathsToRssFeedArchiveFiles)
        {
            var index = 0;
            var nextPauseIndex = 10000; // use negative number to disable
            // read subjects of real news item headers from my RSS feed archive
            foreach (var archiveFilePath in pathsToRssFeedArchiveFiles)
            {
                var lines = File.ReadLines(archiveFilePath);
                foreach (var line in lines)
                {
                    if (TryGetSubjectFromLine(line, out var subject))
                    {
                        _ruleTester.TestFilterOnSingleRssItem(subject);
                        index++;
                    }

                    if (index == nextPauseIndex)
                    {
                        nextPauseIndex += 55;
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine("Press key to see more items...");
                        Console.ReadKey();
                        _ruleTester.UpdateRules();
                    }
                }
            }

            _statsCollector.DisplayMostUsedRules();
            _statsCollector.DisplayLeastUsedRules();
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