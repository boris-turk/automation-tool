using System;

namespace BTurk.Automation.Core.Requests
{
    public interface IFilteredRequest : IRequest
    {
        Func<string, string> FilterTextProvider { get; }
    }
}