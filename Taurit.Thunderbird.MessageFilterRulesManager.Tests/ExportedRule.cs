namespace Taurit.Thunderbird.MessageFilterRulesManager.Tests
{
    public class ExportedRule

    {
        public ExportedRule(string kvpKey, string kvpValue)
        {
            KvpKey = kvpKey;
            KvpValue = kvpValue;
        }

        public string KvpKey { get; }
        public string KvpValue { get; }
    }
}