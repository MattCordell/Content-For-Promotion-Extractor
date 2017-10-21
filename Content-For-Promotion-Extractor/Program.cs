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

            var ExtractTargets = r.ReadListOfIds(file);  // flat list of IDs for cherry picking
            var Localconcepts = r.ReadConceptFile(file); //Local concpets (already available)

            //Check relationship file for any dependencies (stated+full?)
            ExtractTargets = r.IdentifyDependencies(ExtractTargets, file);
            
            var concepts = r.ReadConceptFile(file);
            var descriptions = r.ReadDescriptionFile(file);
            var statedRelationships = r.ReadRelationshipFile(file);
            var relationships = r.ReadRelationshipFile(file);
            
            //some lambda/linq to remove unwanted content 

            //Implement an RF2 writer also.
            //Write the above lists out as RF2 files

            Console.WriteLine("Done");
            Console.ReadKey();

        }
    }
}
