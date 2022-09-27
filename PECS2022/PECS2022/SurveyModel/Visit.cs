using PECS2022.Models;
using Newtonsoft.Json;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace PECS2022
{

    [Table("Visits")]
    public class Visit
    {
        [PrimaryKey]
        public string ID00 { get; set; }
        public string ID01 { get; set; }
        public string ID02 { get; set; }
        public int ID03 { get; set; }
        public int ID04 { get; set; }
        public int ID05 { get; set; }
        public int ID06 { get; set; }
        public int ID07 { get; set; }
        public int ID08 { get; set; }
        public int? ID09 { get; set; }
        public int? ID10 { get; set; }
        public string QC1_1 { get; set; }
        public string QC1_2 { get; set; }
        public string QC1_3 { get; set; }
        public int QC2 { get; set; }
        public string QC2_txt { get; set; }
        public int? QC3 { get; set; }
        public int? QC4 { get; set; }
        public int? QC5 { get; set; }
        public double? S_X { get; set; }
        public double? S_Y { get; set; }
        public double? E_X { get; set; }
        public double? E_Y { get; set; }
        public bool IsComplete { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }



        public bool NeedSend { get; set; }

        public string BuildingNo { get; set; }

        public string BuildBuildingNo()
        {

          //if (ID05.HasValue == false ) return string.Empty;
            string prefix = GeneralFunctions.GetGovPrefix(ID01);
            return $"{prefix}{ID02}{ID03.ToString("000")}{ID05.ToString("000")}";
        }

    }
}
