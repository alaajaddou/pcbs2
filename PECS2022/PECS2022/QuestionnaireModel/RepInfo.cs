using System;

namespace PECS2022
{
    public class RepInfo
    {
        public System.Guid ID { get; set; }
        public int SurveyId { get; set; }
        public string Description { get; set; }
        public System.DateTime StartDate { get; set; }
        public System.DateTime EndDate { get; set; }
        public int ReportingWorkGroupId { get; set; }
        public Nullable<int> ResearcherWorkGroupId { get; set; }
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public int LastModifiedBy { get; set; }
        public System.DateTime LastModifiedDate { get; set; }

       
    }
}
