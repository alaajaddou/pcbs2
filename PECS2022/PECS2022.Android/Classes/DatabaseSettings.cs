using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using PECS2022.Droid.Classes;
using PECS2022.Interfaces;

[assembly: Xamarin.Forms.Dependency(typeof(DatabaseSettings))]
namespace PECS2022.Droid.Classes
{
    public class DatabaseSettings : IDatabaseSettings
    {

        public string DatabasePath
        {
            get
            {

                string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), GeneralApplicationSettings.DataBaseName);

#if DEBUG
                System.Diagnostics.Debug.WriteLine("Database => " + dbPath);
#endif

                return dbPath;
            }
        }

        public string DatabaseFolderPath
        {
            get
            {
                string dbPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);

                return dbPath;
            }
        }

        public string BackupDatabasePath
        {
            get
            {

                string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), $"{GeneralApplicationSettings.BackupDataBasePrefix}_{DateTime.Now.ToString("ddMMyyyyhhmmss")}.db3");

                return dbPath;
            }
        }


        public async Task<string> SampleSectionsFolder()
        {
            string dbPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData);
            //StorageFolder storageFolder = await StorageFolder.GetFolderFromPathAsync(dbPath);

            var folderPath = Path.Combine(dbPath, GeneralApplicationSettings.SurveyDataLabel);
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            return folderPath;
        }


        //public string GetSDCardLocation()
        // {
        //     try
        //     {
        //         var allStatus = new Dictionary<Permission, PermissionStatus>();
        //         var status = await CrossPermissions.Current.CheckPermissionStatusAsync(permission);
        //         if (status != PermissionStatus.Granted)
        //         {

        //             if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.LocationAlways))
        //             {
        //                 await MainPage.DisplayAlert("الصلاحيات", "النظام بحاجة الى الوصول الى صلاحية" + permission.ToString(), GeneralMessages.Cancel);
        //             }

        //             allStatus = await CrossPermissions.Current.RequestPermissionsAsync(Permission.LocationAlways);
        //         }

        //         if (status == PermissionStatus.Granted || allStatus[Permission.LocationAlways] == PermissionStatus.Granted)
        //         {
        //             result = true;
        //         }



        //     }
        //     catch
        //     {
        //         //Something went wrong
        //     }

        // }

        public async Task<string> SampleSectionsFolderBackUpFile()
        {
            DirectoryInfo directoryInfo = await FileStorageAndroid.GetSDCardPathAsync();

            var sdCardPath = directoryInfo.ToString();
            var backupDirectory = directoryInfo.GetDirectories().Where(x => x.Name.Contains("Backups")).FirstOrDefault();

            if (backupDirectory == null)
            {
                string backupPath = Path.Combine(sdCardPath.ToString(), "Backups");

                Directory.CreateDirectory(backupPath);
                return backupPath;
            }

            return backupDirectory.ToString();


        }
    }
}
