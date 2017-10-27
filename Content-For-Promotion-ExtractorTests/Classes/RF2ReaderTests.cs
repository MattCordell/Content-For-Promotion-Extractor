using Microsoft.VisualStudio.TestTools.UnitTesting;
using Content_For_Promotion_Extractor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Content_For_Promotion_Extractor.Tests
{
    [TestClass()]
    public class RF2ReaderTests
    {
        string path = AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\TestData\RF2Release\Snapshot\Terminology\";

        string conceptFile = "sct2_Concept_Snapshot_20171130.txt";
        string descriptionFile = "sct2_Description_Snapshot_20171130.txt";
        string statedRelationshipFile = "sct2_StatedRelationship_Snapshot_20171130.txt";
        string inferredRelationshipFile = "sct2_Relationship_Snapshot_20171130.txt";

        string localPath = AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\TestData\";

        string targetList = "TargetConceptToBeExtracted.txt";
        string localConceptFile = "sct2_Concept_Snapshot_20170401.txt";


        [TestMethod()]
        public void ReadConceptFile_AllStatus_Test()
        {
            string testFile = path + conceptFile;

            RF2Reader r = new RF2Reader();
            var concepts = r.ReadConceptFile(testFile, false);

            Assert.AreEqual(9, concepts.Count());
        }

        [TestMethod()]
        public void ReadConceptFile_ActiveOnly_Test()
        {
            string testFile = path + conceptFile;

            RF2Reader r = new RF2Reader();
            var concepts = r.ReadConceptFile(testFile);

            Assert.AreEqual(6, concepts.Count());
        }

        [TestMethod()]
        public void ReadDescriptionFile_AllStatus_Test()
        {
            string testFile = path + descriptionFile;

            RF2Reader r = new RF2Reader();
            var descriptions = r.ReadDescriptionFile(testFile, false);

            Assert.AreEqual(22, descriptions.Count());
        }

        [TestMethod()]
        public void ReadDescriptionFile_ActiveOnly_Test()
        {
            string testFile = path + descriptionFile;

            RF2Reader r = new RF2Reader();
            var descriptions = r.ReadDescriptionFile(testFile);

            Assert.AreEqual(18, descriptions.Count());
        }

        [TestMethod()]
        public void ReadRelationshipFile_AllStatus_Test()
        {
            string testFile = path + statedRelationshipFile;

            RF2Reader r = new RF2Reader();
            var relationships = r.ReadRelationshipFile(testFile, false);

            Assert.AreEqual(22, relationships.Count());
        }

        [TestMethod()]
        public void ReadRelationshipFile_ActiveOnly_Test()
        {
            string testFile = path + statedRelationshipFile;

            RF2Reader r = new RF2Reader();
            var relationships = r.ReadRelationshipFile(testFile);

            Assert.AreEqual(19, relationships.Count());
        }

        [TestMethod()]
        public void ReadListOfIds_Test()
        {
            string testFile = localPath + targetList;

            RF2Reader r = new RF2Reader();
            var ids = r.ReadListOfIds(testFile);

            Assert.AreEqual(3, ids.Count());
        }

        [TestMethod()]
        public void IdentifyDestinationIdDependencies_Test()
        {
            string targets = localPath + targetList;
            string localConcept = localPath + localConceptFile;
            string xRelationship = path + statedRelationshipFile;

            RF2Reader r = new RF2Reader();

            List<string> extractTargets = r.ReadListOfIds(targets);
            List<Concept> localConcepts = r.ReadConceptFile(localConcept, true, false);
            List<Relationship> localRelationships = r.ReadRelationshipFile(xRelationship);

            // initial number of concepts to extract
            var initialTargets = extractTargets.Count();

            //look for dependencies
            var dependencies = r.GetDestinationIdDependencies(extractTargets, localConcepts, localRelationships);

            //while there are dependencies, add them to the list, and look for more.
            while (dependencies.Count() > 0)
            {
                extractTargets.InsertRange(0, dependencies);

                dependencies = r.GetDestinationIdDependencies(extractTargets, localConcepts, localRelationships);
            }

            // number of dependencies found is the final number to extract - the initial;
            int identifiedDependencycount = extractTargets.Count() - initialTargets;

            Assert.AreEqual(1, identifiedDependencycount);
        }

        [TestMethod()]
        public void IdentifyAllDependencies_Test()
        {
            string targets = localPath + targetList;
            string localConcept = localPath + localConceptFile;
            string statedRelationships = path + statedRelationshipFile;
            string inferredRelationships = path + inferredRelationshipFile;

            RF2Reader r = new RF2Reader();

            List<string> extractTargets = r.ReadListOfIds(targets);
            List<Concept> localConcepts = r.ReadConceptFile(localConcept, true, false);
            List<Relationship> statedRels = r.ReadRelationshipFile(statedRelationships);
            List<Relationship> inferredRels = r.ReadRelationshipFile(inferredRelationships);

            //look for dependencies (both DestinationId + TypeId)
            var dependencies = r.IdentifyAllDependencies(extractTargets, localConcepts, statedRels, inferredRels);

            Assert.AreEqual(2, dependencies.Count());
        }

        [TestMethod()]
        public void ReadConceptFile_ActiveAllModules_Test()
        {
            string localConcept = localPath + localConceptFile;
            RF2Reader r = new RF2Reader();

            List<Concept> localConcepts = r.ReadConceptFile(localConcept, true, false);

            Assert.AreEqual(2, localConcepts.Count());
        }

        [TestMethod()]
        public void GetTypeIdIdDependencies_Test()
        {
            string targets = localPath + targetList;
            string localConcept = localPath + localConceptFile;
            string xRelationship = path + statedRelationshipFile;

            RF2Reader r = new RF2Reader();

            List<string> extractTargets = r.ReadListOfIds(targets);
            List<Concept> localConcepts = r.ReadConceptFile(localConcept, true, false);
            List<Relationship> localRelationships = r.ReadRelationshipFile(xRelationship);

            // initial number of concepts to extract
            int initialTargets = extractTargets.Count();

            //look for dependencies
            var dependencies = r.GetTypeIdIdDependencies(extractTargets, localConcepts, localRelationships);

            //while there are dependencies, add them to the list, and look for more.
            while (dependencies.Count() > 0)
            {
                extractTargets.InsertRange(0, dependencies);

                dependencies = r.GetTypeIdIdDependencies(extractTargets, localConcepts, localRelationships);
            }

            // number of dependencies found is the final number to extract - the initial;
            int identifiedDependencycount = extractTargets.Count() - initialTargets;

            Assert.AreEqual(1, identifiedDependencycount);
        }
    }
}