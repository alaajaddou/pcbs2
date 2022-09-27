using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PECS2022.Interfaces
{
   public  interface IFileStorage 
    {

        Task<string> SelectTPKFolder();

        Task<String> SelectBackUpFolder();
        Task<string> SelectGEODBFolder();


        Task<string> GetTPKLocalPath();
        Task<string> GetGEOLocalPath();


        Task<bool> MoveTPKToLocal(string fileName);
        Task<bool> MoveGEODBToLocal(string fileName);


        Task<bool> SaveBackupFile(string filepath);

    }
}
