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
            int count = 0;

            using (StreamReader sr = new StreamReader("input.txt"))
            {
                string s = sr.ReadToEnd();
                char[] chars = s.ToCharArray();
                for(int i = 0; i < chars.Length; i++)
                {
                    SortedSet<char> set = new SortedSet<char>();
                    set.Add(chars[i]);
                    for (int j = i + 1; j < i + 14; j++)    // PART 1 j goes to i + 4
                    {
                        set.Add(chars[j]);
                    }
                    if (set.Count == 14)    // PART 1  set.Count == 4
                    {
                        count = i + 14;   // PART 1  count = i + 4;
                        break;
                    }
                }
                
            }
            Console.WriteLine(count);
        }
    }
}
