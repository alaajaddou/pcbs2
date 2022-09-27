using PECS2022.Interfaces;
using PECS2022.Managers;
using PECS2022.Models;
using PECS2022.Security;
using Plugin.Geolocator;
using System;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Diagnostics;
using PECS2022.ViewModels;
using System.Linq;

namespace PECS2022.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public User CurrentUser { get; private set; }
        private LoginViewModel model;

        public LoginPage()
        {
            InitializeComponent();

            Init();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
           // await CheckUserFromDB();
        }

        private void Init()
        {
            model = new LoginViewModel();

            this.BindingContext = model;

            if (Device.RuntimePlatform == Device.iOS || Device.RuntimePlatform == Device.Android)
            {
                CloseButton.IsVisible = false;
                //Grid.SetColumn(btnLogin, 0);
                //Grid.SetColumnSpan(btnLogin, 2);
            }

            CrossGeolocator.Current.PositionError += Current_PositionError;
            CrossGeolocator.Current.PositionChanged += Current_PositionChanged;

            lblVersion.Text = $" اصدار رقم {DependencyService.Get<IAppVersion>().GetVersion()} (النسخة المعتمدة)";

            //NavigationPage.SetTitleIcon(this, false);
        }

        private async Task CheckUserFromDB()
        {
            model.IsBusy = true;
            LoginButton.IsEnabled = false;

            var db = await DataBase.GetAsyncConnection();

            //Get first user
            User dbUser = await db.Table<User>().FirstOrDefaultAsync();

            if (dbUser != null)
            {
                CurrentUser = dbUser;
                OnLoginSuccess();
                return;
            }

            LoginButton.IsEnabled = true;
            model.IsBusy = false;
        }

        private async void LoginClicked(object sender, EventArgs e)
        {
            if (GeneralApplicationSettings.GeneralSettingsSaved)
            {
                ValidationResult result = IsLoginInputValid();
                if (result.Result == ValidationResultEnum.Success)
                {
                    model.IsBusy = true;

                    var user = new User
                    {
                        UserName = this.UserName.Text.Trim(),
                        Password = PasswordEncrypter.EncryptPassword(this.Password.Text).Trim(),
                        FullName = "ssss",
                        AreaId = null,
                        GovCode = "01"
                    };

                    bool networkAvailable = true; //await GeneralFunctions.NetworkAvailable();
             
                    bool areCredentialsCorrect = false;


                    areCredentialsCorrect = await AreCredentialsCorrect(user, networkAvailable);
                    //await Task.Run(async () =>
                    //{
                    //    //Check User Credentials from Web API or local Database
                        

                    //});

                    if(CurrentUser == null && !networkAvailable)
                    {
                        model.IsBusy = false;
                        
                        await DisplayAlert("", "لا يوجد اتصال بالشبكة حاول مرة اخرى", GeneralMessages.Close);
                    }
                    else
                    {
                        if (areCredentialsCorrect)
                        {
                            OnLoginSuccess();
                        }
                        else
                        {
                            model.IsBusy = false;
                            await DisplayAlert("", GeneralMessages.UserLoginNotValid, GeneralMessages.Cancel);
                        }
                    }
                }
                else {
                    await DisplayAlert("", result.Message, GeneralMessages.Cancel);
                }
            }else{
                await DisplayAlert("", GeneralMessages.MustSaveGeneralSettingsFirst, GeneralMessages.Cancel);
            }

        }

        private ValidationResult IsLoginInputValid()
        {
            bool valid = true;
            StringBuilder builder = new StringBuilder();

            if (string.IsNullOrWhiteSpace(this.UserName.Text))
            {
                builder.AppendLine(GeneralMessages.UserNameRequired);
                valid = false;
            }
            if (string.IsNullOrWhiteSpace(this.Password.Text))
            {
                builder.AppendLine(GeneralMessages.PasswordRequired);
                valid = false;
            }

            if (!valid) 
                return ValidationResult.CreateError(builder.ToString());

            return ValidationResult.CreateSuccess();
        }

        private async Task<bool> AreCredentialsCorrect(User user, bool networkAvailable)
        {
            if (user == null) return false;

            bool doOfflineCheck = true;

            if (networkAvailable)
            {
                var result = WebApiDataManager.OnlineLogin(user.UserName, user.Password);

                if (result.IsSuccess)
                {
                    if (result.Value != null)
                    {
                        doOfflineCheck = false;

                        result.Value.UserName = result.Value.UserName.Trim();

                        //Save or update user to database
                        var db = await DataBase.GetAsyncConnection();
                        await db.InsertOrReplaceAsync(result.Value);
                        CurrentUser = result.Value;
                        return true;
                    }
                }
            }


            if (doOfflineCheck)
            {
                var db = await DataBase.GetAsyncConnection();

                var result = await db.Table<User>().Where(x => x.UserName == user.UserName && x.Password == user.Password).FirstOrDefaultAsync();
                CurrentUser = result;

                return result != null;
            }

            return false;
        }

        private async void OnLoginSuccess()
        {
            CurrentUserSettings.IsUserLoggedIn = true;
            CurrentUserSettings.CurrentUser = CurrentUser;

            try
            {
                CrossGeolocator.Current.PositionError -= Current_PositionError;
                CrossGeolocator.Current.PositionChanged -= Current_PositionChanged;

                if (CurrentUser.GovCode=="41" )
                {
                    Navigation.InsertPageBefore(new J1MainPage(), this);
                    await Navigation.PopAsync(true);
                }
                else
                {
                    Navigation.InsertPageBefore(new MainPage(), this);
                    await Navigation.PopAsync(true);
                }
            }
            catch (Exception ex)
            {
                //  await DisplayAlert("Error", ex.InnerException.ToString(), "OK");
            }
        }

        private async void SettingsButtonTapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new GeneralPublicSettingsPage(), true);
        }

        private async void OnApplicationClose(object sender, EventArgs e)
        {
            bool result = await DisplayAlert(GeneralMessages.Question, GeneralMessages.CloseApplicatiomn, GeneralMessages.Yes, GeneralMessages.No);
            if (result)
            {
                DependencyService.Get<ICloseApplication>().ExitApplication();
            }
        }

        private void Current_PositionChanged(object sender, Plugin.Geolocator.Abstractions.PositionEventArgs e)
        {
            Device.BeginInvokeOnMainThread(() => { LoginButton.IsEnabled = true; });
        }

        private void Current_PositionError(object sender, Plugin.Geolocator.Abstractions.PositionErrorEventArgs e)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                DisplayAlert(GeneralMessages.Error, "برجاء ضبط اعدادات ال GPS أولا", GeneralMessages.Cancel);

                LoginButton.IsEnabled = false;
            });
        }
    }
}