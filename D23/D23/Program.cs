using System.Drawing;

namespace D23
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Map myMap = new Map();
            myMap.Load(@"..\..\..\input.txt");
            
            for (int i = 0; i < 10; i++)
            {               
                myMap.Round();
            }
            //myMap.Show();
            Console.WriteLine("Part 1 solution:");
            Console.WriteLine(myMap.Score());
            int round = 10;
            while (myMap.isAlive)
            {
                myMap.Round();
                round++;              
            }
            Console.WriteLine("Part 2 solution:");
            Console.WriteLine(round);
        }
    }
    public class Map
    {
        public bool isAlive = true;
        List<Elf> elves = new List<Elf>();
        int[,] map = new int[10000, 10000];
        List<int[]> dirs = new List<int[]>() { new int[] { -1, 0 }, new int[] { 1, 0 }, new int[] { 0, -1 },  new int[] { 0, 1 },  };
        public Map()
        {

        }
        public void Load(string path)
        {
            using (StreamReader sr = new StreamReader(path))
            {
                int i = 0;
                while(!sr.EndOfStream)
                {
                    string line = sr.ReadLine()!;
                    for(int j = 0; j < line.Length; j++)
                    {
                        if (line[j] == '#')
                        {
                            elves.Add(new Elf(5000 + i, 5000 + j));
                            map[5000 + i, 5000 + j] = 1;
                        }
                    }
                    i++;
                }
            }
        }
        public void Round()
        {
            foreach(Elf elf in elves)
            {
                Propose(elf);
                if (elf.HasProposed)
                {
                    map[elf.Proposed.X, elf.Proposed.Y]--;
                }             
            }
            foreach(Elf elf in elves)
            {
                if (elf.HasProposed)
                {
                    elf.HasMoved = TryMove(elf);
                }
                else
                {
                    elf.HasMoved = false;
                }
                
            }
            foreach (Elf elf in elves)
            {
                if (map[elf.Proposed.X, elf.Proposed.Y] != 1)
                {
                    map[elf.Proposed.X, elf.Proposed.Y] = 0;
                }
                elf.HasProposed = false;
            }
            int[] firstdir = dirs[0];
            dirs.Remove(firstdir);
            dirs.Add(firstdir);

            AliveCheck();
        }
        void AliveCheck()
        {
            bool ok = false;
            foreach(Elf elf in elves)
            {
                if (elf.HasMoved)
                {
                    ok = true;
                    break;
                }               
            }
            if (!ok)
            {
                isAlive = false;
            }
        }
        void Propose(Elf elf)
        {
            int neighbors = 0;
            foreach (int[] dir in dirs)
            {
                if(ProposeOK(elf, dir, ref neighbors))
                {
                    if (!elf.HasProposed)
                    {
                        elf.Proposed = new Point(elf.I + dir[0], elf.J + dir[1]);
                        elf.HasProposed = true;
                    }               
                }
            }
            if(neighbors == 0)
            {
                elf.HasProposed = false;
            }
        }
        bool ProposeOK(Elf elf, int[] dir, ref int neighbors)
        {
            int iCheck = 0;
            int jCheck = 0;
            int Check1 = 0;
            int Check2 = 0;
            if (dir[0] != 0)
            {
                iCheck = elf.I + dir[0];
                jCheck = elf.J;
                Check1 = elf.J - 1;
                Check2 = elf.J + 1;

                if (map[iCheck, jCheck] == 1 || map[iCheck, Check1] == 1 || map[iCheck, Check2] == 1)
                {
                    neighbors++;
                    return false;
                }
            }
            else
            {
                iCheck = elf.I;
                jCheck = elf.J + dir[1];
                Check1 = elf.I - 1;
                Check2 = elf.I + 1;

                if (map[iCheck, jCheck] == 1 || map[Check1, jCheck] == 1 || map[Check2, jCheck] == 1)
                {
                    neighbors++;
                    return false;
                }
            }          
            return true;
        }
        bool TryMove(Elf elf)
        {
            if (map[elf.Proposed.X, elf.Proposed.Y] == -1)
            {
                map[elf.I, elf.J] = 0;
                elf.I = elf.Proposed.X;
                elf.J = elf.Proposed.Y;
                map[elf.I, elf.J] = 1;
                return true;
            }
            else
            {
                return false;
            }
                        
        }
        public void Show()
        {
            int minI = 10000;
            int maxI = 0;
            int minJ = 10000;
            int maxJ = 0;
            foreach(Elf elf in elves)
            {
                if (elf.I > maxI)
                    maxI = elf.I;
                if(elf.I < minI)
                    minI = elf.I;
                if(elf.J > maxJ)
                    maxJ = elf.J;
                if(elf.J < minJ)
                    minJ = elf.J;
            }
            for(int i = minI; i <= maxI; i++)
            {
                for(int j = minJ; j <= maxJ; j++)
                {
                    Console.Write(map[i, j]);
                }
                Console.WriteLine();
            }
        }
        public int Score()
        {
            int score = 0;

            int minI = 10000;
            int maxI = 0;
            int minJ = 10000;
            int maxJ = 0;
            foreach (Elf elf in elves)
            {
                if (elf.I > maxI)
                    maxI = elf.I;
                if (elf.I < minI)
                    minI = elf.I;
                if (elf.J > maxJ)
                    maxJ = elf.J;
                if (elf.J < minJ)
                    minJ = elf.J;
            }
            for (int i = minI; i <= maxI; i++)
            {
                for (int j = minJ; j <= maxJ; j++)
                {
                    if (map[i,j] != 1)
                    {
                        score++;
                    }
                }
            }

            return score;
        }
    }
    public class Elf
    {
        public int I;
        public int J;
        public Point Proposed;
        public bool HasProposed = false;
        public bool HasMoved = false;
        public Elf(int I, int J)
        {
            this.I = I;
            this.J = J;
        }
    }
}