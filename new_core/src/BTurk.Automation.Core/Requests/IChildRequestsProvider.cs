using System.Collections.Generic;

namespace BTurk.Automation.Core.Requests;

public interface IChildRequestsProvider
{
    IEnumerable<IRequest> LoadChildren(IRequest request);
}