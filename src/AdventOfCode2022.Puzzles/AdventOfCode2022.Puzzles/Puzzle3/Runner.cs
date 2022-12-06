using AdventOfCode2022.Puzzles.Utils;
using FluentAssertions;
using Xunit;

namespace AdventOfCode2022.Puzzles.Puzzle3;

public class Runner
{
    [Theory]
    [InlineData("TestInput", 157L)]
    [InlineData("Input", 7831L)]
    public void RunAlpha(string filename, long expected)
    {
        var actual = Execute(filename);
        actual.Should().Be(expected);
    }

    [Theory]
    [InlineData("TestInput", 70L)]
    [InlineData("Input", 2683L)]
    public void RunBeta(string filename, long expected)
    {
        var actual = Execute2(filename);
        actual.Should().Be(expected);
    }

    private static Dictionary<char, int> GetPrioritySet()
    {
        var set = new Dictionary<char, int>();
        for (var i = 0; i < 26; i++)
        {
            set.Add((char)(i + 'a'), i + 1);
            set.Add((char)(i + 'A'), i + 27);
        }

        return set;
    }

    private static long Execute(string filename)
    {
        var lines = EmbeddedResourceReader.Read(typeof(Runner).Namespace!.Replace("AdventOfCode2022.Puzzles.", string.Empty), filename)
            .Replace(Environment.NewLine, "\n")
            .Replace("\r", "\n")
            .Split("\n");

        long score = 0;
        var set = GetPrioritySet();
        foreach (var line in lines)
        {
            var part1 = line[..(line.Length / 2)];
            var part2 = line[(line.Length / 2)..];

            var charInBoth = part1.Intersect(part2);
            score += set[charInBoth.First()];
        }

        return score;
    }

    private static long Execute2(string filename)
    {
        var lines = EmbeddedResourceReader.Read(typeof(Runner).Namespace!.Replace("AdventOfCode2022.Puzzles.", string.Empty), filename)
            .Replace(Environment.NewLine, "\n")
            .Replace("\r", "\n")
            .Split("\n")
            .ToArray();

        long score = 0;
        var set = GetPrioritySet();
        var index = 0;
        while (index < lines.Length)
        {
            var charInBoth = lines[index].Intersect(lines[index + 1]).Intersect(lines[index + 2]);
            score += set[charInBoth.First()];

            index += 3;
        }

        return score;
    }
}