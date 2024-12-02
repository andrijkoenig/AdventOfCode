using AdventOfCode.Utils;

namespace AdventOfCode.Puzzles;

public abstract class PuzzleBase<T> : IPuzzleInterface where T : PuzzleBase<T> 
{
    protected IEnumerable<string> Input { get; set; }

    protected PuzzleBase()
    {
        Input = InputAttributeHelper.ReadInput<T>();
    }

    public abstract string SolvePart1();

    public abstract string SolvePart2();
}