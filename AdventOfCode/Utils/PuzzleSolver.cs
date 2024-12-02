using System.Reflection;
using AdventOfCode.Models;
using AdventOfCode.Puzzles;

namespace AdventOfCode.Utils;

/// <summary>
///     Enables me to call and run all Puzzles via Reflection.
///     Not the prettiest Solution and also not the fastest but good enough for me!
/// </summary>
public static class PuzzleSolver
{
    public static IEnumerable<PuzzleResult> RunAllPuzzles()
    {
        var puzzleTypes = GetPuzzleTypes();

        foreach (var puzzleType in puzzleTypes) yield return RunPuzzle(puzzleType);
    }

    public static PuzzleResult RunPuzzle<T>() where T : PuzzleBase<T>
    {
        return RunPuzzle(typeof(T));
    }

    /// <summary>
    ///     Finds and returns all types that inherit from PuzzleBase<>.
    /// </summary>
    private static Type[] GetPuzzleTypes()
    {
        return Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(IsPuzzleType)
            .ToArray();
    }

    /// <summary>
    ///     Determines if a type inherits from PuzzleBase<>.
    /// </summary>
    private static bool IsPuzzleType(Type type)
    {
        return type.BaseType is { IsGenericType: true } &&
               type.BaseType.GetGenericTypeDefinition() == typeof(PuzzleBase<>);
    }

    /// <summary>
    ///     Runs the specified puzzle by instantiating it and calling SolvePart1 and SolvePart2.
    /// </summary>
    private static PuzzleResult RunPuzzle(Type puzzleType)
    {
        // Instantiate the puzzle using reflection
        var puzzleInstance = Activator.CreateInstance(puzzleType) as dynamic;

        var year = puzzleType.Namespace.Split('.').Last().Substring(1);
        
        // Call SolvePart1 and SolvePart2 and return results
        string part1Result = puzzleInstance?.SolvePart1() ?? throw new InvalidOperationException();
        string part2Result = puzzleInstance?.SolvePart2() ?? throw new InvalidOperationException();

        return new(Year: year, Day: puzzleType.Name, SolutionPart1: part1Result, SolutionPart2: part2Result);
    }
}