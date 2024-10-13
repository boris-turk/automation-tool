using System;
using System.Collections.Generic;
using System.Linq;
using BTurk.Automation.Core.SearchEngine;

namespace BTurk.Automation.Core.Requests;

public class RequestConfigurationV2 : IRequestConfigurationV2
{
    private readonly IRequestV2 _request;
    private Func<string> _textProvider;
    private readonly List<IRequestV2> _childRequests = [];
    private bool _scanChildrenIfUnmatched;
    private Predicate<EnvironmentContext> _processCondition;

    public RequestConfigurationV2(IRequestV2 request)
    {
        _request = request;
    }

    public RequestConfigurationV2 SetText(string text)
    {
        return SetText(() => text);
    }

    public RequestConfigurationV2 SetText(Func<string> textProvider)
    {
        _textProvider = textProvider;
        return this;
    }

    public RequestConfigurationV2 AddChildRequests(params IRequestV2[] requests)
    {
        _childRequests.AddRange(requests);
        return this;
    }

    public RequestConfigurationV2 ScanChildrenIfUnmatched()
    {
        _scanChildrenIfUnmatched = true;
        return this;
    }

    public RequestConfigurationV2 ProcessCondition(Predicate<EnvironmentContext> condition)
    {
        _processCondition = condition;
        return this;
    }

    string IRequestConfigurationV2.Text => _textProvider?.Invoke() ?? "";

    bool IRequestConfigurationV2.ScanChildrenIfUnmatched => _scanChildrenIfUnmatched;

    bool IRequestConfigurationV2.CanHaveChildren => _childRequests.Any();

    bool IRequestConfigurationV2.CanProcess(EnvironmentContext environmentContext)
    {
        if (_processCondition == null)
            return true;

        var canProcess = _processCondition.Invoke(environmentContext);

        return canProcess;
    }

    IEnumerable<IRequestV2> IRequestConfigurationV2.GetChildren(IChildRequestsProviderV2 childRequestsProvider)
    {
        foreach (var childRequest in childRequestsProvider.LoadChildren(_request))
            yield return childRequest;

        foreach (var childRequest in _childRequests)
            yield return childRequest;
    }
}