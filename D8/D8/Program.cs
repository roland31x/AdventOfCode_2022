using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D8
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<List<Tree>> list = new List<List<Tree>>();

            using (StreamReader sr = new StreamReader("input.txt"))
            {
                int i = 0;
                while (!sr.EndOfStream)
                {
                    List<Tree> treeList = new List<Tree>();
                    
                    string s = sr.ReadLine();
                    char[] chars = s.ToCharArray();
                    for (int j = 0; j < chars.Length; j++)
                    {
                        treeList.Add(new Tree(i, j, chars[j] - '0'));
                    }
                    list.Add(treeList);
                    i++;
                }
            }
            //int count = 0;
            int score = 0;
            foreach(List<Tree> treegrid in list) 
            {
                foreach(Tree tree in treegrid)
                {
                    tree.VisibilityCheck(list);
                    //if (tree.IsVisible)  // PART 1
                    //{
                    //    count++;
                    //}
                        int helper = tree.ScenicScore(list);
                        if(helper > score)
                        {
                            score = helper;
                        }
                }
            }
            Console.WriteLine(score);
        }
    }
    class Tree
    {
        public bool IsVisible { get; set; }
        public int RowPos { get; set; }
        public int ColumnPos { get; set; }
        public int Height { get; set; }

        public Tree(int i, int j, int HeightVal) 
        {
            Height = HeightVal;
            RowPos= i;
            ColumnPos= j;      
        }
        public void VisibilityCheck(List<List<Tree>> grid)
        {
            Tree tree = this;
            if (tree.RowPos == 0 || tree.ColumnPos == 0 || tree.ColumnPos == grid[tree.ColumnPos].Count - 1 || tree.RowPos == grid.Count - 1)
            {
                IsVisible = true;
                return;
            }
            if (tree.FromTop(grid))
            {
                IsVisible = true; return; 
            }
            if (tree.FromLeft(grid))
            {
                IsVisible = true; return;
            }
            if (tree.FromRight(grid))
            {
                IsVisible = true; return;
            }
            if (tree.FromBottom(grid))
            {
                IsVisible = true; return;
            }

        }
        public int ScenicScore(List<List<Tree>> grid)
        {
            Tree tree = this;
            if (tree.RowPos == 0 || tree.ColumnPos == 0 || tree.ColumnPos == grid[tree.ColumnPos].Count - 1 || tree.RowPos == grid.Count - 1)
            {
                return 0;
            }
            int Top = ScoreTop(grid);
            int Bot = ScoreBot(grid);
            int Left = ScoreLeft(grid);
            int Right = ScoreRight(grid);
            int score = Top * Bot * Left * Right;
            return score;
        }

        private int ScoreTop(List<List<Tree>> grid)
        {
            Tree tree = this;
            int score = 0;
            for (int i = tree.RowPos - 1; i >= 0; i--)
            {
                if (grid[i][tree.ColumnPos].Height >= tree.Height)
                {
                    score++;
                    return score;
                }
                score++;
            }
            return score;
        }
        private int ScoreBot(List<List<Tree>> grid)
        {
            Tree tree = this;
            int score = 0;
            for (int i = tree.RowPos + 1; i < grid.Count; i++)
            {
                if (grid[i][tree.ColumnPos].Height >= tree.Height)
                {
                    score++;
                    return score;
                }
                score++;
            }
            return score;
        }
        private int ScoreLeft(List<List<Tree>> grid)
        {
            Tree tree = this;
            int score = 0;
            for (int i = tree.ColumnPos - 1; i >= 0; i--)
            {
                if (grid[tree.RowPos][i].Height >= tree.Height)
                {
                    score++;
                    return score;
                }
                score++;
            }
            return score;
        }
        private int ScoreRight(List<List<Tree>> grid)
        {
            Tree tree = this;
            int score = 0;
            for (int i = tree.ColumnPos + 1; i < grid[tree.RowPos].Count; i++)
            {
                if (grid[tree.RowPos][i].Height >= tree.Height)
                {
                    score++;
                    return score;
                }
                score++;
            }
            return score;
        }

        private bool FromBottom(List<List<Tree>> grid)
        {
            Tree tree = this;
            int Col = tree.ColumnPos;
            for (int i = grid.Count - 1; i > tree.RowPos; i--)
            {
                if (grid[i][Col].Height >= tree.Height)
                {
                    return false;
                }
            }
            return true;
        }

        private bool FromRight(List<List<Tree>> grid)
        {
            Tree tree = this;
            int Row = tree.RowPos;
            for (int i = grid[Row].Count - 1; i > tree.ColumnPos; i--)
            {
                if (grid[Row][i].Height >= tree.Height)
                {
                    return false;
                }
            }
            return true;
        }

        private bool FromLeft(List<List<Tree>> grid)
        {
            Tree tree = this;
            int Row = tree.RowPos;
            for (int i = 0; i < tree.ColumnPos; i++)
            {
                if (grid[Row][i].Height >= tree.Height)
                {
                    return false;
                }
            }
            return true;
        }

        private bool FromTop(List<List<Tree>> grid)
        {
            Tree tree = this;
            int Col = tree.ColumnPos;
            for(int i = 0; i < tree.RowPos; i++)
            {
                if (grid[i][Col].Height >= tree.Height)
                {
                    return false;
                }
            }
            return true;
        }
    }

}
