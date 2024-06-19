using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace D16
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<Valve> VL = Valve.ValveList;
            List<Valve> FL = Valve.Flows;
            using (StreamReader sr = new StreamReader("input.txt"))
            {
                while (!sr.EndOfStream)
                {
                    Valve v = new Valve();
                    Regex ValveID = new Regex(@"[A-Z]{2}");
                    Regex RFlow = new Regex(@"\brate=\d+\b");
                    string line = sr.ReadLine();
                    MatchCollection flows = RFlow.Matches(line);
                    MatchCollection matches = ValveID.Matches(line);
                    for (int i = 0; i < matches.Count; i++)
                    {
                        if (i == 0)
                        {
                            if (!Valve.Exists(matches[i].Value))
                            {
                                v = new Valve(matches[i].Value);
                            }
                            else
                            {
                                v = Valve.Find(matches[i].Value);
                            }
                            v.Flow = int.Parse(flows[0].Value.Replace("rate=", ""));

                        }
                        else
                        {
                            if (Valve.Exists(matches[i].Value))
                            {
                                v.Cons.Add(Valve.Find(matches[i].Value));
                            }
                            else v.Cons.Add(new Valve(matches[i].Value));
                        }
                    }
                }
            }
            Valve.FindNonZeroValves();
            Driver firstRun = new Driver();
            //firstRun.CalcDist();
            firstRun.CalcDist();
            firstRun.Reset();
            Console.WriteLine("Part 1 solution:");
            firstRun.SolveDay16Part1();
            Console.WriteLine("Part 2 solution:");
            firstRun.SolveDay16Part2();

        }
    }
    class Driver 
    { 
        public Valve current { get; set; }
        public Driver() 
        {
            current = Valve.Find("AA");
        }
        
        public void SolveDay16Part2()
        {
            int max = 0;
            List<Valve> main = Valve.Flows;
            Valve[] arr = new Valve[main.Count];
            for(int i = 0; i < main.Count; i++)
            {
                arr[i] = main[i];
            }
            for(int i = 1; i < main.Count - 1 / 2; i++) // tries every combination of workload
            {
                GetCombination(arr, main.Count, i, ref max);
            }            
            Console.WriteLine(max);
        }

        void Combination(Valve[] arr, Valve[] data, int start, int end, int index, int r, ref int maxscoresofar)
        {
            if (index == r)
            {
                List<Valve> playerlist = data.ToList(); // player workload
                int P1Score = CalcScore(playerlist, 1, this.current, 26);
                List<Valve> other = Valve.Flows.Where(x => !playerlist.Contains(x)).ToList(); // elephant workload
                int P2Score = CalcScore(other, 1, this.current, 26);
                if(P1Score + P2Score > maxscoresofar)
                {
                    maxscoresofar = P1Score + P2Score;
                }
                return;
            }
            for (int i = start; i <= end && end - i + 1 >= r - index; i++)
            {
                data[index] = arr[i];
                Combination(arr, data, i + 1, end, index + 1, r, ref maxscoresofar);
            }
        }

        void GetCombination(Valve[] arr, int n, int r, ref int maxscoresofar)
        {
            Valve[] data = new Valve[r];
            Combination(arr, data, 0, n - 1, 0, r, ref maxscoresofar);
        }

        public void SolveDay16Part1()
        {
            int minute = 1;
            Valve current = this.current;
            Console.WriteLine(CalcScore(Valve.Flows, minute, current,30));
        }
        public int CalcScore(List<Valve> v, int minute, Valve current, int maxmins)
        {
            if(minute > maxmins) 
            {
                return 0;
            }
            if(v.Count == 0)
            {
                return 0;
            }
            int[] scores = new int[v.Count];
            for (int i = 0; i < v.Count; i++)
            {
                int multiplier = maxmins - (minute + current.DistanceTo(v[i]));
                if (multiplier < 0)
                {
                    continue;
                }
                scores[i] += multiplier * v[i].Flow;
                scores[i] += CalcScore(v.Where(x => x != v[i]).ToList(), minute + 1 + current.DistanceTo(v[i]), v[i], maxmins);
            }

            return scores.ToList().Max();
        }    
        public void Reset()
        {
            current = Valve.Find("AA");
        }
        public void CalcDist()
        {
            int i = 0;
            foreach(Valve v in Valve.Flows)
            {
                FindWay(v);
                current.Distance.Add(new VDist(i, v.Mark));
                Valve.ResetDist();
                i++;
            }
            
            foreach(Valve v in Valve.Flows)
            {
                i = 0;
                current = v;
                foreach(Valve v2 in Valve.Flows)
                {
                    if(v2.ID == v.ID)
                    {
                        v.Distance.Add(new VDist(i,0));
                    }
                    else
                    {
                        FindWay(v2);
                        v.Distance.Add(new VDist(i,v2.Mark));
                        Valve.ResetDist();
                    }
                    i++;
                }

            }
        }
        public void FindWay(Valve v)
        {
            current.Visited = true;
            Queue<Valve> q = new Queue<Valve>();
            q.Enqueue(current);
            while(q.Count > 0)
            {
                Valve d = q.Dequeue();
                d.Visited = true;
                foreach(Valve f in d.Cons)
                {
                    f.Mark = d.Mark + 1;
                    if(!f.Visited) q.Enqueue(f);
                    if(f.ID == v.ID)
                    {
                        return;
                    }
                }
            }
        }


    }
    class VDist
    {
        public int Dist { get; set; }
        public string Name { get; set; }

        public VDist(int i, int mark)
        {
            Name = Valve.Flows[i].ID;
            Dist = mark;
        }
        public override string ToString()
        {
            return $"{Dist} away from {Name}";
        }
    }
    class Valve
    {
        public static List<Valve> ValveList = new List<Valve>();
        public static List<Valve> Flows = new List<Valve>();
        public string ID { get; set; }
        public int Flow { get; set; }
        public List<Valve> Cons { get; set; }
        public List<VDist> Distance { get; set; }
        public bool IsOpen { get; set; }
        public int Mark { get; set; }
        public bool Visited { get; set; }

        public bool wasSolved { get; set; }
        public Valve(string s)
        {
            ID = s;
            IsOpen = false;
            Visited = false;
            wasSolved = false;
            Mark = 0;
            Cons = new List<Valve>();
            Distance = new List<VDist>();
            ValveList.Add(this);
        }
        public Valve()
        {
            ID = "test";
        }
        public static void FindNonZeroValves()
        {
            foreach(Valve v in ValveList)
            {
                if(v.Flow > 0)
                {
                    Flows.Add(v);
                }
            }
        }
        public static void ResetDist()
        {
            foreach (Valve v in ValveList)
            {
                v.Mark = 0;
                v.Visited = false;
            }
        }
        public void Open()
        {
            IsOpen = true;
        }
        public static bool Exists(string s)
        {
            foreach(Valve v in ValveList)
            {
                if(v.ID == s) return true;
            }
            return false;
        }
        public static Valve Find(string s)
        {
            foreach(Valve v in ValveList)
            {
                if(v.ID == s)
                {
                    return v;
                }
            }
            Console.WriteLine("You shouldn't see this.");
            return new Valve("NULL");
        }
        public int DistanceTo(Valve other)
        {
            foreach(VDist v in Distance)
            {
                if(v.Name == other.ID)
                {
                    return v.Dist;
                }
            }
            return -1;
        }
        public override string ToString()
        {
            return $"{ID} rate: {Flow}";
        }
    }
}
