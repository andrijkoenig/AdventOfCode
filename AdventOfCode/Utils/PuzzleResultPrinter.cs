using AdventOfCode.Models;

namespace AdventOfCode.Utils;

public static class PuzzleResultPrinter
{
    public static void PrettyPrint(IEnumerable<PuzzleResult> results)
    {
        if (!results.Any())
        {
            Console.WriteLine("No results to display.");
            return;
        }

        var headers = GetHeaders();
        var columnWidths = CalculateColumnWidths(headers, results);

        PrintHeader(headers, columnWidths);
        PrintSeparator(columnWidths);

        foreach (var result in results) PrintRow(GetRowValues(result, headers), columnWidths);

        PrintSeparator(columnWidths);
    }

    /// <summary>
    ///     Retrieves the headers dynamically from PuzzleResult properties.
    /// </summary>
    private static string[] GetHeaders()
    {
        return typeof(PuzzleResult).GetProperties().Select(prop => prop.Name).ToArray();
    }

    /// <summary>
    ///     Calculates the maximum width for each column based on headers and data.
    /// </summary>
    private static int[] CalculateColumnWidths(string[] headers, IEnumerable<PuzzleResult> results)
    {
        return headers.Select((header, index) =>
                Math.Max(header.Length, results.Max(result => GetPropertyValue(result, header).Length)))
            .ToArray();
    }

    /// <summary>
    ///     Retrieves the property value as a string for a given header name.
    /// </summary>
    private static string GetPropertyValue(PuzzleResult result, string propertyName)
    {
        var property = typeof(PuzzleResult).GetProperty(propertyName);
        return property?.GetValue(result)?.ToString() ?? string.Empty;
    }

    /// <summary>
    ///     Gets row values as strings based on property names (headers).
    /// </summary>
    private static string[] GetRowValues(PuzzleResult result, string[] headers)
    {
        return headers.Select(header => GetPropertyValue(result, header)).ToArray();
    }

    /// <summary>
    ///     Prints the table header row.
    /// </summary>
    private static void PrintHeader(string[] headers, int[] columnWidths)
    {
        PrintRow(headers, columnWidths);
    }

    /// <summary>
    ///     Prints a row of values formatted to the specified column widths.
    /// </summary>
    private static void PrintRow(string[] values, int[] columnWidths)
    {
        for (var i = 0; i < values.Length; i++) Console.Write($"| {values[i].PadRight(columnWidths[i])} ");
        Console.WriteLine("|");
    }

    /// <summary>
    ///     Prints a separator line based on column widths.
    /// </summary>
    private static void PrintSeparator(int[] columnWidths)
    {
        foreach (var width in columnWidths)
        {
            Console.Write("+");
            Console.Write(new string('-', width + 2)); // Add padding for aesthetics
        }

        Console.WriteLine("+");
    }
}