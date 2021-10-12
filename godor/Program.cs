using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace godor
{
    class Program
    {
        static SortedDictionary<int, Measurement> Measurements { get; set; } = new();
        static List<Pit> Pits { get; set; } = new();

        static void Main(string[] args)
        {
            // 1.
            ProcessFile(@"melyseg.txt");
            Console.WriteLine("1. feladat");
            Console.Write("A fájl adatainak száma: ");
            Console.WriteLine(Measurements.Count);

            // 2.
            Console.WriteLine("");
            Console.WriteLine("2. feladat");
            Console.Write("Adjon meg egy távolságértéket! ");
            int input = int.Parse(Console.ReadLine()) - 1;
            Console.WriteLine("Ezen a helyen a felszín {0} méter mélyen van. ", Measurements[input].Depth);

            // 3.
            Console.WriteLine("");
            Console.WriteLine("3. feladat");
            Console.Write("Az érintetlen terület aránya {0}%. ", string.Format("{0:0.00}", CalculateRateOfNonContaminated()));
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
            // a,
            Console.WriteLine("a,");
            Pit pitOfInput = Measurements[input].Pit;
            if (pitOfInput == null)
            {
                Console.WriteLine("Az adott helyen nincs gödör.");
            }
            else
            {
                Console.WriteLine("A gödör kezdete: {0} méter, a gödör vége: {1} méter", pitOfInput.Measurements.First().Key + 1, pitOfInput.Measurements.Last().Key + 1);
                // b,
                Console.WriteLine("b,");
                    if (CheckIfDescendingContinously(pitOfInput))
                    {
                        Console.WriteLine("Folyamatosan mélyül.");
                    }
                    else
                    {
                        Console.WriteLine("Nem mélyül folyamatosan.");
                    }
                // c,
                Console.WriteLine("c,");
                Console.WriteLine("A legnagyobb mélysége {0} méter", FindDeepestMeasurement(pitOfInput.Measurements.Values.ToList()).Depth);
                // d,
                Console.WriteLine("d,");
                Console.WriteLine("A térfogata {0} m^3", CalculateVolumeOfPit(pitOfInput));
                // e,
                Console.WriteLine("e,");
                Console.WriteLine("A térfogata {0} m^3", CalculateVolumeOfWaterInPit(pitOfInput));
            }
        }

        private static void ProcessFile(string path)
        {
            string[] lines = File.ReadAllLines(path);

            for (int i = 0; i < lines.Length; i++)
            {
                Measurement measurementFromLine = new(i, int.Parse(lines[i]));
                Measurement previousMeasurement = new();
                if (Measurements.Count != 0)
                {
                    previousMeasurement = Measurements.Last().Value;
                }

                if (previousMeasurement.Depth == 0 && measurementFromLine.Depth != 0)
                {
                    Pits.Add(new Pit());
                } 
                
                if (measurementFromLine.Depth != 0)
                {
                    Pits.Last().Measurements.Add(i, measurementFromLine);
                    measurementFromLine.Pit = Pits.Last();
                }
                
                Measurements.Add(i, measurementFromLine);
            }
        }

        private static double CalculateRateOfNonContaminated()
        {
            int numOfContaminated = Measurements.Count(measurement => measurement.Value.Depth != 0);
            int numOfNonContaminated = Measurements.Count(measurement => measurement.Value.Depth == 0);

            double rateOfNonContaminated = Convert.ToDouble(numOfNonContaminated) / Convert.ToDouble(numOfContaminated) * 100;

            return rateOfNonContaminated;
        }

        public static async void WriteDepthsOfPitsToFile(string path)
        {
            List<string> lines = new();

            foreach (Pit pit in Pits)
            {
                string line = "";
                foreach (KeyValuePair<int, Measurement> measurement in Measurements)
                {
                    line += measurement.Value.Depth + " ";
                }
                lines.Add(line);
            }

            await File.WriteAllLinesAsync(path, lines.ToArray());
        }

        public static bool CheckIfDescendingContinously(Pit pit)
        {
            bool doesItDescendContinously = true;
            Measurement deepestMeasurement = FindDeepestMeasurement(pit.Measurements.Values.ToList());
            
            Measurement previousMeasurement = null;
            foreach (Measurement measurement in pit.Measurements.Values)
            {
                if (previousMeasurement == null)
                {
                    previousMeasurement = measurement;
                } 
                else
                {
                    if (measurement.Location < deepestMeasurement.Location)
                    {
                        if (measurement.Depth !< previousMeasurement.Depth)
                        {
                            doesItDescendContinously = false;
                            return doesItDescendContinously;
                        }
                    }
                    else if (measurement.Location > deepestMeasurement.Location)
                    {
                        if (measurement.Depth !> previousMeasurement.Depth)
                        {
                            doesItDescendContinously = false;
                            return doesItDescendContinously;
                        }
                    }

                    previousMeasurement = measurement;
                }
            }
            return doesItDescendContinously;
        }

        public static Measurement FindDeepestMeasurement(List<Measurement> measurements)
        {
            Measurement deepestMeasurement = null;
            foreach (Measurement measurement in measurements)
            {
                if (deepestMeasurement == null)
                {
                    deepestMeasurement = measurement;
                }
                else
                {
                    if (measurement.Depth < deepestMeasurement.Depth)
                    {
                        deepestMeasurement = measurement;
                    }
                }
            }

            return deepestMeasurement;
        }

        public static int CalculateVolumeOfPit(Pit pit)
        {
            int totalVolumeOfPit = 0;
            List<int> volumesPerMeasurement = new();

            foreach (Measurement measurement in pit.Measurements.Values)
            {
                volumesPerMeasurement.Add(1 * 10 * measurement.Depth);
            }

            foreach (int volume in volumesPerMeasurement)
            {
                totalVolumeOfPit += volume;
            }

            return totalVolumeOfPit;
        }

        public static int CalculateVolumeOfWaterInPit(Pit pit)
        {
            int totalVolumeOfWater = 0;
            List<int> volumesPerMeasurement = new();

            foreach (Measurement measurement in pit.Measurements.Values)
            {
                volumesPerMeasurement.Add(1 * 10 * (measurement.Depth - 1));
            }

            foreach (int volume in volumesPerMeasurement)
            {
                totalVolumeOfWater += volume;
            }

            return totalVolumeOfWater;
        }
    }
}