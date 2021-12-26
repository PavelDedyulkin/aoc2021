using System;
using System.IO;
using System.Runtime.Intrinsics.X86;

namespace Aoc2021.PDedyulk
{
    class Program
    {
        static void aoc1(string input)
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

        static void aoc2(string input)
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

        static void aoc3(string input)
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

        static void Main()
        {
            aoc3(@"./data/3/input.txt");
        }
    }
}