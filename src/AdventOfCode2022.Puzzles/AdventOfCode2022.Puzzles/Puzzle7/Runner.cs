using AdventOfCode2022.Puzzles.Utils;
using FluentAssertions;
using Xunit;

namespace AdventOfCode2022.Puzzles.Puzzle7;

public class Runner
{
    [Theory]
    [InlineData("TestInput", 95437L)]
    [InlineData("Input", 1770595L)]
    public void RunAlpha(string filename, long expected)
    {
        var allFolders = Execute(filename);
        var allFoldersWithSize = allFolders.Select(x => new { Folder = x, TotalSize = x.GetTotalSize() }).ToList();

        const long atMostValue = 100000;

        var actual = allFoldersWithSize
            .Where(x => x.TotalSize <= atMostValue)
            .Select(x => x.TotalSize)
            .Sum();

        actual.Should().Be(expected);
    }

    [Theory]
    [InlineData("TestInput", 24933642L)]
    [InlineData("Input", 2195372L)]
    public void RunBeta(string filename, long expected)
    {
        var allFolders = Execute(filename);
        var allFoldersWithSize = allFolders.Select(x => new { Folder = x, TotalSize = x.GetTotalSize() }).ToList();

        const long TotalSpace = 70000000;
        const long UpdateRequiredSpace = 30000000;

        var occupiedSpace = allFoldersWithSize.Select(x => x.TotalSize).Max();
        var currentlyUnusedSpace = TotalSpace - occupiedSpace;
        var requiredCleanupSpace = UpdateRequiredSpace - currentlyUnusedSpace;

        var folderToDelete = allFoldersWithSize
            .Where(x => x.TotalSize >= requiredCleanupSpace)
            .OrderBy(x => x.TotalSize)
            .First();

        var actual = folderToDelete.TotalSize;

        actual.Should().Be(expected);
    }

    private static List<XFolder> Execute(string filename)
    {
        var input = EmbeddedResourceReader.Read<Runner>(filename);

        var lineIndex = 0;
        var lineCount = input.Length;
        var rootFolder = new XFolder("/", null);
        var currentFolder = rootFolder;

        while (lineIndex < lineCount)
        {
            var line = input[lineIndex];

            if (line.StartsWith("$"))
            {
                var cmdParts = line.Split(' ');
                switch (cmdParts[1])
                {
                    case "cd":
                        {
                            var cdParameter = cmdParts[2];
                            if (cdParameter == "/")
                            {
                                currentFolder = rootFolder;
                            }
                            else if (cdParameter == "..")
                            {
                                currentFolder = currentFolder!.ParentFolder;
                            }
                            else
                            {
                                var nextFolderName = cdParameter;
                                var nextFolder = currentFolder!.SubFolders.First(x => x.Name == nextFolderName);
                                currentFolder = nextFolder;
                            }

                            break;
                        }
                }
            }
            else
            {
                var lsParts = line.Split(" ");
                if (lsParts[0] == "dir")
                {
                    var newFolderName = lsParts[1];
                    currentFolder!.AddFolder(newFolderName);
                }
                else
                {
                    var size = long.Parse(lsParts[0]);
                    var newFileName = lsParts[1];
                    currentFolder!.AddFile(new(newFileName, size));
                }
            }

            lineIndex++;
        }

        return rootFolder.GetAllFolders().ToList();
    }

    public record XFolder(string Name, XFolder? ParentFolder)
    {
        public List<XFolder> SubFolders { get; } = new();
        public List<XFile> Files { get; } = new();

        public XFolder AddFolder(string name)
        {
            var subFolder = new XFolder(name, this);
            SubFolders.Add(subFolder);
            return subFolder;
        }
        public void AddFile(XFile file) => Files.Add(file);

        public long GetTotalSize()
        {
            return Files.Select(x => x.Size).Sum() + SubFolders.Select(x => x.GetTotalSize()).Sum();
        }

        public IEnumerable<XFolder> GetAllFolders()
        {
            yield return this;
            foreach (var subFolderChild in SubFolders.SelectMany(subFolder => subFolder.GetAllFolders()))
            {
                yield return subFolderChild;
            }
        }
    }

    public record XFile(string Name, long Size);
}