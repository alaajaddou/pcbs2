using Newtonsoft.Json;
using PECS2022.Interfaces;
//using PECS2022.VisitViews;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using PECS2022.Models;
using PECS2022.SurveyModel;

namespace PECS2022
{
    public static class QuestionnaireManager
    {


        //public  static Sample CurrentSample { get; private set; }
        public static VisitLog CurrentVisitLog{ get; private set; }

        public static Visit CurrentVisit { get; set; }

        public static SampleInfo CurrentSample { get; set; }


        public static CallLogInfo CurrentCall { get; set; }






        internal static void ClearCurrentSettings()
        {
         
            CurrentVisit = null;
            CurrentVisitLog = null;
            SectionStatusList = null;
            //VisitViews.MembersDetails.Current = null;

        }
        public static Visit StartVisit(SampleInfo sample2, string BuildingNo, string ID00)
        {
            CurrentSample = sample2;
            CurrentVisit = sample2.Visit;
            string userName = Security.CurrentUserSettings.CurrentUser.UserName;
            //GeneralApplicationSettings.LocationForm.EnumArea
            var enumArea = sample2.ID03; //Convert.ToInt32(BuildingNo.Substring(8, 3));
            int ID5 = sample2.ID05??0;//Convert.ToInt32( BuildingNo.Substring(11,3));
            int E14Lookup = 0;
            int.TryParse(LookUpManager.DefaultLookupValue.Code,out E14Lookup);
            var db = DataBase.GetConnection();
            if (CurrentVisit == null)
            {
                SampleInfo sample = GeneralApplicationSettings.LocationForm.Samples.Where(x => x.ID00 == ID00 && x.ID01 == GeneralApplicationSettings.LocationForm.Governorate.Code && x.ID02 == GeneralApplicationSettings.LocationForm.Locality.Code && x.AssignedTo == Security.CurrentUserSettings.CurrentUser.UserName).FirstOrDefault();
                CurrentVisit = new Visit() { ID00 = ID00, ID01 = GeneralApplicationSettings.LocationForm.Governorate.Code , ID02=sample.ID02, ID05=sample.ID05??0, ID06=sample.ID06 ??0, QC1_1=sample.QC1_1, ID03 = sample.ID03,  CreatedBy = userName, CreatedDate = DateTime.Now, S_X = GISCurrentLocation.CurrentX, IsComplete = false, LastModifiedBy = userName, LastModifiedDate = DateTime.Now, NeedSend = true, S_Y = GISCurrentLocation.CurrentY,  E_X = GISCurrentLocation.CurrentX, E_Y = GISCurrentLocation.CurrentY, ID04 = ID5, BuildingNo = BuildingNo 
                  };
            }

            CurrentSample.Visit = CurrentVisit;


           




            CurrentVisitLog = new VisitLog() {  EndDate=null, E_X=GISCurrentLocation.CurrentX, E_Y=GISCurrentLocation.CurrentY, Id=Guid.NewGuid(),  ID00= CurrentVisit.ID00, IsComplete=false, NeedSend=true, Researcher=userName, StartDate=DateTime.Now, S_X= GISCurrentLocation.CurrentX, S_Y=GISCurrentLocation.CurrentY };

          

            db.InsertOrReplace(CurrentVisitLog);
           


           

            return CurrentVisit;
        }

        internal static SectionStatus GetSectionStatus(int secId, int? hr01)
        {
           var statusList = QuestionnaireManager.GetSectionStatuses();
           var CurrentSecStatus= statusList.Where(x => x.SectionId == secId && x.HR01==hr01).FirstOrDefault();
            if (CurrentSecStatus == null)
            {
                CurrentSecStatus= UpdateSectionStatus(CurrentVisit.ID00, secId, CurrentStatus.NotFilled, hr01);
                statusList.Add(CurrentSecStatus);
            }

            return CurrentSecStatus;
        }

        internal static SectionStatus UpdateSectionStatus(string ID00 , int secId, CurrentStatus currentStatus, int? HR01)
        {
            var statusList = QuestionnaireManager.GetSectionStatuses();
            var CurrentSecStatus = statusList.Where(x => x.SectionId == secId && x.HR01==HR01).FirstOrDefault();
            if (CurrentSecStatus == null)
            {
                CurrentSecStatus = new SectionStatus() { CurrentStatusId = (int)currentStatus, SectionId = secId, HR01=HR01,  ID00 = ID00, ID = Guid.NewGuid() };
              


            }


            CurrentSecStatus.CurrentStatusId = (int)currentStatus;

            var db = DataBase.GetConnection();
            db.InsertOrReplace(CurrentSecStatus);

            return CurrentSecStatus;
        }

        private static List<SectionStatus> SectionStatusList { get; set; }
        public static List<SectionStatus> GetSectionStatuses()
        {
            return SectionStatusList;
        }
        internal static List<SectionStatus> GetSectionStatusList(string ID00)
        {
            var db = DataBase.GetConnection();
            SectionStatusList = db.Table<SectionStatus>().Where(x => x.ID00 == ID00).ToList();
            return SectionStatusList;
        }

