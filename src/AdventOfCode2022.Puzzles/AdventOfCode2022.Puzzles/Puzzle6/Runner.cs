using AdventOfCode2022.Puzzles.Utils;
using FluentAssertions;
using Xunit;

namespace AdventOfCode2022.Puzzles.Puzzle6;

public class Runner
{
    [Theory]
    [InlineData("mjqjpqmgbljsphdztnvjfqwrcgsmlb", false, 7)]
    [InlineData("bvwbjplbgvbhsrlpgdmjqwftvncz", false, 5)]
    [InlineData("nppdvjthqldpwncqszvftbrmjlhg", false, 6)]
    [InlineData("nznrnfrfntjfmvfwmzdfjlvtqnbhcprsg", false, 10)]
    [InlineData("zcfzfwzzqfrljwzlrfnpqdbhtmscgvjw", false, 11)]
    [InlineData("Input", true, 1766)]
    public void RunAlpha(string input, bool isEmbeddedResource, int expected)
    {
        var actual = Execute(input, isEmbeddedResource, 4);
        actual.Should().Be(expected);
    }

    [Theory]
    [InlineData("mjqjpqmgbljsphdztnvjfqwrcgsmlb", false, 19)]
    [InlineData("bvwbjplbgvbhsrlpgdmjqwftvncz", false, 23)]
    [InlineData("nppdvjthqldpwncqszvftbrmjlhg", false, 23)]
    [InlineData("nznrnfrfntjfmvfwmzdfjlvtqnbhcprsg", false, 29)]
    [InlineData("zcfzfwzzqfrljwzlrfnpqdbhtmscgvjw", false, 26)]
    [InlineData("Input", true, 2383)]
    public void RunBeta(string input, bool isEmbeddedResource, int expected)
    {
        var actual = Execute(input, isEmbeddedResource, 14);
        actual.Should().Be(expected);
    }

    private static int Execute(string input, bool isEmbeddedResource, int uniqueLength)
    {
        if (isEmbeddedResource)
        {
            input = EmbeddedResourceReader.Read<Runner>(input).Single();
        }

        var index = 0;

        while (true)
        {
            var chars = input.Skip(index).Take(uniqueLength).ToArray();
            var charsAreUnique = chars.GroupBy(x => x).All(x => x.Count() == 1);

            if (charsAreUnique)
            {
                return index + uniqueLength;
            }

            index++;
        }
    }
}