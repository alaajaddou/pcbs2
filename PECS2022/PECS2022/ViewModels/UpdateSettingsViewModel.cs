using PECS2022.Models;
using PECS2022.Security;
using System;
using Xamarin.Forms;

namespace PECS2022.ViewModels
{
    public class UpdateSettingsViewModel : BaseViewModel
    {
        public UpdateSettingsViewModel()
        {
            Title = "الإجراءات";
            User = CurrentUserSettings.CurrentUser;
        }

        private User user;
        public User User
        {
            get { return user; }
            set { SetProperty(ref user, value); }
        }

        public override void RefreshUI()
        {
            throw new System.NotImplementedException();
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
