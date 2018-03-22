using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.Distributions;
using MathNet.Numerics;
using System.IO;



namespace ConsoleApplication2
{ 
    class Program
    {
        static void Main(string[] args)
        {
            var rnd = new Random();
            var ageDist = new Dictionary<int, float>();
            /*
            using (StreamReader rdr = File.OpenText(@"C:\Users\MatthewCordell\Documents\Visual Studio 2015\Projects\Content-For-Promotion-Extractor\ConsoleApplication2\InitialAgeDistribution.txt"))
            {
                int a = 0;
                string s;

                while (!rdr.EndOfStream)
                {
                    s = rdr.ReadLine();
                    ageDist.Add(a, float.Parse(s));
                    a++;
                }
                Console.WriteLine("read lines =" + a.ToString());
                
            }

        int startYear = 2000;
        int m;
        int d;
        int y;
        int age;
            float dist;

        for (int i = 0; i < 20; i++)
        {
                ageDist.TryGetValue(i, out dist);

                Console.WriteLine(dist);
                y = startYear - i;
            m = rnd.Next(1, 13);
            d = rnd.Next(1, DateTime.DaysInMonth(y, m));

            DateTime dob = new DateTime(y, m, d);
            Console.WriteLine(dob.Date.ToString("d"));
        }
        */
            var t = new System.Diagnostics.Stopwatch();
            
            using (TextWriter wrtr = new StreamWriter("sexes.txt"))
            {
                int num = 100;
                t.Start();
                for (int i = 0; i < num; i++)
                {
                    wrtr.WriteLine(Binomial.Sample(rnd, 0.75, 200).ToString());
                }
                t.Stop();
                Console.WriteLine("Wrote {0} lines in {1} seconds", num, t.Elapsed.Seconds);
            }

            

            /*
            using (var wrtr = new StreamWriter("tempdist.txt"))
            {                
                for (int i = 0; i < 10000; i++)
                {
                    wrtr.WriteLine(Math.Round(x.Sample()));
                    

                }

            }
            */


            Console.WriteLine("Done");
            Console.ReadKey();
        }
    }
}
