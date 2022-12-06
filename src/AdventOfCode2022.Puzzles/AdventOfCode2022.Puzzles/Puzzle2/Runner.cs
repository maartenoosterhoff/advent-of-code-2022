using AdventOfCode2022.Puzzles.Utils;
using FluentAssertions;
using Xunit;

namespace AdventOfCode2022.Puzzles.Puzzle2;

public class Runner
{
    [Theory]
    [InlineData("TestInput", 15L)]
    [InlineData("Input", 12458L)]
    public void RunAlpha(string filename, long expected)
    {
        var actual = Execute(filename, 1);
        actual.Should().Be(expected);
    }

    [Theory]
    [InlineData("TestInput", 12L)]
    [InlineData("Input", 12683L)]
    public void RunBeta(string filename, long expected)
    {
        var actual = Execute(filename, 2);
        actual.Should().Be(expected);
    }


    private static IDictionary<string, int> GetScoreSet(int scoreSetId)
    {
        if (scoreSetId == 1)
        {
            return new Dictionary<string, int>()
            {
                // X = rock, Y = paper, Z = scissors
                {"A X", 4}, // shape = 1, outcome = 3, sum = 4
                {"A Y", 8}, // shape = 2, outcome = 6, sum = 8
                {"A Z", 3}, // shape = 3, outcome = 0, sum = 3
                {"B X", 1}, // shape = 1, outcome = 0, sum = 1
                {"B Y", 5}, // shape = 2, outcome = 3, sum = 5
                {"B Z", 9}, // shape = 3, outcome = 6, sum = 9
                {"C X", 7}, // shape = 1, outcome = 6, sum = 7
                {"C Y", 2}, // shape = 2, outcome = 0, sum = 2
                {"C Z", 6}, // shape = 3, outcome = 3, sum = 6
            };
        }

        return new Dictionary<string, int>()
        {
            // X = loss, Y = draw, Z = win
            {"A X", 3}, // X = loss, play = scissors = 3, outcome = 0, sum = 3
            {"A Y", 4}, // Y = draw, play = rock = 1, outcome = 3, sum = 4
            {"A Z", 8}, // Z = win, play = paper = 2, outcome = 6, sum = 8
            {"B X", 1}, // X = loss, play = rock = 1, outcome = 0, sum = 1
            {"B Y", 5}, // Y = draw, play = paper = 2, outcome = 3, sum = 5
            {"B Z", 9}, // Z = win, play = scissors = 3, outcome = 6, sum = 9
            {"C X", 2}, // X = loss, play = paper = 2, outcome = 0, sum = 2
            {"C Y", 6}, // Y = draw, play = scissors = 3, outcome = 3, sum = 6
            {"C Z", 7}, // Z = win, play = rock = 1, outcome = 6, sum = 7
        };
    }

    private static long Execute(string filename, int scoreSetId)
    {
        var lines = EmbeddedResourceReader.Read<Runner>(filename);

        var scoreSet = GetScoreSet(scoreSetId);

        long score = 0;
        foreach (var line in lines)
        {
            score += scoreSet[line];
        }

        return score;
    }
}