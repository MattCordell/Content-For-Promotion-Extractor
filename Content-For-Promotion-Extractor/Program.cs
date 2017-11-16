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
            // These variables are will be set by input arguments.
            string conceptsForPromotionFile = @"C:\Users\MatthewCordell\Documents\Visual Studio 2015\Projects\Content-For-Promotion-Extractor\Content-For-Promotion-ExtractorTests\TestData\TargetConceptToBeExtracted.txt";
            string donorZip = @"C:\Users\MatthewCordell\Documents\Visual Studio 2015\Projects\Content-For-Promotion-Extractor\Content-For-Promotion-ExtractorTests\TestData\TestDataRelease.zip";
            string promotionModule = "10101010";
            string localConceptsFile = @"C:\Users\MatthewCordell\Documents\Visual Studio 2015\Projects\Content-For-Promotion-Extractor\Content-For-Promotion-ExtractorTests\TestData\sct2_Concept_Snapshot_20170401.txt";

            RF2Reader r = new RF2Reader();

            // Import flasy list of IDs for cherry picking from donor edition
            var ExtractTargets = r.ReadListOfIds(conceptsForPromotionFile);
            Console.WriteLine("Initial target concepts for promotion = " + ExtractTargets.Count());

            // Unpack the necessary core files from the Donor Edition
            Unpacker pckr = new Unpacker();
            string donorConceptFile = pckr.Unpack(donorZip, RF2File.sct2_Concept_Snapshot);
            string donorDescriptionFile = pckr.Unpack(donorZip, RF2File.sct2_Description_Snapshot);
            string donorStatedFile = pckr.Unpack(donorZip, RF2File.sct2_StatedRelationship_Snapshot);
            string donorInferredFile = pckr.Unpack(donorZip, RF2File.sct2_Relationship_Snapshot);

            r.ConceptsPath = donorConceptFile;
            r.DescriptionsPath = donorDescriptionFile;
            r.StatedRelsPath = donorStatedFile;
            r.RelationshipsPath = donorInferredFile;

            Console.WriteLine("Donor core files successfully extracted");                       

            // Identify all the dependencies - could probably put this into a method
            // Check this dependency logic... looks messy
            var Localconcepts = r.ReadConceptFile(localConceptsFile, true, false); //Local concpets (already available)
            var statedRelationships = r.ReadRelationshipFile(donorStatedFile);
            var relationships = r.ReadRelationshipFile(donorInferredFile);
            //Check relationship file for any dependencies (stated+full)
            Console.WriteLine("Looking for dependencies");
            //?? var deps = r.GetDestinationIdDependencies(ExtractTargets, Localconcepts, statedRelationships);
            var dependencies = r.IdentifyAllDependencies(ExtractTargets, Localconcepts, statedRelationships, relationships);
            //Add any dependencies to extract list
            ExtractTargets = (ExtractTargets.Union(dependencies)).Distinct().ToList();
            Console.WriteLine("Total target concepts for extraction (including dependencies) = " + ExtractTargets.Count());
            // recover space
            Localconcepts.Clear();
            statedRelationships.Clear();
            relationships.Clear();


            Console.WriteLine("Extracting Target Data from Snapshot");         
            //Identify all the entries from the concepts,descriptions & both relationships for the complete exrtact targets.
            List<Concept> ExtractedConcepts = r.ExtractConcepts(ExtractTargets);
            List<Description> ExtractedDescriptions = r.ExtractDescriptions(ExtractTargets);
            List<Relationship> ExtractedStated = r.ExtractRelationships(ExtractTargets);
            List<Relationship> ExtractedRelationships = r.ExtractRelationships(ExtractTargets);

            pckr.CleanUpExtractedFiles();

            Console.WriteLine("Concepts extracted = " + ExtractedConcepts.Count().ToString());
            Console.WriteLine("Descriptions extracted = " + ExtractedDescriptions.Count().ToString());
            Console.WriteLine("Stated extracted = " + ExtractedStated.Count().ToString());
            Console.WriteLine("Inferred extracted = " + ExtractedRelationships.Count().ToString());

            Console.WriteLine("Creating RF2 Bundle");
            RF2Writer w = new RF2Writer();

            //module update should happen in dedicated class.
            w.CreateRf2File(ExtractedConcepts);
            w.CreateRf2File(ExtractedDescriptions);
            w.CreateRf2File(ExtractedStated, RelationshipType.stated);
            w.CreateRf2File(ExtractedRelationships, RelationshipType.inferred);

            Console.WriteLine("Done");
            Console.ReadKey();

        }
    }
}
