using System;


namespace Content_For_Promotion_Extractor
{

    // public strings used for initial simplicity
    class RF2 {
        public const string GBrefset = "900000000000508004";
        public const string USrefset  = "900000000000509007";
        public const string FSN = "900000000000003001";
            }
    

    public class Concept
    {
        public string id { get; set; }
        public string effectiveTime { get; set; }
        public string active { get; set; }
        public string moduleId { get; set; }
        public string definitionStatusId { get; set; }

        public Concept(string[] s)
        {
            if (s.Length != 5) throw new Exception("Not enough fields to initialise concept class");
            
        id = s[0];
        effectiveTime = s[1];
        active = s[2];
        moduleId = s[3];
        definitionStatusId = s[4];
        }

        override public string ToString()
        {
            char t = '\t';

            return id + t + effectiveTime + t + active + t + moduleId + t + definitionStatusId;
        }

    }

    public class Description
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

        override public string ToString()
        {
            char t = '\t';

            return id + t + effectiveTime + t + active + t + moduleId + t + conceptId + t + languageCode + t + typeId + t + term + t + caseSignificanceId;
        }
    }

    public class Relationship
    {
        public enum RF2CharacteristicType { Stated, Inferred };

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

        override public string ToString()
        {
            char t = '\t';

            return id + t + effectiveTime + t + active + t + moduleId + t + sourceId + t + destinationId + t + relationshipGroup + t + typeId + t + characteristicTypeId + t + modifierId;

        }
    }

    public class Language
    {
        public string id { get; set; }
        public string effectiveTime { get; set; }
        public string active { get; set; }
        public string moduleId { get; set; }
        public string refsetId { get; set; }
        public string referencedComponentId { get; set; }
        public string acceptabilityId { get; set; }

        public Language(Language l)
        {
            id = l.id;
            effectiveTime = l.effectiveTime;
            active = l.active;
            moduleId = l.moduleId;
            refsetId = l.refsetId;
            referencedComponentId = l.referencedComponentId;
            acceptabilityId = l.acceptabilityId;
        }

        public Language(string[] s)
        {
            if (s.Length != 7) throw new Exception("Not enough fields to initialise Language class");

            id = s[0];
            effectiveTime = s[1];
            active = s[2];
            moduleId = s[3];
            refsetId = s[4];
            referencedComponentId = s[5];
            acceptabilityId = s[6];
        }

        override public string ToString()
        {
            char t = '\t';

            return id + t + effectiveTime + t + active + t + moduleId + t + refsetId + t + referencedComponentId + t + acceptabilityId;
        }

    }

}
