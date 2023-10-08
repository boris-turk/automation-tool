using System;
using System.Collections.Generic;
using System.Linq;
using BTurk.Automation.Core;
using BTurk.Automation.Core.Messages;
using BTurk.Automation.Core.Requests;
using BTurk.Automation.Core.SearchEngine;
using BTurk.Automation.Standard;
using Xunit;

namespace BTurk.Automation.DependencyResolution.UnitTests;

public class ContainerTests
{
    [Theory]
    [MemberData(nameof(Types))]
    public void CanCreateInstance(Type type)
    {
        // Act
        Container.GetInstance(type);
    }

    public static IEnumerable<object[]> Types
    {
        get
        {
            var types = new List<Type>
            {
                typeof(ISearchItemsProvider),
                typeof(IResourceProvider),
                typeof(IRequestsProvider<Solution>),
                typeof(IRequestsProvider<Repository>),
                typeof(IMessageHandler<ShowingAutomationWindowMessage>)
            };

            return types.Select(x => new[] {x});
        }
    }
}