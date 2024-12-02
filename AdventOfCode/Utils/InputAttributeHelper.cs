using AdventOfCode.Puzzles;

namespace AdventOfCode.Utils;

internal static class InputAttributeHelper
{
    public static IEnumerable<string> ReadInput<T>() where T: IPuzzleInterface {

        var puzzleType = typeof(T);
        var puzzlePath = puzzleType.FullName!.Split('.')[^2..].ToList(); // Gets "2024/day01" from namespace
        var puzzleYear = puzzlePath[0].Substring(1); // Removes the '_'
        var puzzleDay = puzzlePath[1].ToLower(); 

        var inputFolderPath = FindProjectRootWithInputFolder();
        var inputFilePaths = Directory.GetFiles(Path.Combine(inputFolderPath, puzzleYear)).Where(f => Path.GetFileName(f).StartsWith(puzzleDay));

        var result = new List<string>();

        foreach (var relativePath in inputFilePaths)
        {
            var fullPath = Path.Combine(inputFolderPath, relativePath);
            
            if(File.Exists(fullPath) == false) continue;
            result.AddRange(File.ReadLines(fullPath));
        }

        return result;
    }

    /// <summary>
    ///     Searches for the project root directory containing the "Input" folder by traversing upwards.
    /// </summary>
    private static string FindProjectRootWithInputFolder() {
        var currentDir = Directory.GetCurrentDirectory();

        while(currentDir != null) {
            var inputFolderPath = Path.Combine(currentDir, "Input");
            if(Directory.Exists(inputFolderPath)) {
                return inputFolderPath;
            }

            // Move up one directory
            currentDir = Directory.GetParent(currentDir)?.FullName;
        }

        throw new DirectoryNotFoundException("Input Directory not found");
    }
}