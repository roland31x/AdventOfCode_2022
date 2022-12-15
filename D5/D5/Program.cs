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
                new Stack<char>(), // 0 WILL BE UNUSED
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
                int cont = 1;
                while (true)
                {
                    string p = sr.ReadLine();
                    if (p == "stop")
                    {
                        break;
                    }
                    string[] tokens = p.Split(' ');
                    char[] items = new char[tokens.Length];
                    for(int i = 0; i < tokens.Length; i++)
                    {
                        items[i] = char.Parse(tokens[i]);
                    }
                    for(int j = 0; j < items.Length; j++)
                    {
                        Containers[cont].Push(items[j]);
                    }
                    cont++;
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
