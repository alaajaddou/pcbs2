using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PECS2022.Interfaces
{
    public interface IDatabaseSettings
    {





        string DatabasePath { get; }
        string BackupDatabasePath { get; }

        string DatabaseFolderPath { get; }


        Task<string> SampleSectionsFolder();

        Task<string> SampleSectionsFolderBackUpFile();
    }
}
