
using PECS2022.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PECS2022.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class GeneralPublicSettingsPage : ContentPage
	{
        public GeneralPublicSettingsPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            txtWEPServer.Text = GeneralApplicationSettings.WEBServerPath;
            txtTPKPath.Text = GeneralApplicationSettings.TPKPath;
            txtGeoDatabasePath.Text = GeneralApplicationSettings.GEOPath;
        }

        private async void btnSaveChanges_Clicked(object sender, EventArgs e)
        {

            ValidationResult result = IsValid();
            if (result.Result == ValidationResultEnum.Success)
            {
                GeneralApplicationSettings.WEBServerPath = txtWEPServer.Text.Trim();
                GeneralApplicationSettings.TPKPath = txtTPKPath.Text.Trim();
                GeneralApplicationSettings.GEOPath = txtGeoDatabasePath.Text.Trim();

                DependencyService.Get<IMessage>().LongAlert(GeneralMessages.GeneralSettingsSaved);


                if (GeneralApplicationSettings.GeneralSettingsSaved)
                {
                    await Navigation.PopAsync(true);
                }
                else
                {
                    GeneralApplicationSettings.GeneralSettingsSaved = true;
                    Navigation.InsertPageBefore(new LoginPage(), this);
                    await Navigation.PopAsync(true);
                }
            }
            else
            {
                await DisplayAlert("", result.Message, GeneralMessages.Cancel);
            }
        }

        private ValidationResult IsValid()
        {
            bool valid = true;
            StringBuilder builder = new StringBuilder();

            if (string.IsNullOrWhiteSpace(txtWEPServer.Text))
            {
                builder.AppendLine("الخادم الرئيسي حقل متطلب");
                valid = false;
            }
            else
            {
                string uriName = txtWEPServer.Text.Trim();
                Uri uriResult;
                bool result = Uri.TryCreate(uriName, UriKind.Absolute, out uriResult)
                    && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
                if (!result)
                {
                    builder.AppendLine("الخادم الرئيسي غير صحيح");

                    valid = false;

                }
            }

            if (string.IsNullOrWhiteSpace(txtGeoDatabasePath.Text))
            {
                builder.AppendLine("مسار قواعد البيانات الجغرافية حقل متطلب");
                valid = false;
            }

            if (string.IsNullOrWhiteSpace(txtTPKPath.Text))
            {
                builder.AppendLine("مسار الخرائط حقل متطلب");
                valid = false;
            }

            if (!valid) return ValidationResult.CreateError(builder.ToString());
            return ValidationResult.CreateSuccess();
        }

        private async void btnClose_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync(true);

        }

        private async void btnSelectGEOPath_Clicked(object sender, EventArgs e)
        {
            string path = await DependencyService.Get<IFileStorage>().SelectGEODBFolder();

            if (!string.IsNullOrWhiteSpace(path))
            {
                txtGeoDatabasePath.Text = path;
            }
        }

        private async void btnSelectTPKPath_Clicked(object sender, EventArgs e)
        {
            string path = await DependencyService.Get<IFileStorage>().SelectTPKFolder();
            if (!string.IsNullOrWhiteSpace(path))
            {
                txtTPKPath.Text = path;
            }
        }
    }
}