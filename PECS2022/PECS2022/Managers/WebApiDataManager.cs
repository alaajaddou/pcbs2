using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Http;
using PECS2022.Security;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using PECS2022.Models;

namespace PECS2022.Managers
{
  public  class WebApiDataManager
    {

        #region General Functions
        public static HttpClient GetHttpClient(bool isAuthenticated = true)
        {
            return GetHttpClient(WebApiSettings.FullPortalServiceURL, isAuthenticated);
        }

        public static HttpClient GetHttpClient(string baseUrl, bool isAuthenticated = true)
        {
            HttpClient client = new HttpClient();

            client = new HttpClient();
            if (isAuthenticated)
            {
                client.DefaultRequestHeaders.Authorization = CurrentUserSettings.GetAuthorizationKey();
            }
            client.BaseAddress = new Uri(baseUrl);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Accept.Add(
              new MediaTypeWithQualityHeaderValue("image/png"));

            return client;
        }

        public static DownloadResult<T> GetList<T>(string url, bool isAuthenticated = true) where T : class
        {
            bool success = false;
            var list = new List<T>();
            try
            {
                var client = GetHttpClient(isAuthenticated);

               

                HttpResponseMessage response = client.GetAsync(url).Result;
                if (response.IsSuccessStatusCode)
                {
                    success = true;
                    list = response.Content.ReadAsAsync<List<T>>().Result;

                }

            }
            catch
            {
                success = false;
            }

            return new DownloadResult<T>(success, list);
        }
        public static async Task<DownloadResult<T>> GetListAsync<T>(string url, bool isAuthenticated = true) where T : class
        {
            bool success = false;
            var list = new List<T>();
            try
            {
                var client = GetHttpClient(isAuthenticated);

               

                HttpResponseMessage response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    success = true;
                    list = await response.Content.ReadAsAsync<List<T>>();

                }
            }
            catch(Exception e)
            {
                success = false;
            }
            return new DownloadResult<T>(success, list);
        }

        internal static DownloadResult<User> OnlineLogin( string userName, string password)
        {
            bool success = false;

            try
            {
                var client = GetHttpClient(false);

                HttpResponseMessage response = client.GetAsync(WebApiSettings.OnlineLogin(userName, password)).Result;
                if (response.IsSuccessStatusCode)
                {
                    success = true;
                    var user = response.Content.ReadAsAsync<User>().Result;

                    if (user != null)
                    {

                        return new DownloadResult<User>(success, user);
                    }

                }

            }
            catch
            {
                success = false;
            }

            return new DownloadResult<User>(false,value:null);
        }

        public static DownloadResult<T> GetList<T>(HttpClient client, string url, bool isAuthenticated = true) where T : class
        {
            bool success = false;


            var list = new List<T>();

            try
            {

                HttpResponseMessage response = client.GetAsync(url).Result;
                if (response.IsSuccessStatusCode)
                {
                    success = true;
                    list = response.Content.ReadAsAsync<List<T>>().Result;

                }

            }
            catch
            {
                success = false;
            }

            return new DownloadResult<T>(success, list);
        }


        public static async Task<DownloadResult<T>> GetListAsync<T>(HttpClient client, string url, bool isAuthenticated = true) where T : class
        {
            bool success = false;


            var list = new List<T>();

            try
            {

                HttpResponseMessage response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    success = true;
                    list = await response.Content.ReadAsAsync<List<T>>();

                }

            }
            catch
            {
                success = false;
            }

