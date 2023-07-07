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
                blueprint.CalcScore();
                quality += blueprint.Score;
            }
            Console.WriteLine("Part 1 solution:");
            Console.WriteLine(quality);

            //int part2 = 1;
            //for(int i = 0; i < 3; i++)
            //{
            //    blueprints[i].CalcScorePart2();
            //    part2 *= blueprints[i].Score2;
            //}
            //Console.WriteLine("Part 2 solution:");
            //Console.WriteLine(part2);
        }
    }
    public class Step
    {
        public int[] ores = new int[4];
        public int[] robots = new int[4];
        public int minute;
        public Step(int min, int[] ores, int[] robots)
        {
            minute = min;
            this.ores = ores;
            this.robots = robots;
        }


    }
    public class Blueprint
    {
        int ID;
        public static int AlreadyGeodeRobots = 0;
        public int Score { get; private set; }
        public int Score2 { get; private set; }
        Queue<int[,]> Possibilities = new Queue<int[,]>();
        // ore,clay,obs,geode
        //row 1 is robots
        //row 2 is amount

        int OreCost; 
        int ClayCost;
        int[] ObsidianCost = new int[2]; // ore || clay
        int[] GeodeCost = new int[2]; // ore || obsidian
        List<int[]> robotcosts;

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
        }
        public void CalcScorePart2()
        {
            int[,] Ores = new int[4, 4];
            Ores[0, 0] = 1; // we start with 1 ore robot
            Ores[3, 0] = 1; // minute;
            Possibilities.Enqueue(Ores);

            int maxval = 0;
            while (Possibilities.Count > 0)
            {
                int[,] deq = Possibilities.Dequeue();

                

                if (deq[3, 0] > 32)
                {
                    if (deq[1, 3] > maxval)
                        maxval = deq[1, 3];
                    continue;
                }
                if (deq[3, 0] == 32)
                {
                    AddRobotWorkload(deq);
                    deq[3, 0]++;

                    Possibilities.Enqueue(deq);
                    continue;
                }
                if (deq[3, 0] == 31)
                {
                    TryBuyRobot(deq, Possibilities, 3);

                    AddRobotWorkload(deq);
                    deq[3, 0]++;

                    Possibilities.Enqueue(deq);
                    continue;
                }
                for (int i = 3; i >= 0; i--)
                {
                    TryBuyRobot(deq, Possibilities, i);
                    //deq[2, i] = 0;
                }

                AddRobotWorkload(deq);
                deq[3, 0]++;

                Possibilities.Enqueue(deq);
            }


            Score2 = maxval;
        }
        public void CalcScore()
        {
            int[,] Ores = new int[4, 4];
            Ores[0, 0] = 1; // we start with 1 ore robot
            Ores[3, 0] = 1; // minute;
            Possibilities.Enqueue(Ores);

            int maxval = 0;
            while(Possibilities.Count > 0)
            {
                int[,] deq = Possibilities.Dequeue();
                
                if (deq[3,0] > 24)
                {
                    if (deq[1,3] > maxval)
                        maxval = deq[1,3];
                    continue;
                }
                if (deq[3, 0] == 24)
                {
                    AddRobotWorkload(deq);
                    deq[3, 0]++;

                    Possibilities.Enqueue(deq);
                    continue;
                }
                if (deq[3, 0] == 23)
                {
                    TryBuyRobot(deq, Possibilities, 3);

                    AddRobotWorkload(deq);
                    deq[3, 0]++;

                    Possibilities.Enqueue(deq);
                    continue;
                }

                for (int i = 3; i >= 0; i--)
                { 
                    TryBuyRobot(deq, Possibilities, i);
                }

                AddRobotWorkload(deq);
                deq[3, 0]++;
                

                Possibilities.Enqueue(deq);
            }


            Score = maxval * ID;
        }

        public void AddRobotWorkload(int[,] possibility)
        {
            for(int i = 0; i < 4; i++)
            {
                possibility[1, i] += possibility[0, i];
            }
        }
        private bool TryBuyRobot(int[,] possibility, Queue<int[,]> queue, int robot)
        {
            for(int j = 0; j < 4; j++)
            {
                if (possibility[1, j] < robotcosts[robot][j])
                    return false;
            }

            int ok = 0;
            for (int j = 0; j < 4; j++)
            {
                if (possibility[1, j] - robotcosts[robot][j] >= robotcosts[robot][j])
                {
                    ok++;
                }
            }
            if (ok == 4)
            {
                return false;
            }

            //if (robot == 2)
            //{
            //    int x = 10;
            //}
            if (robot == 3)
            {
                if (possibility[0,1] == 7 && possibility[0, 2] == 3)
                {
                    int x = 10;
                }
                    
            }

            int[,] newpossibility = new int[4, 4];

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    newpossibility[i, j] = possibility[i, j];
                }
            }

            AddRobotWorkload(newpossibility);

            for (int j = 0; j < 4; j++)
            {
                newpossibility[1, j] -= robotcosts[robot][j];
            }
            newpossibility[0, robot] += 1;

            newpossibility[3, 0]++;

            //foreach (int[,] pos2 in queue)
            //{
            //    //int ok = 0;
            //    //for(int i = 0; i < 2; i++)
            //    //{
            //    //    for (int j = 0; j < 4; j++)
            //    //    {
            //    //        if (newpossibility[i, j] == pos2[i, j])
            //    //            ok++;
            //    //    }
            //    //}
            //    //if(ok == 8)
            //    //{
            //    //    return false;
            //    //}
            //}


            queue.Enqueue(newpossibility);

            return true;
        }       
    }
}