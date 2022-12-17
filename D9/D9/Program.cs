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
            for(int i = 0; i < thisHead.tails.Count - 1; i++)
            {
                for(int j = i + 1; j < thisHead.tails.Count; j++)
                {
                    while (thisHead.tails[i].Check(thisHead.tails[j]))
                    {
                        thisHead.tails.Remove(thisHead.tails[i]);
                        j = i + 1;
                    }
                }
            }
            sum = thisHead.tails.Count;
            Console.WriteLine(sum);
        }
    }
    class Head
    {
        public int posX { get; set; }
        public int posY { get; set; }
        public int LastPozX { get; set; }
        public int LastPozY { get; set; }
        public Tail myTail;
        public List<Tail> tails;
        public Head()
        {
            tails = new List<Tail>();
            myTail = new Tail(this);
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
                TailCheck();
                tails.Add(new Tail (myTail.TailposX,myTail.TailposY));
                times--;
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
