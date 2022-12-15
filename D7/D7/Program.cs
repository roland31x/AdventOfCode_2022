using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D7
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Folder MainFolder = new Folder("Main");
            CurrentFolder Current = new CurrentFolder(MainFolder);
            
            using (StreamReader sr = new StreamReader("input.txt"))
            {
                
                while (!sr.EndOfStream)
                {
                    string s = sr.ReadLine();
                    string[] tokens = s.Split(' ');
                    if (ulong.TryParse(tokens[0], out ulong Size))
                    {
                        Current.AddFile(Size);
                        continue;
                    }
                    if (tokens[0] == "dir")
                    {
                        Current.Add(tokens[1]);
                        continue;
                    }
                    if (tokens[0] == "$")
                    {
                        if (tokens[1] == "cd")
                        {
                            if (tokens[2] == "/")
                            {
                                continue;
                            }
                            if (tokens[2] == ".." )
                            {
                                Current.GoBack();
                                continue;
                            }
                            else 
                            { 
                                foreach(Folder f in Current.thisFolder.Folders)
                                {
                                    if (tokens[2] == f.Name)
                                    {
                                        Current.NavigateTo(f);
                                        continue;
                                    }
                                }
                            }
                        }
                    }
                }                            
            }

            /* PART 1
            List<Folder> OK = new List<Folder>();
            CheckBig(MainFolder, OK);
            ulong sum = 0;
            foreach(Folder Nice in OK)
            {
                sum += Nice.GetSize();
            }
            Console.WriteLine(sum);
            */ 

            ulong totalspace = 70000000;
            ulong usedspace = MainFolder.GetSize();
            ulong availablespace = totalspace - usedspace;
            ulong updatespace = 30000000;
            ulong spaceneeded = updatespace - availablespace;
            ulong offset = 100;

            List<Folder> OK = new List<Folder>();
            while(OK.Count == 0)
            {
                CheckBigP2(MainFolder, OK, spaceneeded, offset);
                offset *= 10;
            }
            foreach(Folder f in OK)
            {
                Console.WriteLine(f.GetSize());
            }
            
        }
        public static void CheckBigP2(Folder f, List<Folder> OK, ulong space, ulong offset)
        {
            foreach (Folder sub in f.Folders)
            {
                CheckBigP2(sub, OK, space, offset);
            }
            ulong size = f.GetSize();
            if (size >= space - offset && size <= space + offset)
            {
                OK.Add(f);
            }
        }
        public static void CheckBig(Folder f, List<Folder> OK)
        {
            foreach(Folder sub in f.Folders)
            {
                CheckBig(sub, OK);
            }
            ulong size = f.GetSize();
            if (size <= 100000)
            {
                OK.Add(f);
            }
        }
    }
    class CurrentFolder
    {
        public Stack<Folder> parentFolders = new Stack<Folder>();
        public Folder thisFolder { get; set; }

        public CurrentFolder(Folder F)
        {
            thisFolder = F;
        }
        public void NavigateTo(Folder F)
        {
            parentFolders.Push(thisFolder);
            thisFolder = F;
        }
        public void GoBack()
        {
            thisFolder = parentFolders.Pop();
        }
        public void AddFile(ulong size)
        {
            thisFolder.AddFile(size);
        }
        public void Add(string S)
        {
            thisFolder.Add(S);
        }

    }
    class Folder
    {
        public List<Folder> Folders = new List<Folder>();
        public List<ulong> Files = new List<ulong>();
        public string Name { get; set; }
        public Folder(string name) 
        {
            Name = name;
        }
        public void Add(string f)
        {
            Folders.Add(new Folder(f));
        }
        public void AddFile(ulong S)
        {
            Files.Add(S);
        }
        public ulong GetSize()
        {
            ulong Size = 0;
            foreach(Folder f in Folders)
            {
                Size += f.GetSize();
            }
            foreach(ulong i in Files)
            {
                Size += i;
            }
            return Size;
        }

    }

}
