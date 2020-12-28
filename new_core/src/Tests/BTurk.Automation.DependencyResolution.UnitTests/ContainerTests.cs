using System;
using System.Collections.Generic;
using System.Linq;
using BTurk.Automation.Core;
using BTurk.Automation.Core.Decorators;
using BTurk.Automation.Core.Requests;
using BTurk.Automation.Core.SearchEngine;
using BTurk.Automation.Standard;
using FluentAssertions;
using FluentAssertions.Common;
using Xunit;

namespace BTurk.Automation.DependencyResolution.UnitTests
{
    public class ContainerTests
    {
        [Theory]
        [MemberData(nameof(Types))]
        public void CanCreateInstance(Type type)
        {
            // Act
            Container.GetInstance(type);
        }

        [Fact]
        public void CommandRequestHandlerCorrectlyDecorated()
        {
            // Act
            var instance = Container.GetInstance<IRequestHandler<CommandRequest>>();

            // Assert
            instance.GetType().Should().BeDerivedFrom(typeof(ClearSearchItemsRequestHandlerDecorator<>));
        }

        public static IEnumerable<object[]> Types
        {
            get
            {
                var types = new List<Type>
                {
                    typeof(List<ICommand>),
                    typeof(ISearchItemsProvider),
                    typeof(IResourceProvider),
                    typeof(IRequestHandler<CommandRequest>),
                    typeof(IRequestHandler<CompositeRequest>),
                    typeof(IRequestHandler<SelectionRequest<Solution>>)
                };

                return types.Select(x => new[] {x});
            }
        }
    }
}
