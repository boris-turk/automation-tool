using BTurk.Automation.Core.SearchEngine;
using FluentAssertions;
using Xunit;

namespace BTurk.Automation.Core.UnitTests
{
    public class FilterAlgorithmTests
    {
        [Fact]
        public void GetScore_WordStartsWithGivenFilterCharacter_ReturnsMaximumScore()
        {
            // Arrange
            var filter = new FilterAlgorithm(filterText: "n");
            
            // Act
            var score = filter.GetScore(text: "note");

            // Assert
            score.Should().Be(5000);
        }

        [Fact]
        public void GetScore_WordEndsWithGivenFilterCharacter_ReturnsMinimumScore()
        {
            // Arrange
            var filter = new FilterAlgorithm(filterText: "n");
            
            // Act
            var score = filter.GetScore(text: "solution");

            // Assert
            score.Should().Be(5);
        }
    }
}