        //public static bool IsFirstVisit(string ID00)
        //{

        //    var db = DataBase.GetConnection();
        //   var  visit= db.Table<Visit>().Where(x => x.ID00 == ID00 && (x.IR03==1 || x.IR03 == 2)).FirstOrDefault();

        //    return visit == null;

        //}


        private static int GetIDH04()
        {
            var db = DataBase.GetConnection();
            var s = GeneralApplicationSettings.LocationForm.Locality;

            int max = 0;
            try
            {
                max = db.ExecuteScalar<int>("select  max(IDH04) from Visits where IDH04 is not null  and  E3=?", s.Code);

                //max = db.ExecuteScalar<int>("select  max(IDH04) from Visits where IDH04 is not null  and  E3=? and IDH03=?", s.Code, GeneralApplicationSettings.LocationForm.EnumArea);

            }
            catch
            {
                max = 0;
            }



            return  max + 1;
        }

        public static bool SaveCurrentVisit()
        {

            bool result = false;

            try
            {
                string userName = Security.CurrentUserSettings.CurrentUser.UserName;
                CurrentVisitLog.EndDate = DateTime.Now;
                CurrentVisitLog.IsComplete = CurrentVisit.IsComplete;
                CurrentVisitLog.E_X = GISCurrentLocation.CurrentX;
                CurrentVisitLog.E_Y = GISCurrentLocation.CurrentY;

                CurrentVisitLog.NeedSend = true;
                CurrentVisitLog.QC2 =Convert.ToInt32(CurrentVisit.QC2);
                CurrentVisit.LastModifiedDate = DateTime.Now;
                CurrentVisit.LastModifiedBy = userName;

                CurrentVisit.IsComplete = CurrentVisit.IsComplete;
                CurrentVisit.E_X = GISCurrentLocation.CurrentX;
                CurrentVisit.E_Y = GISCurrentLocation.CurrentY;
                CurrentVisit.NeedSend = true;

                if (CurrentVisit.QC2 == 1 || CurrentVisit.QC2 == 2)
                {
                    CurrentSample.QC1_1 = CurrentVisit.QC1_1;
                   
                    

                    if (CurrentSample.ID01 == "41")
                    {
                        CurrentSample.ID04 = CurrentVisit.ID04;
                        CurrentSample.ID05 = CurrentVisit.ID05;
                    }


                    CurrentSample.NeedSend = true;

                }


                var db = DataBase.GetConnection();

               

                GeneralApplicationSettings.NeedUpdateMap = true;

                if (CurrentCall != null)
                {
                    var diffInSeconds = (DateTime.Now - CurrentCall.C3).TotalSeconds;
                    CurrentCall.C9 = Convert.ToInt32(diffInSeconds);
                    db.InsertOrReplace(CurrentCall);

                }

                db.InsertOrReplace(CurrentVisitLog);

                db.InsertOrReplace(CurrentVisit);

                if (CurrentSample.NeedSend == true)
                {
                    db.InsertOrReplace(CurrentSample);
                }

                    result = true;
            }
            catch
            {

            }



            return result;

        }

       

        public async static Task<SectionPageData> GetSectionMaster(string ID00,  int sectionId, int? HR01=null,  bool createIfNotExists=true)
        {
          


            string filename = await GetFileName(ID00,sectionId, HR01);
            SectionPageData sectionPageData = null;
            if (File.Exists(filename))
            {
                string data = File.ReadAllText(filename);

                sectionPageData = JsonConvert.DeserializeObject<SectionPageData>(data);
            }


            if (sectionPageData == null && createIfNotExists )
            {
                string userName = Security.CurrentUserSettings.CurrentUser.UserName;

                SectionMaster sectionMaster = new SectionMaster() { VisitDate = DateTime.Now, SectionId = sectionId, ResearcherId = userName, CreatedBy = userName, CreatedDate = DateTime.Now, ID00 = ID00, IsComplete = false, Id = Guid.NewGuid(), LastModifiedBy = userName, LastModifiedDate = DateTime.Now, NeedSend = true, Comments = string.Empty };


                 sectionPageData = new SectionPageData();
                sectionPageData.OldAnswerList = new List<QuestionAnswer>();
                sectionPageData.SectionVisit = sectionMaster;
            }


            return sectionPageData;






        }


        public static async Task<string> GetFileName(string ID00,int sectionId, int? HR01)
        {
            string fileName = $"VIOL_{ID00.ToString()}_{sectionId}.json";

            if (HR01.HasValue)
            {
                fileName = $"VIOL_{ID00.ToString()}_{sectionId}_HR01_{HR01.Value}.json";
            }
            var path = await DependencyService.Get<IDatabaseSettings>().SampleSectionsFolder();




            string filename = Path.Combine(path, fileName);
            return filename;
        }

    }
}
