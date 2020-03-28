using System;
using System.Diagnostics;

namespace Taurit.Thunderbird.ExcelToCsvConverter
{
    [DebuggerDisplay("{Text} -> {Category}")]
    public class ExcelRule
    {
        public ExcelRule(string text, string wholeWordsOnly, string category)
        {
            if (wholeWordsOnly == null) throw new ArgumentNullException(nameof(wholeWordsOnly));

            if (text.Contains("\"") || text.Contains("(") || text.Contains(")") || text.Contains(",") ||
                text.Contains("\\"))
                throw new ArgumentException($"Illegal character found in input (text). Invalid entry: {text}");
            if (category.Contains("\"") || category.Contains("(") || category.Contains(")") || category.Contains(",") ||
                category.Contains("\\"))
                throw new ArgumentException($"Illegal character found in input (category). Invalid entry: {category}");


            // ? = didn't think of it, because it doesnt matter now, rule unused
            if (wholeWordsOnly != "y" && wholeWordsOnly != "n")
                throw new ArgumentException($"WholeWordsOnly must have value `y` or `n`. Found `{wholeWordsOnly}`.");

            Text = text;
            WholeWordsOnly = wholeWordsOnly == "y";
            Category = category;
        }

        public string Text { get; }
        public bool WholeWordsOnly { get; }
        public string Category { get; }
    }
}