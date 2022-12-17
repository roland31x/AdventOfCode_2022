using System;
using System.CodeDom;
using System.Numerics;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace D11
{
    internal class Program
    {
        static void Main(string[] args)
        {
            MonkeyList monkeys = new MonkeyList();

            using (StreamReader sr = new StreamReader("input.txt"))
            {
                char[] sep = new char[] { ' ' };
                while (!sr.EndOfStream)
                {
                    Monkey monke = new Monkey();
                    for(int k = 0; k < 7; k++)
                    {
                        string s = sr.ReadLine();
                        if(s == string.Empty || s == null)
                        {
                            break;
                        }
                        s = s.Replace(",", "");
                        string[] tokens = s.Split(sep, StringSplitOptions.RemoveEmptyEntries);
                        if (tokens[0] == "Starting")
                        {
                            for (int i = 2; i < tokens.Length; i++)
                            {
                                monke.items.Add(ulong.Parse(tokens[i]));
                            }
                        }
                        if (tokens[0] == "Operation:")
                        {
                            Operation op = new Operation(tokens);
                            monke.MonkeyOp = op;
                        }
                        if (tokens[0] == "Test:")
                        {
                            monke.DivTest = ulong.Parse(tokens[3]);
                        }
                        if (tokens[1] == "true:")
                        {
                            monke.ThrowTo[0] = (int.Parse(tokens[5]));
                        }
                        if (tokens[1] == "false:")
                        {
                            monke.ThrowTo[1] = (int.Parse(tokens[5]));
                        }
                    }
                    monkeys.Add(monke);                                
                }
            }
            for(int i = 1; i <= 10000; i++)  // 20 for part 1
            {
                foreach(Monkey monke in monkeys.monkeyList)
                {
                    monke.Operate(monkeys);
                }
            }
            List<ulong> Activities = new List<ulong>();
            foreach (Monkey monke in monkeys.monkeyList)
            {
                Activities.Add(monke.Activity);
            }
            ulong sum = 1;
            for(int i = 0; i < 2; i++)
            {
                sum *= Activities.Max();
                Activities.Remove(Activities.Max());
            }
            Console.WriteLine(sum);
        }
    }
    class MonkeyList 
    { 
        public List<Monkey> monkeyList { get; set; }
        public MonkeyList()
        {
            monkeyList = new List<Monkey>();
        }
        public void Add(Monkey monkey)
        {
            monkeyList.Add(monkey);
        }

    }

    class Monkey : MonkeyList
    {
        public ulong Activity { get; set; }
        public List<ulong> items { get; set; }

        public Operation MonkeyOp { get; set; }

        public ulong DivTest { get; set; }

        public int[] ThrowTo { get; set; }

        public Monkey()
        {
            items = new List<ulong>();
            Activity = 0;
            ThrowTo = new int[2];
        }
        public void ThrowItem(ulong i, int monk,MonkeyList monks)
        {
            monks.monkeyList[monk].items.Add(i);
            items.Remove(i);
        }
        public void Operate(MonkeyList monks)
        {
            while (items.Count > 0)
            {
                ulong maxdiv = 1;
                foreach(Monkey monkey in monks.monkeyList)
                {
                    maxdiv *= monkey.DivTest;
                }
                checked
                {
                    Activity++;
                    if (MonkeyOp.Op == '+')
                    {
                        items[0] = items[0] % maxdiv + MonkeyOp.Right;
                    }
                    if (MonkeyOp.Op == '*')
                    {
                        items[0] = (items[0] % maxdiv) * (MonkeyOp.Right % maxdiv);
                    }
                    if (MonkeyOp.Op == '^')
                    {
                        items[0] = (items[0] % maxdiv) * (items[0] % maxdiv);
                    }
                   // items[0] = items[0] / 3;  // PART 1
                    DoTest(items[0], monks);
                }
            }
        }

        private void DoTest(ulong v,MonkeyList monks)
        {
            if(v % DivTest == 0)
            {
                ThrowItem(v, ThrowTo[0], monks);
            }
            else ThrowItem(v, ThrowTo[1], monks);
        }
    }
    class Operation
    {
        public char Op { get; set; }

        public ulong Right { get; set; }

        public Operation(string[] tokens)
        {
            Op = char.Parse(tokens[4]);
            if (tokens[5] == "old")
            {
                Op = '^';
                Right = 0;
                return;
            }
            Right = ulong.Parse(tokens[5]);
        }
    }
}
