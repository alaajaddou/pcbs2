using PECS2022.UWP.Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Search;
using static System.Environment;

[assembly: Xamarin.Forms.Dependency(typeof(FileStorageUWP))]

namespace PECS2022.UWP.Classes
{
    public class FileStorageUWP : Interfaces.IFileStorage
    {
        SpecialFolder TPKStorageFolder = Environment.SpecialFolder.LocalApplicationData;
        SpecialFolder GEODBStorageFolder = Environment.SpecialFolder.LocalApplicationData;



        public async Task<string> GetGEOLocalPath()
        {

            return await Task.Run<string>(() => { return Path.Combine(GetFolderPath(GEODBStorageFolder), "GEODB"); });



        }

        public async Task<string> GetTPKLocalPath()
        {
            return await Task.Run<string>(() => { return Path.Combine(GetFolderPath(TPKStorageFolder), "TPKs"); });


           
           
        }

        public async Task<bool> MoveGEODBToLocal(string fileName)
        {

            bool result = true;

            try
            {
                StorageFolder storageFolder = await StorageFolder.GetFolderFromPathAsync(GetFolderPath(GEODBStorageFolder));

                var targetFolder = await storageFolder.CreateFolderAsync("GEODB", CreationCollisionOption.OpenIfExists);
                if (targetFolder == null)
                {
                    targetFolder = await storageFolder.CreateFolderAsync("GEODB");
                    //targetFolder =Path.Combine(targetFolder,fileName.Substring(0,4))
                }


                var orginalFolder = await Windows.Storage.AccessCache.StorageApplicationPermissions.
                    FutureAccessList.GetFolderAsync("GEODBFolder");

                var subFolder =await orginalFolder.GetFolderAsync(fileName.Substring(0, 4));

               
                var extension = Path.GetExtension(fileName);
                var queryOptions = new QueryOptions(CommonFileQuery.DefaultQuery, new[] { $"{extension}" });

                var query = subFolder.CreateFileQueryWithOptions(queryOptions);

                var files = await query.GetFilesAsync();

                var file = files.Where(x => x.Name == fileName).FirstOrDefault();

                if (file != null)
                {
                    await file.CopyAsync(targetFolder, fileName, NameCollisionOption.ReplaceExisting);
                }


            }
            catch
            {
                result = false;
            }


            return result;

        }

        public async Task<bool> MoveTPKToLocal(string fileName)
        {
            bool result = true;

            try
            {
                StorageFolder storageFolder = await StorageFolder.GetFolderFromPathAsync(GetFolderPath(GEODBStorageFolder));

                var targetFolder = await storageFolder.CreateFolderAsync("TPKs", CreationCollisionOption.OpenIfExists);
                if (targetFolder == null)
                {
                    targetFolder = await storageFolder.CreateFolderAsync("TPKs");
                }


                var orginalFolder = await Windows.Storage.AccessCache.StorageApplicationPermissions.
                    FutureAccessList.GetFolderAsync("TPKFolder");
               var extension= Path.GetExtension(fileName);
                var queryOptions = new QueryOptions(CommonFileQuery.DefaultQuery, new[] { $"{extension}" });
               
                var query = orginalFolder.CreateFileQueryWithOptions(queryOptions);
            
                var files = await query.GetFilesAsync();

                 var  file =files.Where(x => x.Name == fileName).FirstOrDefault();

                if (file != null)
                {
                 await   file.CopyAsync(targetFolder, fileName, NameCollisionOption.ReplaceExisting);
                }


            }
            catch
            {
                result = false;
            }


            return result;
        }

        public async Task<bool> SaveBackupFile(string filepath)
        {

            var savePicker = new Windows.Storage.Pickers.FileSavePicker();
            savePicker.SuggestedStartLocation =
                Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;
            // Dropdown of file types the user can save the file as
            savePicker.FileTypeChoices.Add("Zip file", new List<string>() { ".zip" });
            // Default file name if the user does not type one in or select a file to replace
            savePicker.SuggestedFileName = Path.GetFileName(filepath);
            Windows.Storage.StorageFile file = await savePicker.PickSaveFileAsync();


            var fileToCopy = await StorageFile.GetFileFromPathAsync(filepath);

            if (file != null)
            {

                await fileToCopy.CopyAndReplaceAsync(file);
                // Prevent updates to the remote version of the file until
                // we finish making changes and call CompleteUpdatesAsync.
                //Windows.Storage.CachedFileManager.DeferUpdates(file);
                // write to file
                //await Windows.Storage.FileIO.res(file)
                //// Let Windows know that we're finished changing the file so
                //// the other app can update the remote version of the file.
                //// Completing updates may require Windows to ask for user input.
                //Windows.Storage.Provider.FileUpdateStatus status =
                //    await Windows.Storage.CachedFileManager.CompleteUpdatesAsync(file);
                //if (status == Windows.Storage.Provider.FileUpdateStatus.Complete)
                //{
                //    this.textBlock.Text = "File " + file.Name + " was saved.";
                //}
                //else
                //{
                //    this.textBlock.Text = "File " + file.Name + " couldn't be saved.";
                //}
            }
            else
            {

                return false;
                // this.textBlock.Text = "Operation cancelled.";
            }

            return true;
        }

        public Task<string> SelectBackUpFolder()
        {
            throw new NotImplementedException();
        }

        public async Task<string> SelectGEODBFolder()
        {
            var folderPicker = new FolderPicker();
            folderPicker.SuggestedStartLocation = PickerLocationId.ComputerFolder;
            folderPicker.FileTypeFilter.Add("*");

            Windows.Storage.StorageFolder folder = await folderPicker.PickSingleFolderAsync();
            if (folder != null)
            {


                // Application now has read/write access to all contents in the picked folder
                // (including other sub-folder contents)


                Windows.Storage.AccessCache.StorageApplicationPermissions.
                FutureAccessList.AddOrReplace("GEODBFolder", folder);

             
                return folder.Path;

            }
            else
            {
                return string.Empty;
            }

        }

        public async Task<string> SelectTPKFolder()
        {
            var folderPicker = new FolderPicker();
            folderPicker.SuggestedStartLocation = PickerLocationId.ComputerFolder;
            folderPicker.FileTypeFilter.Add("*");

            Windows.Storage.StorageFolder folder = await folderPicker.PickSingleFolderAsync();
            if (folder != null)
            {


                // Application now has read/write access to all contents in the picked folder
                // (including other sub-folder contents)


                Windows.Storage.AccessCache.StorageApplicationPermissions.
                FutureAccessList.AddOrReplace("TPKFolder", folder);


                return folder.Path;

            }
            else
            {
                return string.Empty;
            }
        }
    }
}
