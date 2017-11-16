using System.IO.Compression;
using System.IO;
using System;
using System.Collections.Generic;

namespace Content_For_Promotion_Extractor
{

    public enum RF2File { sct2_Concept_Snapshot, sct2_Description_Snapshot, sct2_Relationship_Snapshot, sct2_StatedRelationship_Snapshot };

    // Unpacker Extracts the desired file from an RF2 Bundle.
    // Paths to extracted files are returned.
    // All Extracted files can be cleaned up on deman. Or with finalise.

    public class Unpacker
    {      
        private string extractedFilePath;
        private List<string> createdFiles = new List<string>();

        //Unzip a specific file from an archive and return path
        public string Unpack(string donorZip, RF2File targetToken)
        {        
            //
            using (ZipArchive archive = ZipFile.OpenRead(donorZip))
            {
                foreach (var entry in archive.Entries)
                {
                    if (entry.FullName.Contains(targetToken.ToString()))
                    {
                        extractedFilePath = Directory.GetCurrentDirectory() + @"\" + targetToken.ToString() + ".txt";
                        Console.WriteLine(extractedFilePath);
                        entry.ExtractToFile(extractedFilePath, true);                        
                    }
                }
            }

            createdFiles.Add(extractedFilePath); //store the path for later cleanup
            return extractedFilePath;            //path to extracted file
        }

        //clean up all the files that have been extracted
        public void CleanUpExtractedFiles()
        {
            foreach (var file in createdFiles)
            {
                File.Delete(file);
            }
        }

        ~Unpacker()
        {
            CleanUpExtractedFiles();
        }
    }
}