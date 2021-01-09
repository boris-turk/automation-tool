using System.Collections.Generic;

namespace E3kWorkReports
{
    public interface IWorkEntriesProvider
    {
        IEnumerable<ReportEntry> GetAllEntries();
    }
}