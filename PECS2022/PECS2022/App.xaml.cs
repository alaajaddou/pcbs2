using Plugin.Geolocator;
using PECS2022.Views;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static Xamarin.Essentials.Permissions;


namespace PECS2022
{
    public partial class App : Application
    { 
        public App()
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("NTg1MDYzQDMxMzkyZTM0MmUzMFgrSTRtNFZmWUxNSHpXVDhvUGVhTkZ2ZE9uakUrUHFqei96cE81VGZocHc9");

            InitializeComponent();

            MainPage = new NavigationPage(new LoginPage());
        }

        protected override void OnStart()
        {
            base.OnStart();

            StartGPS();

            CheckStorage();
            // Handle when your app starts
        }

        public static Page CurrentPage
        {
            get
            {
                Page last = null;
                int stackSize = Application.Current.MainPage.Navigation.NavigationStack.Count;
                if (stackSize != 0)
                {
                    last = Application.Current.MainPage.Navigation.NavigationStack[stackSize - 1];
                }
                return last;
            }
        }

        private async void CheckStorage()
        {
            await CheckPermissions<StorageWrite>();
        }

        private async void StartGPS()
        {

            try
            {
                if (await CheckPermissions<Xamarin.Essentials.Permissions.LocationAlways>())
                {
                    if (CrossGeolocator.Current.IsListening)
                    {

                    }
                    else
                    {
                        CrossGeolocator.Current.PositionChanged += CrossGeolocator_Current_PositionChanged;
                        CrossGeolocator.Current.PositionError += CrossGeolocator_Current_PositionError;
                        if (await CrossGeolocator.Current.StartListeningAsync(new TimeSpan(0, 0, 30), 0))
                        {

                        }

                        else
                        {

                        }
                    }
                    if (Views.MainPage.instance != null)
                    {
                        Views.MainPage.instance.StartLocationDisplay();
                    }

                }

            }
            catch
            {
                //Something went wrong
            }

        }


        private async Task<bool> CheckPermissions<T>() where T : BasePermission, new()
        {

            bool result = false;

            try
            {
                //var allStatus = new Dictionary<BasePermission, PermissionStatus>();
                var status = await CheckStatusAsync<T>();
                if (status != PermissionStatus.Granted)
                {

                    //if (await show<T>() != PermissionStatus.Granted)
                    //{
                    //    await MainPage.DisplayAlert("الصلاحيات", "النظام بحاجة الى الوصول الى صلاحية" + permission.ToString(), GeneralMessages.Cancel);
                    //}

                    status = await RequestAsync<T>();
                }

                if (status == PermissionStatus.Granted)
                {
                    result = true;
                }



            }
            catch
            {
                //Something went wrong
            }

            return result;
        }




        void CrossGeolocator_Current_PositionError(object sender, Plugin.Geolocator.Abstractions.PositionErrorEventArgs e)
        {

            // labelGPSTrack.Text = "Location error: " + e.Error.ToString();
        }

        void CrossGeolocator_Current_PositionChanged(object sender, Plugin.Geolocator.Abstractions.PositionEventArgs e)
        {
            var position = e.Position;
            GISCurrentLocation.CurrentX = position.Longitude;
            GISCurrentLocation.CurrentY = position.Latitude;

            //labelGPSTrack.Text = string.Format("Time: {0} \nLat: {1} \nLong: {2} \nAltitude: {3} \nAltitude Accuracy: {4} \nAccuracy: {5} \nHeading: {6} \nSpeed: {7}",
            //    position.Timestamp, position.Latitude, position.Longitude,
            //    position.Altitude, position.AltitudeAccuracy, position.Accuracy, position.Heading, position.Speed);


        }

        protected override void OnSleep()
        {
            base.OnSleep();
            CrossGeolocator.Current.PositionChanged -= CrossGeolocator_Current_PositionChanged;
            CrossGeolocator.Current.PositionError -= CrossGeolocator_Current_PositionError;

            CrossGeolocator.Current.StopListeningAsync();
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            base.OnResume();
            StartGPS();
            // Handle when your app resumes
        }

        private void CloseLayout_Tapped(object sender, EventArgs e)
        {
            Image img = (Image)sender;

            Layout parent = (Layout)(img.Parent.Parent.Parent.Parent.Parent);
            parent.IsVisible = false;

            Layout panelOverlay = parent.FindByName<Layout>("panelOverlay");

            if (panelOverlay != null)
            {
                panelOverlay.IsVisible = false;
            }
        }
    }
}