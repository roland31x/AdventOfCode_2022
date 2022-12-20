using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace D12
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<List<Point>> map = new List<List<Point>>();
            using (StreamReader sr = new StreamReader("input.txt"))
            {               
                int i = 0;               
                while (!sr.EndOfStream)
                {
                    map.Add(new List<Point>());
                    string s = sr.ReadLine();
                    char[] chars = s.ToCharArray();
                    for(int j = 0; j < chars.Length; j++)
                    {
                        if (chars[j] == 'S' || chars[j] == 'a') 
                        {
                            map[i].Add(new Point(chars[j], i, j,'S'));
                           // Console.WriteLine(i+" "+ j);
                            continue;
                        }
                        if (chars[j] == 'E')
                        {
                            map[i].Add(new Point(chars[j], i, j,'E'));
                            continue;
                        }
                        map[i].Add(new Point(chars[j], i, j));
                    }                   
                    i++;
                }
            }
            int check = Int32.MaxValue;

            Path prob = new Path(map);
            foreach(List<Point> list in map)
            {
                foreach(Point p in list)
                {
                    if (p.isStart)
                    {
                        prob.SetStart(p.posY, p.posX);
                        prob.FindWay();
                        if (prob.GetDist() < check && prob.GetDist() > 0)  // if distance is 0 it means no path was found
                        {
                            check = prob.GetDist();
                        }
                        prob.Reset();
                    }
                }
            }
            Console.WriteLine(check);
        }
        
    }
    class Point
    {
        public char id { get; set; }

        public int posX { get; set; }

        public int posY { get; set; }

        public bool wasVisited { get; set; }
        public bool isStart { get; set; }
        public bool isFinish { get; set; }

        public int level { get; set; }
        public bool stop { get; set; }
        public int Mark { get; set; }

        public Point(char id, int i, int j)
        {
            stop = false;
            wasVisited = false;
            Mark = 0;
            level = id - 'a';
            this.id = id;
            posX = j;
            posY = i;
        }
        public Point(char id, int i, int j,char s)
        {
            stop = false;
            wasVisited = false;
            Mark = 0;
            this.id = id;
            posX = j;
            posY = i;
            switch (s)
            {
                case 'S':
                    isStart = true;
                    level = 0;
                    break;
                case 'E':
                    isFinish = true;
                    level = 26;
                    break;
            }
        }       
    }
    class Path
    {
        List<List<Point>> map { get; set; }
        Point startpoint { get; set; }
        Point endpoint { get; set; }

        Queue<Point> queue = new Queue<Point>();
        public Path(List<List<Point>> mapchar )
        {
            map = mapchar;

            foreach(List<Point> s in map)
            {
                foreach(Point p in s)
                {
                    //if (p.isMainStart)   // p.isStart for part1
                    //{
                    //    startpoint = p;
                    //    Console.WriteLine(p.posY + p.posY);
                    //}
                    if(p.isFinish) endpoint = p;
                }
            }
        }
        public void Reset()
        {
            foreach(List<Point> s in map)
            {
                foreach(Point p in s)
                {
                    p.wasVisited = false;
                    p.Mark = 0;
                    queue = new Queue<Point>();
                }
            }
        }
        public void SetStart(int i, int j)
        {
            startpoint = map[i][j];
        }
        public int GetDist()
        {
            return endpoint.Mark;
        }
        public void FindWay()
        {
            startpoint.Mark = 0;
            int i = 1;
            startpoint.wasVisited = true;
            queue.Enqueue(startpoint);
            Mark();
            //foreach (List<Point> lp in map)
            //{
            //    foreach (Point p in lp)
            //    {
            //        Console.Write($"{p.Mark,6}");

            //    }
            //    Console.WriteLine();
            //}
            //Console.WriteLine(endpoint.Mark);
        }
        public void Neighbors(Point p)
        {
            p.wasVisited = true;
            if(!(p.posX + 1 >= map[p.posY].Count))
            {
                if (p.level - map[p.posY][p.posX + 1].level >= -1 && !map[p.posY][p.posX + 1].wasVisited)
                {
                    queue.Enqueue(map[p.posY][p.posX + 1]);
                    map[p.posY][p.posX + 1].Mark = p.Mark + 1;
                    map[p.posY][p.posX + 1].wasVisited = true;
                }
            }
            if (!(p.posX - 1 < 0))
            {
                if (p.level - map[p.posY][p.posX - 1].level >= -1 && !map[p.posY][p.posX - 1].wasVisited)
                {
                    queue.Enqueue(map[p.posY][p.posX - 1]);
                    map[p.posY][p.posX - 1].Mark = p.Mark + 1;
                    map[p.posY][p.posX - 1].wasVisited = true;
                }
            }
            if (!(p.posY - 1 < 0))
            {
                if (p.level - map[p.posY - 1][p.posX].level >= -1 && !map[p.posY - 1][p.posX].wasVisited)
                {
                    queue.Enqueue(map[p.posY - 1][p.posX]);
                    map[p.posY - 1][p.posX].Mark = p.Mark + 1;
                    map[p.posY - 1][p.posX].wasVisited = true;
                }
            }
            if (!(p.posY + 1 >= map.Count))
            {
                if (p.level - map[p.posY + 1][p.posX].level >= -1 && !map[p.posY + 1][p.posX].wasVisited)
                {
                    queue.Enqueue(map[p.posY + 1][p.posX]);
                    map[p.posY + 1][p.posX].Mark = p.Mark + 1;
                    map[p.posY + 1][p.posX].wasVisited= true;
                }             
            }
        }
        public void Mark()
        {           
            while(queue.Count != 0)
            {
                Point p = queue.Dequeue();               
                //Console.WriteLine($"({p.posX},{p.posY}) visited");
                if (p.isFinish)
                {
                    //Console.WriteLine("Found!");
                    return;
                }
                Neighbors(p);
            }
        }
    }
}
