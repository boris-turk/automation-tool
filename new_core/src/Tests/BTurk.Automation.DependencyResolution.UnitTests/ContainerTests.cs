using System;
using System.Collections.Generic;
using System.Linq;
using BTurk.Automation.Core;
using BTurk.Automation.Core.Commands;
using BTurk.Automation.Core.Converters;
using BTurk.Automation.Core.Messages;
using BTurk.Automation.Core.Requests;
using BTurk.Automation.Core.SearchEngine;
using BTurk.Automation.Core.Views;
using BTurk.Automation.Standard;
using BTurk.Automation.WinForms.Providers;
using Xunit;

namespace BTurk.Automation.DependencyResolution.UnitTests;

public class ContainerTests
{
    [Theory]
    [MemberData(nameof(Types))]
    public void CanCreateInstance(Type type)
    {
        // Arrange
        Bootstrapper.InitializeContainer();
        var sut = Bootstrapper.Container;

        // Act
        sut.GetInstance(type);
    }

    public static IEnumerable<object[]> Types
    {
        get
        {
            var types = new List<Type>
            {
                typeof(ISearchItemsProvider),
                typeof(IResourceProvider),
                typeof(IRequestsProvider<Repository>),
                typeof(IRequestsProvider<FakeRequest>),
                typeof(ICommandHandler<CommitRepositoryCommand>),
                typeof(IMessageHandler<ShowingAutomationWindowMessage>),
                typeof(IGuiValueConverter<int, int>),
                typeof(IGuiValueConverter<string, string>),
                typeof(IControlProvider<FieldConfiguration<string>>)
            };

            return types.Select(x => new[] {x});
        }
    }
}