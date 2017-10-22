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
            //temp strings for development. Pull these via arg            
            string conceptsForPromotionFile = @"C:\Users\MatthewCordell\Documents\Visual Studio 2015\Projects\promocodes.txt";
            string localConceptsFile = @"C:\Extension RoundUp Jan 2017\Australia\NCTS_SCT_RF2_DISTRIBUTION_32506021000036107-20161231-ALL\SnomedCT_Release_AU1000036_20161231\RF2Release\Snapshot\AU_Terminology\sct2_Concept_Snapshot_AU1000036_20161231.txt";

            string donorConceptFile = @"C:\Extension RoundUp Jan 2017\United States\SnomedCT_USEditionRF2_Production_20170301T120000\SnomedCT_USEditionRF2_Production_20170301T120000\Snapshot\Terminology\sct2_Concept_Snapshot_US1000124_20170301.txt";
            string donorDescriptionFile = @"C:\Extension RoundUp Jan 2017\United States\SnomedCT_USEditionRF2_Production_20170301T120000\SnomedCT_USEditionRF2_Production_20170301T120000\Snapshot\Terminology\sct2_Description_Snapshot-en_US1000124_20170301.txt";
            string donorStatedFile = @"C:\Extension RoundUp Jan 2017\United States\SnomedCT_USEditionRF2_Production_20170301T120000\SnomedCT_USEditionRF2_Production_20170301T120000\Snapshot\Terminology\sct2_StatedRelationship_Snapshot_US1000124_20170301.txt";
            string donorInferredFile = @"C:\Extension RoundUp Jan 2017\United States\SnomedCT_USEditionRF2_Production_20170301T120000\SnomedCT_USEditionRF2_Production_20170301T120000\Snapshot\Terminology\sct2_Relationship_Snapshot_US1000124_20170301.txt";            

            RF2Reader r = new RF2Reader();

            Console.WriteLine("Importing local stuff");
            var ExtractTargets = r.ReadListOfIds(conceptsForPromotionFile);  // flat list of IDs for cherry picking
            var Localconcepts = r.ReadConceptFile(localConceptsFile); //Local concpets (already available)

            Console.WriteLine("Importing Target Extension Snapshot");
            var concepts = r.ReadConceptFile(donorConceptFile);
            var descriptions = r.ReadDescriptionFile(donorDescriptionFile);
            var statedRelationships = r.ReadRelationshipFile(donorStatedFile);
            var relationships = r.ReadRelationshipFile(donorInferredFile);

            //Check relationship file for any dependencies (stated+full?)
            //ExtractTargets = r.IdentifyDependencies(ExtractTargets, file);

            //Identify all the descriptions and relationships for the Target Concepts.
   
            //Implement an RF2 writer also.
            //Write the above lists out as RF2 files/extension bundle


            Console.WriteLine("Done");
            Console.ReadKey();

        }
    }
}
