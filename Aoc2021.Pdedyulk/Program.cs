using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Intrinsics.X86;

namespace Aoc2021.PDedyulk
{
    class Program
    {
        static void day1(string input)
        {
            int cnt = 0;
            var s = new int[3];
            
            var index = 0;
            
            foreach (var line in File.ReadLines(input))
            {
               
               var curr = int.Parse(line);
               if (index < 3)
               {
                   s[index] = curr;
               
                   index++;
                   continue;
               }
               
               var prev = s[0] + s[1] + s[2];
               if ((s[2] + s[1] + curr) > prev)
               {
                   cnt++;
               }

               s[0] = s[1];
               s[1] = s[2];
               s[2] = curr;
            }
            Console.WriteLine(cnt);

            
            
        }

        static void day2(string input)
        {
            decimal hor = 0;
            decimal aim = 0;
            decimal depth = 0;
            foreach (var line in File.ReadLines(input))
            {
               var  words = line.Split(' ');
               var move = int.Parse(words[1]);
               if (words[0][0] == 'f')
               {
                   hor += move;
                   depth += aim * move;
                   continue;
               }

               if (words[0][0] == 'd')
               {
                   aim += move;
                   continue;
               }

               if (words[0][0] == 'u')
               {
                   aim-=move;
                   continue;
               }
            }
            Console.WriteLine(depth*hor);

        }

        static void day3(string input)
        {
            
            int Count1InCol(char[] mask, int col, out int total, out string firstLine)
            {
                total = 0;
                var result = 0;
                firstLine = String.Empty;
                foreach (var line in File.ReadLines(input))
                {
                    var skip = false;
                    for (var i = 0; i < col; ++i)
                    {
                        if (line[i] == mask[i]) continue;
                        skip = true;
                        break;
                    }
                    if (skip) continue;
                    firstLine = line;
                    if (line[col] == '1') result++;
                    total++;
                }

                return result;
            }


            char[] CalcCoeff(Func<int, int, char> maskValue)
            {
                var mask = new char[sizeof(UInt64)*8];
            
                int i=0;
            
                while(true)
                {
                    var cnt=Count1InCol(mask, i, out var total, out var line);
                    mask[i] = maskValue(cnt, total);

                    if (total == 1)
                    {
                        return line.ToCharArray();
                    }

                    i++;
                    if (i == line.Length)
                    {
                        break;
                    }
                }
                
                var result = new char[i];
                for (int j = 0; j < result.Length; j++)
                {
                    result[j] = mask[j];
                }

                return result;
            }


            var gammas = CalcCoeff((cnt1,total)=> cnt1>=total-cnt1?'1':'0');
            
            var epsilons = CalcCoeff((cnt1,total)=> cnt1<total-cnt1?'1':'0');
            
            
            var gamma = Convert.ToUInt64(new string(gammas), 2);
            var epsilon = Convert.ToUInt64(new string(epsilons), 2);
           
            Console.WriteLine(gamma*epsilon);

        }

        static void day4(string input)
        {
            int[] numbers = null;
            
            var boardNumbers = new List<int>();
            
            //fill numbers and board numbers
            foreach (var line in File.ReadLines(input))
            {
                if (numbers == null)
                {
                    numbers = line.Split(',').Select(r => int.Parse(r)).ToArray();
                    continue;
                }
                
                if (line == "") continue;

                foreach (var n in line.Split(' ',StringSplitOptions.RemoveEmptyEntries)
                    .Select(r => int.Parse(r)))
                {
                    boardNumbers.Add(n);
                }
            }

            const int size = 5;
     
            int checkWinBoard(int crossingIndex)
            {
              var board =  crossingIndex / (size * size);
              var boardIndex = crossingIndex % (size * size);
              var boardRow = boardIndex / size;
              var boardCol = boardIndex % size;

              var boardStart = board * size * size;
              bool crossed = true;
              for (int col = 0; col < size; ++col)
              {
                  if (boardNumbers[boardStart + boardRow * size + col] != -1)
                  {
                      crossed = false;
                      break;
                  }
              }

              if (crossed)
                  return board;
              
              crossed = true;
              for (int row = 0; row < size; ++row)
              {
                  if (boardNumbers[boardStart + boardCol  + row*size] != -1)
                  {
                      crossed = false;
                      break;
                  }
              }
              
              if (crossed)
                  return board;


              return -1;
            }

            
             var lastBoardResult = -1;
             var lastBoard = -1;


             foreach (var numberToCross in numbers)
             {
                 for (int i = 0; i < boardNumbers.Count; i++)
                 {
                     if (boardNumbers[i] == numberToCross)
                     {
                         boardNumbers[i] = -1;
                         var winBoard = checkWinBoard(i);
                         if (winBoard != -1)
                         {
                             
                                 lastBoard = winBoard;
                                 var s = 0;
                                 for (int k = winBoard * size * size; k < winBoard * size * size + (size * size); ++k)
                                 {
                                     if (boardNumbers[k] != -1)
                                     {
                                         s += boardNumbers[k];
                                         boardNumbers[k] = -1;
                                     }
                                 }

                                 lastBoardResult = s * numberToCross;
                             
                     }

                     }
                 }
             }


             Console.WriteLine(lastBoard);
            Console.WriteLine(lastBoardResult);
        }

        static void Main()
        {
            day4(@"./data/4/input.txt");
        }
    }
}