using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D6
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int count;

            using (StreamReader sr = new StreamReader("input.txt"))
            {
                string s = sr.ReadToEnd();
                char[] chars = s.ToCharArray();
                count = FirstDiffOfAmount(4, chars);
            }
            Console.WriteLine("Part 1 solution:");
            Console.WriteLine(count);

            using (StreamReader sr = new StreamReader("input.txt"))
            {
                string s = sr.ReadToEnd();
                char[] chars = s.ToCharArray();
                count = FirstDiffOfAmount(14, chars);
            }
            Console.WriteLine("Part 2 solution:");
            Console.WriteLine(count);
        }
        static int FirstDiffOfAmount(int amount, char[] chars)
        {
            for (int i = 0; i < chars.Length; i++)
            {
                SortedSet<char> set = new SortedSet<char>();
                set.Add(chars[i]);
                for (int j = i + 1; j < i + amount; j++)    
                {
                    set.Add(chars[j]);
                }
                if (set.Count == amount)    
                {
                    return i + amount;   
                }
            }
            return -1;
        }
    }
}
