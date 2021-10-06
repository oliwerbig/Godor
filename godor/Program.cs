using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace tarsalgo
{
    class Program
    {
        static List<int> Depths { get; set; } = new List<int>();
        static SortedDictionary<int, int> Pits { get; set; } = new SortedDictionary<int, int>();

        static void Main(string[] args)
        {
            // 1.
            ProcessFile(@"melyseg.txt");
            Console.WriteLine("1. feladat");
            Console.Write("A fájl adatainak száma: ");
            Console.WriteLine(Depths.Count);

            // 2.
            Console.WriteLine("");
            Console.WriteLine("2. feladat"); 
            Console.Write("Adjon meg egy távolságértéket! ");
            int input = Int32.Parse(Console.ReadLine());
            Console.WriteLine("Ezen a helyen a felszín {0} méter mélyen van. ", Depths[input]);

            // 3.
            Console.WriteLine("");
            Console.WriteLine("3. feladat");
            Console.Write("Az érintetlen terület aránya {0}%. ");
        }

        private static void ProcessFile(string path)
        {
            string[] lines = File.ReadAllLines(path);

            for (int i = 0; i < lines.Length; i++)
            {
                int depth = Int32.Parse(lines[i]);
                if (Depths.Count != 0 && Depths.Last() == 0 && depth != 1)
                {
                    Pits.Add(i, depth);
                }
                Depths.Add(depth);
            }
        }

        private static double CalculateRateOfNonContaminated()
        {
            int numOfContaminated = Depths.Count(depth => depth != 0);
            int numOfNonContaminated = Depths.Count(depth => depth == 0);

            double rateOfNonContaminated = numOfNonContaminated / numOfContaminated * 100;

            return numOfContaminated 
        }
    }
}