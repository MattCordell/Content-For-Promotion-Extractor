using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Content_For_Promotion_Extractor
{
    class Program
    {
        static void Main(string[] args)
        {
            string file = @"C:\BP RF2Release\Delta\Terminology\sct2_Concept_Delta_AU1000032_20171006.txt";

            RF2Reader r = new RF2Reader();

            var concepts = r.ReadConceptFile(file);

            Console.WriteLine("Done");
            Console.ReadKey();

        }
    }
}
