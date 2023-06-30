using System.Drawing;
using System.Text;

namespace D17
{
    internal class Program
    {
        static void Main(string[] args)
        {
            JetPattern myJets = new JetPattern("input.txt");
            Map map = new Map(myJets);
            map.StartSim();
        }
    }
    public class JetPattern
    {
        int current = 0;
        List<char> chars = new List<char>();
        public JetPattern(string file) 
        {
            using (StreamReader sr = new StreamReader(file))
            {
                string line = sr.ReadLine();
                for(int i = 0; i < line.Length; i++)
                {
                    chars.Add(line[i]);
                }
            }
        }
        public char GetNextPush()
        {
            char toreturn = chars[current % chars.Count];
            current++;
            return toreturn;
        }
    }

    public class Map
    {
        public List<Point> SetRocks = new List<Point>();
        public Point Highest = new Point(0, 0);
        JetPattern pattern { get; set; }

        public Map(JetPattern jets)
        {
            pattern = jets;          
        }
        public void StartSim()
        {
            List<int> Heights = new List<int>();
            for (int i = 0; i < 10000; i++) // increase max iterations if you can't find a solution for day 2
            {
                SpawnRock(new Rock(i % 5));
                if(i == 2021) // aka 2022 rocks have fallen
                {
                    Console.WriteLine("Day 1 Solution: " + Highest.Y);
                }
                Heights.Add(Highest.Y);
            }
            Console.WriteLine("Possible Solutions for Day 2: ");
            Analyze(Heights);
        }
        public void Analyze(List<int> Heights)
        {
            List<int[]> Possible = new List<int[]>();
            List<int> diffs = new List<int>();
            for(int i = 1; i < Heights.Count / 2; i++)
            {
                for(int k = 1; k < Heights.Count / 2; k++)
                {
                    int ok = 0;
                    int j = i + k;
                    int diff = Heights[i + k] - Heights[i];
                    while (j + k < Heights.Count)
                    {
                        ok++;
                        int diff2 = Heights[j + k] - Heights[j];
                        if (diff != diff2)
                        {
                            ok = 0;
                            break;
                        }
                        j = j + k;
                    }
                    if (ok >= 3)
                    {
                        //if (!diffs.Contains(diff))
                        //{
                            //Console.WriteLine("Possible cycle:" + i + " to " + (i + k).ToString());
                            //Console.WriteLine(diff);
                            diffs.Add(diff);
                            Possible.Add(new int[] { i, i + k });
                        //}                       
                    }
                }             
            }
            List<long> results = new List<long>();
            checked
            {
                for(int i = 0; i < Possible.Count; i++)
                {
                    int[] poss = Possible[i];
                    long max = 1_000_000_000_000;
                    long left = max - poss[0];

                    //long amountofcycles = left / (poss[1] - poss[0]);
                    //long amountmoretofall = max - left;
                    if (left % (poss[1] - poss[0]) == 0)
                    {
                        //Console.WriteLine(poss[0] + "->" + poss[1]);
                        long res = Heights[poss[0]] + (left / (poss[1] - poss[0])) * (diffs[i]);
                        Console.WriteLine(res - 1);
                        results.Add(res - 1); // for some reason it yields a value of + 1 always.
                    }
                    //else
                    //{
                    //    Console.WriteLine(poss[0] + "! ->" + poss[1]);
                    //    long res1 = amountofcycles * diffs[i] + Heights[poss[0] + int.Parse(amountmoretofall.ToString())];
                    //    Console.WriteLine(res1);
                    //}
                }
            }           
        }
        public void SpawnRock(Rock r)
        {
            Point spawnpoint = new Point(2, Highest.Y + 3 + r.Height);
            r.Pos = spawnpoint;
            while (r.isAlive)
            {
                JetMove(r);
                if (CanFall(r))
                    r.Pos.Y = r.Pos.Y - 1;
                else
                    SetRock(r);               
            }
            CheckHighest();
        }
        void JetMove(Rock r)
        {
            char move = pattern.GetNextPush();
            if(move == '<')
            {
                if(r.Pos.X - 1 >= 0)
                {
                    r.Pos.X = r.Pos.X - 1;
                    if (!OK(r))
                    {
                        r.Pos.X += 1;
                    }
                } 
            }
            else if(move == '>')
            {
                if (r.Pos.X + r.Width + 1 <= 7)
                {
                    r.Pos.X = r.Pos.X + 1;
                    if (!OK(r))
                    {
                        r.Pos.X -= 1;
                    }
                }
            }
        }
        void CheckHighest()
        {
            foreach(Point p in SetRocks)
            {
                if(p.Y > Highest.Y)
                {
                    Highest = p;
                }
            }
        }
        public void SetRock(Rock r)
        {
            for(int i = 0; i < r.Height; i++)
            {
                for(int j = 0; j < r.Width; j++)
                {
                    if (r.Body[i,j] == 1)
                        SetRocks.Add(new Point(r.Pos.X + j,r.Pos.Y - i));
                }
            }
            r.isAlive = false;
        }
        public bool OK(Rock r)
        {
            for (int i = 0; i < r.Height; i++)
            {
                for (int j = 0; j < r.Width; j++)
                {
                    if (r.Body[i, j] == 0)
                    {
                        continue;
                    }
                    foreach (Point rock in SetRocks)
                    {
                        if (rock.X == r.Pos.X + j && rock.Y == r.Pos.Y - i)
                        {
                            return false;
                        }

                    }
                }
            }
            return true;
        }
        public bool CanFall(Rock r)
        {
            for(int i = 0; i < r.Height; i++)
            {
                for(int j = 0; j < r.Width; j++)
                {
                    if (r.Body[i,j] == 0)
                    {
                        continue;
                    }
                    foreach(Point rock in SetRocks)
                    {
                        if (rock.X == r.Pos.X + j && rock.Y == r.Pos.Y - i - 1)
                        {
                            return false;
                        }
                        
                    }                  
                }
                if (r.Pos.Y - i - 1 <= 0)
                {
                    return false;
                }
            }
            return true;
        }
    }
    public class Rock
    {
        public Point Pos;
        public int[,] Body { get; private set; }
        public int Height { get { return Body.GetLength(0); } }
        public int Width { get { return Body.GetLength(1); } }
        public bool isAlive = true;
        public Rock(int type)
        {
            Body = GetBody(type);
        }

        static int[,] GetBody(int type)
        {
            switch (type)
            {
                case 0:
                    return new int[,] { { 1, 1, 1, 1 } 
                    };
                case 1:
                    return new int[,] { { 0, 1, 0 },
                                        { 1, 1, 1 },
                                        { 0, 1, 0 }
                    };
                case 2:
                    return new int[,]{  { 0, 0, 1 },
                                        { 0, 0, 1 },
                                        { 1, 1, 1 },
                    };
                case 3:
                    return new int[,] { { 1 },
                                        { 1 },
                                        { 1 },
                                        { 1 },
                    };
                case 4:
                    return new int[,] { { 1, 1 },
                                        { 1, 1 },
                    };
            }
            Console.WriteLine("error body");
            return new int[0, 0];
        }
    }
}