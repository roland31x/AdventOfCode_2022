using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D3
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int Sum = 0;
            
            using (StreamReader sr = new StreamReader("input.txt"))
            {
                while (true)
                {
                    // PART 1 FROM HERE
                    //List<char> comp1 = new List<char>();
                    //List<char> comp2 = new List<char>();
                    //string s = sr.ReadLine();
                    //if (s == null)
                    //{
                    //    break;
                    //}
                    //char[] chars = s.ToCharArray();
                    //for (int i = 0; i < chars.Length / 2; i++)
                    //{
                    //    comp1.Add(chars[i]);
                    //}
                    //for (int j = chars.Length / 2; j < chars.Length; j++)
                    //{
                    //    comp2.Add(chars[j]);
                    //}
                    //bool found = false;
                    //for (int i = 0; i < comp1.Count; i++)
                    //{
                    //    for (int j = 0; j < comp2.Count; j++)
                    //    {
                    //        if (comp1[i] == comp2[j])
                    //        {
                    //            if (comp1[i] - '@' <= 27)
                    //            {
                    //                Sum += (comp1[i] - '@') + 26;
                    //            }
                    //            else Sum += comp1[i] - '`';
                    //            found = true;
                    //            break;
                    //        }
                    //    }
                    //    if (found) break;
                    //}
                    // PART 1 UNTIL HERE

                    // PART 2 FROM HERE
                    List<List<char>> Group = new List<List<char>>
                    {
                        new List<char>(),
                        new List<char>(),
                        new List<char>()
                    };
                    bool done = false;
                    for (int i = 0; i < 3; i++)
                    {
                        string s = sr.ReadLine();
                        if (s == null)
                        {
                            done = true;
                            break;
                        }
                        Group[i] = new List<char>();
                        char[] chars = s.ToCharArray();
                        foreach (char c in chars)
                        {
                            Group[i].Add(c);
                        }
                    }
                    if (done)
                    {
                        break;
                    }
                    bool found = false;
                    for (int i = 0; i < Group[0].Count; i++)
                    {
                        for (int j = 0; j < Group[1].Count; j++)
                        {
                            for (int k = 0; k < Group[2].Count; k++)
                            {
                                if (Group[0][i] == Group[1][j] && Group[1][j] == Group[2][k])
                                {
                                    if (Group[0][i] - '@' <= 27)
                                    {
                                        Sum += (Group[0][i] - '@') + 26;
                                    }
                                    else Sum += Group[0][i] - '`';
                                    found = true;
                                    break;
                                }
                            }
                            if (found) break;
                        }
                        if (found) break;
                    }
                    //
                }
            }
            Console.WriteLine(Sum);
        }
    }
}
