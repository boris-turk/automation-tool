namespace BTurk.Automation.Core.SearchEngine
{
    public interface IClearSearchItemsRequest
    {
        bool ShouldClearSearchItems(string text);
    }
}