using System;
using System.Collections.Generic;
using System.Linq;
using BTurk.Automation.Core.Commands;
using BTurk.Automation.Core.SearchEngine;

namespace BTurk.Automation.Core.Requests;

public class RequestConfiguration : IRequestConfiguration
{
    private ICommand _command;
    private Func<string> _textProvider;
    private readonly List<IRequest> _childRequests = [];
    private bool _scanChildrenIfUnmatched;
    private Predicate<EnvironmentContext> _processCondition;
    private readonly List<Func<IChildRequestsProvider, IEnumerable<IRequest>>> _childRequestProviders = [];

    public RequestConfiguration SetText(string text)
    {
        return SetText(() => text);
    }

    public RequestConfiguration SetText(Func<string> textProvider)
    {
        _textProvider = textProvider;
        return this;
    }

    public void SetCommand(ICommand command)
    {
        _command = command;
    }

    public RequestConfiguration AddChildRequests(params IRequest[] requests)
    {
        _childRequests.AddRange(requests);
        return this;
    }

    public RequestConfiguration AddChildRequestsProvider<TRequest>() where TRequest : IRequest
    {
        _childRequestProviders.Add(p => p.LoadChildren<TRequest>().Cast<IRequest>());
        return this;
    }

    public RequestConfiguration ScanChildrenIfUnmatched()
    {
        _scanChildrenIfUnmatched = true;
        return this;
    }

    public RequestConfiguration ProcessCondition(Predicate<EnvironmentContext> condition)
    {
        _processCondition = condition;
        return this;
    }

    string IRequestConfiguration.Text => _textProvider?.Invoke() ?? "";

    ICommand IRequestConfiguration.Command => _command;

    bool IRequestConfiguration.ScanChildrenIfUnmatched => _scanChildrenIfUnmatched;

    bool IRequestConfiguration.CanHaveChildren => _childRequests.Any() || _childRequestProviders.Any();

    bool IRequestConfiguration.CanProcess(EnvironmentContext environmentContext)
    {
        if (_processCondition == null)
            return true;

        var canProcess = _processCondition.Invoke(environmentContext);

        return canProcess;
    }

    IEnumerable<IRequest> IRequestConfiguration.GetChildren(IChildRequestsProvider childRequestsProvider)
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