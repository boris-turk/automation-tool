using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using Aspose.Cells;

// ReSharper disable StringLiteralTypo

namespace E3kWorkReports
{
    public class ExcelWorkReportGenerator
    {
        private Workbook _workBook;

        private Workbook Workbook => _workBook ?? (_workBook = new Workbook(OutputFilePath));

        public IWorkEntriesProvider WorkEntriesProvider { get; }

        private Cells Cells => Workbook.Worksheets[_currentWorksheet].Cells;

        private int _currentWorksheet;

        public ExcelWorkReportGenerator(string outputFilePath, IWorkEntriesProvider workEntriesProvider)
        {
            OutputFilePath = outputFilePath;
            WorkEntriesProvider = workEntriesProvider;
        }

        public string OutputFilePath { get; set; }

        private Cell DateCell(int rowIndex) => Cells[rowIndex, 0];

        private Cell ProjectCodeCell(int rowIndex) => Cells[rowIndex, 1];

        private Cell TaskCell(int rowIndex) => Cells[rowIndex, 2];

        private Cell DescriptionCell(int rowIndex) => Cells[rowIndex, 3];

        private Cell HoursCell(int rowIndex) => Cells[rowIndex, 4];

        private Cell ChargeableCell(int rowIndex) => Cells[rowIndex, 5];

        public void Execute()
        {
            _currentWorksheet = -1;

            Dictionary<string, List<ReportEntry>> groups = (
                from entry in WorkEntriesProvider.GetAllEntries()
                group entry by entry.EmployeeFullName into g
                select new
                {
                    EmployeeFullName = g.Key,
                    Entries = g.OrderBy(_ => _.Date).ToList()
                }
            ).ToDictionary(_ => _.EmployeeFullName, _ => _.Entries);

            Export("Boris Turk", groups);
            Export("Katarina Krišelj", groups);
            Export("Borut Kaučič", groups);

            Workbook.Save(OutputFilePath);
        }

        private void Export(string employeeFullName, Dictionary<string, List<ReportEntry>> groups)
        {
            if (!groups.TryGetValue(employeeFullName, out var entries))
                return;

            _currentWorksheet += 1;

            entries = entries.OrderBy(_ => _.Date).ToList();

            SetMonthName();

            for (int index = 0; index < entries.Count; index++)
            {
                TransformEntry(entries[index]);
                ValidateEntry(entries[index]);
                WriteEntry(index + 1, entries[index]);
                WriteHoursSum(entries);
            }
        }

        private void ValidateEntry(ReportEntry entry)
        {
            if (entry.Date == DateTime.MinValue)
                throw new InvalidOperationException("Missing date");

            if (string.IsNullOrWhiteSpace(entry.Description))
                throw new InvalidOperationException("Missing description");

            if (string.IsNullOrWhiteSpace(entry.ProjectCode))
                throw new InvalidOperationException("Missing project code");

            if (string.IsNullOrWhiteSpace(entry.Task))
                throw new InvalidOperationException("Missing task");

            if (entry.Hours <= 0.15M || entry.Hours > 20)
                throw new InvalidOperationException($"Hour duration ({entry.Hours}) outside valid range");
        }

        private void WriteEntry(int rowIndex, ReportEntry entry)
        {
            DateCell(rowIndex).Value = entry.Date;
            ProjectCodeCell(rowIndex).Value = entry.ProjectCode;
            TaskCell(rowIndex).Value = entry.Task;
            DescriptionCell(rowIndex).Value = entry.Description;
            HoursCell(rowIndex).Value = entry.Hours;
            ChargeableCell(rowIndex).Value = entry.Chargeable ? "Y" : "";
        }

        private void WriteHoursSum(List<ReportEntry> entries)
        {
            var sumCell = HoursCell(entries.Count + 2);
            var totalTextCell = DescriptionCell(entries.Count + 2);

            totalTextCell.Value = "TOTAL:";
            Style style = totalTextCell.GetStyle();
            style.Font.IsBold = true;
            style.HorizontalAlignment = TextAlignmentType.Right;
            totalTextCell.SetStyle(style);

            sumCell.Value = entries.Sum(_ => _.Hours);
            style = sumCell.GetStyle();
            style.Font.IsBold = true;
            sumCell.SetStyle(style);
        }

        private void SetMonthName()
        {
            CultureInfo ci = new CultureInfo("en-US");
            var month = DateTime.Now.AddMonths(-1).ToString("MMMM", ci);
            Cells["A1"].Value = $"{(string) Cells["A1"].Value}, {month}";
        }

        private void TransformEntry(ReportEntry entry)
        {
            var match = Regex.Match(entry.Description, @"^know.?.?.?how\s+exchange\W*(.*)", RegexOptions.IgnoreCase);
            if (match.Success)
            {
                entry.Description = CapitalizeFirstLetter(match.Groups[1].Value);
                entry.Task = "Know how exchange";
            }
            if (string.IsNullOrWhiteSpace(entry.ProjectCode))
            {
                if (entry.Task.IndexOf("infrastructure", StringComparison.InvariantCultureIgnoreCase) >= 0)
                {
                    entry.ProjectCode = "STANDARD";
                }
                else if (entry.Task.IndexOf("meeting", StringComparison.InvariantCultureIgnoreCase) >= 0)
                {
                    entry.ProjectCode = "INTERNAL";
                }
            }
        }

        private string CapitalizeFirstLetter(string input)
        {
            return input.First().ToString().ToUpper() + input.Substring(1);
        }
    }
}