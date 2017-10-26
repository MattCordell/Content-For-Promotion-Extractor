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
        string relationshipFile = "sct2_StatedRelationship_Snapshot_20171130.txt";

        string targetList = "sct2_Concept_Snapshot_20171130.txt";
        string localConceptFile = "sct2_Concept_Snapshot_20171130.txt";


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
            string testFile = path + relationshipFile;

            RF2Reader r = new RF2Reader();
            var relationships = r.ReadRelationshipFile(testFile, false);

            Assert.AreEqual(22, relationships.Count());
        }

        [TestMethod()]
        public void ReadRelationshipFile_ActiveOnly_Test()
        {
            string testFile = path + relationshipFile;

            RF2Reader r = new RF2Reader();
            var relationships = r.ReadRelationshipFile(testFile);

            Assert.AreEqual(19, relationships.Count());
        }

        [TestMethod()]
        public void ReadListOfIdsTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void IdentifyDependenciesTest()
        {
            Assert.Fail();
        }
    }
}