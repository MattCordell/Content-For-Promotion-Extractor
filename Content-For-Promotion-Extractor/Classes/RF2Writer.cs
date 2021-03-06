﻿using System;
using System.Collections.Generic;
using System.IO;

namespace Content_For_Promotion_Extractor
{
    public enum RelationshipType { stated, inferred};

    public class RF2Writer
    {
        private string timeStamp;
        string extractPath;

        public RF2Writer()
        {
            timeStamp = System.DateTime.Now.ToString("yyyyMMdd");            
            extractPath = AppDomain.CurrentDomain.BaseDirectory + @"\Extract\Delta\Terminology\";
            Directory.CreateDirectory(extractPath);

        }

        public void CreateRf2File( List<Concept> extractedConcepts)
        {
            string path = extractPath  + "sct2_Concept_Delta_Extract_" + timeStamp + ".txt";   
                     
            using (TextWriter w = new StreamWriter(path, false, System.Text.Encoding.UTF8))
            {
                //write out the header
                w.WriteLine("id	effectiveTime	active	moduleId	definitionStatusId");

                //write out all the entries
                foreach (var concept in extractedConcepts)
                {
                    w.WriteLine(concept.ToString());
                }     
                           
            }            
        }

        public void CreateRf2File(List<Description> extractedDescriptions)
        {
            string path = extractPath + "sct2_Description_Delta_Extract_" + timeStamp + ".txt";

            using (TextWriter w = new StreamWriter(path, false, System.Text.Encoding.UTF8))
            {
                //write out the header
                w.WriteLine("id	effectiveTime	active	moduleId	conceptId	languageCode	typeId	term	caseSignificanceId");

                //write out all the entries
                foreach (var description in extractedDescriptions)
                {
                    w.WriteLine(description.ToString());
                }

            }
        }

        //this needs to handle state+inferred elegantly
        public void CreateRf2File(List<Relationship> extractedRelationships, RelationshipType type)
        {
            string path;

            if (type == RelationshipType.stated)
            {
                path = extractPath + "sct2_StatedRelationship_Delta_Extract_" + timeStamp + ".txt";
            }
            else
            {
                path = extractPath + "sct2_Relationship_Delta_Extract_" + timeStamp + ".txt";
            }
            

            using (TextWriter w = new StreamWriter(path, false, System.Text.Encoding.UTF8))
            {
                //write out the header
                w.WriteLine("id	effectiveTime	active	moduleId	sourceId	destinationId	relationshipGroup	typeId	characteristicTypeId	modifierId");

                //write out all the entries
                foreach (var relationship in extractedRelationships)
                {
                    w.WriteLine(relationship.ToString());
                }

            }
        }

        public void CreateRf2File(List<Language> LanguagePreferences)
        {
            string path = extractPath + RF2File.der2_cRefset_LanguageSnapshot +"_Extract_en_" + timeStamp + ".txt";

            using (TextWriter w = new StreamWriter(path, false, System.Text.Encoding.UTF8))
            {
                //write out the header
                w.WriteLine("id	effectiveTime	active	moduleId	refsetId	referencedComponentId	acceptabilityId");

                //write out all the entries
                foreach (var l in LanguagePreferences)
                {
                    w.WriteLine(l.ToString());
                }

            }
        }
    }
}