using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Puzzles._2024;

internal class Day02 : PuzzleBase<Day02> {
    public override string SolvePart1()
    {
        int saveReports = 0;
        foreach (var row in Input)
        {
            var numbers = row.Split(' ').Select(int.Parse).ToList();
            if (IsSave(numbers)) saveReports++;
        }

        return saveReports.ToString();
    }

    public override string SolvePart2()
    {
        int saveReports = 0;
        foreach(var row in Input) {
            var numbers = row.Split( ).Select(int.Parse).ToList();
            var saveResult = IsSave(numbers);
            if(saveResult) saveReports++;
            else
            {
                // Try removeing any number
                for (int i = 0; i < numbers.Count; i++)
                {
                    var list1 = new List<int>(numbers);
                    list1.RemoveAt(i);

                    if (IsSave(list1))
                    {
                        saveReports++;
                        break;
                    }
                }
            }
        }

        return saveReports.ToString();
    }


    public bool IsSave(List<int> reportNumbers)
    {
        int? lastNumber = null;
        bool isIncreasing = false;
        bool isDecreasing = false;


        for (var i = 0; i < reportNumbers.Count; i++)
        {
            var number = reportNumbers[i];
            if (lastNumber is null)
            {
                lastNumber = number;
                continue;
            }

            // Check if last number is difference of 1 - 3
            var difference = lastNumber - number;
            List<int> saveNumbers = [-1,-2,-3,1,2,3];
            if (difference is null || saveNumbers.Contains(difference.Value) is false) 
                return false;

            if (number > lastNumber)
            {
                if (isDecreasing) return false;
                isIncreasing = true;
            }
            else
            {
                if (isIncreasing) return false;
                isDecreasing = true;
            }

            lastNumber = number;
        }

        return true;
    }

   
}