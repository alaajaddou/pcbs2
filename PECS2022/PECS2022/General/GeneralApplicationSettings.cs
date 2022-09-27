using PECS2022.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PECS2022
{
   public static class GeneralApplicationSettings
    {
        //TODO On Publish - Database
        public static readonly string DataBaseName = "PPCWG2022_V2.db3";
        public static readonly string BackupDataBasePrefix = "PPCWG2022_V2";

        public static readonly string SurveyDataLabel = "SurveyData_V2";

        public static string WEBServerPath
        {
            get
            {
                if (Xamarin.Forms.Application.Current.Properties.ContainsKey(ApplicationSettingKeys.WEBServerPath))
                {
                    return Xamarin.Forms.Application.Current.Properties[ApplicationSettingKeys.WEBServerPath] as string;
                }

                //TODO On Publish - Server Path
                return @"http://172.20.2.110:8015";
            }
            set
            {
               Xamarin.Forms.Application.Current.Properties[ApplicationSettingKeys.WEBServerPath] = value;
                Xamarin.Forms.Application.Current.SavePropertiesAsync();
            }
        }

        public static string TPKPath
        {
            get
            {
                if (Xamarin.Forms.Application.Current.Properties.ContainsKey(ApplicationSettingKeys.TPKPath))
                {
                    return Xamarin.Forms.Application.Current.Properties[ApplicationSettingKeys.TPKPath] as string;
                }
                return string.Empty;
            }
            set
            {
                Xamarin.Forms.Application.Current.Properties[ApplicationSettingKeys.TPKPath] = value;
                Xamarin.Forms.Application.Current.SavePropertiesAsync();
            }
        }

        public static string GEOPath
        {
            get
            {
                if (Xamarin.Forms.Application.Current.Properties.ContainsKey(ApplicationSettingKeys.GEOPath))
                {
                    return Xamarin.Forms.Application.Current.Properties[ApplicationSettingKeys.GEOPath] as string;
                }
                return string.Empty;
            }
            set
            {
                Xamarin.Forms.Application.Current.Properties[ApplicationSettingKeys.GEOPath] = value;
                Xamarin.Forms.Application.Current.SavePropertiesAsync();
            }
        }

        public static bool GeneralSettingsSaved
        {
            get
            {
                if (Xamarin.Forms.Application.Current.Properties.ContainsKey(ApplicationSettingKeys.GeneralSettingsSaved))
                {
                    return (bool)Xamarin.Forms.Application.Current.Properties[ApplicationSettingKeys.GeneralSettingsSaved] ;
                }
                return false;
            }
            set
            {
                Xamarin.Forms.Application.Current.Properties[ApplicationSettingKeys.GeneralSettingsSaved] = value;
                Xamarin.Forms.Application.Current.SavePropertiesAsync();
            }
        }


        //IsFirstTimeSyncDone

        public static bool IsFirstTimeSyncDone
        {
            get
            {
                if (Xamarin.Forms.Application.Current.Properties.ContainsKey(ApplicationSettingKeys.IsFirstTimeSyncDone))
                {
                    return (bool)Xamarin.Forms.Application.Current.Properties[ApplicationSettingKeys.IsFirstTimeSyncDone];

                   
                }
                return false;
            }
            set
            {
                Xamarin.Forms.Application.Current.Properties[ApplicationSettingKeys.IsFirstTimeSyncDone] = value;
                Xamarin.Forms.Application.Current.SavePropertiesAsync();
            }
        }

        public static bool NeedUpdateMap { get; internal set; }
        public static bool NeedUpdateLocs { get; internal set; } = true;

        public static LocationForm LocationForm { get; set; }

        public readonly static string[] northGovs = new string[] { "01", "05", "10", "15", "20", "25" };
        public readonly static string[] midleGovs = new string[] { "30", "35", "40", "41" };
        public readonly static string[] westGovs = new string[] { "45", "50" };
        public readonly static string[] GazaGovs = new string[] { "55", "60", "65", "70", "75" };

        public readonly static string[] WBGovs = new string[] { "01", "05", "10", "15", "20", "25", "30", "35", "40", "41","45", "50" };
    }
}
