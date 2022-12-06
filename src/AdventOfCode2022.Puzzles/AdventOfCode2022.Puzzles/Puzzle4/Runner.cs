using AdventOfCode2022.Puzzles.Utils;
using FluentAssertions;
using Xunit;

namespace AdventOfCode2022.Puzzles.Puzzle4;

public class Runner
{
    [Theory]
    [InlineData("TestInput", 2L)]
    [InlineData("Input", 485L)]
    public void RunAlpha(string filename, long expected)
    {
        var actual = Execute(filename, CheckType.Contains);
        actual.Should().Be(expected);
    }

    [Theory]
    [InlineData("TestInput", 4L)]
    [InlineData("Input", 857L)]
    public void RunBeta(string filename, long expected)
    {
        var actual = Execute(filename, CheckType.HasOverlap);
        actual.Should().Be(expected);
    }

    private static long Execute(string filename, CheckType checkType)
    {
        var lines = EmbeddedResourceReader.Read(typeof(Runner).Namespace!.Replace("AdventOfCode2022.Puzzles.", string.Empty), filename)
            .Replace(Environment.NewLine, "\n")
            .Replace("\r", "\n")
            .Split("\n");

        long score = 0;
        foreach (var line in lines)
        {
            var pair = line.Split(',');
            var range1 = SectionRange.Parse(pair[0]);
            var range2 = SectionRange.Parse(pair[1]);

            if (checkType == CheckType.Contains)
            {
                if (range1.Contains(range2) || range2.Contains(range1))
                {
                    score++;
                }
            }
            else if (checkType == CheckType.HasOverlap)
            {
                if (range1.HasOverlap(range2) || range2.HasOverlap(range1))
                {
                    score++;
                }
            }
        }

        return score;
    }

    public enum CheckType
    {
        Contains,
        HasOverlap
    }

    public class SectionRange
    {
        private readonly int _start;
        private readonly int _stop;

        public SectionRange(int start, int stop)
        {
            _start = start;
            _stop = stop;
        }

        public static SectionRange Parse(string value)
        {
            var numbers = value.Split('-').ToArray();
            return new(int.Parse(numbers[0]), int.Parse(numbers[1]));
        }

        public bool Contains(SectionRange other)
        {
            if (other._start >= this._start &&
                    other._stop <= this._stop)
            {
                return true;
            }

            return false;
        }

        public bool HasOverlap(SectionRange other)
        {
            var otherIsBefore = other._stop < this._start;
            var otherIsAfter = other._start > this._stop;

            return !(otherIsBefore || otherIsAfter);
        }
    }
}