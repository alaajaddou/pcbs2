using PECS2022.Interfaces;
using PECS2022.UWP.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

[assembly: Xamarin.Forms.Dependency(typeof(CloseApplication))]

namespace PECS2022.UWP.Classes
{
    public class CloseApplication : ICloseApplication
    {
        public void ExitApplication()
        {

            Application.Current.Exit();

        }
    }
}
