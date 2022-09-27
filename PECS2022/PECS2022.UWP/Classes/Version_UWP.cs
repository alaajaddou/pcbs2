using PECS2022.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;

[assembly: Xamarin.Forms.Dependency(typeof(PECS2022.UWP.Classes.Version_UWP))]
namespace PECS2022.UWP.Classes
{
    public class Version_UWP : IAppVersion
    {
        public string GetVersion()
        {


            Package package = Package.Current;
            PackageId packageId = package.Id;
            PackageVersion version = packageId.Version;

            return string.Format("{0}.{1}.{2}.{3}", version.Major, version.Minor, version.Build, version.Revision);
        }

        public int GetBuild()
        {
            return typeof(App).GetTypeInfo().Assembly.GetName().Version.Build;
        }

       
    }
}
