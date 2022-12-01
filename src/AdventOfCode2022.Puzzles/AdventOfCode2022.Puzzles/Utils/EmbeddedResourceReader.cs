using System.Reflection;

namespace AdventOfCode2022.Puzzles.Utils
{
    internal class EmbeddedResourceReader
    {
        public static string Read(string folder, string filename)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = $"{typeof(EmbeddedResourceReader).Namespace?.Replace(".Utils", "")}.{folder}.{filename}.txt";

            using var stream = assembly.GetManifestResourceStream(resourceName);
            using var reader = new StreamReader(stream!);
            var result = reader.ReadToEnd();
            return result;
        }
    }
}
