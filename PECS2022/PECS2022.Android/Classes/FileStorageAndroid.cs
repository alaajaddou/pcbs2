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
using Java.IO;
using Java.Util.Zip;
using PECS2022.Droid.Classes;
using Xamarin.Forms;
//using Plugin.CurrentActivity;

[assembly: Xamarin.Forms.Dependency(typeof(FileStorageAndroid))]

namespace PECS2022.Droid.Classes
{
    public class FileStorageAndroid : Interfaces.IFileStorage
    {
        public async Task<string> GetGEOLocalPath()
        {

            var path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);


            return Path.Combine(path, "GEODB");
            //// await new Task(() => { });

            //     var drives = DriveInfo.GetDrives().Where(x => x.DriveType == DriveType.Ram && x.Name.Contains("emulated") == false).ToList();

            //     var sdCardPath = drives[0].RootDirectory;

            // // var GEODirectory = drives[0].RootDirectory.GetDirectories().Where(x => x.Name.Contains("GEODBS")).FirstOrDefault();
            // var GEODirectory = drives[0].RootDirectory;

            //     if (GEODirectory != null)
            //     {
            //     return   GEODirectory.ToString(); ;

            //     }

            //     return string.Empty;


            //  return await Task.Run<string>(() => { return "GEODB"; });
        }

        public async Task<string> GetTPKLocalPath()
        {


            //var sdCardPath = Android.OS.Environment.ExternalStorageDirectory.Path;

            DirectoryInfo directoryInfo = await GetSDCardPathAsync();

            //var TPKDirectory = directoryInfo.GetDirectories().Where(x => x.Name.Contains("TPKs")).FirstOrDefault();

            //if (TPKDirectory != null)
            //{
            //    return TPKDirectory.ToString();

            //}

            return directoryInfo.ToString();
        }

        public async Task<bool> MoveGEODBToLocal(string fileName)
        {
            bool result = false;

            try
            {
                var path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
                var geoDB = Path.Combine(path, "GEODB");
                // DirectoryInfo dir = new DirectoryInfo(path);

                if (!Directory.Exists(geoDB))
                {
                    Directory.CreateDirectory(geoDB);
                }


                var filePath = Path.Combine(geoDB, fileName);

                if (!System.IO.File.Exists(filePath))
                {
                    //var sdCardPath = Android.OS.Environment.ExternalStorageDirectory.Path;

                    DirectoryInfo directoryInfo = await GetSDCardPathAsync();

                    var TPKDirectory = directoryInfo;

                    if (TPKDirectory != null)
                    {

                        string govCode = fileName.Substring(0, 4);

                        var sourcePath = Path.Combine(TPKDirectory.ToString(), govCode, fileName);

                        System.IO.File.Copy(sourcePath, filePath);
                        result = true;
                    }
                }

                else
                {
                    result = true;
                }






            }
            catch
            {
                result = false;
            }


            return result;
            // return await Task.Run<bool>(() => { return false; });
        }

        public async Task<bool> MoveTPKToLocal(string fileName)
        {
            return await Task.Run<bool>(() => { return false; });

        }

        public async Task<string> SelectGEODBFolder()
        {
            //   var ddd = DriveInfo.GetDrives().Where(x =>  x.Name.Contains("emulated") == false);

            //Directory dir=
            //  var drives = DriveInfo.GetDrives().Where(x => x.DriveType == DriveType.Ram && x.Name.Contains("emulated") == false).ToList();


            // var GEODirectory = drives[0].RootDirectory.GetDirectories().Where(x => x.Name.Contains("GEODBS")).FirstOrDefault();
            //if ((int)Android.OS.Build.VERSION.SdkInt > 28)
            //{
            //    var path2 = Path.Combine(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath);
            //    return path2;
            //}
            var GEODirectory = await GetSDCardPathAsync();

            if (GEODirectory != null)
            {
                var dirs = GEODirectory.GetDirectories();
                return GEODirectory.FullName.ToString();

            }

            return string.Empty;


        }

