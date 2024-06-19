namespace D19
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<Blueprint> blueprints = new List<Blueprint>();
            using(StreamReader sr = new StreamReader(@"..\..\..\input.txt"))
            {
                int i = 1;
                while(!sr.EndOfStream)
                {
                    Blueprint blueprint = new Blueprint(i,sr.ReadLine()!);
                    i++;
                    blueprints.Add(blueprint);
                }               
            }

            int quality = 0;
            foreach(Blueprint blueprint in blueprints)
            {
                blueprint.Solve(24);
                quality += blueprint.Score * blueprint.ID;
            }
            Console.WriteLine("Part 1 solution:");
            Console.WriteLine(quality);

            int part2 = 1;
            for (int i = 0; i < 3; i++)
            {
                blueprints[i].Solve(32);
                part2 *= blueprints[i].Score;
            }
            Console.WriteLine("Part 2 solution:");
            Console.WriteLine(part2);
        }
    }
    public class Blueprint
    {
        public int ID { get; }
        public int Score { get; private set; }

        int OreCost; 
        int ClayCost;
        int[] ObsidianCost = new int[2]; // ore || clay
        int[] GeodeCost = new int[2]; // ore || obsidian
        List<int[]> robotcosts;
        int[] max = new int[4];

        public Blueprint(int id, string toParse) 
        {
            ID = id;
            string[] tokens = toParse.Split(" ");
            OreCost = int.Parse(tokens[6]);
            ClayCost = int.Parse(tokens[12]);
            ObsidianCost[0] = int.Parse(tokens[18]);
            ObsidianCost[1] = int.Parse(tokens[21]);
            GeodeCost[0] = int.Parse(tokens[27]);
            GeodeCost[1] = int.Parse(tokens[30]);
            robotcosts = new List<int[]>() { new int[] { OreCost, 0, 0, 0 }, new int[] { ClayCost, 0, 0, 0 }, new int[] { ObsidianCost[0], ObsidianCost[1], 0, 0 }, new int[] { GeodeCost[0], 0, GeodeCost[1], 0 } };
            max[0] = OreCost;
            if (ClayCost > max[0])
                max[0] = ClayCost;
            if (ObsidianCost[0] > max[0])
                max[0] = ObsidianCost[0];
            if (GeodeCost[0] > max[0])
                max[0] = GeodeCost[0];
            max[1] = ObsidianCost[1];
            max[2] = GeodeCost[1];
        }
        public void Solve(int maxmin)
        {
            int max = 0;
            DFS(1, 0, 0, 0, new int[4], 1, 1, -1, ref max, maxmin);
            Score = max;
        }
        void DFS(int oreR, int clayR, int obsR, int geodeR, int[] pastores, int min, int passedmin, int robotbuilt, ref int max, int maxmin)
        {
            if (min > maxmin)
            {
                return;
            }         

            int[] ores = new int[4];

            ores[0] += oreR * passedmin;
            ores[0] += pastores[0];
            ores[1] += clayR * passedmin;
            ores[1] += pastores[1];
            ores[2] += obsR * passedmin;
            ores[2] += pastores[2];
            ores[3] += geodeR * passedmin;
            ores[3] += pastores[3];

            if(robotbuilt != -1)
            {
                for(int i = 0; i < 4; i++)
                {
                    ores[i] -= robotcosts[robotbuilt][i];
                }
                ores[robotbuilt] -= passedmin;
            }

            int[] robots = new int[4];
            robots[0] = oreR;
            robots[1] = clayR;
            robots[2] = obsR;
            robots[3] = geodeR;

            int nextOre = MinCalc(robots, ores, 0);
            int nextClay = MinCalc(robots, ores, 1);
            int nextObs = MinCalc(robots, ores, 2);
            int nextGeode = MinCalc(robots, ores, 3);

            DFS(oreR + 1, clayR, obsR, geodeR, ores, min + nextOre, nextOre, 0, ref max, maxmin);
            DFS(oreR, clayR + 1, obsR, geodeR, ores, min + nextClay, nextClay, 1, ref max, maxmin);
            DFS(oreR, clayR, obsR + 1, geodeR, ores, min + nextObs, nextObs, 2, ref max, maxmin);
            DFS(oreR, clayR, obsR, geodeR + 1, ores, min + nextGeode, nextGeode, 3, ref max, maxmin);

            while (min < maxmin)
            {
                ores[0] += oreR;
                ores[1] += clayR;
                ores[2] += obsR;
                ores[3] += geodeR;
                min++;
            }
            if (ores[3] > max)
            {
                max = ores[3];
            }

        }
        int MinCalc(int[] robots, int[] ores, int robottype)
        {
            switch (robottype)
            {
                case 0:
                    return NextOreRobot(robots, ores);
                case 1:
                    return NextClayRobot(robots, ores);
                case 2:
                    return NextObsRobot(robots, ores);
                case 3:
                    return NextGeodeRobot(robots, ores);
            }
            return 0;
        }
        int NextOreRobot(int[] robots, int[] ores)
        {
            if (robots[0] >= max[0])
                return 60;
            int[] simores = new int[4];
            for(int i = 0; i < 4; i++)
            {
                simores[i] = ores[i];
            }
            int min = 0;
            while (simores[0] < robotcosts[0][0] && min < 40)
            {
                for (int i = 0; i < 4; i++)
                {
                    simores[i] += robots[i];
                }
                min++;
            }
            return min + 1;
        }
        int NextClayRobot(int[] robots, int[] ores)
        {
            if (robots[1] >= max[1])
                return 60;
            int[] simores = new int[4];
            for (int i = 0; i < 4; i++)
            {
                simores[i] = ores[i];
            }
            int min = 0;
            while (simores[0] < robotcosts[1][0] && min < 40)
            {
                for (int i = 0; i < 4; i++)
                {
                    simores[i] += robots[i];
                }
                min++;
            }
            return min + 1;
        }
        int NextObsRobot(int[] robots, int[] ores)
        {
            if (robots[2] >= max[2])
                return 60;
            int[] simores = new int[4];
            for (int i = 0; i < 4; i++)
            {
                simores[i] = ores[i];
            }
            int min = 0;
            while ( ( simores[0] < robotcosts[2][0] || simores[1] < robotcosts[2][1] ) && min < 40)
            {
                for (int i = 0; i < 4; i++)
                {
                    simores[i] += robots[i];
                }
                min++;
            }
            return min + 1;
        }
        int NextGeodeRobot(int[] robots, int[] ores)
        {
            int[] simores = new int[4];
            for (int i = 0; i < 4; i++)
            {
                simores[i] = ores[i];
            }
            int min = 0;
            while ( ( simores[0] < robotcosts[3][0] || simores[2] < robotcosts[3][2] ) && min < 40)
            {
                for (int i = 0; i < 4; i++)
                {
                    simores[i] += robots[i];
                }
                min++;
            }
            return min + 1;
        }
    }
}