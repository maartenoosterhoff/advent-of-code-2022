using AdventOfCode2022.Puzzles.Utils;
using FluentAssertions;
using Xunit;

namespace AdventOfCode2022.Puzzles.Puzzle1;

public class Runner
{
    [Theory]
    [InlineData("TestInput", 24000L)]
    [InlineData("Input", 69912L)]
    public void RunAlpha(string filename, long expected)
    {
        var actual = Execute(filename).MaxBy(x => x.calorieTotal).calorieTotal;
        actual.Should().Be(expected);
    }

    [Theory]
    [InlineData("TestInput", 45000L)]
    [InlineData("Input", 208180L)]
    public void RunBeta(string filename, long expected)
    {
        var actual = Execute(filename)
            .OrderByDescending(x => x.calorieTotal)
            .Take(3)
            .Select(x => x.calorieTotal)
            .Sum();

        actual.Should().Be(expected);
    }

    private static IEnumerable<(int elfId, long calorieTotal)> Execute(string filename)
    {
        var lines = EmbeddedResourceReader.Read<Runner>(filename);

        List<(int elfId, List<long> calories)> data = new();

        (int elfId, List<long> calories) current = (1, new List<long>());
        foreach (var line in lines)
        {
            if (string.IsNullOrEmpty(line))
            {
                data.Add(current);
                current = (current.elfId + 1, new List<long>());
            }
            else
            {
                current.calories.Add(long.Parse(line));
            }
        }

        data.Add(current);

        return data.Select(x => (x.elfId, x.calories.Sum()));
    }
}