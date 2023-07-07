namespace D20
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Node? first = null;
            Node? current = null;

            Node? firstP1 = null;
            Node? currentP1 = null;
            int len = 0;
            using (StreamReader sr = new StreamReader(@"..\..\..\input.txt"))
            {
                while (!sr.EndOfStream)
                {
                    long val = long.Parse(sr.ReadLine()!);
                    Node toadd = new Node(val * (long)811589153, len);

                    Node toaddP1 = new Node(val, len);

                    if (first == null)
                    {
                        first = toadd;
                        current = toadd;

                        firstP1 = toaddP1;
                        currentP1 = toaddP1;
                    }
                    current!.next = toadd;
                    toadd.prev = current;
                    current = toadd;
                    
                    currentP1!.next = toaddP1;
                    toaddP1.prev = currentP1;
                    currentP1 = toaddP1;

                    len++;
                }
                first!.prev = current;
                current!.next = first;

                firstP1!.prev = currentP1;
                currentP1!.next = firstP1;
            }
            Part1(currentP1, len);
            Part2(current, len);            
        }
        public static void Part1(Node current, int len)
        {
            for (int i = 0; i < len; i++)
            {
                while (current!.InitialPos != i)
                {
                    current = current.next;
                }
                int dir = 0;
                if (current.Value < 0)
                {
                    dir = 1;
                }

                long times = Math.Abs(current.Value % (len - 1));

                for (long j = 0; j < times; j++)
                {
                    if (dir == 0)
                    {
                        (current.next.Value, current.Value) = (current.Value, current.next.Value);
                        (current.next.InitialPos, current.InitialPos) = (current.InitialPos, current.next.InitialPos);
                        current = current.next;
                    }
                    else
                    {
                        (current.prev.Value, current.Value) = (current.Value, current.prev.Value);
                        (current.prev.InitialPos, current.InitialPos) = (current.InitialPos, current.prev.InitialPos);
                        current = current.prev;
                    }
                }

            }
            while (current.Value != 0)
            {
                current = current.next;
            }

            long sumP1 = 0;
            for (int i = 1; i <= 3000; i++)
            {
                current = current.next;
                if (i % 1000 == 0)
                {
                    sumP1 += current.Value;
                }
            }
            Console.WriteLine("Part 1 solution:");
            Console.WriteLine(sumP1);
        }
        public static void Part2(Node current, int len)
        {
            long sum = 0;
            for (int round = 1; round <= 10; round++)
            {
                for (int i = 0; i < len; i++)
                {
                    while (current!.InitialPos != i)
                    {
                        current = current.next;
                    }
                    int dir = 0;
                    if (current.Value < 0)
                    {
                        dir = 1;
                    }

                    long times = Math.Abs(current.Value % (len - 1));

                    for (long j = 0; j < times; j++)
                    {
                        if (dir == 0)
                        {
                            (current.next.Value, current.Value) = (current.Value, current.next.Value);
                            (current.next.InitialPos, current.InitialPos) = (current.InitialPos, current.next.InitialPos);
                            current = current.next;
                        }
                        else
                        {
                            (current.prev.Value, current.Value) = (current.Value, current.prev.Value);
                            (current.prev.InitialPos, current.InitialPos) = (current.InitialPos, current.prev.InitialPos);
                            current = current.prev;
                        }
                    }
                }
            }
            while (current.Value != 0)
            {
                current = current.next;
            }
            for (int i = 1; i <= 3000; i++)
            {
                current = current.next;
                if (i % 1000 == 0)
                {
                    sum += current.Value;
                }
            }
            Console.WriteLine("Part 2 solution: ");
            Console.WriteLine(sum);
        }
    }
    public class Node
    {
        public long Value { get; set; }
        public int InitialPos { get; set; }
        public int CurrentPos { get; set; }
        public bool wasMarked = false;
        public Node? prev;
        public Node? next;
        public Node(long value, int initialpos) 
        {
            Value = value;
            InitialPos = initialpos;
            CurrentPos = initialpos;
        }
        public override string ToString()
        {
            return Value.ToString();
        }
    }
}