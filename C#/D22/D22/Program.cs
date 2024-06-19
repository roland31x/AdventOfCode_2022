using System.Numerics;

namespace D22
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Map myMap = new Map(@"..\..\..\input.txt");
            //myMap.Show();

            int start = 0;
            for (int i = 0; i < myMap.map[0].Count; i++)
            {
                if (myMap.map[0][i] == 0)
                {
                    start = i;
                    break;
                }
            }

            Player player1 = new Player(0, start, 0, myMap);
            Player player2 = new Player(0, start, 0, myMap);

            Console.WriteLine("Part1 solution");
            Console.WriteLine(player1.Part1());
            Console.WriteLine("Part2 solution");
            Console.WriteLine(player2.Part2());

        }
    }
    public class Player
    {
        Map Map;
        public int i;
        public int j;
        public int dir; // 0 = right, 1 = down, 2 = left, 3 = up
        public Player(int i, int j, int dir, Map map)
        {
            this.i = i;
            this.j = j;
            this.dir = dir;
            this.Map = map;
        }
        void ChangeDir(string direction)
        {
            if (direction == "R")
            {
                if (dir + 1 > 3)
                    dir = 0;
                else
                    dir++;
            }
            else
            {
                if (dir - 1 < 0)
                    dir = 3;
                else
                    dir--;
            }
        }
        public int Part1()
        {
            foreach (Pathing p in Map.Path)
            {
                if (p.turn == null)
                {
                    int times = (int)p.times;
                    int x = 0;
                    while (x < times && MovePart1())
                    {
                        x++;
                    }
                }
                else
                {
                    ChangeDir(p.turn);
                }

            }
            int score = 1000 * (i + 1) + 4 * (j + 1) + dir;
            return score;
        }
        public int Part2()
        {
            foreach (Pathing p in Map.Path)
            {
                if (p.turn == null)
                {
                    int times = (int)p.times;
                    int x = 0;
                    while (x < times && MovePart2())
                    {
                        x++;
                    }
                }
                else
                {
                    ChangeDir(p.turn);
                }
            }
            int score = 1000 * (i + 1) + 4 * (j + 1) + dir;
            return score;
        }
        
        bool MovePart2()
        {
            int starti = i;
            int startj = j;
            int startdir = dir;
            switch (dir)
            {
                case 0:
                    j++;
                    break;
                case 1:
                    i++;
                    break;
                case 2:
                    j--;
                    break;
                case 3:
                    i--;
                    break;

            }

            CheckTP();

            if (Map.map[i][j] == 1)
            {
                i = starti;
                j = startj;
                dir = startdir;
                return false;
            }
            else
            {
                return true;
            }
        }
        void CheckTP()
        {
            // got sides like:  12
            //                  3
            //                 54
            //                 6


            // dirs: 0 = right, 1 = down, 2 = left, 3 = up
            if (i < 0)
            {
                if(j >= 50 && j < 100) // SIDE 1 TO SIDE 6 // C
                {
                    i = 150 + j - 50;
                    j = 0;
                    dir = 0;
                }
                else if(j >= 100 && j < 150) // SIDE 2 TO SIDE 6 // C
                {
                    i = 200 - 1;
                    j = j - 100;
                    dir = 3;
                }
            }
            else if (i >= 200) // SIDE 6 TO SIDE 2 // C
            {
                i = 0;
                j = 100 + j;
                dir = 1;
            }
            else if(j < 0)
            {
                if(i >= 100 && i < 150) // SIDE 5 TO SIDE 1 // C
                {
                    i = 50 - 1 - (i - 100);
                    j = 50;
                    dir = 0;
                }
                else if(i >= 150 && i < 200) // SIDE 6 TO SIDE 1 // C
                {
                    j = 50 + i - 150;
                    i = 0;
                    dir = 1;
                }
            }
            else if(j >= 150) // SIDE 2 TO 4 // C
            {
                i = 100 + (49 - i);
                j = 100 - 1;
                dir = 2;
            }
            else if (Map.map[i][j] == 9)
            {
                if (i >= 0 && i < 50) // SIDE 1 TO SIDE 5 // C
                {
                    i = 150 - 1 - i;
                    j = 0;
                    dir = 0;
                }
                else if (i >= 50 && i < 100) // SIDE 3 range
                {
                    if(j >= 100 && j < 150 && dir == 0) // SIDE 3 TO SIDE 2 
                    {
                        j = 100 + i - 50;
                        i = 50 - 1;
                        dir = 3;

                    }
                    else if(j >= 100 && j < 150 && dir == 1) // SIDE 2 TO SIDE 3
                    {
                        i = 50 + j - 100;
                        j = 100 - 1;
                        dir = 2;

                    }
                    else if (j >= 0 && j < 50 && dir == 2) // SIDE 3 TO SIDE 5
                    {
                        j = i - 50;
                        i = 100;
                        dir = 1;
                    }
                    else if (j >= 0 && j < 50 && dir == 3) // SIDE 5 TO SIDE 3
                    {
                        i = 50 + j;
                        j = 50;
                        dir = 0;
                    }
                }
                else if(i >= 100 && i < 150)
                {
                                                // SIDE 4 TO SIDE 2
                    i = 50 - 1 - (i - 100);
                    j = 150 - 1;
                    dir = 2;
                }
                else if (i >= 150 && i < 200)
                {
                    if (j >= 50 && j < 100 && dir == 0) // SIDE 6 TO SIDE 4
                    {
                        j = 50 + i - 150;
                        i = 150 - 1;
                        dir = 3;
                    }
                    else if (j >= 50 && j < 100 && dir == 1) // SIDE 4 TO SIDE 6
                    {
                        i = 150 + j - 50;
                        j = 50 - 1;
                        dir = 2;
                    }
                }
            }
        }
        bool MovePart1()
        {
            int starti = i;
            int startj = j;
            switch (dir)
            {
                case 0:
                    j++;
                    break;
                case 1:
                    i++;
                    break;
                case 2:
                    j--;
                    break;
                case 3:
                    i--;
                    break;

            }
            if (j < 0)
                j = Map.map[starti].Count - 1;
            if (j > Map.map[starti].Count - 1)
                j = 0;
            if (i < 0)
                i = Map.map.Count - 1;
            if (i > Map.map.Count - 1)
                i = 0;
                        
            if (Map.map[i][j] == 9)
            {
                do
                {
                    switch (dir)
                    {
                        case 0:
                            j++;
                            break;
                        case 1:
                            i++;
                            break;
                        case 2:
                            j--;
                            break;
                        case 3:
                            i--;
                            break;
                    }
                    if (i < 0)
                        i = Map.map.Count - 1;
                    if (i > Map.map.Count - 1)
                        i = 0;
                    if (j < 0)
                        j = Map.map[i].Count - 1;
                    if (j > Map.map[i].Count - 1)
                        j = 0;
                } while (Map.map[i][j] == 9);
            }
            if (Map.map[i][j] == 1)
            {
                i = starti;
                j = startj;
                return false;
            }
            else
            {
                return true;
            }
        }
    }
    public class Pathing
    {
        public int? times;
        public string? turn;
        public Pathing(string op)
        {
            if (int.TryParse(op, out int res))
                times = res;
            else
                turn = op;
        }
        public Pathing(int op)
        {
            times = op;
        }
        public override string ToString()
        {
            if (times != null)
                return times.ToString()!;
            else
                return turn!;
        }
    }
    public class Map
    {
        public List<List<int>> map = new List<List<int>>();
        public List<Pathing> Path = new List<Pathing>();
        public Map(string file) 
        {
            List<string> buffers = new List<string>();
            string buffer;
            using(StreamReader sr = new StreamReader(file))
            {
                buffer = sr.ReadLine()!;
                while(buffer != string.Empty)
                {
                    buffers.Add(buffer!);
                    buffer = sr.ReadLine()!;
                }
                buffer = sr.ReadLine()!;
                int nr = 0;
                for (int i = 0; i < buffer.Length; i++)
                {                  
                    if (buffer[i] - '0' >= 0 && buffer[i] - '0' <= 9)
                    {
                        nr *= 10;
                        nr += buffer[i] - '0';
                        if(i == buffer.Length - 1)
                        {
                            Path.Add(new Pathing(nr));
                        }
                    }
                    else
                    {
                        Path.Add(new Pathing(nr));
                        nr = 0;
                        Path.Add(new Pathing(buffer[i].ToString()));
                    }
                }
            }
            int max = 0;
            foreach(string line in buffers)
            {
                if(line.Length > max)
                    max = line.Length;
            }

            for(int i = 0; i < buffers.Count; i++)
            {
                List<int> row = new List<int>();
                for(int j = 0; j < buffers[i].Length; j++)
                {
                    if (buffers[i][j] == ' ')
                        row.Add(9);
                    else if (buffers[i][j] == '.')
                        row.Add(0);
                    else
                        row.Add(1);
                }
                int k = buffers[i].Length;
                while(k < max)
                {
                    row.Add(9);
                    k++;
                }
                map.Add(row);
            }
        }
        public void Show()
        {
            for(int i = 0; i < map.Count; i++)
            {
                for(int j = 0; j < map[i].Count; j++)
                {
                    Console.Write(map[i][j]);
                }
                Console.WriteLine();
            }
        }
        public void Show(int pi, int pj)
        {
            for (int i = 0; i < map.Count; i++)
            {
                for (int j = 0; j < map[i].Count; j++)
                {
                    if (i == pi && j == pj)
                    {
                        Console.Write("E"); 
                        continue;
                    }
                    Console.Write(map[i][j]);
                }
                Console.WriteLine();
            }
        }
    }
}