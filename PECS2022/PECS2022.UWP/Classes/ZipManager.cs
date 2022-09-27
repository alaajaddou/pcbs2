using PECS2022.Interfaces;
using PECS2022.UWP.Classes;
using System;
using System.IO;
using System.IO.Compression;


[assembly: Xamarin.Forms.Dependency(typeof(ZipManager))]

namespace PECS2022.UWP.Classes
{


    public class ZipManager : IZipManager
    {

        public bool QuickZip(string[] filesToZip, string destinationZipFullPath)
        {
            try
            {
                // Delete existing zip file if exists
                if (File.Exists(destinationZipFullPath))
                    File.Delete(destinationZipFullPath);



                using (ZipArchive zip = ZipFile.Open(destinationZipFullPath, ZipArchiveMode.Create))
                {


                    foreach (var file in filesToZip)
                    {

                        string filename = Path.GetFileName(file);

                        if (filename.Contains("PPCWG2022_V2")) filename = "PPCWG2022_V2.db3";
                        ZipArchiveEntry entry = zip.CreateEntryFromFile(file, filename, CompressionLevel.Optimal);




                    }
                }

                return File.Exists(destinationZipFullPath);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception: {e.Message}");
                return false;
            }
        }
    }
}
