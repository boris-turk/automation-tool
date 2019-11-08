using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Aspose.Cells;

// ReSharper disable StringLiteralTypo
// ReSharper disable IdentifierTypo

namespace E3kWorkReports
{
    public class XlabReportGenerator : ReportGeneratorBase
    {
        public XlabReportGenerator(string sourceFilePath, Worksheet sheet) :
            base(sourceFilePath, sheet)
        {
        }

        protected override IEnumerable<ReportEntry> GetAllEntries()
        {
            var workBook = new Workbook(SourceFilePath);
            var cells = workBook.Worksheets[0].Cells;

            for (int rowIndex = 1; rowIndex < 500; rowIndex++)
            {
                var entry = new ReportEntry
                {
                    Date = GetDate(cells, rowIndex),
                    ProjectCode = GetProjectCode(cells, rowIndex),
                    Task = GetTask(cells, rowIndex),
                    Description = GetDescription(cells, rowIndex),
                    Hours = GetHours(cells, rowIndex),
                    Chargeable = GetChargeableFlag(cells, rowIndex)
                };

                if (entry.Date != DateTime.MinValue)
                    yield return entry;
            }
        }

        private DateTime GetDate(Cells cells, int rowIndex)
        {
            var value = cells[rowIndex, 0].Value;
            return (DateTime?)value ?? DateTime.MinValue;
        }

        private string GetProjectCode(Cells cells, int rowIndex)
        {
            var text = (string)cells[rowIndex, 3].Value ?? "";
            text = Regex.Replace(text, @"^[^/]*/\s*", "");
            text = text.Trim();
            return text;
        }

        private string GetTask(Cells cells, int rowIndex)
        {
            var text = (string)cells[rowIndex, 4].Value ?? "";
            text = Regex.Replace(text, @"^[^/]*/\s*", "");
            text = text.Trim();
            return text;
        }

        private string GetDescription(Cells cells, int rowIndex)
        {
            var text = (string) cells[rowIndex, 6].Value ?? "";
            text = Regex.Replace(text, @"^[^/]*/\s*", "");
            text = text.Trim();
            return text;
        }

        private decimal GetHours(Cells cells, int rowIndex)
        {
            var hours = cells[rowIndex, 7].Value;
            return hours == null ? 0 : (decimal)(double)hours;
        }

        private bool GetChargeableFlag(Cells cells, int rowIndex)
        {
            return false;
        }
    }
}