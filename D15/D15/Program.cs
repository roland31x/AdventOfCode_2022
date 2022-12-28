using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace D15
{
    internal class Program
    {
        public static int LineCheck = 2000000;
        static void Main(string[] args)
        {
            List<Sensor> sensors= new List<Sensor>();
            //SortedSet<int> marks = new SortedSet<int>(); // FOR PART 1

            List<int[]> ToCheck = new List<int[]>();

            using (StreamReader sr = new StreamReader("input.txt"))
            {
                while (!sr.EndOfStream)
                {
                    string input = sr.ReadLine();
                    string pattern = @"Sensor at x=(-?\d+), y=(-?\d+): closest beacon is at x=(-?\d+), y=(-?\d+)";
                    Match match = Regex.Match(input, pattern);
                    if (match.Success)
                    {
                        int x1 = int.Parse(match.Groups[1].Value);
                        int y1 = int.Parse(match.Groups[2].Value);
                        int x2 = int.Parse(match.Groups[3].Value);
                        int y2 = int.Parse(match.Groups[4].Value);
                        //if(y2 == LineCheck)
                        //{
                        //    marks.Add(x2);
                        //}
                        Sensor sns = new Sensor(new Beacon(y2,x2), y1 , x1);
                        sensors.Add(sns);
                    }
                    else
                    {
                        Console.WriteLine("Match not found.");
                    }
                }              
            }
            // FOR PART 1
            //foreach(Sensor sns in sensors)
            //{
            //    int dist = 0; 
            //    if(sns.yPos > LineCheck)
            //    {
            //        dist = sns.yPos - sns.AreaCov - LineCheck;
            //    }
            //    else
            //    {
            //        dist = -sns.yPos - sns.AreaCov + LineCheck;
            //    }
            //    if(dist == 0)
            //    {
            //        marks.Add(sns.xPos);
            //    }
            //    if(dist <= -1)
            //    {
            //        for(int i = sns.xPos + dist; i < sns.xPos - dist; i++)
            //        {
            //            marks.Add(i);
            //        }
            //    }              
            //}
            //Console.WriteLine(marks.Count);
            // FOR PART 1 ABOVE
            // FROM BELOW FOR PART 2
            bool found = false;
            foreach (Sensor sns in sensors)
            {
                if (!found)
                {
                    ToCheck = new List<int[]>();
                }
                if (found)
                {
                    Console.WriteLine();
                    break;
                }
                Addpts(sns, ToCheck);

                foreach (int[] p in ToCheck)
                {
                    bool ok = true;
                    foreach (Sensor snsn in sensors)
                    {
                        int dist = Math.Abs(snsn.yPos - p[0]) + Math.Abs(snsn.xPos - p[1]);
                        if (dist <= snsn.AreaCov)
                        {
                            ok = false;
                            break;
                        }
                    }
                    if (ok)
                    {
                        found = true;
                        ulong rez = 4000000 * (ulong)p[1] + (ulong)p[0];
                        Console.WriteLine(rez);
                    }
                }
            }
        }
        public static void Addpts(Sensor p, List<int[]> toCheck)
        {
            for(int y = p.yPos - p.AreaCov - 1, xL = p.xPos, xR = p.xPos; y <= p.yPos + p.AreaCov + 1; y++)
            {   
                if(xL >= 0 && xL <= 4000000 && y >= 0 && y <= 4000000)
                {
                    toCheck.Add(new int[] { y, xL });
                }
                if (xR >= 0 && xR <= 4000000 && y >= 0 && y <= 4000000 && xR != xL)
                {
                    toCheck.Add(new int[] { y, xR });
                }
                if (y < p.yPos)
                {
                    xL -= 1;
                    xR += 1;
                }
                else
                {
                    xR -= 1;
                    xL += 1;
                }
            }          
        }
    }
    class Sensor
    {
        public int AreaCov { get; set; }

        public int yPos { get; set; }

        public int xPos { get; set; }

        public Sensor(Beacon b, int y, int x)
        {
            yPos = y; xPos = x;
            AreaCov = Math.Abs(y - b.yPos) + Math.Abs(x - b.xPos);
        }
    }
    class Beacon
    {
        public int yPos { get; set;}
        public int xPos { get; set;}
        public Beacon(int yPos, int xPos)
        {
            this.yPos = yPos;
            this.xPos = xPos;
        }
    }  
}
