using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using CsvHelper;

namespace Content_For_Promotion_Extractor
{
    class RF2Reader
    {
        //Reads all of a Concepts file into List
        public List<Concept> ReadConceptFile(string fileName, bool onlyactivecomponents = true)
        {
            using (StreamReader file = File.OpenText(fileName))
            {
                var line = file.ReadLine(); // skip header
                var concepts = new List<Concept>();

                while (!file.EndOfStream)
                {
                    line = file.ReadLine(); // skip header
                    var fields = line.Split('\t');

                    if (fields[3] != "900000000000207008" && fields[3] != "900000000000012004")
                    {
                        if (onlyactivecomponents && fields[2] == "1")
                        {
                            Concept c = new Concept(fields);
                            concepts.Add(c);
                        }
                        else
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
                    line = file.ReadLine(); // skip header
                    var fields = line.Split('\t');

                    if (fields[3] != "900000000000207008" && fields[3] != "900000000000012004")
                    {
                        if (onlyactivecomponents && fields[2] == "1")
                        {
                            Description d = new Description(fields);
                            descriptions.Add(d);
                        }
                        else
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
                    line = file.ReadLine(); // skip header
                    var fields = line.Split('\t');

                    if (fields[3] != "900000000000207008" && fields[3] != "900000000000012004")
                    {                    
                        if (onlyactivecomponents && fields[2] == "1")
                        {
                            Relationship r = new Relationship(fields);
                            relationships.Add(r);
                        }
                        else
                        {
                            Relationship r = new Relationship(fields);
                            relationships.Add(r);
                        }
                    }

                }
                return relationships;
            }
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
                    ids.Add(file.ReadLine());
                }
                return ids;
            }
        }

        internal List<string> IdentifyDependencies(List<string> extractTargets, string file)
        {
            throw new NotImplementedException();
        }

    }

}