            return new DownloadResult<T>(success, list);
        }

        public static DownloadResult<T> GetList<T>(string baseUrl, string url, bool isAuthenticated = true) where T : class
        {
            bool success = false;
            var list = new List<T>();
            try
            {

                var client = GetHttpClient(baseUrl, isAuthenticated);
               

                HttpResponseMessage response = client.GetAsync(url).Result;
                if (response.IsSuccessStatusCode)
                {
                    success = true;
                    list = response.Content.ReadAsAsync<List<T>>().Result;

                }

            }
            catch
            {
                success = false;
            }

            return new DownloadResult<T>(success, list);
        }


        public static bool UploadList<T>(string baseUrl, string url, List<T> lst, bool isAuthenticated = true) where T : class
        {


            bool returnValue = false;
            try
            {
                var client = GetHttpClient(baseUrl, isAuthenticated);
                var list = new List<T>();

                HttpResponseMessage response = client.PostAsJsonAsync(url, lst);
                if (response.IsSuccessStatusCode)
                {

                    returnValue = response.Content.ReadStructAsAsync<bool>().Result;

                }
            }
            catch
            {
                returnValue = false;
            }


            return returnValue;
        }

        public static bool Upload<T>(string baseUrl, string url, T item, bool isAuthenticated = true) where T : class
        {

            bool returnValue = false;
            try
            {
                var client = GetHttpClient(baseUrl, isAuthenticated);
                var list = new List<T>();

                HttpResponseMessage response = client.PostAsJsonAsync(url, item);
                if (response.IsSuccessStatusCode)
                {

                    returnValue = response.Content.ReadStructAsAsync<bool>().Result;

                }

            }
            catch
            {
                returnValue = false;
            }

            return returnValue;
        }


        public static async Task<DownloadResult<T>> GetListAsync<T>(string baseUrl, string url, bool isAuthenticated = true) where T : class
        {
            bool success = false;
            var list = new List<T>();

            try { 

            var client = GetHttpClient(baseUrl, isAuthenticated);
         

            HttpResponseMessage response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                success = true;
                list = await response.Content.ReadAsAsync<List<T>>();

            }

            }
            catch
            {
                success = false;
            }

            return new DownloadResult<T>(success, list);
        }

        #endregion 

        #region Govenorate

        public static DownloadResult<Governorate> GetGovernorateList()
        {

            return GetList<Governorate>(WebApiSettings.GovernorateListURL, false);

        }

        public static async Task<DownloadResult<Governorate>> GetGovernorateListAsync()
        {
            return await GetListAsync<Governorate>(WebApiSettings.GovernorateListURL);
        }



        #endregion

        #region Locality



        public static DownloadResult<Locality> GetLocalityList()
        {
            return GetList<Locality>(WebApiSettings.LocalityListURL);

        }
        public static async Task<DownloadResult<Locality>> GetLocalityListAsync()
        {
            return await GetListAsync<Locality>(WebApiSettings.LocalityListURL);

        }


        #endregion

        #region Researchers
        public static DownloadResult<User> GetResearcherList()
        {
            return GetList<User>(WebApiSettings.ResearcherListURL, false);

        }
        public static async Task<DownloadResult<User>> GetResearcherListAsync()
        {
            return await GetListAsync<User>(WebApiSettings.ResearcherListURL, false);

        }


        #endregion

        #region Economic Group
        public static DownloadResult<EconomicGroup> GetEconomicGroupList()
        {
            return GetList<EconomicGroup>(WebApiSettings.EconomicActivityGroupsURL);

        }
        public static async Task<DownloadResult<EconomicGroup>> GetEconomicGroupListAsync()
        {
            return await GetListAsync<EconomicGroup>(WebApiSettings.EconomicActivityGroupsURL);

        }


        #endregion
    
        #region Economic Details
        public static DownloadResult<EconomicActivity> GetEconomicActivityList()
        {
            return GetList<EconomicActivity>(WebApiSettings.EconomicActivityDetailsURL);

        }
        public static async Task<DownloadResult<EconomicActivity>> GetEconomicActivityListAsync()
        {
            return await GetListAsync<EconomicActivity>(WebApiSettings.EconomicActivityDetailsURL);

        }

        #endregion

        #region LookupVal
        public static DownloadResult<LookupVal> GetLookUpValueList()
        {
            return GetList<LookupVal>(WebApiSettings.LookUpsURL);

        }
        public static async Task<DownloadResult<LookupVal>> GetLookUpValueListAsync()
        {
            return await GetListAsync<LookupVal>(WebApiSettings.LookUpsURL);

        }

        #endregion

        #region Profession
        public static DownloadResult<Profession> GetProfessionList()
        {
            return GetList<Profession>(WebApiSettings.ProfessionListURL);

        }
        public static async Task<DownloadResult<Profession>> GetProfessionListAsync()
        {
            return await GetListAsync<Profession>(WebApiSettings.ProfessionListURL);

        }

        #endregion

        public static  async Task<byte[]> DownloadImageAsync(string imageUrl, bool isAuth=false)
        {
            try
            {
                var httpClient = GetHttpClient(isAuth);
                string path = WebApiSettings.FullPortalServiceURL + imageUrl;
                return await httpClient.GetByteArrayAsync(path);
            }
            catch
            {
                //Handle Exception
                return null;
            }
        }
    }
}
