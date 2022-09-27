using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace PECS2022.SurveyModel
{
    public class CallLogInfo
    {

        [PrimaryKey]
        public System.Guid ID { get; set; }
        public string ID00 { get; set; }
        public int C1 { get; set; }
        public string C2 { get; set; }
        public DateTime C3 { get; set; }
        public int? C4 { get; set; }
        public int? C5 { get; set; }
        public int? C6 { get; set; }
        public string C6_Name { get; set; }
        public int? C7 { get; set; }
        public int? C8 { get; set; }

        public int C9 { get; set; }
        public string Comments { get; set; }
        public string CreatedBy { get; set; }

        public bool IsSent { get; set; }
    }
}
