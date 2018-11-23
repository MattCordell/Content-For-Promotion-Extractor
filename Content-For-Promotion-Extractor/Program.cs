using System;
using System.Collections.Generic;
using System.Linq;


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

            //input parameters
            string conceptsForPromotionFile = @"C:\tmp\Sample_AU_concepts_for_Promotion_20181123.txt";
            string donorZip = @"C:\tmp\combined-releasefiles_Nov2018.zip";
            string promotionModule = "900000000000207008";           
            string localConceptsFile = @"C:\tmp\sct2_Concept_Snapshot_INT_20180731.txt";

            /*
            //need to validate arguments

            string conceptsForPromotionFile =args[0];
            string donorZip = args[1];
            string promotionModule = args[2];
            string localConceptsFile = args[3];            
            */

            RF2Reader r = new RF2Reader();

            // Import list of IDs for cherry picking from donor edition
            var ExtractTargets = r.ReadListOfIds(conceptsForPromotionFile);
            Console.WriteLine("Initial target concepts for promotion = " + ExtractTargets.Count());

            // Unpack the necessary core files from the Donor Edition
            Unpacker pckr = new Unpacker();
            string donorConceptFile = pckr.Unpack(donorZip, RF2File.sct2_Concept_Snapshot);
            string donorDescriptionFile = pckr.Unpack(donorZip, RF2File.sct2_Description_Snapshot);
            string donorStatedFile = pckr.Unpack(donorZip, RF2File.sct2_StatedRelationship_Snapshot);
            string donorInferredFile = pckr.Unpack(donorZip, RF2File.sct2_Relationship_Snapshot);
            string donorLanguageFile = pckr.Unpack(donorZip, RF2File.der2_cRefset_LanguageSnapshot);

            r.ConceptsPath = donorConceptFile;
            r.DescriptionsPath = donorDescriptionFile;
            r.StatedRelsPath = donorStatedFile;
            r.InferredRelsPath = donorInferredFile;
            r.LanguagePath = donorLanguageFile;

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
            List<Relationship> ExtractedStated = r.ExtractRelationships(ExtractTargets, Relationship.RF2CharacteristicType.Stated);
            List<Relationship> ExtractedRelationships = r.ExtractRelationships(ExtractTargets, Relationship.RF2CharacteristicType.Inferred);
            List<Language> ExtractedLanguagePreferences = r.ExtractLanguagePreferences(ExtractedDescriptions);

            pckr.CleanUpExtractedFiles();

            Console.WriteLine("Concepts extracted = " + ExtractedConcepts.Count().ToString());
            Console.WriteLine("Descriptions extracted = " + ExtractedDescriptions.Count().ToString());
            Console.WriteLine("Stated extracted = " + ExtractedStated.Count().ToString());
            Console.WriteLine("Inferred extracted = " + ExtractedRelationships.Count().ToString());
            Console.WriteLine("LaguagePreferences extracted = " + ExtractedLanguagePreferences.Count().ToString());

            Console.WriteLine("Creating RF2 Bundle");
            RF2Writer w = new RF2Writer();

            //module update should happen in dedicated class.
            ExtractedConcepts = r.PromoteComponent(ExtractedConcepts, promotionModule);
            ExtractedDescriptions = r.PromoteComponent(ExtractedDescriptions, promotionModule);
            ExtractedStated = r.PromoteComponent(ExtractedStated, promotionModule);
            ExtractedRelationships = r.PromoteComponent(ExtractedRelationships, promotionModule);
            ExtractedLanguagePreferences = r.PromoteComponent(ExtractedLanguagePreferences, promotionModule);
            //Munge in some FSN preferences
            r.AddFSN_Preferences(ExtractedDescriptions, ExtractedLanguagePreferences, promotionModule);

            //write all the stuff out (into Extract\RF2\ )
            w.CreateRf2File(ExtractedConcepts);
            w.CreateRf2File(ExtractedDescriptions);
            w.CreateRf2File(ExtractedStated, RelationshipType.stated);
            w.CreateRf2File(ExtractedRelationships, RelationshipType.inferred);
            w.CreateRf2File(ExtractedLanguagePreferences);

            Console.WriteLine("Done");
            Console.ReadKey();

        }
    }
}
