using System;

namespace Taurit.Thunderbird.MessageFilterRulesManager
{
    internal static class Logger
    {
        public static void Log(string text)
        {
            Console.WriteLine($"[{DateTime.UtcNow}] {text}");
        }
    }
}