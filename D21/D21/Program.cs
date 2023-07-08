using System.Numerics;

namespace D21
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Monkey root = new Monkey("useless", "0");
            Monkey human = new Monkey("useless2", "0");
            using(StreamReader sr = new StreamReader(@"..\..\..\input.txt"))
            {
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine()!;
                    string[] tokens = line.Split(":");
                    Monkey monke = new Monkey(tokens[0], tokens[1].TrimStart());
                    if (monke.Name == "root")
                        root = monke;
                    if (monke.Name == "humn")
                        human = monke;
                }
            }
            Console.WriteLine("Part 1 solution:");
            Console.WriteLine(root.Yell());
            Console.WriteLine("Part 2 solution:");
            Console.WriteLine(Monkey.ComputeRootEquality(root,human));
        }
    }
    public class UnknownVal
    {
        public decimal KnownValue = 0;
        public decimal Unknown = 0;
        public UnknownVal(decimal Kvalue, decimal Uvalue)
        {
            KnownValue = Kvalue;
            Unknown = Uvalue;
        }
        public static UnknownVal operator + (UnknownVal left, UnknownVal right)
        {
            UnknownVal toret = new UnknownVal(0, 0);
            toret.KnownValue = left.KnownValue + right.KnownValue;
            toret.Unknown = left.Unknown + right.Unknown;

            return toret;
        }
        public static UnknownVal operator - (UnknownVal left, UnknownVal right)
        {
            UnknownVal toret = new UnknownVal(0, 0);
            toret.KnownValue = left.KnownValue - right.KnownValue;
            toret.Unknown = left.Unknown - right.Unknown;
            return toret;
        }

        public static UnknownVal operator * (UnknownVal left, UnknownVal right)
        {
            UnknownVal toret = new UnknownVal(0, 0);
            if(left.Unknown == 0)
            {
                toret.KnownValue = left.KnownValue * right.KnownValue;
                toret.Unknown = left.KnownValue * right.Unknown;
            }
            else
            {
                toret.KnownValue = left.KnownValue * right.KnownValue;
                toret.Unknown = right.KnownValue * left.Unknown;
            }
            return toret;
        }
        public static UnknownVal operator / (UnknownVal left, UnknownVal right)
        {
            UnknownVal toret = new UnknownVal(0, 0);
            if (left.Unknown == 0)
            {
                if(right.Unknown != 0)
                {
                    Console.WriteLine("fuck");
                    toret.Unknown = right.Unknown;
                }
                toret.KnownValue = left.KnownValue / right.KnownValue;
                
            }
            else
            {
                toret.KnownValue = left.KnownValue / right.KnownValue;
                toret.Unknown = left.Unknown / right.KnownValue;
            }
            return toret;
        }
        public static bool TryParse(string number, out UnknownVal u)
        {           
            if (int.TryParse(number, out int res))
            {
                u = new UnknownVal(res, 0);
                return true;
            }
            else
            {
                u = new UnknownVal(0, 1);
                return false;
            }         
        }
        public override string ToString()
        {
            return $"{KnownValue} + {Unknown} * X";
        }
    }
    public class Monkey
    {
        public static List<Monkey> Monkeys = new List<Monkey>();
        public static decimal ComputeRootEquality(Monkey root, Monkey human)
        {
            string[] rootop = root.Operation.Split(" ");
            human.Operation = "failed";
            UnknownVal left = FindMonkey(rootop[0]).Yell2();
            UnknownVal right = FindMonkey(rootop[2]).Yell2();
            decimal result = 0;
            if (left.Unknown == 0)
            {
                result = (left.KnownValue - right.KnownValue ) / right.Unknown;
            }
            else
            {
                result = (right.KnownValue - left.KnownValue) / left.Unknown;
            }



            return result;
        }
        public static Monkey FindMonkey(string name)
        {
            foreach (Monkey m in Monkeys)
            {
                if (m.Name == name)
                    return m;
            }

            throw new InvalidDataException();
        }
        public string Name { get; }
        public string Operation { get; private set; }
        public Monkey(string name, string operation)
        {
            Operation = operation;
            Name = name;
            Monkeys.Add(this);
        }
        public UnknownVal Yell2()
        {
            if (UnknownVal.TryParse(Operation, out UnknownVal result))
                return result;
            else if(Name == "humn")
            {
                return new UnknownVal(0, 1);
            }
            else
            {
                string[] ops = Operation.Split(" ");
                string op = ops[1];
                switch (op)
                {
                    case "+":
                        return FindMonkey(ops[0]).Yell2() + FindMonkey(ops[2]).Yell2();
                    case "-":
                        return FindMonkey(ops[0]).Yell2() - FindMonkey(ops[2]).Yell2();
                    case "*":
                        return FindMonkey(ops[0]).Yell2() * FindMonkey(ops[2]).Yell2();
                    case "/":
                        return FindMonkey(ops[0]).Yell2() / FindMonkey(ops[2]).Yell2();
                    default:
                        throw new InvalidOperationException();
                }
            }
        }
        public long Yell()
        {
            if(long.TryParse(Operation, out long result))
                return result;
            else
            {
                string[] ops = Operation.Split(" ");
                string op = ops[1];
                switch (op)
                {
                    case "+":
                        return FindMonkey(ops[0]).Yell() + FindMonkey(ops[2]).Yell();
                    case "-":
                        return FindMonkey(ops[0]).Yell() - FindMonkey(ops[2]).Yell();
                    case "*":
                        return FindMonkey(ops[0]).Yell() * FindMonkey(ops[2]).Yell();
                    case "/":
                        return FindMonkey(ops[0]).Yell() / FindMonkey(ops[2]).Yell();
                    default:
                        throw new InvalidOperationException();
                }
            }
        }
    }
}