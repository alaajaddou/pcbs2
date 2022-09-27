using PECS2022.Models;
using PECS2022.SurveyModel;
using Syncfusion.SfDataGrid.XForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PECS2022.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class J1MainPage : ContentPage
    {
        public J1MainPage()
        {
            InitializeComponent();
            CreateContextMenu();
        }
        StackLayout contextMenu;
        Button sortButton;
        Button clearSortButton;
        private string currentColumnName;
        private bool isContextMenuDisplayed = false;
        private void CreateContextMenu()
        {
            contextMenu = new StackLayout();

            sortButton = new Button();
            sortButton.Text = "Sort";
            sortButton.BackgroundColor = Color.Black;
            sortButton.TextColor = Color.White;
            sortButton.Clicked += SortButton_Clicked;

            clearSortButton = new Button();
            clearSortButton.Text = "Clear sort";
            clearSortButton.BackgroundColor = Color.Black;
            clearSortButton.TextColor = Color.White;
            clearSortButton.Clicked += ClearSortButton_Clicked;

            // A custom view hosting two buttons are now created
            contextMenu.Children.Add(sortButton);
            contextMenu.Children.Add(clearSortButton);
        }

        private void ClearSortButton_Clicked(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
        }

        private void SortButton_Clicked(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
        }

        private bool isSettingsDone = false;

        private async void Initialize()
        {
            if (cmbStatus.SelectedIndex < 0) cmbStatus.SelectedIndex = 0;
            if (cmbLocality.SelectedIndex >= 0)
            {

                Locality formInfo = cmbLocality.SelectedItem as Locality;

                GeneralApplicationSettings.LocationForm = new LocationForm();
                GeneralApplicationSettings.LocationForm.Locality = formInfo;
                var govs = cmbGovernorate.SelectedItem as GovernorateInfo;
                GeneralApplicationSettings.LocationForm.Governorate = new Governorate() { Code = govs.Code, Description = govs.Description };
                GeneralApplicationSettings.LocationForm.Init();

                string fID = formInfo.Code;
                string search = string.Empty;

                if (!string.IsNullOrWhiteSpace(txtSearch.Text))
                {
                    search = txtSearch.Text.Trim();

                }
                string userName = Security.CurrentUserSettings.CurrentUser.UserName;
                var db = await DataBase.GetAsyncConnection();
                List<SampleInfo> samples = new List<SampleInfo>();

                if (cmbStatus.SelectedIndex == 0)
                {
                    samples = await db.Table<SampleInfo>().Where(s => s.AssignedTo == userName && s.ID02 == fID && (s.QC1_1.Contains(search) || s.ID00.Contains(search))).OrderBy(s => s.ID00).ToListAsync();
                }
                if (cmbStatus.SelectedIndex == 1)
                {


                    samples = await db.QueryAsync<SampleInfo>("select  * from SampleInfo where AssignedTo=? and  ID2=? and (QC3_1 like ? || ID00 like ? )  and ID00 not  in (select  ID00  from Visits )", userName, fID, $"%{search}%", $"%{search}%");
                }
                if (cmbStatus.SelectedIndex == 2)
                {


                    samples = await db.QueryAsync<SampleInfo>("select  * from SampleInfo where AssignedTo=? and  ID2=? and (QC3_1 like ? || ID00 like ? )  and ID00   in (select  ID00  from Visits  where IsComplete=0)", userName, fID, $"%{search}%", $"%{search}%");
                }

                if (cmbStatus.SelectedIndex == 3)
                {


                    samples = await db.QueryAsync<SampleInfo>("select  * from SampleInfo where AssignedTo=? and  ID2=? and (QC3_1 like ? || ID00 like ?)  and ID00   in (select  ID00  from Visits  where IsComplete=1)", userName, fID, $"%{search}%", $"%{search}%");
                }





                dataGrid.ItemsSource = samples;

            }
            else
            {
                dataGrid.ItemsSource = new List<SampleInfo>();
            }
        }



        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (!GeneralApplicationSettings.IsFirstTimeSyncDone)
            {
                UpdateSettingsPage page = new UpdateSettingsPage();
                Navigation.PushAsync(page, true);
            }

            else
            {





                if (isSettingsDone == false || cmbLocality.ItemsSource.Count == 0)
                {
                    var db = DataBase.GetConnection();




                    string username = Security.CurrentUserSettings.CurrentUser.UserName;

                    var vs = db.Table<SampleInfo>().Where(x => x.AssignedTo == username).ToList().Select(x => x.ID01).Distinct().ToList();
                    cmbGovernorate.ItemDisplayBinding = new Binding("Description");
                    cmbGovernorate.ItemsSource = ApplicationMainSettings.GovernorateList.Where(x => vs.Contains(x.Code)).ToList();
                    cmbGovernorate.SelectedIndex = 0;

                    FillLocalities();


                    //var codes = vs.Select(x => x.Code).ToList();
                    isSettingsDone = true;

                }


                Initialize();





            }

        }


        private void FillLocalities()
        {

            if (cmbGovernorate.SelectedIndex >= 0)
            {
                string govCode = (cmbGovernorate.SelectedItem as GovernorateInfo).Code;
                cmbLocality.IsEnabled = true;
                cmbLocality.ItemDisplayBinding = new Binding("Description");
                var db = DataBase.GetConnection();
                string username = Security.CurrentUserSettings.CurrentUser.UserName;
                var vs = db.Table<SampleInfo>().Where(x => x.AssignedTo == username && x.ID01 == govCode).ToList().Select(x => x.ID02).Distinct().ToList();
                cmbLocality.ItemsSource = ApplicationMainSettings.LocalityList.Where(x => vs.Contains(x.Code)).ToList();
                cmbLocality.SelectedIndex = 0;

            }

            else
            {
                cmbLocality.ItemsSource = new List<Locality>();

                cmbLocality.IsEnabled = false;
            }

            Initialize();


        }

        private void Button_Clicked(object sender, EventArgs e)
        {

            //if (GISCurrentLocation.CurrentX.HasValue && GISCurrentLocation.CurrentY.HasValue)
            //{
            UpdateSettingsPage updateSettings = new UpdateSettingsPage();
            Navigation.PushAsync(updateSettings);

            //}

            //else
            //{
            //    DisplayAlert(GeneralMessages.Error, "لا يوجد احداثيات برجاء ضبط الاعدادات أولا", GeneralMessages.Cancel);
            //}
        }

        private void btnFill_Clicked(object sender, EventArgs e)
        {
            var idsam = ((Button)sender).CommandParameter.ToString();

            var db = DataBase.GetConnection();
            var CurrentSample = db.Get<SampleInfo>(idsam);

            if (CurrentSample.ID01 == "41")
            {
                QuestionnaireManager.CurrentCall = null;
                Navigation.PushAsync(new VisitViews.VisitPage(CurrentSample));
                //await Navigation.PopAsync();
            }
            else
            {
                Navigation.PushAsync(new VisitViews.CallLogPage(CurrentSample, string.Empty));
            }


           
          
           
        }

        private void cmbSurvey_SelectedIndexChanged(object sender, EventArgs e)
        {
            Initialize();

        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            Initialize();

        }

        private void cmbGovernorate_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillLocalities();
        }

        private void btnFill_Clicked_1(object sender, EventArgs e)
        {

        }

        private async void btnQC_2_Clicked(object sender, EventArgs e)
        {

            var idsam = ((Button)sender).CommandParameter.ToString();
            var telNo = ((Button)sender).Text.ToString();
            var db = DataBase.GetConnection();
            var CurrentSample = db.Get<SampleInfo>(idsam);

            bool valid = true;
            if (!GeneralApplicationSettings.GazaGovs.Contains(CurrentSample.ID01) && CurrentSample.ID01!="41")
            {

                if (!string.IsNullOrWhiteSpace(telNo))
                {
                    if (await DisplayAlert("", $"هل تريد الاستمرار في الاتصال على الرقم {telNo}", GeneralMessages.Yes, GeneralMessages.No))
                    {

                        try
                        {

                            //await  Launcher.OpenAsync("tel:0592701035");
                            PhoneDialer.Open(telNo);
                        }
                        catch (FeatureNotSupportedException)
                        {
                            await DisplayAlert("", "حدث خطاء اثناء اجراء الاتصال", GeneralMessages.Ok);
                        }
                        catch
                        {
                            // Other error has occurred.  
                        }



                    }
                    else
                    {
                        valid = false;

                        ToastManager.LongAlert("تم الغاء محاولة الاتصال");
                    }



                }
                else
                {
                    valid = false;
                    ToastManager.LongAlert("لا يمكن الاتصال في الرقم المطلوب");
                }
            }

            if (valid)
            {
                if (CurrentSample.ID01 == "41")
                {
                    QuestionnaireManager.CurrentCall = null;
                    Navigation.InsertPageBefore(new VisitViews.VisitPage(CurrentSample), this);
                    await Navigation.PopAsync();
                }
                else
                {
                    await Navigation.PushAsync(new VisitViews.CallLogPage(CurrentSample, telNo));
                }
            }

            //Navigation.PushAsync(new VisitViews.CallLogPage(CurrentSample));
        }

        private void dataGrid_GridTapped(object sender, Syncfusion.SfDataGrid.XForms.GridTappedEventArgs e)
        {
            //relativeLayout.Children.Remove(contextMenu);
            isContextMenuDisplayed = false;
        }

        private void dataGrid_GridLongPressed(object sender, Syncfusion.SfDataGrid.XForms.GridLongPressedEventArgs e)
        {
            if (!isContextMenuDisplayed)
            {

                currentColumnName = dataGrid.Columns[e.RowColumnIndex.ColumnIndex].MappingName;
                var point = dataGrid.RowColumnIndexToPoint(e.RowColumnIndex);
                // Display the ContextMenu when the SfDataGrid is long pressed
                //relativeLayout.Children.Add(contextMenu, Constraint.Constant(point.X), Constraint.Constant(point.Y));
                isContextMenuDisplayed = true;
            }
            else
            {
                // Hides the context menu when SfDataGrid is long pressed when the context menu is already visible in screen
               // relativeLayout.Children.Remove(contextMenu);
                isContextMenuDisplayed = false;
            }
        }
    }
}