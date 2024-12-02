using AdventOfCode.Utils;

namespace AdventOfCode.Puzzles._2023;

internal class Day01 : PuzzleBase<Day01>
{
    private readonly List<char> _numbers = ['0', '1', '2', '3', '4', '5', '6', '7', '8', '9'];

    private readonly Dictionary<string, char> _textNumbers = new()
    {
        { "zero", '0' },
        { "one", '1' },
        { "two", '2' },
        { "three", '3' },
        { "four", '4' },
        { "five", '5' },
        { "six", '6' },
        { "seven", '7' },
        { "eight", '8' },
        { "nine", '9' },
    };


    public override string SolvePart1()
    {
        List<int> numbers = new List<int>();

        foreach (var row in Input)
        {
            char? firstNumberInThisRow = null;
            char lastNumberInThisRow = '-';
            foreach (var charEntry in row.Where(charEntry => _numbers.Contains(charEntry)))
            {
                firstNumberInThisRow ??= charEntry;
                lastNumberInThisRow = charEntry;
            }

            if (lastNumberInThisRow == '-' || firstNumberInThisRow is null) continue;

            numbers.Add(int.Parse($"{firstNumberInThisRow}{lastNumberInThisRow}"));

        }

        return numbers.Sum().ToString();
    }

    public override string SolvePart2()
    {
        List<int> numbers = new List<int>();

        foreach(var row in Input) {
            char? firstNumberInThisRow = null;
            char? lastNumberInThisRow = null;
            // replace text with char
            string rowData = row;
            foreach (var keyValuePair in _textNumbers)
            {
                rowData = rowData.Replace(keyValuePair.Key, keyValuePair.Value.ToString());
            }

            foreach(var charEntry in rowData.Where(charEntry => _numbers.Contains(charEntry))) {
                firstNumberInThisRow ??= charEntry;
                lastNumberInThisRow = charEntry;
            }

            if(lastNumberInThisRow == '-' || firstNumberInThisRow is null) continue;

            numbers.Add(int.Parse($"{firstNumberInThisRow}{lastNumberInThisRow}"));

        }

        return numbers.Sum().ToString();
    }
}