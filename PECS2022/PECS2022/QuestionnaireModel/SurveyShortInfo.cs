using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PECS2022
{
    public class SurveyShortInfo
    {
        public int ID { get; set; }
        public string Description { get; set; }
        public string RepDescription { get; set; }

        public System.DateTime StartDate { get; set; }
        public System.DateTime EndDate { get; set; }
        public System.DateTime LastModifiedDate { get; set; }
    }
}