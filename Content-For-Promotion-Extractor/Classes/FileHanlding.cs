using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using CsvHelper;

namespace Content_For_Promotion_Extractor
{
    public class RF2Reader
    {
        //Reads all of a Concepts file into List
        public List<Concept> ReadConceptFile(string fileName, bool onlyactivecomponents = true, bool excludeCoreModules = true)
        {
            using (StreamReader file = File.OpenText(fileName))
            {
                var line = file.ReadLine(); // skip header
                var concepts = new List<Concept>();

                while (!file.EndOfStream)
                {
                    line = file.ReadLine();
                    var fields = line.Split('\t');

                    if (excludeCoreModules && fields[3] != "900000000000207008" && fields[3] != "900000000000012004")
                    {
                        if (onlyactivecomponents && fields[2] == "1")
                        {
                            Concept c = new Concept(fields);
                            concepts.Add(c);
                        }
                        else if (!onlyactivecomponents)
                        {
                            Concept c = new Concept(fields);
                            concepts.Add(c);
                        }
                    }
                    else if (!excludeCoreModules)
                    {
                        if (onlyactivecomponents && fields[2] == "1")
                        {
                            Concept c = new Concept(fields);
                            concepts.Add(c);
                        }
                        else if (!onlyactivecomponents)
                        {
                            Concept c = new Concept(fields);
                            concepts.Add(c);
                        }
                    }
                }
                return concepts;
            }
        }

        //Reads all of a Descriptions file into List
        public List<Description> ReadDescriptionFile(string fileName, bool onlyactivecomponents = true)
        {
            using (StreamReader file = File.OpenText(fileName))
            {
                var line = file.ReadLine(); // skip header
                var descriptions = new List<Description>();

                while (!file.EndOfStream)
                {
                    line = file.ReadLine();
                    var fields = line.Split('\t');

                    if (fields[3] != "900000000000207008" && fields[3] != "900000000000012004")
                    {
                        if (onlyactivecomponents && fields[2] == "1")
                        {
                            Description d = new Description(fields);
                            descriptions.Add(d);
                        }
                        else if (!onlyactivecomponents)
                        {
                            Description d = new Description(fields);
                            descriptions.Add(d);
                        }
                    }

                }
                return descriptions;
            }
        }

        //Reads all of a Relationships file into List
        public List<Relationship> ReadRelationshipFile(string fileName, bool onlyactivecomponents = true)
        {
            using (StreamReader file = File.OpenText(fileName))
            {
                var line = file.ReadLine(); // skip header
                var relationships = new List<Relationship>();

                while (!file.EndOfStream)
                {
                    line = file.ReadLine();
                    var fields = line.Split('\t');

                    if (fields[3] != "900000000000207008" && fields[3] != "900000000000012004")
                    {                    
                        if (onlyactivecomponents && fields[2] == "1")
                        {
                            Relationship r = new Relationship(fields);
                            relationships.Add(r);
                        }
                        else if (!onlyactivecomponents)
                        {
                            Relationship r = new Relationship(fields);
                            relationships.Add(r);
                        }
                    }

                }
                return relationships;
            }
        }

        public List<string> IdentifyAllDependencies(List<string> extractTargets, List<Concept> localConcepts, List<Relationship> statedRelationships, List<Relationship> inferredrelationships)
        {
            //Get Stated Dependendencies Destination + TypeId
            //Get Inferred Dependendencies Destination + TypeId
            var foo = GetTypeIdIdDependencies(extractTargets, localConcepts, statedRelationships);

            throw new NotImplementedException();
        }

        public List<string> GetTypeIdIdDependencies(List<string> extractTargets, List<Concept> localConcepts, List<Relationship> Relationships)
        {
            // select all the destinationIds for target concepts
            var query1 = (from relationship in Relationships
                          join target in extractTargets
                          on relationship.sourceId equals target
                          select relationship.typeId).Distinct();

            var query2 = (from c in localConcepts
                          select c.id).Distinct();

            var localAndExtract = extractTargets.Union(query2);

            var dependencies = query1.Except(localAndExtract).Distinct();


            return dependencies.ToList();
        }

        //Reads a list of ids from a file into List
        public List<string> ReadListOfIds(string fileName)
        {
            using (StreamReader file = File.OpenText(fileName))
            {
                var line = file.ReadLine(); // skip header
                List<string> ids = new List<string>();

                while (!file.EndOfStream)
                {
                    line = file.ReadLine();
                    var fields = line.Split('\t');

                    ids.Add(fields[0]);
                }
                return ids;
            }
        }

        public List<string> GetDestinationIdDependencies(List<string> extractTargets, List<Concept> localConcepts, List<Relationship> xRelationships)
        {

            // select all the destinationIds for target concepts
            var query1 = (from relationship in xRelationships
                        join target in extractTargets
                        on relationship.sourceId equals target
                        select relationship.destinationId).Distinct();

            var query2 = (from c in localConcepts
                          select c.id).Distinct();

            var localAndExtract = extractTargets.Union(query2);

            var dependencies = query1.Except(localAndExtract).Distinct();


            return dependencies.ToList();
        }

    }

}
