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

            Node nstart = new Node(myMap.map[0][start], 0, start);

            Player player1 = new Player(0, start, 0, myMap);
            NodePlayer player2 = new NodePlayer(nstart, myMap.Path);

            Console.WriteLine("Part1 solution");
            Console.WriteLine(player1.Part1());
            Console.WriteLine("Part2 solution");
            Console.WriteLine(player2.Part2());

        }
    }
    public class NodePlayer
    {
        Node current;
        List<Pathing> Path;
        int dir = 0;
        public NodePlayer(Node node, List<Pathing> path)
        {
            current = node;
            Path = path;
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
        public int Part2()
        {
            foreach(Pathing p in Path)
            {
                if(p.turn == null)
                {
                    int times = (int)p.times;
                    int x = 0;
                    while(x < times && current.neighbors[dir].Val == 0)
                    {
                        current = current.neighbors[dir];
                        x++;
                    }
                }
                else
                {
                    ChangeDir(p.turn);
                }
            }



            int Score = current.mapI * 1000 + current.mapJ * 4 + dir;
            return Score;
        }
    }
    public class NodeMap
    {
        List<Node> nodes = new List<Node>();

    }
    public class Node
    {
        public Node[] neighbors = new Node[4]; // 0 - right, 1 - down, 2 - left, 3 - up
        public int Val { get; }
        public int mapI { get; }
        public int mapJ { get; }
        public Node(int value, int mapI, int mapJ)
        {
            Val = value;
            this.mapJ = mapJ;
            this.mapI = mapI;
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
            foreach(Pathing p in Map.Path)
            {
                if(p.turn == null)
                {
                    int times = (int)p.times;
                    int x = 0;
                    while(x < times && MovePart1())
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
    }
}