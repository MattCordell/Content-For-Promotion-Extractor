using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Compression;
using System.IO;

namespace Content_For_Promotion_Extractor
{
    // XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
    //This Progam is a scratchpad until all the classes are written out...
    // XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX

    enum ComponentType { Concept, Description, Relationship };
    
    
    
    class Program
    {
        static void Main(string[] args)
        {
         string promotionModule = "10101010";

        //temp strings for development. Pull these via arg            
        /*
        string conceptsForPromotionFile = @"C:\Users\MatthewCordell\Documents\Visual Studio 2015\Projects\promocodes.txt";
        string localConceptsFile = @"C:\Extension RoundUp Jan 2017\Australia\NCTS_SCT_RF2_DISTRIBUTION_32506021000036107-20161231-ALL\SnomedCT_Release_AU1000036_20161231\RF2Release\Snapshot\AU_Terminology\sct2_Concept_Snapshot_AU1000036_20161231.txt";

        string donorConceptFile = @"C:\Extension RoundUp Jan 2017\United States\SnomedCT_USEditionRF2_Production_20170301T120000\SnomedCT_USEditionRF2_Production_20170301T120000\Snapshot\Terminology\sct2_Concept_Snapshot_US1000124_20170301.txt";
        string donorDescriptionFile = @"C:\Extension RoundUp Jan 2017\United States\SnomedCT_USEditionRF2_Production_20170301T120000\SnomedCT_USEditionRF2_Production_20170301T120000\Snapshot\Terminology\sct2_Description_Snapshot-en_US1000124_20170301.txt";
        string donorStatedFile = @"C:\Extension RoundUp Jan 2017\United States\SnomedCT_USEditionRF2_Production_20170301T120000\SnomedCT_USEditionRF2_Production_20170301T120000\Snapshot\Terminology\sct2_StatedRelationship_Snapshot_US1000124_20170301.txt";
        string donorInferredFile = @"C:\Extension RoundUp Jan 2017\United States\SnomedCT_USEditionRF2_Production_20170301T120000\SnomedCT_USEditionRF2_Production_20170301T120000\Snapshot\Terminology\sct2_Relationship_Snapshot_US1000124_20170301.txt";            
        */
        Console.WriteLine("Reading Zip");

            string donorZip = @"C:\Users\MatthewCordell\Documents\Visual Studio 2015\Projects\Content-For-Promotion-Extractor\Content-For-Promotion-ExtractorTests\TestData\TestDataRelease.zip";

            var X = new Unpacker();

            string donorConceptFile = X.Unpack(donorZip, RF2File.sct2_Concept_Snapshot);
            string donorDescriptionFile = X.Unpack(donorZip, RF2File.sct2_Description_Snapshot);
            string donorStatedFile = X.Unpack(donorZip, RF2File.sct2_StatedRelationship_Snapshot);
            string donorInferredFile = X.Unpack(donorZip, RF2File.sct2_Relationship_Snapshot);



            Console.WriteLine("Done with zip");

            string conceptsForPromotionFile = @"C:\Users\MatthewCordell\Documents\Visual Studio 2015\Projects\Content-For-Promotion-Extractor\Content-For-Promotion-ExtractorTests\TestData\TargetConceptToBeExtracted.txt";
            string localConceptsFile = @"C:\Users\MatthewCordell\Documents\Visual Studio 2015\Projects\Content-For-Promotion-Extractor\Content-For-Promotion-ExtractorTests\TestData\sct2_Concept_Snapshot_20170401.txt";
            /*
            string donorConceptFile = @"C:\Users\MatthewCordell\Documents\Visual Studio 2015\Projects\Content-For-Promotion-Extractor\Content-For-Promotion-ExtractorTests\TestData\RF2Release\Snapshot\Terminology\sct2_Concept_Snapshot_20171130.txt";
            string donorDescriptionFile = @"C:\Users\MatthewCordell\Documents\Visual Studio 2015\Projects\Content-For-Promotion-Extractor\Content-For-Promotion-ExtractorTests\TestData\RF2Release\Snapshot\Terminology\sct2_Description_Snapshot_20171130.txt";
            string donorStatedFile = @"C:\Users\MatthewCordell\Documents\Visual Studio 2015\Projects\Content-For-Promotion-Extractor\Content-For-Promotion-ExtractorTests\TestData\RF2Release\Snapshot\Terminology\sct2_StatedRelationship_Snapshot_20171130.txt";
            string donorInferredFile = @"C:\Users\MatthewCordell\Documents\Visual Studio 2015\Projects\Content-For-Promotion-Extractor\Content-For-Promotion-ExtractorTests\TestData\RF2Release\Snapshot\Terminology\sct2_Relationship_Snapshot_20171130.txt";
            */
            RF2Reader r = new RF2Reader();
            r.ConceptsPath = donorConceptFile;
            r.DescriptionsPath = donorDescriptionFile;
            r.StatedRelsPath = donorStatedFile;
            r.RelationshipsPath = donorInferredFile;

            Console.WriteLine("Importing List of Target Concepts");
            var ExtractTargets = r.ReadListOfIds(conceptsForPromotionFile);  // flat list of IDs for cherry picking
            Console.WriteLine("Target Concepts = " + ExtractTargets.Count());


            Console.WriteLine("Identifying Dependencies");
            // Check this dependency logic... looks messy
            var Localconcepts = r.ReadConceptFile(localConceptsFile, true, false); //Local concpets (already available)
            var statedRelationships = r.ReadRelationshipFile(donorStatedFile);
            var relationships = r.ReadRelationshipFile(donorInferredFile);
            //Check relationship file for any dependencies (stated+full)
            var dependencies = r.IdentifyAllDependencies(ExtractTargets, Localconcepts, statedRelationships, relationships);
            //Add any dependencies to extract list
            ExtractTargets = (ExtractTargets.Union(dependencies)).Distinct().ToList();
            Console.WriteLine("Total Target Concepts = " + ExtractTargets.Count());


            Console.WriteLine("Extracting Target Data from Snapshot");         
            //Identify all the entries from the concepts,descriptions & both relationships for the complete exrtact targets.
            List<Concept> ExtractedConcepts = r.ExtractConcepts(ExtractTargets);
            List<Description> ExtractedDescriptions = r.ExtractDescriptions(ExtractTargets);
            List<Relationship> ExtractedStated = r.ExtractRelationships(ExtractTargets);
            List<Relationship> ExtractedRelationships = r.ExtractRelationships(ExtractTargets);
            Console.WriteLine("Concepts extracted = " + ExtractedConcepts.Count().ToString());
            Console.WriteLine("Descriptions extracted = " + ExtractedDescriptions.Count().ToString());
            Console.WriteLine("Stated extracted = " + ExtractedStated.Count().ToString());
            Console.WriteLine("Inferred extracted = " + ExtractedRelationships.Count().ToString());

            //Implement an RF2 writer also.
            //Write the above lists out as RF2 files/extension bundle

            Console.WriteLine("Looking for dependencies");
            var deps = r.GetDestinationIdDependencies(ExtractTargets, Localconcepts, statedRelationships);

            Console.WriteLine(deps.Count().ToString() + " dependencies found");

            Console.WriteLine("Creating RF2 Bundle");
            RF2Writer w = new RF2Writer();

            //module update should happen in dedicated class.
            w.CreateRf2File(ExtractedConcepts, promotionModule);
            w.CreateRf2File(ExtractedDescriptions, promotionModule);
            w.CreateRf2File(ExtractedStated, RelationshipType.stated);
            w.CreateRf2File(ExtractedRelationships, RelationshipType.inferred);

            Console.WriteLine("Done");
            Console.ReadKey();

        }
    }
}
