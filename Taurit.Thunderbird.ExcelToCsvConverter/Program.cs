using System;
using System.IO;
using System.Text;
using ExcelDataReader;

namespace Taurit.Thunderbird.ExcelToCsvConverter
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            using (var stream = File.Open("Example.xlsx", FileMode.Open, FileAccess.Read))
            {
                // Auto-detect format, supports:
                //  - Binary Excel files (2.0-2003 format; *.xls)
                //  - OpenXml Excel files (2007 format; *.xlsx)
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    // Choose one of either 1 or 2:

                    // 1. Use the reader methods
                    do
                    {
                        while (reader.Read())
                        {
                            var column0 = reader.GetString(0);
                            var column2 = reader.GetString(1);
                        }
                    } while (reader.NextResult());


                    // The result of each spreadsheet is in result.Tables
                }
            }
        }
    }
}
