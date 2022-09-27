using PECS2022.Interfaces;
using PECS2022.UWP.Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

[assembly: Xamarin.Forms.Dependency(typeof(DatabaseSettings))]

namespace PECS2022.UWP.Classes
{
    public class DatabaseSettings : IDatabaseSettings
    {

        public string DatabasePath
        {
            get
            {
                
                string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), GeneralApplicationSettings.DataBaseName);

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
                string dbPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

                return dbPath;
            }
        }

        public string BackupDatabasePath
        {
            get
            {

                string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), $"{GeneralApplicationSettings.BackupDataBasePrefix}_{DateTime.Now.ToString("ddMMyyyyhhmmss")}.db3");

                return dbPath;
            }
        }


        public async Task<string> SampleSectionsFolder()
        {
                string dbPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                StorageFolder storageFolder = await StorageFolder.GetFolderFromPathAsync(dbPath);
                var targetFolder = await storageFolder.CreateFolderAsync(GeneralApplicationSettings.SurveyDataLabel, CreationCollisionOption.OpenIfExists);
                if (targetFolder == null)
                {
                    targetFolder = await storageFolder.CreateFolderAsync(GeneralApplicationSettings.SurveyDataLabel);
                    //targetFolder =Path.Combine(targetFolder,fileName.Substring(0,4))
                }

                return targetFolder.Path;
            
        }

        public async Task<string> SampleSectionsFolderBackUpFile()
        {
            string dbPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            
            StorageFolder storageFolder = await StorageFolder.GetFolderFromPathAsync(dbPath);

            var folderName = $"{GeneralApplicationSettings.SurveyDataLabel}_{DateTime.Now.ToString("ddMMyyyyhhmmss")}";
            var targetFolder = await storageFolder.CreateFolderAsync(folderName, CreationCollisionOption.OpenIfExists);
            if (targetFolder == null)
            {
                targetFolder = await storageFolder.CreateFolderAsync(folderName);
                //targetFolder =Path.Combine(targetFolder,fileName.Substring(0,4))
            }

            return targetFolder.Path;
        }
    }
}
