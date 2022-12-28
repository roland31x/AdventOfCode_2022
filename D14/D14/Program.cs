using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace D14
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // FOR PART 1 GO TO LINE 132 AND COMMENT THE LAST PART OF THE FUNCTION BELOW MY MARKED COMMENT ( RESULT WILL BE 1 HIGHER THAN ANSWER BECAUSE IT COUNTS THE SPAWNED SAND THAT WILL FALL INTO ABYSS )
            // FOR PART 2 UNCOMMENT IT
            Map Scan = new Map(1000, 1000);
            using (StreamReader sr = new StreamReader("input.txt"))
            {
                while(!sr.EndOfStream)
                {
                    string s = sr.ReadLine();
                    Regex xCoord = new Regex(@"\b[0-9]+,");
                    Regex yCoord = new Regex(@",\b[0-9]+\b");
                    MatchCollection xCoordsR = xCoord.Matches(s);
                    MatchCollection yCoordsR = yCoord.Matches(s);
                    List<int> xCoords = new List<int>();
                    List<int> yCoords = new List<int>();
                    foreach(Match m in xCoordsR)
                    {
                        string f = m.Value;
                        f = f.Replace(",", "");
                        xCoords.Add(int.Parse(f));
                    }
                    foreach (Match m in yCoordsR)
                    {
                        string f = m.Value;
                        f = f.Replace(",", "");
                        yCoords.Add(int.Parse(f));
                    }
                    for (int i = 0; i < xCoords.Count - 1; i++)
                    {
                        if (xCoords[i] == xCoords[i + 1])
                        {
                            int yT = yCoords[i];
                            int yT2 = yCoords[i + 1];
                            if(yT > yT2)
                            {
                                (yT, yT2) = (yT2, yT);
                            }
                            for (int j = yT; j <= yT2; j++)
                            {
                                int xT = xCoords[i];
                                Scan.map[j][xT].isRock = true;
                            }
                            continue;
                        }
                        if (yCoords[i] == yCoords[i + 1])
                        {
                            int xT = xCoords[i];
                            int xT2 = xCoords[i + 1];
                            if (xT > xT2)
                            {
                                (xT, xT2) = (xT2, xT);
                            }
                            for (int j = xT; j <= xT2; j++)
                            {
                                int yT = yCoords[i];
                                Scan.map[yT][j].isRock = true;
                            }
                            continue;
                        }
                    }
                    
                }
            }
            Scan.FindMinPoint();
            // Scan.Draw(); // visualization of the scan
            int sands = 0;
            while (!Scan.reachedAbyss)
            {
                Scan.mFall();
                //Scan.Draw(); 
                //Thread.Sleep(100);
                sands++;
                if (Scan.StartSand())
                {
                    break;
                }
                //Console.Clear();
            }
            //Scan.Draw();
            Console.WriteLine(sands); // USE (sands - 1) FOR PART 1 coz it will drop a sand to check for abyss.
        }
    }
    public class Map
    {
        public Point start;
        public List<List<Point>> map { get; set; }
        public int minPoint { get; set; }
        public bool reachedAbyss { get; set; }
        public Map(int y, int x)
        {
            map = new List<List<Point>>();
            for(int i = 0; i < y; i++)
            {
                map.Add(new List<Point>());
                for(int j = 0; j < x; j++)
                {
                    map[i].Add(new Point(j, i)); 
                }
            }
            start = map[0][500];
            start.isStart = true;
            reachedAbyss = false;
        }
        public void FindMinPoint()
        {
            int minY = 0;
            foreach(List<Point> p in map)
            {
                foreach(Point pt in p)
                {
                    if(pt.isRock && pt.posY > minY)
                    {
                        minY = pt.posY;
                    }
                }
            }
            minPoint = minY + 2;

            // BELOW ONLY FOR PART 2

            for (int i = 0; i < map[0].Count; i++)
            {
                map[minPoint][i].isRock = true;
            }

            // COMMENT THIS PART FOR PART 1 ANSWER
        }
        public void Draw()
        {
            for (int y = 0; y < 20; y++)
            {
                for (int x = 450; x < 550; x++)
                {
                    Console.Write(map[y][x].ToString());
                }
                Console.WriteLine();
            }

        }
        public bool StartSand()
        {
            if (start.isSand)
            {
                return true;
            }
            else return false;
        }
        public void mFall()
        {
            start.Fall(this);
        }
    }
    public class Point
    {
        public int posX { get; set; }
        public int posY { get; set; }

        public bool isRock { get; set; }
        public bool isSand { get; set; }

        public bool isStart { get; set; }

        public Point(int x, int y)
        {
            isRock = false;
            isSand = false;
            isStart = false;
            posX = x; posY = y;
        }
        public override string ToString()
        {
            if (isRock)
            {
                return "#";
            }
            if (isSand)
            {
                return "o";
            }
            if (isStart)
            {
                return "+";
            }
            else return ".";
        }
        public void Fall(Map scan)
        {
            int yPos = scan.start.posY;
            int xPos = scan.start.posX;
            if (scan.reachedAbyss)
            {
                return;
            }
            while (!scan.map[yPos + 1][xPos].isSand && !scan.map[yPos + 1][xPos].isRock)
            {
                yPos++;
                if (yPos == scan.minPoint)
                {
                    scan.reachedAbyss = true;
                    return;
                }
            }
            if (!scan.map[yPos + 1][xPos-1].isSand && !scan.map[yPos + 1][xPos - 1].isRock)
            {
                this.Fall(scan, yPos + 1, xPos - 1);
                return;
            }
            if (!scan.map[yPos + 1][xPos + 1].isSand && !scan.map[yPos + 1][xPos + 1].isRock )
            {
                this.Fall(scan, yPos + 1, xPos + 1);
                return;
            }            
            else scan.map[yPos][xPos].isSand = true;
        }
        public void Fall(Map scan, int y, int x)
        {
            int yPos = y;
            int xPos = x;
            if (scan.reachedAbyss)
            {
                return;
            }
            while (!scan.map[yPos + 1][xPos].isSand && !scan.map[yPos + 1][xPos].isRock)
            {
                yPos++;
                if (yPos == scan.minPoint)
                {
                    scan.reachedAbyss = true;
                    return;
                }
            }
            if (!scan.map[yPos + 1][xPos - 1].isSand && !scan.map[yPos + 1][xPos - 1].isRock)
            {
                this.Fall(scan, yPos + 1, xPos - 1);
                return;
            }
            if (!scan.map[yPos + 1][xPos + 1].isSand && !scan.map[yPos + 1][xPos + 1].isRock)
            {
                this.Fall(scan, yPos + 1, xPos + 1);
                return;
            }           
            else 
            {
                scan.map[yPos][xPos].isSand = true;
                return;
            }
        }
    }
}
