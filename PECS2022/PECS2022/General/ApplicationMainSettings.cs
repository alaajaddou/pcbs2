using Newtonsoft.Json;
using PECS2022.Interfaces;
using PECS2022.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using PECS2022.General;

namespace PECS2022
{
    public static class ApplicationMainSettings
    {
        //TODO On Publish - SurveyId
        public const int SurveyId = 3084;
        private static List<SurveyInfo> _surveyInfos = new List<SurveyInfo>();
        private static List<GovernorateInfo> _GeovernorateList;
        private static List<Locality> _LocalityList;

        public static int SamplesNotInBuildingsCount()
        {
            var db = DataBase.GetConnection();
            return db.Table<SampleInfo>().Count(Expressions.SamplesNotInBuildings);
        }

        public static  List<GovernorateInfo> GovernorateList
        {
            get
            {
                if (_GeovernorateList == null || _GeovernorateList.Count==0)
                {
                    var db = DataBase.GetConnection();
                    var  govs = db.Table<Governorate>().Select(x => new GovernorateInfo() { Code = x.Code, Description = x.Description }).Distinct().ToList() ;


                    string code = string.Empty;
                    _GeovernorateList = new List<GovernorateInfo>();
                    foreach(var  g  in govs)
                    {
                        if (code != g.Code)
                        {
                            _GeovernorateList.Add(g);
                        }

                        code = g.Code;
                    }
                }

                return _GeovernorateList;
            }
        }

        public static List<Locality> LocalityList
        {
            get
            {
                if (_LocalityList == null || _LocalityList.Count==0)
                {
                    var db = DataBase.GetConnection();
                    _LocalityList = db.Table<Locality>().ToList();
                }

                return _LocalityList;
            }
        }

        public static List<Locality> GetLocalityByGovCode(string gCode)
        {
            var locs = LocalityList;

            return locs.FindAll(x => x.GovCode == gCode);
        }

        public static SurveyInfo GetSurveyInfo(int? surveyId)
        {
            if (surveyId == null) return null;
            if (_surveyInfos == null)
            {
                _surveyInfos = new List<SurveyInfo>();
            }

            if (!_surveyInfos.Exists(x => x.ID == surveyId))
            {
                var path = DependencyService.Get<IDatabaseSettings>().DatabaseFolderPath;
                string filename = Path.Combine(path, $"EsurveyLast_{surveyId}.json");

                if (!File.Exists(filename))
                {
                    return null;
                }

                string data = File.ReadAllText(filename);

                _surveyInfos.Add(JsonConvert.DeserializeObject<SurveyInfo>(data));
            }

            return _surveyInfos.FirstOrDefault(x => x.ID == surveyId);
        }

        public static SurveyInfo GetSurveyInfo()
        {
            return GetSurveyInfo(SurveyId);
        }

        public static void ResetSurvey()
        {
            _surveyInfos = new List<SurveyInfo>();
        }

        private static List<OptionInfo> _QC02Results;

        private static List<LookupVal> _lookupResults;

        public static List<OptionInfo> E14Results
        {
            get
            {

                if (_QC02Results == null)
                {
                    _QC02Results = new List<OptionInfo>()
                    {  OptionInfo.Default,
                        new OptionInfo(){Id=1, Description="اكتملت" },
                        new OptionInfo(){Id=2, Description="مكتمل جزئي" },
                        new OptionInfo(){Id=3, Description="الأسرة مسافرة" },
                        new OptionInfo(){Id=4, Description="وحدة غير موجودة " },

                        new OptionInfo(){Id=5, Description="لا أحد في البيت" },
                        new OptionInfo(){Id=6, Description="رفض/ السبب", NeedComments=true },
                        new OptionInfo(){Id=7, Description="وحدة سكنية غير مأهولة", NeedComments=false },

                        new OptionInfo(){Id=8, Description="لم يتوفر معلومات" },
                        new OptionInfo(){Id=9, Description="أخرى", NeedComments=true }
                    };
                }

                return _QC02Results;

            }

        }



        public static bool IsGazeGovs(string govCode)
        {

            return (new string[] { "55", "60", "65", "70", "75" }).Contains(govCode);
        }


        public static List<LookupVal> E14LookUpValue
        {
            get
            {

                if (_lookupResults == null)
                {
                    _lookupResults = new List<LookupVal>()
                    {
                         new LookupVal() { Id = 0,  Description = "اختار" },
                         new LookupVal(){Id=1, Code="01", Description="تمت اكتملت" },
                        new LookupVal(){Id=2, Code="02", Description="تمت ولم تكتمل" },
                        new LookupVal(){Id=3, Code="03", Description="رفض" },
                       new LookupVal(){Id=3, Code="04", Description="لا احد بالبيت" },
                        new LookupVal(){Id=9,Code="05", Description="أخرى" }
                    };
                }

                return _lookupResults;

            }

        }
    }
}
