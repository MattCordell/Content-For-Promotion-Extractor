using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Content_For_Promotion_Extractor
{

    // public strings used for initial simplicity

    class Concept
    {
        public string id { get; set; }
        public string effectiveTime { get; set; }
        public string active { get; set; }
        public string moduleId { get; set; }
        public string definitionStatusId { get; set; }

        public Concept(string[] s)
        {
            if (s.Length != 5) throw new Exception("Not enough fields to initials concept class");
            
        id = s[0];
        effectiveTime = s[1];
        active = s[2];
        moduleId = s[3];
        definitionStatusId = s[4];
        }
    }

    class Description
    {
        public string id;
        public string effectiveTime;
        public string active;
        public string moduleId;
        public string conceptId;
        public string languageCode;
        public string typeId;
        public string term;
        public string caseSignificanceId;

        public Description(string[] s)
        {
            if (s.Length != 9) throw new Exception("Not enough fields to initialise descriptions class");

            id = s[0];
            effectiveTime = s[1];
            active = s[2];
            moduleId = s[3];
            conceptId = s[4];
            languageCode = s[5];
            typeId = s[6];
            term = s[7];
            caseSignificanceId = s[8];
        }
    }

    class Relationship
    {
        public string id;
        public string effectiveTime;
        public string active;
        public string moduleId;
        public string sourceId;
        public string destinationId;
        public string relationshipGroup;
        public string typeId;
        public string characteristicTypeId;
        public string modifierId;

        public Relationship(string[] s)
        {
            if (s.Length != 10) throw new Exception("Not enough fields to initialise relationship class");

            id = s[0];
            effectiveTime = s[1];
            active = s[2];
            moduleId = s[3];
            sourceId = s[4];
            destinationId = s[5];
            relationshipGroup = s[6];
            typeId = s[7];
            characteristicTypeId = s[8];
            modifierId = s[9];
        }
    }

}
