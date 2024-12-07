using AdventOfCode.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Puzzles._2024; 

internal class Day03 : PuzzleBase<Day03> {
    public override string SolvePart1() {

        List<int> multipliedNumbers = new();

        var text = string.Join(string.Empty, Input);
        
        int index = 0;

        while(index < text.Length) {

            var startingInBracket = indexOfNextMul(text, index);
            // if no nextmul foun break 
            if(startingInBracket == -1) break;

            var bracketClosing = indexOfNextClosingBracket(text, startingInBracket);

            var textInside = text.Substring(startingInBracket, bracketClosing-startingInBracket);
            if(CheckIfTextInsideBracketsIsValid(textInside, out int left, out int right)) {
                multipliedNumbers.Add(left*right);
                // If success continue looking from the end of the found mul
                index = bracketClosing;
            } else {
                // If failed start looking after the found mul
                index = startingInBracket;
            }
        }

        return multipliedNumbers.Sum().ToString();
    }

    public override string SolvePart2() {
        // Index the Which Parts are active and which parts are disabled
        List<int> multipliedNumbers = new();

        var text = string.Join(string.Empty, Input);

        Dictionary<int, bool> AreaActiveDictionary = IndexTheActiveAndInactiveAreas(text);

        int index = 0;

        while(index < text.Length) {

            var startingInBracket = indexOfNextMul(text, index);
            // if no nextmul foun break 
            if(startingInBracket == -1)
                break;

            var bracketClosing = indexOfNextClosingBracket(text, startingInBracket);

            var textInside = text.Substring(startingInBracket, bracketClosing-startingInBracket);
            if(CheckIfTextInsideBracketsIsValid(textInside, out int left, out int right)) {
                // Only add if its in an active area
                if(AreaActiveDictionary[startingInBracket]) multipliedNumbers.Add(left*right);

                // If success continue looking from the end of the found mul
                index = bracketClosing;
            } else {
                // If failed start looking after the found mul
                index = startingInBracket;
            }
        }

        return multipliedNumbers.Sum().ToString();
    }



    private Dictionary<int, bool> IndexTheActiveAndInactiveAreas(string text) {
        Dictionary<int, bool> result = new();

        var doText = "do()";
        var dontText = "don't()";

        var allStartDo = FindAllIndicesInString(text, doText);
        var allStartDont = FindAllIndicesInString(text, dontText);


        bool doIt = true;
        for(int i = 0; i<text.Length; i++) { 
            if(allStartDo.Contains(i)) doIt = true;
            if(allStartDont.Contains(i)) doIt = false;

            result.Add(i, doIt);
        }


        return result;
    }
    public static List<int> FindAllIndicesInString(string searchText, string match) {
        if(searchText == null)
            throw new ArgumentNullException(nameof(searchText));
        if(match == null)
            throw new ArgumentNullException(nameof(match));
        if(string.IsNullOrEmpty(match))
            throw new ArgumentException("Match string cannot be empty.", nameof(match));

        List<int> indices = new List<int>();
        int startIndex = 0;

        while((startIndex = searchText.IndexOf(match, startIndex, StringComparison.Ordinal)) != -1) {
            indices.Add(startIndex);
            startIndex += match.Length; // Move past the last found match to avoid overlapping results
        }

        return indices;
    }

    private bool CheckIfTextInsideBracketsIsValid(string text, out int leftNumber, out int rightNumber) {

        leftNumber = 0;
        rightNumber = 0;

        try {
            var twonumbers = text.Split(',');

            if(twonumbers.Length != 2) return false;
            // Could be Problem as i dont make sure its ONLY the number as whitespaces are fine with this code but shouldnt be 
            leftNumber = int.Parse(twonumbers[0]);
            rightNumber = int.Parse(twonumbers[1]);
            return true;
        } 
        catch {
            return false;
        }    
    }

    // xmul(2,4)%&mul[3,7]!@^
    // returns the index of inside the brackets
    private int indexOfNextMul(string input, int startIndex = 0) {
        var searchMulString = "mul(";

        var indexOf = input.IndexOf(searchMulString, startIndex);
        if(indexOf == -1)
            return -1;
        return indexOf + searchMulString.Length;
    }
    private int indexOfNextClosingBracket(string input, int startIndex = 0) {
        var searchMulString = ")";

        var indexOf = input.IndexOf(searchMulString, startIndex);
        return indexOf;
    }
}
