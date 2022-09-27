using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace PECS2022.Managers
{
    public class WebApiSettings
    {
        private const string ResearcherList = "api/Researcher/ListbyApp/{0}";
        private const string ProfessionList = "api/Profession/All";
        private const string LookUps = "api/Lookups/AllLookupsByApp/{0}";
        private const string EconomicActivityGroups = "api/EconomicActivity/Groups";
        private const string EconomicActivityDetails = "api/EconomicActivity/All";
        private const string GovernorateList = "/api/LookUps/GovernoratesByAppId/{0}";
        private const string LocalityList = "/api/LookUps/AllLocalities";
        //TODO On Publish - App ID
        private const string AppIdValue = "53";
        private const string OnlineLoginStr = "/api/Researcher/onlineLogin/{0}/{1}/{2}";
        private const string PortalWebApiAppName = "PortalServices/";

        private static string _FullPortalServiceURL = null;

        public static string FullPortalServiceURL
        {
            get
            {
                if (_FullPortalServiceURL == null)
                {

                    _FullPortalServiceURL = GeneralApplicationSettings.WEBServerPath;

                }

                return _FullPortalServiceURL;

            }

            private set
            {
                _FullPortalServiceURL = value;

            }
        }


        #region RemoteReachable Host

        //TODO : On Publish - Ping Host
        private const string PingHost = "172.20.2.110";
        private const int PingPort = 8015;

        public static string RemoteReachableHost
        {
            get { return PingHost; }
        }

        public static int RemoteReachablePort
        {
            get { return PingPort; }
        }
        #endregion

        public static string OnlineLogin(string userName, string password)
        {
            var val = OnlineLoginStr;
            return GetWebApiPath(val, AppIdValue, userName, password);
        }


        public static string GetWebApiPath(string appiURL, params string[] values)
        {

            return string.Format(string.Format("{0}{1}", WebApiSettings.PortalWebApiAppName, appiURL), values);

        }

        public static string ResearcherListURL
        {
            get { return GetWebApiPath(ResearcherList, AppIdValue); }
        }

        public static string ProfessionListURL
        {
            get
            {
                return GetWebApiPath(ProfessionList);
            }
        }

        public static string LookUpsURL
        {
            get
            {
                return GetWebApiPath(LookUps, AppIdValue);
            }
        }

        public static string EconomicActivityGroupsURL
        {
            get
            {
                return GetWebApiPath(EconomicActivityGroups);
            }
        }

        public static string GovernorateListURL
        {
            get
            {
                return GetWebApiPath(GovernorateList, AppIdValue);
            }
        }

        public static string LocalityListURL
        {
            get
            {
                return GetWebApiPath(LocalityList, AppIdValue);
            }
        }
        public static string EconomicActivityDetailsURL
        {
            get
            {
                return GetWebApiPath(EconomicActivityDetails);
            }
        }




    }
}