using System;

namespace Taurit.Thunderbird.MessageFilterRulesManager
{
    class Program
    {
        static void Main(string[] args)
        {
            var msgFilterRulesFileName = args[0];
            var msgFilterRulesFile = new MsgFilterRulesFileParser().Parse(msgFilterRulesFileName);
            var version = msgFilterRulesFile.Version;

        }
    }
}
