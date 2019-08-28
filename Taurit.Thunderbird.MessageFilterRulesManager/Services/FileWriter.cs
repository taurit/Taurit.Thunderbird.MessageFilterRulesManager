using System.Text;
using System.Text.RegularExpressions;

namespace Taurit.Thunderbird.MessageFilterRulesManager.Services
{
    public class FileWriter
    {
        public string SerializeModel(RulesFile fileModel)
        {
            var sb = new StringBuilder();

            // serialize metadata
            foreach (var kvp in fileModel.Metadata)
            {
                var line = SerializeLine(kvp.Key, kvp.Value);
                sb.AppendLine(line);
            }

            // serialize rules
            foreach (var rule in fileModel.Rules)
            {
                // name
                var nameLine = SerializeLine("name", rule.Name);
                sb.AppendLine(nameLine);

                // enabled
                var enabledLine = SerializeLine("enabled", rule.Enabled);
                sb.AppendLine(enabledLine);

                // type
                var typeLine = SerializeLine("type", rule.Type);
                sb.AppendLine(typeLine);

                // actions
                foreach (var action in rule.Actions)
                {
                    var actionLine = SerializeLine("action", action.Action);
                    sb.AppendLine(actionLine);

                    if (action.ActionValue != null) // this one's optional
                    {
                        var actoinValueLine = SerializeLine("actionValue", action.ActionValue);
                        sb.AppendLine(actoinValueLine);
                    }
                }

                // conditions
                var conditionsLine = SerializeLine("condition", rule.Condition);
                sb.AppendLine(conditionsLine);
            }

            return sb.ToString();
        }

        private string SerializeLine(string key, string value)
        {
            return $"{key}=\"{value.Replace("\"", "\\\"")}\"";
        }
    }
}