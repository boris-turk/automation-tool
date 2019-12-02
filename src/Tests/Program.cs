using System;
using System.Diagnostics;
using System.IO;
using E3kWorkReports;

// ReSharper disable StringLiteralTypo
// ReSharper disable ArgumentsStyleStringLiteral
// ReSharper disable ArgumentsStyleNamedExpression

namespace Tests
{
    class Program
    {
        static void Main()
        {
            var yearMonth = $"{DateTime.Now.Year:0000}-{DateTime.Now.Month - 1:00}";
            var directory = $@"C:\Users\boris\Dropbox\Work\Projects\E3k\Work_Reports\{yearMonth}";

            var generator = new ReportGenerator(
                directory: directory,
                borisFileName: @"boris.xls",
                andrejFileName: @"andrej.xls",
                borutFileName: @"borut.txt");

            if (File.Exists(generator.OutputFilePath))
                File.Delete(generator.OutputFilePath);

            generator.Execute();

            Process.Start(generator.OutputFilePath);
        }
    }
}
