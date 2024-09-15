using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;

// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

namespace E3kWorkReports
{
    public class BorutWorkEntriesProvider : IWorkEntriesProvider
    {
        public string SourceFilePath { get; }

        public BorutWorkEntriesProvider(string sourceFilePath)
        {
            SourceFilePath = sourceFilePath;
        }

        public IEnumerable<ReportEntry> GetAllEntries()
        {
            var datePattern = @"^\s*(?<day>\d+)\.(?<month>\d+)\.(?<year>\d+)";

            var contentPattern = @"^(?:\W+(?<projectCode>\w+)(?<separator>\W+))?" +
                                 @"(?<description>.*[^.:])" + 
                                 @"(\.\.\.*\s*|:\s*)(?<duration>\d+.*)h\s*$";

            var totalPattern = @"^\W*TOTAL\W*(\d|\.|h|\s)*$";

            var date = DateTime.MinValue;

            foreach (var line in File.ReadAllLines(SourceFilePath))
            {
                var dateMatch = Regex.Match(line, datePattern);

                if (dateMatch.Success)
                {
                    date = GetDate(dateMatch);
                    continue;
                }

                var totalMatch = Regex.Match(line, totalPattern, RegexOptions.IgnoreCase);

                if (totalMatch.Success)
                    continue;

                var contentText = RemoveLeadingXlabMarker(line);

                var contentMatch = Regex.Match(contentText, contentPattern);

                if (!contentMatch.Success)
                    continue;

                if (date == DateTime.MinValue)
                    throw new InvalidOperationException("Could not determine initial date");

                var entry = GetSingleEntry(contentMatch, date);

                yield return entry;
            }
        }

        private string RemoveLeadingXlabMarker(string text)
        {
            var pattern = @"^\W+(?<xlab>\w+)(\W+)(?<description>.*)";
            var match = Regex.Match(text, pattern);

            if (!match.Success)
                return text;

            var properText = match.Groups["description"].Value.Trim();

            return properText;
        }

        private ReportEntry GetSingleEntry(Match match, DateTime date)
        {
            var allowedProjectCodes = new List<string>()
            {
                "INTERNAL", "STANDARD", "TROUBLESHOOTING", "DEDICATED"
            };

            var projectCode = match.Groups["projectCode"].Value.Trim();
            var description = match.Groups["description"].Value.Trim();
            var hours = GetHours(match.Groups["duration"].Value.Trim());
            var task = "New core";

            if (string.IsNullOrWhiteSpace(projectCode))
                projectCode = "STANDARD";

            if (!allowedProjectCodes.Contains(projectCode.ToUpper()))
            {
                projectCode = match.Groups["projectCode"].Value;
                description = match.Groups["description"].Value;
                description = $"{projectCode}{match.Groups["separator"].Value}{description}";
                description = description.Trim();
                projectCode = allowedProjectCodes[0];
            }

            return new ReportEntry
            {
                ProjectCode = projectCode,
                Task = task,
                Description = description,
                Hours = hours,
                Date = date,
                EmployeeFullName = "Borut Kaučič"
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