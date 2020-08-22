using BTurk.Automation.Core.SearchEngine;
using FluentAssertions;
using Xunit;

namespace BTurk.Automation.Standard.UnitTests
{
    public class CommitSearchHandlerTests
    {
        [Fact]
        public void Handle_EmptyString_ReturnsInactiveResult()
        {
            // Arrange
            var instance = new CommitSearchHandler();
            var parameters = new SearchParameters("", ActionType.TextChanged);
            
            // Act
            var result = instance.Handle(parameters);

            // Assert
            result.IsActive.Should().BeFalse();
        }

        [Fact]
        public void Handle_ExactLeadingMatch_ReturnsActiveResult()
        {
            // Arrange
            var instance = new CommitSearchHandler();
            var parameters = new SearchParameters("commit ", ActionType.TextChanged);
            
            // Act
            var result = instance.Handle(parameters);

            // Assert
            result.IsActive.Should().BeTrue();
        }
    }
}
