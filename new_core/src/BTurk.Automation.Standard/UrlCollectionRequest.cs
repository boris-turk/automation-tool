using BTurk.Automation.Core.Requests;

namespace BTurk.Automation.Standard;

public class UrlCollectionRequest : Request
{
    public UrlCollectionRequest()
    {
        Configure()
            .SetText("url")
            .ScanChildrenIfUnmatched()
            .AddChildRequestsProvider<UrlRequest>()
            .SetCommand(r => new OpenWithDefaultProgramCommand(r));
    }
}