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
            Console.WriteLine("Answer for Part 2:");
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
            Console.WriteLine();
            Console.WriteLine("Answer for Part 1: ");
            Console.Write(Sum);
            Console.WriteLine();
        }
        public static void Cycle(ref int cycle)
        {
            // this draws the screen

            int refc = cycle % 40;

            if (refc == Reg - 1 || refc == Reg + 1 || refc == Reg)
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
            if ( cycle % 40 == 0) // checks for scanline newline
            {               
                Console.WriteLine();
            }
            if( ( cycle + 20 ) % 40 == 0) // checks for part1 criteria
            {
                Sum += Reg * cycle;
            }
        }
    }
}
