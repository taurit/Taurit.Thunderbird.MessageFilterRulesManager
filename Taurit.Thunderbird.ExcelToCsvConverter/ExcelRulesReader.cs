using System.Collections.Generic;
using System.IO;
using System.Text;
using ExcelDataReader;

namespace Taurit.Thunderbird.ExcelToCsvConverter
{
    public class ExcelRulesReader
    {
        public IReadOnlyList<ExcelRule> Read(string excelRulesFileName)
        {
            var result = new List<ExcelRule>();

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            using var stream = File.Open(excelRulesFileName, FileMode.Open, FileAccess.Read);
            using var reader = ExcelReaderFactory.CreateReader(stream);
            reader.Read(); // skip headers row

            do
            {
                while (reader.Read())
                {
                    var text = reader.GetString(0);
                    var wholeWordsOnly = reader.GetString(1);
                    var alsoMatchContent = reader.GetString(2);
                    var category = reader.GetString(3);

                    var excelRule = new ExcelRule(text, wholeWordsOnly, alsoMatchContent, category);
                    result.Add(excelRule);
                }
            } while (reader.NextResult());

            return result;
        }
    }
}