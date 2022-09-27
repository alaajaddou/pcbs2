using PECS2022.General;
using PECS2022.Models;
using PECS2022.ViewModels;
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
	public partial class SampleListPage : ContentPage
	{
       
        private List<SampleStatusInfo> CurrentSamples = null;
        private SampleInfo CurrentSample = null;
        private Locality CurrentLocality = null;

        public SampleListPage(Locality locality)
        {
            this.CurrentLocality = locality;
            InitializeComponent();
          

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            Initialize();
            int selectedIndex = picker.SelectedIndex;
            SetSampleToListView(selectedIndex);
        }


        private async void Initialize()
        {
            ClearScreen();
           
           
            picker.SelectedIndex = 0;
        }


        private void btnClose_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        private void lstSamples_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {

            SampleStatusInfo sample = (SampleStatusInfo)e.SelectedItem;
            if(sample != null)
            SetObjectToScreen(sample.Sample);

        }

        private  void SetObjectToScreen(SampleInfo sample)
        {
            ClearScreen();
          
            if (sample == null) { return; }
            CurrentSample = sample;

            SampleListViewModel viewModel = new SampleListViewModel(sample);

            this.sampleView.BindingContext = viewModel;
            btnFindInMap.IsVisible = true;
        }

        private void ClearScreen()
        {
            btnFindInMap.IsVisible = false;
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            DoSearch();
        }

        private void cmbEA_SelectedIndexChanged(object sender, EventArgs e)
        {
            DoSearch();
        }


        private void DoSearch()
        {
            string search = txtSearch.Text;


            var data = CurrentSamples;

            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim();
                data = data.Where(x => x.Sample.Description.Contains(search)).ToList();
            }


            lstSamples.ItemsSource = data;


        }

        private async void SetSampleToListView(int statusFilter)
        {
            var db = await DataBase.GetAsyncConnection();

            var samples = new List<SampleInfo>();

            if (statusFilter == 0)
                samples = GeneralApplicationSettings.LocationForm.Samples.Where(x=> x.GetBuildingFullCode() != string.Empty).ToList();
            else if (statusFilter == 1)
            {
                samples = await db.Table<SampleInfo>().Where(Expressions.SamplesNotInBuildings).ToListAsync();
            }


            if (!samples.Any() && statusFilter == 0)
            {
                await DisplayAlert("", "لا يوحد عينات في منطقة العد المختارة", GeneralMessages.Ok);
                lstSamples.ItemsSource = null;
                return;
            }

            SampleStatusInfo smpStatus;
            CurrentSamples = new List<SampleStatusInfo>();
            foreach (SampleInfo sample in samples)
            {
                smpStatus = new SampleStatusInfo();
                smpStatus.Sample = sample;

                if (sample.Visit == null)
                {
                    smpStatus.CurrentStatus = CurrentStatus.None;
                }
                else
                {
                    if (sample.Visit.IsComplete)
                    {
                        smpStatus.CurrentStatus = CurrentStatus.Complete;
                    }
                    else
                    {
                        smpStatus.CurrentStatus = CurrentStatus.NotComplete;
                    }
                }

                CurrentSamples.Add(smpStatus);
            }

            lstSamples.ItemsSource = CurrentSamples;

        }

        private void btnFindInMap_Clicked(object sender, EventArgs e)
        {
            if(CurrentSample.GetBuildingFullCode() != string.Empty)
            {
                MainPage.CurrentBuildingNo = CurrentSample.GetBuildingFullCode();
                GeneralApplicationSettings.NeedUpdateMap = true;
                Navigation.PopAsync();
            }
            else
            {
                DisplayAlert("", "هذا المبنى غير مسكن", GeneralMessages.Ok);
            }
           
        }

        private void Picker_SelectedIndexChanged(object sender, EventArgs e)
        {
            var picker = (Picker)sender;
            int selectedIndex = picker.SelectedIndex;
            SetSampleToListView(selectedIndex);
            ClearScreen();
            txtSearch.Text = string.Empty;
        }

        private bool ValidDistance(bool showMsg, SampleInfo sample)
        {
            bool valid = true;

            if ( sample.AllowedDistance.HasValue && sample.AllowedDistance != 0)
            {
                if (GISCurrentLocation.CurrentX.HasValue && GISCurrentLocation.CurrentY.HasValue)
                {
                  

                    if ( sample.AllowedDistance.HasValue && sample.AllowedDistance<500)
                    {
                        valid = false;
                        if (showMsg)
                            DisplayAlert(GeneralMessages.Error, $" يجب تعبئة هذه الزيارة  من خلال زيارة المبنى", GeneralMessages.Cancel);
                    }

                }
                else
                {
                    valid = false;
                    if (showMsg)
                        DisplayAlert(GeneralMessages.Error, "لا يوجد احداثيات برجاء ضبط الاعدادات أولا", GeneralMessages.Cancel);
                }
            }

            return valid;
        }
        private void btnStartVisit_Clicked(object sender, EventArgs e)
        {
            if (!ValidDistance(true, CurrentSample)) return;

           
            Navigation.PushAsync(new VisitViews.VisitPage(CurrentSample));
            //Navigation.PopAsync();

        }
    }
}