﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ulong Sum = 0;
            ulong Sum2 = 0;
          
            List<ulong> list = new List<ulong>();
            using (StreamReader sr = new StreamReader("input.txt"))
            {
                while (true)
                {
                    string s = sr.ReadLine();
                    if (s == null)
                    {
                        break;
                    }
                    string[] chars = s.Split(' ');
                    // A = ROCK, B = PAPER, C = SCISSORS
                    // Y = PAPER, X = ROCK, Z = SCISSORS

                    switch (chars[1])
                    {
                        case "X":
                            switch (chars[0])
                            {
                                case "A":
                                    Sum += 3;
                                    break;
                                case "B":
                                    break;
                                case "C":
                                    Sum += 6;
                                    break;
                            }
                            Sum += 1;
                            break;
                        case "Y":
                            switch (chars[0])
                            {
                                case "A":
                                    Sum += 6;
                                    break;
                                case "B":
                                    Sum += 3;
                                    break;
                                case "C":
                                    break;
                            }
                            Sum += 2;
                            break;
                        case "Z":
                            switch (chars[0])
                            {
                                case "A":
                                    break;
                                case "B":
                                    Sum += 6;
                                    break;
                                case "C":
                                    Sum += 3;
                                    break;
                            }
                            Sum += 3;
                            break;
                    }

                    // A = ROCK, B = PAPER, C = SCISSORS
                    // Y = DRAW, X = LOSE, Z = WIN
                    switch (chars[1])
                    {
                        case "X":
                            switch (chars[0])
                            {
                                case "A":
                                    Sum2 += 3;
                                    break;
                                case "B":
                                    Sum2 += 1;
                                    break;
                                case "C":
                                    Sum2 += 2;
                                    break;
                            }
                            break;
                        case "Y":
                            switch (chars[0])
                            {
                                case "A":
                                    Sum2 += 1;
                                    break;
                                case "B":
                                    Sum2 += 2;
                                    break;
                                case "C":
                                    Sum2 += 3;
                                    break;
                            }
                            Sum2 += 3;
                            break;
                        case "Z":
                            switch (chars[0])
                            {
                                case "A":
                                    Sum2 += 2;
                                    break;
                                case "B":
                                    Sum2 += 3;
                                    break;
                                case "C":
                                    Sum2 += 1;
                                    break;
                            }
                            Sum2 += 6;
                            break;
                    }
                }
                Console.WriteLine("Part 1 answer: " + Sum);
                Console.WriteLine("Part 2 answer: " + Sum2);
            }
        }
    }
}
