using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace D13
{
    internal class Program
    {
        static int sum = 1; // 0 for part 1 !!!!!
        static void Main(string[] args)
        {
            List<ListConv> packets = new List<ListConv>();
            using (StreamReader sr = new StreamReader("input.txt"))
            {
                /*
                ListConv left = new ListConv();
                ListConv right = new ListConv();
                Browser browser;
                int i = 1;
                int line = 0;
                while (!sr.EndOfStream)
                {
                    string s = sr.ReadLine();
                    s = s.Replace("[", "[ ");
                    s = s.Replace("]"," ]");
                    s = s.Replace(",", " ");
                    if (s == string.Empty)
                    {
                        Compare(i, left, right);
                        i++;
                        line = 0;
                        left = new ListConv();
                        right = new ListConv();
                        continue;
                    }
                    string[] tokens = s.Split(' ');
                    if (line == 0)
                    {

                        browser = new Browser(left);
                        for (int j = 1; j < tokens.Length - 1; j++)
                        {
                            if (tokens[j] == "[")
                            {
                                browser.Enter(new ListConv());
                            }
                            if (int.TryParse(tokens[j], out int toAdd))
                            {
                                browser.AddInt(toAdd);
                            }
                            if (tokens[j] == "]")
                            {
                                browser.Back();
                            }
                        }
                        line++;
                        continue;
                    }
                    if (line == 1)
                    {
                        browser = new Browser(right);
                        for (int j = 1; j < tokens.Length - 1; j++)
                        {
                            if (tokens[j] == "[")
                            {
                                browser.Enter(new ListConv());
                            }
                            if (int.TryParse(tokens[j], out int toAdd))
                            {
                                browser.AddInt(toAdd);
                            }
                            if (tokens[j] == "]")
                            {
                                browser.Back();
                            }
                        }
                        line++;
                    }
                }
                Console.WriteLine(sum);  // UNTIL HERE FOR PART 1
                */
                while (!sr.EndOfStream)
                {
                    ListConv toAddlist = new ListConv();
                    Browser browser = new Browser(toAddlist);
                    string s = sr.ReadLine();
                    if (s == string.Empty)
                    {
                        continue;
                    }
                    s = s.Replace("[", "[ ");
                    s = s.Replace("]", " ]");
                    s = s.Replace(",", " ");
                    string[] tokens = s.Split(' ');
                    for (int j = 1; j < tokens.Length - 1; j++)
                    {
                        if (tokens[j] == "[")
                        {
                            browser.Enter(new ListConv());
                        }
                        if (int.TryParse(tokens[j], out int toAdd))
                        {
                            browser.AddInt(toAdd);
                        }
                        if (tokens[j] == "]")
                        {
                            browser.Back();
                        }
                    }
                    packets.Add(toAddlist);
                }
                // adding decoder keys
                ListConv Key1 = new ListConv();
                ListConv Key1Temp = new ListConv();
                ListConv Key2 = new ListConv();
                ListConv Key2Temp = new ListConv();
                Key1Temp.AddInt(2);
                Key2Temp.AddInt(6);
                Key1.AddList(Key1Temp);
                Key2.AddList(Key2Temp);
                packets.Add(Key1);
                packets.Add(Key2);
                // for part 2
                InsertionSort(packets);
                for(int i = 0; i < packets.Count; i++)
                {
                    if (packets[i] == Key1)
                    {
                        sum *= i + 1;
                    }
                    if (packets[i] == Key2)
                    {
                        sum *= i + 1;
                    }
                }
                Console.WriteLine(sum);

            }
        }
        public static void InsertionSort(List<ListConv> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                int j = i;
                while(j > 0)
                {
                    ListConv Left = new ListConv(list[j - 1]);
                    ListConv Right = new ListConv(list[j]);
                    if (Right.CompareTo(Left).Value)
                    {
                        (list[j], list[j - 1]) = (list[j - 1], list[j]);
                        j--;
                    }
                    else break;
                }
            }
        }
        static void Compare(int index, ListConv left, ListConv right)
        {
            bool? toCheck = left.CompareTo(right);
            //Console.WriteLine($"{index} pair {toCheck}");
            if (toCheck.HasValue)
            {
                if (toCheck.Value)
                {
                    sum += index;
                }               
            }
        }
    }
    public class Browser
    {
        Stack<ListConv> parents { get; set; }
        ListConv current { get; set; }

        public Browser(ListConv ls)
        {
            parents = new Stack<ListConv>();
            current = ls;
        }
        public void Enter(ListConv n)
        {
            parents.Push(current);
            current.AddList(n);
            current = n;
        }
        public void AddInt(int i)
        {
            current.AddInt(i);
        }
        public void Back()
        {
            current = parents.Pop();
        }
        
    }
    public class ListConv
    {
        Queue<object> _contents { get; set; }
        public ListConv()
        {
            _contents = new Queue<object>();
        }
        public ListConv(ListConv ls)
        {
            _contents = new Queue<object>();
            foreach (object obj in ls._contents)
            {
                if (obj is ListConv)
                {
                    this._contents.Enqueue(new ListConv(obj as ListConv));
                }
                else this._contents.Enqueue(Convert.ToInt32(obj));
            }
        }
        public void AddInt(int i)
        {
            _contents.Enqueue(i);
        }
        public void AddList(ListConv listConv)
        {
            _contents.Enqueue(listConv);
        }
        public bool? CompareTo(ListConv ls)
        {
          //  Console.WriteLine($"Compare {this} with {ls}");

            while (this._contents.Count > 0 && ls._contents.Count > 0)
            {
                
                    if (this._contents.Peek() is int && ls._contents.Peek() is int)
                    {
                        int left = Convert.ToInt16(this._contents.Dequeue());
                        int right = Convert.ToInt16(ls._contents.Dequeue());

                   //     Console.WriteLine($"Compare {left} with {right}");

                        if (left > right)
                        {
                            return false;
                        }
                        if (left < right)
                        {
                            return true;
                        }
                        continue;
                    }
                    if(this._contents.Peek() is ListConv && ls._contents.Peek() is ListConv)
                    {

                        bool? check = (this._contents.Dequeue() as ListConv).CompareTo(ls._contents.Dequeue() as ListConv);
                        if (check != null)
                        {
                            return check;
                        }
                        else continue;
                    }
                    if(this._contents.Peek() is int && ls._contents.Peek() is ListConv)
                    {

                        ListConv temp = new ListConv();
                        temp.AddInt(Convert.ToInt32(this._contents.Dequeue()));
                        bool? check = temp.CompareTo(ls._contents.Dequeue() as ListConv);
                        if (check != null)
                        {
                            return check;
                        }
                        else continue;
                    }
                    if (this._contents.Peek() is ListConv && ls._contents.Peek() is int)
                    {

                        ListConv temp = new ListConv();
                        temp.AddInt(Convert.ToInt32(ls._contents.Dequeue()));
                        bool? check = (this._contents.Dequeue() as ListConv).CompareTo(temp);
                        if (check != null)
                        {
                            return check;
                        }
                        else continue;
                    }
            }
            if (this._contents.Count > 0)
            {
               // Console.WriteLine("Left side ran out of items");
                return false;
            }
            else if (ls._contents.Count > 0)
            {
               // Console.WriteLine("Right side ran out of items");
                return true;
            }
            else return null;
        }
        public override string ToString()
        {
            StringBuilder str = new StringBuilder();
            str.Append("[");
            foreach(object obj in this._contents) 
            {
                str.Append(' ');
                str.Append(obj.ToString());
                str.Append(' ');
            }
            str.Append("]");
            return str.ToString();
        }
    }
}
