using AdventOfCode2022.Puzzles.Utils;
using FluentAssertions;
using Xunit;

namespace AdventOfCode2022.Puzzles.Puzzle5;

public class Runner
{
    [Theory]
    [InlineData("TestInput", "CMZ")]
    [InlineData("Input", "FRDSQRRCD")]
    public void RunAlpha(string filename, string expected)
    {
        var actual = Execute(filename, false);
        actual.Should().Be(expected);
    }

    [Theory]
    [InlineData("TestInput", "MCD")]
    [InlineData("Input", "HRFTQVWNN")]
    public void RunBeta(string filename, string expected)
    {
        var actual = Execute(filename, true);
        actual.Should().Be(expected);
    }

    private string Execute(string filename, bool reverse)
    {
        var lines = EmbeddedResourceReader.Read<Runner>(filename);

        var emptyLineIndex = FindFirstEmptyLine(lines);
        var stacks = InitializeStacks(emptyLineIndex, lines);

        var index = emptyLineIndex + 1;
        while (index < lines.Length)
        {
            HandleCommand(stacks, lines[index], reverse);
            index++;
        }

        var result = new List<char>();
        foreach (var stack in stacks)
        {
            result.Add(stack.Count == 0 ? ' ' : stack.Peek());
        }

        return new string(result.ToArray());
    }

    private static void HandleCommand(List<Stack<char>> stacks, string command, bool reverse)
    {
        command = command
            .Replace("move", string.Empty)
            .Replace("from", string.Empty)
            .Replace("to", string.Empty)
            .Replace("  ", " ")
            .Trim();

        var numbers = command.Split(" ").ToArray();
        var quantity = int.Parse(numbers[0]);
        var sourceStack = stacks[int.Parse(numbers[1]) - 1];
        var targetStack = stacks[int.Parse(numbers[2]) - 1];

        var tempStack = new List<char>();
        for (var i = 0; i < quantity; i++)
        {
            tempStack.Add(sourceStack.Pop());
        }

        if (reverse)
        {
            tempStack.Reverse();
        }

        for (var i = 0; i < quantity; i++)
        {
            targetStack.Push(tempStack[i]);
        }
    }

    private static List<Stack<char>> InitializeStacks(int emptyLineIndex, string[] lines)
    {
        const string numbers = "0123456789";

        var stackNumberingLineIndex = emptyLineIndex - 1;
        var stackNumberingLine = lines[stackNumberingLineIndex];
        var stacks = new List<Stack<char>>();
        var stackCount = stackNumberingLine.Count(numbers.Contains);
        for (var i = 0; i < stackCount; i++)
        {
            stacks.Add(new Stack<char>());
        }

        var stackPosition = new int[stackCount];
        for (var i = 0; i < stackCount; i++)
        {
            var stackId = i + 1;
            var pos = stackNumberingLine.IndexOf(stackId.ToString(), StringComparison.Ordinal);
            stackPosition[i] = pos;
        }

        var index = stackNumberingLineIndex - 1;
        while (index >= 0)
        {
            var line = $"{lines[index]}{new string(' ', 100)}";
            for (var i = 0; i < stackCount; i++)
            {
                var stackChar = line.Substring(stackPosition[i], 1).First();
                if (stackChar != ' ')
                {
                    stacks[i].Push(stackChar);
                }
            }

            index--;
        }

        return stacks;
    }

    private static int FindFirstEmptyLine(string[] lines)
    {
        var index = 0;
        while (!string.IsNullOrEmpty(lines[index]))
        {
            index++;
        }

        return index;
    }
}