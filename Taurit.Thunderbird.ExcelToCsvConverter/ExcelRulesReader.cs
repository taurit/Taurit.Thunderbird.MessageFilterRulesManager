using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ExcelDataReader;

namespace Taurit.Thunderbird.ExcelToCsvConverter
{
    public class ExcelRulesReader
    {
        public IReadOnlyList<ExcelRule> Read(string excelRulesFileName)
        {
            var allRules = new List<ExcelRule>();
            var enabledCategoriesNames = new List<string>();
            var disabledCategoriesNames = new List<string>();

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            using var stream = File.Open(excelRulesFileName, FileMode.Open, FileAccess.Read);
            using var reader = ExcelReaderFactory.CreateReader(stream);
            reader.Read(); // skip headers row

            do
            {
                while (reader.Read())
                {
                    var sheetName = reader.Name;

                    if (sheetName == "Phrases")
                    {
                        allRules.Add(ReadExcelRule(reader));
                    }
                    else if (sheetName == "EnabledFilters")
                    {
                        var enabledCategoryName = reader.GetString(0);
                        enabledCategoriesNames.Add(enabledCategoryName);
                    }
                    else if (sheetName == "DisabledFilters")
                    {
                        var disabledCatgoryName = reader.GetString(0);
                        disabledCategoriesNames.Add(disabledCatgoryName);
                    }
                }
            } while (reader.NextResult());

            if (allRules.Count != allRules.ToHashSet().Count)
                throw new InvalidOperationException(
                    "Some rule is duplicated in excel file! sort by text and find a duplicate.");

            // make sure that "enabled" and "disabled" list all rules
            var duplicatedRules = enabledCategoriesNames.Intersect(disabledCategoriesNames).ToList();
            if (duplicatedRules.Any())
            {
                throw new InvalidOperationException($"Some rules are present in both enabled and disabled lists. Fix `{String.Join(",", duplicatedRules)}`.");
            }

            var categoriesNames = allRules.Select(x => x.Category).ToHashSet().ToList();
            foreach (var categoryName in categoriesNames)
            {
                if (!enabledCategoriesNames.Contains(categoryName) && !disabledCategoriesNames.Contains(categoryName))
                {
                    throw new InvalidOperationException($"Category `{categoryName}` is not present in EnabledRules nor DisabledRules sheets.");
                }
            }

            var enabledCategoriesNamesHashSet = enabledCategoriesNames.ToHashSet();
            return allRules.Where(x => enabledCategoriesNamesHashSet.Contains(x.Category)).ToList();
        }

        private static ExcelRule ReadExcelRule(IExcelDataReader reader)
        {
            var text = reader.GetString(0);
            var wholeWordsOnly = reader.GetString(1);
            var alsoMatchContent = reader.GetString(2);
            var category = reader.GetString(3);

            var excelRule = new ExcelRule(text, wholeWordsOnly, alsoMatchContent, category);
            return excelRule;
        }
    }
}