using System.Diagnostics;
using System.IO;
using System.Net;
using E3kWorkReports;

// ReSharper disable StringLiteralTypo
// ReSharper disable ArgumentsStyleStringLiteral

namespace Tests
{
    class Program
    {
        static void Main()
        {
            var generator = new ReportGenerator(
                directory: @"C:\Users\boris\Dropbox\Work\Projects\E3k\Work_Reports\2019-10",
                borisFileName: @"vpisi_oseb_projektno_delo_8-11-2019_8-5.xls",
                andrejFileName: @"vpisi_oseb_projektno_delo_8-11-2019_8-6.xls",
                borutFileName: @"borut.txt");

            if (File.Exists(generator.OutputFilePath))
                File.Delete(generator.OutputFilePath);

            generator.Execute();

            Process.Start(generator.OutputFilePath);
        }
    }
}
