using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace godor
{
    class Program
    {
        static List<int> Depths { get; set; } = new();
        static List<Pit> Pits { get; set; } = new();

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
            Console.Write("Az érintetlen terület aránya {0}%. ", String.Format("{0:0.00}", CalculateRateOfNonContaminated()));
            Console.WriteLine("");

            // 4.
            WriteDepthsOfPitsToFile(@"godrok.txt");

            // 5.
            Console.WriteLine("");
            Console.WriteLine("5. feladat");
            Console.WriteLine("A gödrök száma: {0}", Pits.Count);

            // 6.
            Console.WriteLine("");
            Console.WriteLine("6. feladat");
            Console.WriteLine("a,");
            if (input != 0)
            {
                Pit pitOfInput = new();
                foreach (Pit pit in Pits)
                {
                    if (pit.Depths.ContainsKey(input)) {
                        pitOfInput = pit;
                    }
                }
                Console.WriteLine("A gödör kezdete: {0} méter, a gödör vége: {1} méter", pitOfInput.Depths.First().Key, pitOfInput.Depths.Last().Key);
            } else
            {
                Console.WriteLine("Az adott helyen nincs gödör.");
            }
        }

        private static void ProcessFile(string path)
        {
            string[] lines = File.ReadAllLines(path);

            for (int i = 0; i < lines.Length; i++)
            {
                int depthFromLine = Int32.Parse(lines[i]);
                int previousDepth = 0;
                if (Depths.Count != 0)
                {
                    previousDepth = Depths.Last();
                }

                Depths.Add(depthFromLine);

                if (previousDepth == 0 && depthFromLine != 0)
                {
                    Pits.Add(new Pit());
                    Pits.Last().Depths.Add(i, depthFromLine);
                } else if (previousDepth != 0)
                {
                    Pits.Last().Depths.Add(i, depthFromLine);
                } 

                
            }
        }

        private static double CalculateRateOfNonContaminated()
        {
            int numOfContaminated = Depths.Count(depth => depth != 0);
            int numOfNonContaminated = Depths.Count(depth => depth == 0);

            double rateOfNonContaminated = Convert.ToDouble(numOfNonContaminated) / Convert.ToDouble(numOfContaminated) * 100;

            return rateOfNonContaminated;
        }

        public static async void WriteDepthsOfPitsToFile(string path)
        {
            List<string> lines = new();

            foreach (Pit pit in Pits)
            {
                string line = "";
                foreach (KeyValuePair<int, int> depth in pit.Depths)
                {
                    line += depth.Value + " ";
                }
                lines.Add(line);
            }

            await File.WriteAllLinesAsync(path, lines.ToArray());
        }
    }
}