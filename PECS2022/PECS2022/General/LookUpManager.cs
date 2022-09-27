using PECS2022.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PECS2022
{
    public class LookUpManager
    {

       
        internal static List<LookupVal> LookupVals
        {
            get; set;
        }


        internal static List<LookupVal> LookupGenderVals
        {
            get; set;
        }


        public static LookupVal DefaultLookupValue { get; set; } = new LookupVal() { Id = -1, Code = "0", Description = "اختيار" };


        public static  List<LookUpValueInfo> GetSurveyLookupById(int id)
        {
           var CurrentSurvey = ApplicationMainSettings.GetSurveyInfo();

            return CurrentSurvey.LookUps.Where(x => x.Id == id).SelectMany(x => x.LookUpValues).ToList();
        }
        public static async Task<List<LookupVal>> GetLookupVals(string lookupCode)
        {
            if (LookupVals == null)
            {
                var db = await DataBase.GetAsyncConnection();
                LookupVals = await db.Table<LookupVal>().ToListAsync();
            }
            var result = LookupVals.FindAll(x => x.LookUpCode == lookupCode);
            result.Insert(0, DefaultLookupValue);
            return result;
        }

        public static async Task<List<LookupVal>> GetLookupGenderVals(string lookupCode)
        {
            if (LookupGenderVals == null)
            {
                var db = await DataBase.GetAsyncConnection();
                LookupGenderVals = await db.Table<LookupVal>().ToListAsync();
            }
            var result = LookupGenderVals.FindAll(x => x.LookUpCode == lookupCode);
            result.Insert(0, DefaultLookupValue);
            return result;
        }

    }
}
