using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using BTurk.Automation.Core.Requests;

namespace BTurk.Automation.Core.SearchEngine;

public class SearchResult
{
    private List<Item> _items = [];

    public IReadOnlyList<Item> Items => _items.AsReadOnly();

    public SearchResult Append(IRequest request)
    {
        var searchResult = new SearchResult
        {
            _items = _items.ToList()
        };

        searchResult._items.Add(new Item(request, request.Configuration.Text));

        return searchResult;
    }

    public override string ToString()
    {
        var parts = _items.Select(x => x.Text).Where(x => x.HasLength()).ToList();
        var text = string.Join(" ", parts);
        return text;
    }

    public class Item
    {
        public Item(IRequest request, string text = null)
        {
            Text = text ?? request.Configuration.Text;
            Request = request;
        }

        public string Text { get; }
        public IRequest Request { get; }
    }
}