using System;
using System.Diagnostics;
using System.IO;
using AutomationEngine;
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
            //var tests = new ClockifyRestApiTests();
            //tests.Run();
            //return;


            var dateTime = DateTime.Now.GetPreviousMonthStart();
            var yearMonth = $"{dateTime.Year:0000}-{dateTime.Month:00}";
            var directory = $@"C:\Users\boris\Dropbox\Work\Projects\E3k\Work_Reports\{yearMonth}";

            var generator = new ReportGenerator(
                directory: directory,
                borutFileName: @"borut.txt");

            if (File.Exists(generator.OutputFilePath))
                File.Delete(generator.OutputFilePath);

            generator.Execute();

            Process.Start(generator.OutputFilePath);
        }
    }
}
