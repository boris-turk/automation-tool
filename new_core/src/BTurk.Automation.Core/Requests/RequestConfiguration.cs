using System;
using System.Collections.Generic;
using System.Linq;
using BTurk.Automation.Core.Commands;
using BTurk.Automation.Core.SearchEngine;

namespace BTurk.Automation.Core.Requests;

public class RequestConfiguration : IRequestConfiguration
{
    private Func<string> _textProvider;
    private readonly List<IRequest> _childRequests = [];
    private bool _scanChildrenIfUnmatched;
    private Predicate<EnvironmentContext> _processCondition;
    private Predicate<IRequest> _childProcessCondition;
    private readonly List<Func<IChildRequestsProvider, IEnumerable<IRequest>>> _childRequestProviders = [];
    private Func<ICommandProcessor, IRequest, bool> _commandDispatcher;

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
        _commandDispatcher = (processor, _) =>
        {
            processor.Process(command);
            return true;
        };
    }

    public RequestConfiguration AddChildRequests(params IRequest[] requests)
    {
        _childRequests.AddRange(requests);
        return this;
    }

    public Child<TRequest> AddChildRequestsProvider<TRequest>() where TRequest : IRequest
    {
        _childRequestProviders.Add(p => p.LoadChildren<TRequest>().Cast<IRequest>());
        return new Child<TRequest>(this);
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

    bool IRequestConfiguration.ScanChildrenIfUnmatched => _scanChildrenIfUnmatched;

    bool IRequestConfiguration.CanHaveChildren => _childRequests.Any() || _childRequestProviders.Any();

    bool IRequestConfiguration.CanProcess(IRequest childRequest, EnvironmentContext environmentContext)
    {
        if (_processCondition != null && childRequest == null)
            return _processCondition.Invoke(environmentContext);

        if (_childProcessCondition != null && childRequest != null)
            return _childProcessCondition.Invoke(childRequest);

        return true;
    }

    bool IRequestConfiguration.ExecuteCommand(ICommandProcessor commandProcessor, IRequest childRequest)
    {
        if (_commandDispatcher == null)
            return false;

        var executed = _commandDispatcher.Invoke(commandProcessor, childRequest);

        return executed;
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

    public class Child<TRequest> : RequestConfiguration where TRequest : IRequest
    {
        private readonly RequestConfiguration _configuration;

        public Child(RequestConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Child<TRequest> SetCommand(Func<TRequest, ICommand> commandProvider)
        {
            _configuration._commandDispatcher = (processor, childRequest) =>
            {
                if (childRequest is not TRequest properChildRequest)
                    return false;

                var command = commandProvider.Invoke(properChildRequest);
                processor.Process(command);

                return true;
            };

            return this;
        }

        public RequestConfiguration ProcessCondition(Predicate<TRequest> condition)
        {
            _configuration._childProcessCondition = r =>
                r is TRequest properRequest && condition.Invoke(properRequest);

            return this;
        }
    }
}