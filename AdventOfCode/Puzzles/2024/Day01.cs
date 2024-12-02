using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventOfCode.Utils;

namespace AdventOfCode.Puzzles._2024;

internal class Day01 : PuzzleBase<Day01> {

    public override string SolvePart1()
    {
        List<int> leftList = new List<int>();
        List<int> rightList = new List<int>();

        foreach (var row in Input)
        {
            var numbers = row.Split("   ");

            var leftNumber = int.Parse(numbers[0]);
            var rightNumber = int.Parse(numbers[1]);

            leftList.Add(leftNumber);
            rightList.Add(rightNumber);
        }

        leftList = leftList.Order().ToList();
        rightList = rightList.Order().ToList();

        int result = 0;
        for (int i = 0; i < leftList.Count; i++)
        {
            result += GetDistance(leftList[i], rightList[i]);
        }

        return result.ToString();
    }

    public override string SolvePart2()
    {
        List<int> leftList = new ();
        Dictionary<int,int> rightList = new ();

        foreach(var row in Input) {
            var numbers = row.Split("   ");

            var leftNumber = int.Parse(numbers[0]);
            var rightNumber = int.Parse(numbers[1]);

            leftList.Add(leftNumber);

            if (rightList.TryGetValue(rightNumber, out var value))
            {
                rightList[rightNumber] = value +1;
            }
            else
            {
                rightList.Add(rightNumber,1);
            }
        }
        
        int result = 0;
        for(int i = 0; i < leftList.Count; i++) {

            var leftNumber = leftList[i];
            int multiplier = 0;
            if(rightList.TryGetValue(leftNumber, out var value)) {
               multiplier = value;
            }

            result += leftNumber * multiplier;
        }

        return result.ToString();
    }


    private int GetDistance(int number1, int number2)
    {
        if(number1 >= number2) return number1 - number2;
        return number2 - number1;
    }

}