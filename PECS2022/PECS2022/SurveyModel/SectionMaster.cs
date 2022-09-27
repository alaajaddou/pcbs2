using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace PECS2022
{
    public class SectionMaster
    {
        public SectionMaster()
        {
            Answers = new List<QuestionAnswer>();
        }

        [PrimaryKey]

        public Guid Id { get; set; }
        public string ID00 { get; set; }
        public int SectionId { get; set; }

        public int? HR01 { get; set; }

        public string ResearcherId { get; set; }

        public DateTime VisitDate { get; set; }

        public DateTime CreatedDate { get; set; }

        public string CreatedBy { get; set; }

        public string LastModifiedBy { get; set; }

        public DateTime LastModifiedDate { get; set; }

        public string Comments { get; set; }

        public bool IsComplete { get; set; }
        public bool IsPartialComplete { get; set; }

        public bool NeedSend { get; set; }

        [Ignore]
        public List<QuestionAnswer> Answers { get; set; }

    }
}
