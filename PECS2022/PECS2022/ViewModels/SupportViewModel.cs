using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace PECS2022.ViewModels
{
    public class SupportViewModel : BaseViewModel
    {
        public override void RefreshUI()
        {
            throw new NotImplementedException();
        }

        public override void SetBusy()
        {
            IsBusy = true;
            //NavigationPage.SetHasBackButton(App.CurrentPage, false);
        }

        public override void SetNotBusy()
        {
            IsBusy = false;
            //NavigationPage.SetHasBackButton(App.CurrentPage, true);
        }
    }
}
