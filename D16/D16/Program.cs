using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

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
            int Max = 0;

            //firstRun.Solve();
            //int score = firstRun.Score;

            //if(score > Max)
            //{
            //    Max = score;
            //}


            //Console.WriteLine(Max);
            firstRun.CalcPot();
            firstRun.NavigateTo("DD");
            //Console.WriteLine($"Minute {firstRun.Minute}");
            firstRun.ActivateValve();
            firstRun.Potentials.Add(560);
            Console.WriteLine($"Minute {firstRun.Minute}, DD open");
            firstRun.CalcPot();
            firstRun.NavigateTo("JJ");
            Console.WriteLine($"Minute {firstRun.Minute}, DD, JJ open");
            firstRun.ActivateValve();
            firstRun.Potentials.Add(504);
            firstRun.CalcPot();

        }
    }
    class Driver
    {
        public List<int> Potentials = new List<int>();
        public int Score { get; set; }
        public int Minute { get; set; }
        public Valve current { get; set; }
        public Driver() 
        {
            current = Valve.Find("AA");
            Score = 0;
            Minute = 0;
        }
        public void CalcPot()
        {
            int i = 0;
            int j = 0;
            int average = 0;
            foreach(Valve v in Valve.Flows)
            {
                if (!(v.ID == current.ID) && v.IsOpen == false)
                {
                    average += ((29 - Minute) - current.Distance[i].Dist) * v.Flow;
                    j++;
                    Console.WriteLine($"{v.ID} = {(29 - Minute) - current.Distance[i].Dist} * {v.Flow} = {((29 - Minute) - current.Distance[i].Dist) * v.Flow} ");
                }
                i++;
                
            }
            foreach(int k in Potentials)
            {
                average += k;
            }
            average = average / j + Potentials.Count;
            Console.WriteLine(average);
        }
        public void Solve()
        {
            Valve[] rank = new Valve[Valve.Flows.Count];
            for (int i = 0; i < Valve.Flows.Count; i++)
            {
                rank[i] = Valve.Flows[i];
            }
            for (int i = 0; i < rank.Length - 1; i++)
            {
                for (int j = i + 1; j < rank.Length; j++)
                {
                    if (rank[i].Flow < rank[j].Flow)
                    {
                        (rank[i], rank[j]) = (rank[j], rank[i]);
                    }
                }
            }
            List<Valve> ranks = rank.ToList();             //new List<Valve> { Valve.Find("DD"), Valve.Find("BB"),Valve.Find("JJ"),Valve.Find("HH"),Valve.Find("EE"),Valve.Find("CC") };
            while(Minute < 30)
            {
                foreach(Valve v in ranks)
                {                   
                    NavigateTo(v.ID);
                    if(Minute >= 30)
                    {
                        return;
                    }
                    ActivateValve();
                    if (Minute >= 30)
                    {
                        return;
                    }
                }
            }
        }
        public void Reset()
        {
            current = Valve.Find("AA");
            Score = 0;
            Minute = 0;
        }
        public void NavigateTo(string s)
        {           
            int i = 0;
            foreach(Valve v in Valve.Flows)
            {
                if(v.ID == s)
                {                   
                    for(int j = 0; j < current.Distance[i].Dist; j++)
                    {
                        Minute++;
                        CalcScore();
                        if (Minute >= 30)
                        {
                            return;
                        }                        
                    }
                    break;
                }
                i++;
            }
            current = Valve.Find(s);
        }
        public void ActivateValve()
        {
            CalcScore();
            current.Open();
            Minute++;
        }
        public void CalcScore()
        {
            //Console.WriteLine("Minute " + Minute);
            foreach(Valve v in Valve.ValveList)
            {
                if (v.IsOpen)
                {
                    //Console.Write($"{v.ID} ");
                    Score += v.Flow;
                }
            }
            //Console.Write(Score);
            //Console.WriteLine();
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
        public Valve(string s)
        {
            ID = s;
            IsOpen = false;
            Visited = false;
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
        public void Open()
        {
            IsOpen = true;
        }
        public static void ResetDist()
        {
            foreach(Valve v in ValveList)
            {
                v.Mark = 0;
                v.Visited = false;
            }
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
        public override string ToString()
        {
            return $"{ID} rate: {Flow}";
        }
    }
}
