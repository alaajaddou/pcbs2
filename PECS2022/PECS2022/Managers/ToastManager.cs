using PECS2022.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace PECS2022
{
  public static  class ToastManager
    {

        public static void LongAlert(string txt)
        {
            DependencyService.Get<IMessage>().LongAlert(txt);
        }

        public static void ShortAlert(string txt)
        {
            DependencyService.Get<IMessage>().ShortAlert(txt);
        }
    }
}
