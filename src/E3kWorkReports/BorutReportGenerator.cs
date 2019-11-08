using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using Aspose.Cells;

// ReSharper disable StringLiteralTypo
// ReSharper disable IdentifierTypo

namespace E3kWorkReports
{
    public class BorutReportGenerator : ReportGeneratorBase
    {
        public BorutReportGenerator(string sourceFilePath, Worksheet sheet)
            : base(sourceFilePath, sheet)
        {
        }

        protected override IEnumerable<ReportEntry> GetAllEntries()
        {
            var datePattern = @"^\s*(?<day>\d+)\.(?<month>\d+)\.(?<year>\d+)";
            var contentPattern = @"^\W+(?<projectCode>\w+)\W+(?<description>.*[^.])\.\.\.*\s*(?<duration>\d+.*)";

            var date = DateTime.MinValue;

            foreach (var line in File.ReadAllLines(SourceFilePath))
            {
                var dateMatch = Regex.Match(line, datePattern);

                if (dateMatch.Success)
                {
                    date = GetDate(dateMatch);
                    continue;
                }

                var contentMatch = Regex.Match(line, contentPattern);

                if (contentMatch.Success)
                {
                    if (date == DateTime.MinValue)
                        throw new InvalidOperationException("Could not determine initial date");

                    var entry = GetSingleEntry(contentMatch, date);
                    yield return entry;
                }
            }
        }

        private ReportEntry GetSingleEntry(Match match, DateTime date)
        {
            var projectCode = match.Groups["projectCode"].Value.Trim();
            var description = match.Groups["description"].Value.Trim();
            var hours = GetHours(match.Groups["duration"].Value.Trim());
            var task = "New core";

            return new ReportEntry
            {
                ProjectCode = projectCode,
                Task = task,
                Description = description,
                Hours = hours,
                Date = date
            };
        }

        private decimal GetHours(string text)
        {
            var number = Regex.Replace(text, @"\s*h", "");
            number = Regex.Replace(number, @",", ".");
            return decimal.Parse(number, CultureInfo.InvariantCulture);
        }

        private static DateTime GetDate(Match match)
        {
            var day = int.Parse(match.Groups["day"].Value);
            var month = int.Parse(match.Groups["month"].Value);
            var year = int.Parse(match.Groups["year"].Value);

            if (year < 100)
                year = year + 2000;

            return new DateTime(year, month, day);
        }
    }
}