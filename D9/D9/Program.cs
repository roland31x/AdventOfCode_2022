using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D9
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int sum = 0;
            Head thisHead = new Head();
            using (StreamReader sr = new StreamReader("input.txt"))
            {
                while (!sr.EndOfStream)
                {
                    string s = sr.ReadLine();
                    string[] tokens = s.Split(' ');
                    char op = char.Parse(tokens[0]);
                    int times = int.Parse(tokens[1]);
                    thisHead.Move(op, times);
                }
            }
            sum = thisHead.tails.Count;
            Console.WriteLine(sum);
        }
    }
    class Head
    {
        public List<Tail> myTails { get; set; }  // NOT NEEDED FOR PART 1
        public int posX { get; set; }
        public int posY { get; set; }
        public int LastPozX { get; set; }
        public int LastPozY { get; set; }

        public Tail myTail;          // has a single tail

        public List<Tail> tails;   // STORES PAST TAIL POSITIONS
        public Head()
        {
            myTails = new List<Tail>() { new Tail(this), new Tail(this), new Tail(this), new Tail(this), new Tail(this), new Tail(this), new Tail(this), new Tail(this), new Tail(this)}; // NOT NEEDED FOR PART 1
            tails = new List<Tail>();
            myTail = myTails[0];
            posX = 0;
            posY = 0;
        }
        public void Move(char op, int times)
        {
            while (times > 0)
            {
                SavePos();
                switch (op)
                {
                    case 'R':
                        MoveRight();
                        break;
                    case 'L':
                        MoveLeft();
                        break;
                    case 'U':
                        MoveUp();
                        break;
                    case 'D':
                        MoveDown();
                        break;
                }
                TailsCheck();  // ONLY USE TAILCHECK() FOR PART 1 
                times--;
                bool OK = true;
                foreach (Tail t in tails)
                {
                    if (t.Check(myTails[8]))
                    {
                        OK = false;
                        break;
                    }
                }
                if (OK)
                {
                    tails.Add(new Tail(myTails[8].TailposX, myTails[8].TailposY));   // ADD myTail instead of myTails[8] for PART 1
                }                             
            }
        }
        public void MoveRight()
        {
            posX += 1;
        }
        public void MoveLeft()
        {
            posX -= 1;
        }
        public void MoveUp()
        {
            posY += 1;
        }
        public void MoveDown()
        {
            posY -= 1;
        }
        public void SavePos()
        {
            LastPozX = posX;
            LastPozY = posY;
        }
        public void TailCheck()
        {
            if(Math.Abs(this.posX - myTail.TailposX) > 1 || Math.Abs(this.posY - myTail.TailposY) > 1)
            {
                myTail.MoveTo(this);
            }
        }
        public void TailsCheck()
        {
            for(int i = 0; i < myTails.Count - 1; i++) 
            {
                if(i == 0)
                {
                    TailCheck();
                }
                if (Math.Abs(myTails[i + 1].TailposX - myTails[i].TailposX) > 1 || Math.Abs(myTails[i+1].TailposY - myTails[i].TailposY) > 1)
                {
                    myTails[i + 1].MoveTo(myTails[i]);
                }
            }
        }
    }
    class Tail
    {
        public int TailposX { get; set; }
        public int TailposY { get; set; }
        public Tail(Head head)
        {
            TailposX = head.posX;
            TailposY = head.posY;
        }
        public Tail(int posx, int posy)
        {
            TailposX = posx;
            TailposY = posy;
        }
        public void MoveTo(Head head)
        {
            TailposX = head.LastPozX;
            TailposY = head.LastPozY;
        }
        public void MoveTo(Tail tail)
        {
            int bY = this.TailposY;
            int aY = tail.TailposY;
            int bX = this.TailposX;
            int aX = tail.TailposX;
            if (bY - aY == -2 && bX == aX)   // chasing tail is below
            {
                this.TailposY += 1;
                return;
            }
            if (bX - aX == 2 && bY == aY)    // chasing tail is to the right 
            {
                this.TailposX -= 1;
                return;
            }
            if (bX - aX == -2 && bY == aY)
            {
                this.TailposX += 1;
                return;
            }
            if (bY - aY == 2 && bX == aX)
            {
                this.TailposY -= 1;
                return;
            }
            // DIAGONALS
            if (bY - aY == -2 && bX > aX)
            {
                this.TailposY += 1;
                this.TailposX -= 1;
                return;
            }
            if (bY - aY == -2 && bX < aX)
            {
                this.TailposY += 1;
                this.TailposX += 1;
                return;
            }

            if (bX - aX == 2 && bY > aY)
            {
                this.TailposX -= 1;
                this.TailposY -= 1;
                return;
            }
            if (bX - aX == 2 && bY < aY)
            {
                this.TailposX -= 1;
                this.TailposY += 1;
                return;
            }
            
            if (bX - aX == -2 && bY > aY)
            {
                this.TailposX += 1;
                this.TailposY -= 1;
                return;
            }
            if (bX - aX == -2 && bY < aY)
            {
                this.TailposX += 1;
                this.TailposY += 1;
                return;
            }


            if (bY - aY == 2 && bX > aX)
            {
                this.TailposY -= 1;
                this.TailposX -= 1;
                return;
            }
            if (bY - aY == 2 && bX < aX)
            {
                this.TailposY -= 1;
                this.TailposX += 1;
                return;
            }
            
        }
        public bool Check(Tail tail)
        {
            if (this.TailposY == tail.TailposY && this.TailposX == tail.TailposX)
            {
                return true;
            }
            else return false;
        }
    }
}
