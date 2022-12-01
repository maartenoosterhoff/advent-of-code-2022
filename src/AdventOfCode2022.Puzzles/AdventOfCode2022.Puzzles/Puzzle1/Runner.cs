using AdventOfCode2022.Puzzles.Utils;
using FluentAssertions;
using Xunit;

namespace AdventOfCode2022.Puzzles.Puzzle1
{
    public class Runner
    {
        [Fact]
        public void RunAlphaTest()
        {
            var actual = Execute("TestInput").MaxBy(x => x.calorieTotal).calorieTotal;
            actual.Should().Be(24000L);
        }

        [Fact]
        public void RunAlpha()
        {
            var actual = Execute("Input").MaxBy(x => x.calorieTotal).calorieTotal;
            actual.Should().Be(69912L);
        }

        [Fact]
        public void RunBetaTest()
        {
            var actual = Execute("TestInput")
                .OrderByDescending(x => x.calorieTotal)
                .Take(3)
                .Select(x => x.calorieTotal)
                .Sum();

            actual.Should().Be(45000L);
        }

        [Fact]
        public void RunBeta()
        {
            var actual = Execute("Input")
                .OrderByDescending(x => x.calorieTotal)
                .Take(3)
                .Select(x => x.calorieTotal)
                .Sum();
         
            actual.Should().Be(208180L);
        }

        private static IEnumerable<(int elfId, long calorieTotal)> Execute(string filename)
        {
            var lines = EmbeddedResourceReader.Read("Puzzle1", filename)
                .Replace(Environment.NewLine, "\n")
                .Replace("\r", "\n")
                .Split("\n");
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
}
