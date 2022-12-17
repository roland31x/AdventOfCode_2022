using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D10
{
    internal class Program
    {
        public static int Sum = 0;
        public static int Reg = 1;
        static void Main(string[] args)
        {
            int cycle = 0;
            using(StreamReader sr = new StreamReader("input.txt"))
            {
                while (!sr.EndOfStream)
                {
                    string s = sr.ReadLine();
                    string[] tokens = s.Split(' ');
                    if (tokens[0] == "noop")
                    {
                        Cycle(ref cycle);
                        continue;
                    }
                    if (tokens[0] == "addx")
                    {
                        Cycle(ref cycle);
                        Cycle(ref cycle);
                        Reg += int.Parse(tokens[1]);
                    }
                }
            }
            Console.WriteLine(Sum);
        }
        public static void Cycle(ref int cycle)
        {
            // PART 2 
            int refc = cycle % 40;

            if (refc == Reg - 1 || refc == Reg + 1 || refc == Reg )
            {
                Console.Write("#");
            }
            else Console.Write(".");
            //

            cycle++;
            CycleCheck(cycle);
        }
        public static void CycleCheck(int cycle)
        {
            if (cycle % 40 == 0)  // ( CYCLE + 20 ) % 40 == 0  PART 1
            {
               // Sum += Reg * cycle; // - PART 1

                Console.WriteLine();
            }           
        }
    }
}
