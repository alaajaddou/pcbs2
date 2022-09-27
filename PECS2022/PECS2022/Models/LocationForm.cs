using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PECS2022.Models
{
    public class LocationForm
    {
        public Governorate Governorate { get; set; }
        public Locality Locality { get; set; }
        //public int EnumArea { get; set; } //selected EnumArea
        public List<SampleInfo> Samples { get; set; } // all samples based on controls in the form

        public int RB = -1;

        public void Init()
        {
            LoadSamples();
        }

        public void LoadSamples()
        {
            Samples = new List<SampleInfo>();

            if (Governorate == null || Locality == null)
            {
                return;
            }

            var db = DataBase.GetConnection();

           Samples = db.Table<SampleInfo>().Where(x => x.ID01 == Governorate.Code && x.ID02 == Locality.Code).ToList();
            //Samples = db.Table<SampleInfo>().Where(x => x.ID1 == Governorate.Code && x.E3 == Locality.Code && x.ID3 == EnumArea).ToList();

            LoadSamplesVisits();
        }

        public void LoadSamplesVisits()
        {
            Samples.ForEach(x =>
            {
                x.LoadVisit();
            });
        }

        public List<string> GetSamplesBuildingNo()
        {
            return Samples.Select(x => x.GetBuildingFullCode()).Distinct().Where(x => x != string.Empty).ToList();
        }

        //public string GetEAFullCode()
        //{
        //    if (Locality == null) return string.Empty;

        //    string prefix = GeneralFunctions.GetGovPrefix(Locality.GovCode);

        //    string locCode = $"{prefix}{Locality.Code}{EnumArea.ToString("000")}";

        //    return locCode;
        //}

        public List<string> GetListEAFullCode(List<string> ID3)
        {
            List<string> EAFullCode = new List<string>();
            if (Locality == null) return EAFullCode;

            string prefix = GeneralFunctions.GetGovPrefix(Locality.GovCode);
            if(ID3 != null && ID3.Count>0)
            {
                string locCode = "";
                int temp = 0;
                foreach (string s in ID3)
                {
                    int.TryParse(s, out temp);
                    locCode = $"{prefix}{Locality.Code}{temp.ToString("000")}";
                    EAFullCode.Add(locCode);
                }
            }
            

            return EAFullCode;
        }


        public string GetLocalityFullCode()
        {
            if (Locality == null) return string.Empty;

            string prefix = GeneralFunctions.GetGovPrefix(Locality.GovCode);

            string locCode = $"{prefix}{Locality.Code}";

            return locCode;
        }

        //public string GetStartBuildingFullCode()
        //{
        //    if (Locality == null) return string.Empty;

        //    string prefix = GeneralFunctions.GetGovPrefix(Locality.GovCode);

        //    string locCode = $"{prefix}{Locality.Code}{EnumArea.ToString("000")}{RB.ToString("000")}";

        //    return locCode;
        //}

    }
}
