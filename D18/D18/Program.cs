namespace D18
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<Cube> Cubes = new List<Cube>();
            StreamReader sr = new StreamReader(@"..\..\..\input.txt");
            int[,,] map = new int[22,22,22];
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
                        else
                        {
                            if (!TryEscape(n, Cubes, map))
                            {
                                Cubes[i].VisibleSides--;
                                if (Cubes[i].VisibleSides < 0)
                                {
                                    Console.WriteLine("error");
                                }
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
        public static bool TryEscape(Cube air, List<Cube> Cubes, int[,,] map)
        {
            if (air.x > 21 || air.x < 0 || air.y > 21 || air.y < 0 || air.z > 21 || air.z < 0)
                return true;
            if (map[air.x, air.y, air.z] == 1)
            {
                return false;
            }
            if (map[air.x, air.y, air.z] == -2)
            {
                return false;
            }
            else if (map[air.x, air.y, air.z] == -1)
            {
                return true;
            }

            map[air.x, air.y, air.z] = -2;

            Cube Left = new Cube(air.x - 1, air.y, air.z);
            Cube Right = new Cube(air.x + 1, air.y, air.z);
            Cube Top = new Cube(air.x, air.y + 1, air.z);
            Cube Bottom = new Cube(air.x, air.y - 1, air.z);
            Cube Behind = new Cube(air.x, air.y, air.z - 1);
            Cube InFront = new Cube(air.x, air.y, air.z - 1);
            List<Cube> Neighbors = new List<Cube>()
            {
                Left,Top,Right,Behind,InFront,Bottom,
            };
            bool escape = false;
            foreach (Cube n in Neighbors)
            {
                escape = escape || TryEscape(n, Cubes, map);
                if(escape)
                {
                    map[air.x, air.y, air.z] = -1;
                    break;
                }
            }
            return escape;
        }
    }
}