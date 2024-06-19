using System;
using System.Security.AccessControl;

namespace D24
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Map myMap = new Map(@"..\..\..\input.txt");
            myMap.Simulate();
        }
    }
    public class Blizzard
    {
        public static List<int[]> dirs = new List<int[]>() { new int[] { 1, 0 }, new int[] { 0, -1 }, new int[] { 0, 1 }, new int[] { -1, 0 } };
        public int I;
        public int J;
        public int dir;
        public Blizzard(int i, int j, char direction)
        {
            I = i;
            J = j;
            if (direction == 'v')
                dir = 0;
            if (direction == '<')
                dir = 1;
            if (direction == '>')
                dir = 2;
            if (direction == '^')
                dir = 3;
        }
    }
    public class Player
    {
        public static List<int[]> dirs = new List<int[]>() { new int[] { 1, 0 }, new int[] { 0, -1 }, new int[] { 0, 1 }, new int[] { -1, 0 } };
        public int I;
        public int J;
        public Player(int i, int j)
        {
            I = i;
            J = j;
        }
    }

    public class Map
    {
        int[,] map;
        List<Blizzard> blizzs = new List<Blizzard>();
        List<Player> players = new List<Player>();
        int[] goal = new int[2];
        public Map(string file) 
        {
            List<string> buffers = new List<string>();
            using(StreamReader sr = new StreamReader(file))
            {
                while(!sr.EndOfStream)
                {
                    buffers.Add(sr.ReadLine()!);
                }
            }
            map = new int[buffers.Count, buffers[0].Length];

            for(int i = 0; i < buffers.Count; i++)
            {
                for(int j = 0; j < buffers[i].Length; j++)
                {
                    if (buffers[i][j] == 'v' || buffers[i][j] == '^' || buffers[i][j] == '<' || buffers[i][j] == '>')
                    {
                        map[i, j]++;
                        blizzs.Add(new Blizzard(i, j, buffers[i][j]));
                    }
                    else if(buffers[i][j] == '#')
                    {
                        map[i, j] = 9;
                    }
                    
                }
            }
            players.Add(new Player(0, 1));
        }
        void MoveBlizzard(Blizzard blizzard)
        {
            map[blizzard.I, blizzard.J]--;
            blizzard.I += Blizzard.dirs[blizzard.dir][0];
            blizzard.J += Blizzard.dirs[blizzard.dir][1];
            if(blizzard.I == 0)
                blizzard.I = map.GetLength(0) - 2;
            if(blizzard.J == 0)
                blizzard.J = map.GetLength(1) - 2;
            if (blizzard.I == map.GetLength(0) - 1)
                blizzard.I = 1;
            if (blizzard.J == map.GetLength(1) - 1)
                blizzard.J = 1;
            map[blizzard.I, blizzard.J]++;
        }
        public void Simulate()
        {
            int min = 0;
            SetGoal(map.GetLength(0) - 1, map.GetLength(1) - 2);
            SimGoal(ref min);
            Console.WriteLine("Part 1 solution:");
            Console.WriteLine(min);
            
            SetGoal(0, 1);
            SimGoal(ref min);

            SetGoal(map.GetLength(0) - 1, map.GetLength(1) - 2);
            SimGoal(ref min);
            Console.WriteLine("Part 2 solution:");
            Console.WriteLine(min);

        }
        void SetGoal(int i, int j)
        {
            goal[0] = i;
            goal[1] = j;
        }
        void SimGoal(ref int min)
        {
            while (!CheckIfAnyPlayerReachedGoal())
            {
                foreach (Blizzard blizz in blizzs)
                {
                    MoveBlizzard(blizz);
                }
                List<Player> possiblemoves = new List<Player>();
                foreach (Player p in players)
                {
                    bool ok = true;
                    foreach (int[] dir in Player.dirs)
                    {
                        int newI = p.I + dir[0];
                        int newJ = p.J + dir[1];
                        if (newI < 0 || newI >= map.GetLength(0) || newJ < 0 || newJ >= map.GetLength(1))
                            continue;
                        if (map[newI, newJ] == 0)
                        {
                            ok = true;
                            foreach (Player p2 in possiblemoves)
                            {
                                if (p2.I == newI && p2.J == newJ)
                                {
                                    ok = false;
                                    break;
                                }
                            }
                            if (ok)
                            {
                                possiblemoves.Add(new Player(newI, newJ));
                            }
                        }
                    }
                    ok = true;
                    foreach (Player p2 in possiblemoves)
                    {
                        if (p2.I == p.I && p2.J == p.J)
                        {
                            ok = false;
                            break;
                        }
                    }
                    if (ok)
                    {
                        if (map[p.I, p.J] == 0)
                            possiblemoves.Add(p);
                    }
                }
                players = possiblemoves;
                min++;
            }

            Player good = null;
            foreach (Player p in players)
            {
                if (p.I == goal[0] && p.J == goal[1])
                {
                    good = p;
                    break;
                }
            }
            players.Clear();
            players.Add(good!);
        }
        bool CheckIfAnyPlayerReachedGoal()
        {
            foreach(Player p in players)
            {
                if(p.I == goal[0] && p.J == goal[1])
                {
                    return true;
                }
            }
            return false;
        }
        public void Show()
        {
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    bool ok = false;
                    foreach (Player player in players)
                    {
                        if (player.I == i && player.J == j)
                        {
                            Console.Write("E");
                            ok = true;
                        }
                    }
                    if(!ok)
                        Console.Write(map[i, j]);
                }
                Console.WriteLine();
            }
        }
    }
}