        public async Task<string> SelectTPKFolder()
        {



            //var sdCardPath =new DirectoryInfo(Android.OS.Environment..Path);
            // Java.IO.File[] dirs =Android.App.Application.Context.GetExternalFilesDirs(null);



            //var drives2 = DriveInfo.GetDrives().Where(x => (x.Name.Contains("storage"))).ToList();
            //

            //var child= drives2[0].RootDirectory.GetDirectories();
            //bool s = false;
            //if ((int)Android.OS.Build.VERSION.SdkInt > 28)
            //{
            //    var path2 = Path.Combine(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath, "TPKs");
            //    return path2;
            //}
            //else
            //{

            var path = await GetSDCardPathAsync();
            //if (path != null)
            //{

            //    return path.ToString();
            //}

            //if (path != null)
            //{
            //    var dir = path.GetDirectories();
            //    var TPKDirectory = dir.Where(x => x.Name.Contains("TPKs")).FirstOrDefault();

            //    if (TPKDirectory != null)
            //    {
            //        return TPKDirectory.ToString();

            //    }
            //}

            // }
            return path.ToString();





        }
        private static async Task<bool> CheckPermissions(Plugin.Permissions.Abstractions.Permission permission)
        {

            bool result = false;
            try
            {
                var allStatus = new Dictionary<Plugin.Permissions.Abstractions.Permission, Plugin.Permissions.Abstractions.PermissionStatus>();
                var status = await Plugin.Permissions.CrossPermissions.Current.CheckPermissionStatusAsync(permission);
                if (status != Plugin.Permissions.Abstractions.PermissionStatus.Granted)
                {

                    if (await Plugin.Permissions.CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(permission))
                    {

                    }

                    allStatus = await Plugin.Permissions.CrossPermissions.Current.RequestPermissionsAsync(permission);
                }

                if (status == Plugin.Permissions.Abstractions.PermissionStatus.Granted || allStatus[permission] == Plugin.Permissions.Abstractions.PermissionStatus.Granted)
                {
                    result = true;
                }



            }
            catch
            {
                //Something went wrong
            }

            return result;
        }


        public static async Task<DirectoryInfo> GetSDCardPathAsync()
        {

            bool granted = await CheckPermissions(Plugin.Permissions.Abstractions.Permission.Storage);

            if (granted)
            {

                Java.IO.File[] dirs = Android.App.Application.Context.GetExternalFilesDirs(null);

                var dir = Android.OS.Environment.ExternalStorageDirectory;

                // Android.OS.Environment.GetExternalStoragePublicDirectory(null);
                //DirectoryInfo dir = new DirectoryInfo(@"/storage/3362-3738/");
                //var dir = Android.App.Application.Context.EXter;
                Java.IO.File toResturn = dir;
                if (dirs.Where(x=>x!=null).ToList().Count > 1)
                {
                    var sdKPath = dirs.Where(x => x != null).ToList().Where(x => x.Path.StartsWith(dir.Path) == false).FirstOrDefault();
                    toResturn = sdKPath;

                }



                if (toResturn != null)
                {
                    string[] data = toResturn.Path.Split("Android");

                    if (data.Count() > 0)
                    {
                        var path = data[0];

                        DirectoryInfo directoryInfo = new DirectoryInfo(path);
                        //var drives = DriveInfo.GetDrives().Where(x => (x.Name.Contains(directoryInfo.Name) && x.Name.Contains("storage"))).ToList();
                        return directoryInfo; //drives[0].RootDirectory;


                    }
                }

                return null;
                //var files= dir.GetFiles();
                //Java.IO.File[] rootDrive = Java.IO.File.ListRoots();

                // return dir;

                //var path =await MainActivity.Instance.OpenDocumentTreeAsync();
                //if (!string.IsNullOrWhiteSpace(path))
                //{


                //    DirectoryInfo directoryInfo = new DirectoryInfo(path);
                //    return directoryInfo;
                //}
                //if ((int)Android.OS.Build.VERSION.SdkInt > 28)
                //{
                //    var path2 = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath;
                //    DirectoryInfo directoryInfo = new DirectoryInfo(path2);
                //    return directoryInfo;
                //}

                //
                //var dir = Android.App.Application.Context.GetExternalFilesDir(null);

                //var sdKPath = dirs.Where(x => x.Path != dir.Path).FirstOrDefault();
                //var toResturn = sdKPath;


                ////Java.IO.File[] rootDrive = Java.IO.File.ListRoots();

                ////foreach (var sysDrive in rootDrive)
                ////{

                ////}

                //if (toResturn != null)
                //{
                //    string[] data = toResturn.Path.Split("Android");

                //    if (data.Count() > 0)
                //    {
                //        var path = data[0];

                //        DirectoryInfo directoryInfo = new DirectoryInfo(path);
                //        //var drives = DriveInfo.GetDrives().Where(x => (x.Name.Contains(directoryInfo.Name) && x.Name.Contains("storage"))).ToList();
                //        return directoryInfo; //drives[0].RootDirectory;


                //    }
                //}

            }


            return null;


            //return 
        }





        public Task<bool> SaveBackupFile(string filepath)
        {
            return new Task<bool>(() => { return true; });
        }

        public async Task<string> SelectBackUpFolder()
        {
            return await Task.Run<string>(() => { return "Backup"; });


        }

    }
}