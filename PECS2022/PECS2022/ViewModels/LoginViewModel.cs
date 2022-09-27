using System;
using System.Windows.Input;

using Xamarin.Forms;

namespace PECS2022.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public LoginViewModel()
        {
            Title = "تسجيل دخول المستخدم";
        }

        public override void RefreshUI()
        {
            throw new System.NotImplementedException();
        }

        public override void SetBusy()
        {
            throw new NotImplementedException();
        }

        public override void SetNotBusy()
        {
            throw new NotImplementedException();
        }
    }
}