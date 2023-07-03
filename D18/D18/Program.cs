using System.ComponentModel.Design;

namespace D18
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<Cube> Cubes = new List<Cube>();
            StreamReader sr = new StreamReader(@"..\..\..\input.txt");
            int[,,] map = new int[24,24,24];
            while (!sr.EndOfStream)
            {
                string[] tokens = sr.ReadLine().Split(",");
                int x = int.Parse(tokens[0]);
                int y = int.Parse(tokens[1]);
                int z = int.Parse(tokens[2]);
                map[x, y, z] = 1;
                Cube c = new Cube(x, y, z);
                Cubes.Add(c);
            }
            Cube.SolveList(Cubes, map);
            int sum = 0;
            int sum2 = 0;
            foreach (Cube c in Cubes)
            {
                sum += c.UntouchedSides;
                sum2 += c.VisibleSides;
            }
            Console.WriteLine("Part1 solution:");
            Console.WriteLine(sum);           
            Console.WriteLine("Part2 solution:");
            Console.WriteLine(sum2);

        }
    }
    public class Cube
    {
        public int x;
        public int y;
        public int z;
        public int UntouchedSides = 6;
        public int VisibleSides = 6;

        public Cube(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
        public static void SolveList(List<Cube> Cubes, int[,,] map)
        {
            DFS(0, 0, 0, map);
            for (int i = 0; i < Cubes.Count; i++)
            {
                Cube Left = new Cube(Cubes[i].x - 1, Cubes[i].y, Cubes[i].z);
                Cube Right = new Cube(Cubes[i].x + 1, Cubes[i].y, Cubes[i].z);
                Cube Top = new Cube(Cubes[i].x, Cubes[i].y + 1, Cubes[i].z);             
                Cube Bottom = new Cube(Cubes[i].x, Cubes[i].y - 1, Cubes[i].z);
                Cube Behind = new Cube(Cubes[i].x, Cubes[i].y, Cubes[i].z - 1);
                Cube InFront = new Cube(Cubes[i].x, Cubes[i].y, Cubes[i].z - 1);
                List<Cube> Neighbors = new List<Cube>()
                {
                    Left,Top,Right,Behind,InFront,Bottom,
                };
                foreach(Cube n in Neighbors)
                {
                    if(n.z < 0 || n.y < 0 || n.x < 0)
                    {
                        Console.WriteLine("whoops");
                        continue;
                    }
                    try
                    {
                        if (map[n.x, n.y, n.z] == 1)
                        {
                            Cubes[i].UntouchedSides--;
                            //Cubes[i].VisibleSides--;
                            if (Cubes[i].VisibleSides < 0)
                            {
                                Console.WriteLine("error");
                            }
                        }
                        if (map[n.x, n.y, n.z] <= 1)
                        {
                            Cubes[i].VisibleSides--;
                            if (Cubes[i].VisibleSides < 0)
                            {
                                Console.WriteLine("error");
                            }
                        }
                    }
                    catch (IndexOutOfRangeException)
                    {
                        continue;
                    }                  
                }
            }
               

        }
        public static void DFS(int x, int y, int z, int[,,] map)
        {
            if (x > 23 || x < 0 || y < 0 || y > 23 || z < 0 || z > 23)
                return;
            if (map[x, y, z] >= 1)
                return;
            map[x, y, z] = 2;
            DFS(x + 1, y, z, map);
            DFS(x - 1, y, z, map);
            DFS(x, y + 1, z, map);
            DFS(x, y - 1, z, map);
            DFS(x, y, z + 1, map);
            DFS(x, y, z - 1, map);
        }        
    }
}