using System;
using System.Diagnostics;

namespace Taurit.Thunderbird.ExcelToCsvConverter
{
    [DebuggerDisplay("{Text} -> {Category}")]
    public class ExcelRule
    {
        public ExcelRule(string text, string wholeWordsOnly, string alsoMatchContent, string category)
        {
            if (wholeWordsOnly == null) throw new ArgumentNullException(nameof(wholeWordsOnly));
            if (alsoMatchContent == null) throw new ArgumentNullException(nameof(alsoMatchContent));

            if (wholeWordsOnly != "y" && wholeWordsOnly != "n")
                throw new ArgumentException($"WholeWordsOnly must have value `y` or `n`. Found `{wholeWordsOnly}`.");
            if (alsoMatchContent != "y" && alsoMatchContent != "n")
                throw new ArgumentException(
                    $"AlsoMatchContent must have value `y` or `n`. Found `{alsoMatchContent}`.");

            Text = text;
            WholeWordsOnly = wholeWordsOnly == "y";
            AlsoMatchContent = alsoMatchContent == "y";
            Category = category;
        }

        public string Text { get; }
        public bool WholeWordsOnly { get; }
        public bool AlsoMatchContent { get; }
        public string Category { get; }
    }
}