using Esri.ArcGISRuntime.Geometry;
using PECS2022.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.ComponentModel;
using PECS2022.ViewModels;
using PECS2022.General;

namespace PECS2022.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BuildingPage : ContentPage, INotifyPropertyChanged
    {
        private string CurrentBuildingNo { get; set; }
        private string CurrentBuildingName { get; set; }
        private List<SampleStatusInfo> CurrentSamples = null;
        private SampleInfo CurrentSample = null;
        private Locality CurrentLocality = null;
        private Geometry _geometry;
        private BuildingViewModel model;

        public BuildingPage(string buildingNo, string buildingName, Locality locality, Geometry geometry)
        {
            CurrentLocality = locality;
            CurrentBuildingName = buildingName;
            CurrentBuildingNo = buildingNo;
            _geometry = geometry;
            InitializeComponent();
            this.BindingContext = this;

         

            model = new BuildingViewModel();

            this.BindingContext = model;

            model.Title = $"{CurrentBuildingNo}  {(CurrentBuildingName == null ? string.Empty : "-" + CurrentBuildingName)}";
        }




        protected override void OnAppearing()
        {
            base.OnAppearing();
            Initialize();
        }

        private async void Initialize()
        {
            ClearScreen();

            var db = await DataBase.GetAsyncConnection();

        

            GeneralApplicationSettings.LocationForm.LoadSamplesVisits();
            var samples = GeneralApplicationSettings.LocationForm.Samples;

            List<SampleInfo> samplesInBuilding = samples.Where(s => s.GetBuildingFullCode() == CurrentBuildingNo).ToList();

            samplesNotInBuildings.ItemsSource = await db.Table<SampleInfo>().Where(Expressions.SamplesNotInBuildings).ToListAsync();

            if (samplesNotInBuildings.ItemsSource.Count == 0)
            {
                btnAddSampleToBuilding.IsEnabled = samplesNotInBuildings.IsEnabled = false;
            }
            else
            {
                samplesNotInBuildings.SelectedIndex = 0;
                btnAddSampleToBuilding.IsEnabled = samplesNotInBuildings.IsEnabled = true;
            }

            SampleStatusInfo smpStatus;
            CurrentSamples = new List<SampleStatusInfo>();
            foreach (SampleInfo sample in samplesInBuilding)
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


        private async void SetObjectToScreen(SampleInfo sample)
        {
            ClearScreen();


          
            if (sample == null) { return; }
            CurrentSample = sample;
            btnStartVisit.IsEnabled = true;


            SampleListViewModel viewModel = new SampleListViewModel(sample);

            this.sampleView.BindingContext = viewModel;
            if (sample.ID00.StartsWith("000"))
            {
                btnRemoveSample.IsEnabled = true;
            }
            //

          
            

            //txtE4.Text = sample.E4.ToString();// string.Empty; //sample.Visit.E4 .ToString();
            //txtE5.Text = sample.E5.ToString();  //string.Empty; //sample.Visit.E5.ToString();
            //txtE6.Text = sample.E6.ToString();  //string.Empty; //sample.Visit.E6.ToString();

            //txtE11.Text = sample.E11.ToString();// string.Empty; //sample.Visit.E11.ToString();
            //txtE12.Text = sample.E12.ToString();  //string.Empty; //sample.Visit.E12.ToString();
            //txtE13.Text = sample.E13.ToString();  //string.Empty; //sample.Visit.E13.ToString();


        }

        private void ClearScreen()
        {

            btnRemoveSample.IsEnabled = false;
            btnStartVisit.IsEnabled = false;
            //btnRemoveSample.IsEnabled = false;

           
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

        private void btnCancel_Clicked(object sender, EventArgs e)
        {
            SetObjectToScreen(CurrentSample);
        }

        private double GetDistance(MapPoint point1, MapPoint point2)
        {
            double currentDistance = 0;
            try
            {
                currentDistance = (Math.Round((6378.7 * Math.Acos(Math.Sin(point1.Y / 57.2958) * Math.Sin(point2.Y / 57.2958) + Math.Cos(point1.Y / 57.2958) * Math.Cos(point2.Y / 57.2958) * Math.Cos(point2.X / 57.2958 - point1.X / 57.2958))) * 1000));
                return currentDistance;

            }
            catch (Exception)
            {
                return -1;
            }
        }

        private bool ValidDistance(bool showMsg , SampleInfo sample)
        {
            bool valid = true;

            if (_geometry != null && sample.AllowedDistance.HasValue && sample.AllowedDistance != 0)
            {
                if (GISCurrentLocation.CurrentX.HasValue && GISCurrentLocation.CurrentY.HasValue)
                {
                    MapPoint mapPoint = new MapPoint(GISCurrentLocation.CurrentX.Value, GISCurrentLocation.CurrentY.Value);
                    MapPoint mpLatLon = GeometryEngine.Project(_geometry, SpatialReferences.Wgs84) as MapPoint;
                    double distance = GetDistance(mapPoint, mpLatLon);

                    if (distance > sample.AllowedDistance)
                    {
                        valid = false;
                        if(showMsg)
                        DisplayAlert(GeneralMessages.Error, $" انت تبعد {distance} متر. المسافة المسموحه هي {sample.AllowedDistance} متر", GeneralMessages.Cancel);
                    }

                }
                else
                {
                    valid = false;
                    if(showMsg)
                    DisplayAlert(GeneralMessages.Error, "لا يوجد احداثيات برجاء ضبط الاعدادات أولا", GeneralMessages.Cancel);
                }
            }

            return valid;
        }

        private bool UpdateSampleWithVisit(SampleInfo sample)
        {
            if (sample == null) return false;

            var db = DataBase.GetConnection();

            bool updated = true;

            if (db.Update(sample) != 0)
            {
                var visit = db.Table<Visit>().Where(x => x.ID00 == sample.ID00).FirstOrDefault();
                if (visit != null)
                {
                    visit.ID01 = sample.ID01;
                    visit.ID02 = sample.ID02;
                    visit.ID03 = sample.ID03;
                    visit.ID05 = sample.ID05.Value;

                    var buildingCode = visit.BuildBuildingNo();
                    visit.BuildingNo = buildingCode;

                    if (string.IsNullOrEmpty(buildingCode))
                    {
                        visit.BuildingNo = null;
                    }

                    updated = db.Update(visit) != 0;
                }

            }
            else
            {
                updated = false;
            }

            return updated;
        }


        private void btnStartVisit_Clicked(object sender, EventArgs e)
        {
            if (!ValidDistance(true,CurrentSample)) return;

            Navigation.PushAsync(new VisitViews.VisitPage(CurrentSample));
           // Navigation.PopAsync();
        }

        private async void btnRemoveSampleFromBuilding_Clicked(object sender, EventArgs e)
        {
            var sample = CurrentSample;
            if (sample == null) return;

            bool delete = await DisplayAlert(sample.Description, "هل أنت متأكد من تغيير موقع العينة من البناية ؟", GeneralMessages.Yes, GeneralMessages.No);

            if (!delete) return;

            //btnRemoveSample.IsEnabled = false;

           

            if (UpdateSampleWithVisit(sample))
            {
                CurrentSample = null;

                GeneralApplicationSettings.LocationForm.LoadSamples();
                GeneralApplicationSettings.NeedUpdateMap = true;

                Initialize();

                ToastManager.LongAlert("تم ازلة العينة من المبنى بنجاح");
            }

        }


        private void btnAddSampleToBuilding_Clicked(object sender, EventArgs e)
        {
            var sample = samplesNotInBuildings.SelectedItem as SampleInfo;
            if (sample == null) return;

            if (!ValidDistance(true, sample)) return;

            btnAddSampleToBuilding.IsEnabled = false;

            var govCode = CurrentBuildingNo.Substring(2, 2);
            var locCode = CurrentBuildingNo.Substring(2, 6);
            var enumArea = CurrentBuildingNo.Substring(8, 3);
            var ID4 = CurrentBuildingNo.Substring(11, 3);

            sample.ID01 = govCode;
            sample.ID02 = locCode;
            sample.ID03 = int.Parse(enumArea);
            sample.ID05 = int.Parse(ID4);

            if (UpdateSampleWithVisit(sample))
            {
                GeneralApplicationSettings.LocationForm.LoadSamples();
                GeneralApplicationSettings.NeedUpdateMap = true;

                Initialize();
                ToastManager.LongAlert("تم تسكين العينة بنجاح");
            }

            btnAddSampleToBuilding.IsEnabled = true;
        }

    }
}