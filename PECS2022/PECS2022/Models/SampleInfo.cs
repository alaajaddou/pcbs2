using Newtonsoft.Json;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace PECS2022.Models
{
    public class SampleInfo
    {
        [PrimaryKey]
        public string ID00 { get; set; }
        public string ID01 { get; set; }
        public string ID02 { get; set; }
        public int ID03 { get; set; }
        public int? ID04 { get; set; }
        public int? ID05 { get; set; }
        public int? ID06 { get; set; }
        public int ID08 { get; set; }
        public string QC1_1 { get; set; }
        public int? AllowedDistance { get; set; }
        public string AssignedTo { get; set; }
        public DateTime? AssignDate { get; set; }
        public string AssignedBy { get; set; }


        [Ignore]
        public IList<Individual> Individuals { get; set; }

        [Ignore]
        [JsonIgnore]
        public string Description {
            get {

                if (string.IsNullOrEmpty(QC1_1) || QC1_1 == "لا يوجد")
                    return QC1_1;

                return QC1_1;
            }
            set {
            } 
        }

       
        [JsonIgnore]
        public bool NeedSend { get; set; }


        [Ignore]
        [JsonIgnore]
        public Visit Visit { get; set; }

        public string GetBuildingFullCode()
        {
            if (string.IsNullOrEmpty(ID02 ) || string.IsNullOrEmpty(ID02)  || (ID04 == null || !ID04.HasValue)) return string.Empty;

            string prefix = GeneralFunctions.GetGovPrefix(ID01);

            string buildingCode = $"{prefix}{ID02}{ID03.ToString("000")}{ID05.Value.ToString("000")}";

            return buildingCode;
        }

        public void LoadVisit()
        {
            var db = DataBase.GetConnection();
            Visit = db.Table<Visit>().Where(x => x.ID00 == this.ID00).FirstOrDefault();
        }

        public int InsertOrUpdateVisit()
        {
            if (Visit == null) return 0;

            var db = DataBase.GetConnection();

            if(db.Table<Visit>().Where(x => x.ID00 == Visit.ID00).FirstOrDefault() == null)
            {
                return db.Insert(Visit);
            }

            return db.Update(Visit);
        }
    }
}
