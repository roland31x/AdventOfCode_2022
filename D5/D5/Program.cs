using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D5
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<Stack<char>> Containers = new List<Stack<char>>
            {
                new Stack<char>(), // 0 WILL BE UNUSED FOR PART 1
                new Stack<char>(), // 1
                new Stack<char>(), // 2
                new Stack<char>(), // 3
                new Stack<char>(), // 4
                new Stack<char>(), // 5
                new Stack<char>(), // 6
                new Stack<char>(), // 7
                new Stack<char>(), // 8
                new Stack<char>()  // 9
            };


            using (StreamReader sr = new StreamReader("input.txt"))
            {
                // parsing containers
                while (true)
                {
                    string p = sr.ReadLine();
                    if (p == string.Empty || char.IsNumber(p[1]))
                    {
                        break;
                    }
                    char[] items = p.ToCharArray();                   
                    for(int i = 1, j = 1; i < items.Length; i += 4, j++)
                    {
                        if (!char.IsWhiteSpace(items[i]))
                        {
                            Containers[j].Push(items[i]);
                        }                       
                    }
                }
                // reverse contents of containers
                for(int i = 0; i < Containers.Count; i++)
                {
                    Stack<char> temp = new Stack<char>();
                        Stack<char> temp2 = new Stack<char>();
                    while (Containers[i].Count > 0)
                    {
                        temp.Push(Containers[i].Pop());                        
                    }
                    while(temp.Count > 0)
                    {
                        temp2.Push(temp.Pop());
                    }
                    while(temp2.Count > 0)
                    {
                        Containers[i].Push(temp2.Pop()); 
                    }
                }

                /* PART 1
                while (true)
                {
                    string s = sr.ReadLine();
                    if (s == null)
                    {
                        break;
                    }
                    string[] tokens = s.Split(' ');
                    int[] commands = new int[3];
                    commands[0] = int.Parse(tokens[1]);
                    commands[1] = int.Parse(tokens[3]);
                    commands[2] = int.Parse(tokens[5]);
                    while (commands[0] > 0)
                    {
                        Containers[commands[2]].Push(Containers[commands[1]].Pop());
                        commands[0]--;
                    }
                }
                */
                while (!sr.EndOfStream)
                {
                    string s = sr.ReadLine();
                    if (s == string.Empty)
                    {
                        continue;
                    }
                    string[] tokens = s.Split(' ');
                    int[] commands = new int[3];
                    commands[0] = int.Parse(tokens[1]);
                    commands[1] = int.Parse(tokens[3]);
                    commands[2] = int.Parse(tokens[5]);
                    while (commands[0] > 0)
                    {
                        Containers[0].Push(Containers[commands[1]].Pop());                        
                        commands[0]--;
                    }
                    while (Containers[0].Count > 0)
                    {
                        Containers[commands[2]].Push(Containers[0].Pop());
                    }
                }
            }
            StringBuilder str = new StringBuilder();
            for(int i = 1; i < Containers.Count; i++)
            {
                str.Append(Containers[i].Pop());
            }
            Console.WriteLine(str.ToString());
        }
    }
}
