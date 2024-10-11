using System.Collections.Generic;
using System.Linq;
using BTurk.Automation.Core.Requests;

namespace BTurk.Automation.Core.SearchEngine;

public class SearchResult
{
    private List<Item> _items = [];

    public IReadOnlyList<Item> Items => _items.AsReadOnly();

    public SearchResult Append(IRequestV2 request)
    {
        var searchResult = new SearchResult
        {
            _items = _items.ToList()
        };

        searchResult._items.Add(new Item(request, request.Configuration.Text));

        return searchResult;
    }

    public class Item
    {
        public Item(IRequestV2 request, string text = null)
        {
            Text = text ?? request.Configuration.Text;
            Request = request;
        }

        public string Text { get; }
        public IRequestV2 Request { get; }
    }
}