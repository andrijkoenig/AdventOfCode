using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Puzzles._2024;
internal class Day04 : PuzzleBase<Day04> {
    public override string SolvePart1() {

        var Array2D = Create2DArray();

        int totalCount = 0;

        for(int y = 0; y < Array2D.Length; y++) {            
            for(int x = 0; x < Array2D[y].Length; x++) {
                totalCount += FindXmasFromCordinate(Array2D, x, y);
            }
        }

        return totalCount.ToString();
    }

    public override string SolvePart2() {
        var Array2D = Create2DArray();

        int totalCount = 0;

        for(int y = 0; y < Array2D.Length; y++) {
            for(int x = 0; x < Array2D[y].Length; x++) {
                totalCount += FindMasInFormOfAnXFromCordinate(Array2D, x, y);
            }
        }

        return totalCount.ToString();
    }

    int FindMasInFormOfAnXFromCordinate(char[][] array, int x, int y) {

        if(array[y][x] != 'A') return 0;
        // I now only need to check if the diagonals both match to MAS

        // SO I FIRST GET THE 4 CHARS AGAIN IN TRY SO I DONT HAVE TO CHECK BOUNDS
        try {

            char topLeft = array[y-1][x-1];
            char topRight = array[y-1][x+1];
            char bottomLeft = array[y+1][x-1];
            char bottomRight = array[y+1][x+1];

            bool firstDiagonalIsValid = (topLeft == 'M' && bottomRight == 'S') || (topLeft == 'S' && bottomRight == 'M');

            bool secondDiagonalIsValid = (bottomLeft == 'M' && topRight == 'S') || (bottomLeft == 'S' && topRight == 'M');

            if(firstDiagonalIsValid && secondDiagonalIsValid)
                return 1;

            return 0;
        }
        catch { 
            return 0; 
        }     
    }

    char[][] Create2DArray() { 
        return Input.Select(x=>x.ToCharArray()).ToArray();    
    }

    int FindXmasFromCordinate(char[][] array, int x, int y) {
        int counter = 0;

        if(array[y][x] != 'X') return counter;
        

        // TODO implement Methods for check horizontal backwards and forwards
        var getRow = array[y];
        counter += GetXmasFromArray(getRow, x);
        
        // TODO implement Methods for check vertical backwards and forwards
        char[] columnArray = new char[array.Length];

        // Extract the values from the specified column
        for(int i = 0; i < array.Length; i++) {
            columnArray[i] = array[i][x];
        }
        counter += GetXmasFromArray(columnArray, y);

        // TODO implement Methods for check diagonal backwards and forwards all 4 directions

        counter += extractsXmasFromDiagonals(array, x, y);
        return counter;
    }

    public int extractsXmasFromDiagonals(char[][] array, int x, int y) {
        int total = 0;
        // top left
        if(checkTopLeft(array, x, y))
            total++;
        if(checkTopRight(array, x, y))
            total++;
        if(checkBottomLeft(array, x, y))
            total++;
        if(checkBottomRight(array, x, y))
            total++;


        return total;
        bool checkTopLeft(char[][] array, int x, int y) {
            // stupid i know but i dont want to sanity check the array accessors atm
            try {
                var char1 = array[y-1][x-1];
                var char2 = array[y-2][x-2];
                var char3 = array[y-3][x-3];

                if(char1 == 'M' && char2 == 'A' && char3 == 'S')
                    return true;
                return false;
            }
            catch {
                return false;
            }
        }
        bool checkTopRight(char[][] array, int x, int y) {
            // stupid i know but i dont want to sanity check the array accessors atm
            try {
                var char1 = array[y-1][x+1];
                var char2 = array[y-2][x+2];
                var char3 = array[y-3][x+3];

                if(char1 == 'M' && char2 == 'A' && char3 == 'S')
                    return true;
                return false;
            } catch {
                return false;
            }
        }
        bool checkBottomLeft(char[][] array, int x, int y) {
            // stupid i know but i dont want to sanity check the array accessors atm
            try {
                var char1 = array[y+1][x-1];
                var char2 = array[y+2][x-2];
                var char3 = array[y+3][x-3];

                if(char1 == 'M' && char2 == 'A' && char3 == 'S')
                    return true;
                return false;
            } catch {
                return false;
            }
        }
        bool checkBottomRight(char[][] array, int x, int y) {
            // stupid i know but i dont want to sanity check the array accessors atm
            try {
                var char1 = array[y+1][x+1];
                var char2 = array[y+2][x+2];
                var char3 = array[y+3][x+3];

                if(char1 == 'M' && char2 == 'A' && char3 == 'S')
                    return true;
                return false;
            } catch {
                return false;
            }
        }
    }



    int GetXmasFromArray(char[] array, int x) {
        int total = 0;

        // sanity check - its redundant
        if(array[x] != 'X') return 0;

        // check forward is possible
        if(array.Length > x + 3) {
            //now check if char matches
            var char1 = array[x +1];
            var char2 = array[x +2];
            var char3 = array[x +3];
            if(char1 == 'M' && char2 == 'A' && char3 == 'S') total++;
        }

        //Check if backwards is possible
        if(x >= 3) {
            //now check if char matches
            //now check if char matches
            var char1 = array[x -1];
            var char2 = array[x -2];
            var char3 = array[x -3];
            if(char1 == 'M' && char2 == 'A' && char3 == 'S') total++;
        }

        return total;
    }
}
