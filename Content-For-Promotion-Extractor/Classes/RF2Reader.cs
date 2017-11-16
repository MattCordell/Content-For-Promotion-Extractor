using System.Collections.Generic;
using System.Linq;
using System.IO;


namespace Content_For_Promotion_Extractor
{
    public class RF2Reader
    {
        public string EditionPath;

        // paths to the files that are required
        public string ConceptsPath;
        public string DescriptionsPath;
        public string StatedRelsPath;
        public string RelationshipsPath;
        
        public RF2Reader()
        {

        }


        public RF2Reader(string filepath)
        {
            EditionPath = filepath;
        }

        //Reads all of a Concepts file into List
        public List<Concept> ReadConceptFile(string fileName, bool onlyActiveComponents = true, bool excludeCoreModules = true)
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
                        if (onlyActiveComponents && fields[2] == "1")
                        {
                            Concept c = new Concept(fields);
                            concepts.Add(c);
                        }
                        else if (!onlyActiveComponents)
                        {
                            Concept c = new Concept(fields);
                            concepts.Add(c);
                        }
                    }
                    else if (!excludeCoreModules)
                    {
                        if (onlyActiveComponents && fields[2] == "1")
                        {
                            Concept c = new Concept(fields);
                            concepts.Add(c);
                        }
                        else if (!onlyActiveComponents)
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

        // checks both stated and inferred for dependencies
        public List<string> IdentifyAllDependencies(List<string> extractTargets, List<Concept> localconcepts, List<Relationship> statedRelationships, List<Relationship> inferredRelationships)
        {
            // check the stated relationships for dependencies
            List<string> dependencies = IdentifyDependencies(extractTargets, localconcepts, statedRelationships);
            var allTargets = extractTargets.Union(dependencies).ToList();

            //check the inferred for any extra dependencies
            dependencies.InsertRange(0,IdentifyDependencies(allTargets, localconcepts, inferredRelationships));           

            return dependencies.Distinct().ToList();
        }

        public List<string> IdentifyDependencies(List<string> extractTargets, List<Concept> localConcepts, List<Relationship> Relationships)
        {
            List<string> dependencies = new List<string>();

            var typeIdDependencies = GetTypeIdIdDependencies(extractTargets, localConcepts, Relationships);
            var destinationIdDependencies = GetDestinationIdDependencies(extractTargets, localConcepts, Relationships);

            var newDependencies = typeIdDependencies.Union(destinationIdDependencies);

            //recursively check for new dependencies.
            //each new dependency needs to be added to the extract targets to find further dependencies
            while (newDependencies.Count() > 0)
            {
                dependencies.InsertRange(0, newDependencies);
                extractTargets.InsertRange(0, dependencies);

                typeIdDependencies = GetTypeIdIdDependencies(extractTargets, localConcepts, Relationships);
                destinationIdDependencies = GetDestinationIdDependencies(extractTargets, localConcepts, Relationships);

                newDependencies = typeIdDependencies.Union(destinationIdDependencies);
            }

            // return deduplicated list
            return dependencies.Distinct().ToList();
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

        public List<Concept> ExtractConcepts(List<string> extractTargets)
        {
            // Read in all the concepts active/non-core
            var allConcepts = ReadConceptFile(ConceptsPath);

            List<Concept> conceptsToExtract = (from c in allConcepts
                                     join target in extractTargets
                                     on c.id equals target
                                     select c).ToList();

            return conceptsToExtract;
        }

        public List<Description> ExtractDescriptions(List<string> extractTargets)
        {
            // Read in all the descriptions active/non-core
            var allDescriptions = ReadDescriptionFile(DescriptionsPath);

            List<Description> descriptionsToExtract = (from c in allDescriptions
                                               join target in extractTargets
                                               on c.conceptId equals target
                                               select c).ToList();

            return descriptionsToExtract;
        }

        public List<Relationship> ExtractRelationships(List<string> extractTargets)
        {
            // Read in all the descriptions active/non-core
            var allRelationships = ReadRelationshipFile(RelationshipsPath);

            List<Relationship> relationshipsToExtract = (from c in allRelationships
                                                       join target in extractTargets
                                                       on c.sourceId equals target
                                                       select c).ToList();

            return relationshipsToExtract;
        }
        
    }

}
