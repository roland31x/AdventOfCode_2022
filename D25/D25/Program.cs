using System.Runtime.CompilerServices;
using System.Text;

namespace D25
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Snafu result = new Snafu("0");
            using (StreamReader sr = new StreamReader(@"..\..\..\input.txt"))
            {
                while (!sr.EndOfStream)
                {
                    Snafu prev = result;
                    string buf = sr.ReadLine()!;
                    result += new Snafu(buf);
                }
            }
            Console.WriteLine(result);
        }
    }
    public class Snafu
    {
        char[] number;
        int[] modulos;
        public Snafu(string number)
        {
            this.number = number.ToCharArray();
            modulos = new int[number.Length];
            for (int i = 0; i < modulos.Length; i++)
            {
                switch (number[modulos.Length - 1 - i])
                {
                    case '2':
                        modulos[i] = 2; break;
                    case '1':
                        modulos[i] = 1; break;
                    case '0':
                        modulos[i] = 0; break;
                    case '-':
                        modulos[i] = -1; break;
                    case '=':
                        modulos[i] = -2; break;

                }
            }
        }
        Snafu()
        {
            number = new char[64];
            modulos = new int[64];
        }
        public static Snafu operator + (Snafu a, Snafu b)
        {
            Snafu tor = new Snafu();
            int maxchar = 0;
            for(int i = 0; i < a.modulos.Length; i++)
            {
                tor.modulos[i] += a.modulos[i];
            }
            for (int i = 0; i < b.modulos.Length; i++)
            {
                tor.modulos[i] += b.modulos[i];
                if (tor.modulos[i] > 2)
                {
                    tor.modulos[i + 1] += 1;
                    tor.modulos[i] -= 5;
                }
                if (tor.modulos[i] < -2)
                {
                    tor.modulos[i + 1] -= 1;
                    tor.modulos[i] += 5;
                }
            }

            for(int i = 0; i < 64; i++)
            {
                if (tor.modulos[i] != 0)
                {
                    maxchar = i + 1;
                }                
            }

            int[] truenumber = new int[maxchar];
            for(int i = 0; i < maxchar; i++)
            {
                truenumber[i] = tor.modulos[i];
            }
            tor.modulos = truenumber;
            tor.number = new char[tor.modulos.Length];

            for (int i = 0; i < tor.modulos.Length; i++)
            {
                switch (tor.modulos[tor.modulos.Length - 1 - i])
                {
                    case -1:
                        tor.number[i] = '-'; break;
                    case -2:
                        tor.number[i] = '='; break;
                    case 2:
                        tor.number[i] = '2'; break;
                    case 1:
                        tor.number[i] = '1'; break;
                    case 0:
                        tor.number[i] = '0'; break;

                }
            }

            return tor;
        }
        public long ToLong()
        {
            long res = 0;
            for(int i = number.Length - 1; i >= 0; i--)
            {
                long multiplier = Pow(5, number.Length - 1 - i);
                int nr;
                if (number[i] == '=')
                {
                    nr = -2;
                }
                else if (number[i] == '-')
                {
                    nr = -1;
                }
                else
                {
                    nr = number[i] - '0';
                }
                res += nr * multiplier;
            }
            return res;
        }
        long Pow(long number, long pwr)
        {
            if (pwr == 0)
                return 1;
            else
                return number * Pow(number, pwr - 1);
        }
        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            for(int i = 0; i < number.Length; i++)
            {
                stringBuilder.Append(number[i]);
            }
            return stringBuilder.ToString();
        }
    }
}