using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ulong Calories = 0;
            ulong p1 = 0;
            ulong maxCal = 0;
            List<ulong> list = new List<ulong>();
            using (StreamReader sr = new StreamReader("input.txt"))
            {
                while (true)
                {
                    string s = sr.ReadLine();
                    if(s == null)
                    {
                        break;
                    }
                    if (!ulong.TryParse(s, out ulong calorie))
                    {
                        list.Add(Calories);
                        Calories = 0;
                        calorie = 0;
                    }
                    Calories += calorie;
                }

                   
            }

            p1 = list.Max();

            for(int i = 0; i < 3; i++)
            {
                maxCal += list.Max();
                list.Remove(list.Max());
            }
            
            
            Console.WriteLine("Part 1 answer: " + p1);
            Console.WriteLine("Part 2 answer: " + maxCal);

        }
    }
}
