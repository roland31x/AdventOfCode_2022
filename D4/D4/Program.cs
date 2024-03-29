﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D4
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int Sum1 = 0;
            int Sum2 = 0;

            using (StreamReader sr = new StreamReader("input.txt"))
            {
                while (true)
                {
                    string s = sr.ReadLine();
                    if (s == null)
                    {
                        break;
                    }
                    string[] tokens = s.Split(',');
                    string[] firstElf = tokens[0].Split('-');
                    string[] secondElf = tokens[1].Split('-');
                    int[] Elf1 = new int[2];
                    int[] Elf2 = new int[2];
                    for(int i = 0; i < 2; i++)
                    {
                        Elf1[i] = int.Parse(firstElf[i]);
                        Elf2[i] = int.Parse(secondElf[i]);
                    }

                    if (Elf1[0] <= Elf2[0] && Elf1[1] >= Elf2[1])
                        Sum1++;
                    else if (Elf1[0] >= Elf2[0] && Elf1[1] <= Elf2[1])
                        Sum1++;

                    if (Elf1[0] <= Elf2[0])
                        if (Elf1[1] - Elf2[0] >= 0)
                            Sum2++;
                    if (Elf1[0] > Elf2[0])
                        if (Elf2[1] - Elf1[0] >= 0)
                            Sum2++;
                }
            }
            Console.WriteLine("Part 1 solution:");
            Console.WriteLine(Sum1);
            Console.WriteLine("Part 2 solution:");
            Console.WriteLine(Sum2);
        }
    }
}
