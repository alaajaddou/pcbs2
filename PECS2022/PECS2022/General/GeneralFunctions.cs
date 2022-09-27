using PECS2022.Interfaces;
using PECS2022.Managers;
using PECS2022.Models;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PECS2022
{
    public static class GeneralFunctions
    {
        public static async Task<bool> UpdateSamples()
        {
            var db = await DataBase.GetAsyncConnection();

            var sampleResult = await WebApiDataManager.GetListAsync<SampleInfo>("/PECSServices/api/Samples/Samples", true);
            if (sampleResult.IsSuccess)
            {
              await  db.Table<Individual>().DeleteAsync(X => X.ID00 != null);
              await  db.Table<SampleInfo>().DeleteAsync(X => X.ID00 != null);

                var individuals = sampleResult.Data.SelectMany(x => x.Individuals).ToList();
                var samples = sampleResult.Data;

                individuals.ForEach((ind) => { ind.ID = Guid.NewGuid(); });
              

               await db.InsertAllAsync(samples);
               await db.InsertAllAsync(individuals);
                return true;
            }

            return false;
        }

        public  static bool NetworkAvailable()
        {
            return true;
            //return CrossConnectivity.Current.IsConnected && await CrossConnectivity.Current.IsRemoteReachable(WebApiSettings.RemoteReachableHost, WebApiSettings.RemoteReachablePort, 5000);
        }

        public static string GetGovPrefix(string GovCode)
        {
            string prefix = "24";

            if (GeneralApplicationSettings.northGovs.Contains(GovCode))
            {
                prefix = "11";
            }

            else if (GeneralApplicationSettings.midleGovs.Contains(GovCode))
            {
                prefix = "12";
            }
            else if (GeneralApplicationSettings.westGovs.Contains(GovCode))
            {
                prefix = "13";
            }

            return prefix;
        }
    }
}
