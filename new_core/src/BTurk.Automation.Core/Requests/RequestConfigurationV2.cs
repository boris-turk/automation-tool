using System;
using System.Collections.Generic;
using System.Linq;
using BTurk.Automation.Core.Commands;
using BTurk.Automation.Core.SearchEngine;

namespace BTurk.Automation.Core.Requests;

public class RequestConfigurationV2 : IRequestConfigurationV2
{
    private ICommand _command;
    private Func<string> _textProvider;
    private readonly List<IRequestV2> _childRequests = [];
    private bool _scanChildrenIfUnmatched;
    private Predicate<EnvironmentContext> _processCondition;
    private readonly List<Func<IChildRequestsProviderV2, IEnumerable<IRequestV2>>> _childRequestProviders = [];

    public RequestConfigurationV2 SetText(string text)
    {
        return SetText(() => text);
    }

    public RequestConfigurationV2 SetText(Func<string> textProvider)
    {
        _textProvider = textProvider;
        return this;
    }

    public void SetCommand(ICommand command)
    {
        _command = command;
    }

    public RequestConfigurationV2 AddChildRequests(params IRequestV2[] requests)
    {
        _childRequests.AddRange(requests);
        return this;
    }

    public RequestConfigurationV2 AddChildRequestsProvider<TRequest>() where TRequest : IRequestV2
    {
        _childRequestProviders.Add(p => p.LoadChildren<TRequest>().Cast<IRequestV2>());
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

    ICommand IRequestConfigurationV2.Command => _command;

    bool IRequestConfigurationV2.ScanChildrenIfUnmatched => _scanChildrenIfUnmatched;

    bool IRequestConfigurationV2.CanHaveChildren => _childRequests.Any() || _childRequestProviders.Any();

    bool IRequestConfigurationV2.CanProcess(EnvironmentContext environmentContext)
    {
        if (_processCondition == null)
            return true;

        var canProcess = _processCondition.Invoke(environmentContext);

        return canProcess;
    }

    IEnumerable<IRequestV2> IRequestConfigurationV2.GetChildren(IChildRequestsProviderV2 childRequestsProvider)
    {
        foreach (var provider in _childRequestProviders)
        {
            foreach (var childRequest in provider.Invoke(childRequestsProvider))
            {
                yield return childRequest;
            }
        }

        foreach (var childRequest in _childRequests)
        {
            yield return childRequest;
        }
    }
}