using System.Collections.Generic;
using BTurk.Automation.Core.Requests;

namespace BTurk.Automation.Core.SearchEngine
{
    public interface ISearchItemsProvider
    {
        List<Request> Items { get; }
    }
}