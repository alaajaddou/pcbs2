using System;
using System.Collections.Generic;
using System.Text;

namespace PECS2022.Interfaces
{
    public interface IZipManager
    {

        bool QuickZip(string[] filesToZip, string destinationZipFullPath);
    }
}
