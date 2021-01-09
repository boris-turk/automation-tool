using System.Collections.Generic;

namespace E3kWorkReports
{
    internal class CompositeEntriesProvider : IWorkEntriesProvider
    {
        private readonly IWorkEntriesProvider[] _providers;

        public CompositeEntriesProvider(params IWorkEntriesProvider[] providers)
        {
            _providers = providers;
        }

        public IEnumerable<ReportEntry> GetAllEntries()
        {
            foreach (var provider in _providers)
            {
                foreach (var entry in provider.GetAllEntries())
                {
                    yield return entry;
                }
            }
        }
    }
}