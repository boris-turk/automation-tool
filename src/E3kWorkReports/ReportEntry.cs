using System;

namespace E3kWorkReports
{
    public class ReportEntry
    {
        public decimal Hours { get; set; }

        public string ProjectCode { get; set; }

        public string Task { get; set; }

        public string Description { get; set; }

        public DateTime Date { get; set; }

        public bool Chargeable { get; set; }

        public string EmployeeFullName { get; set; }
    }
}