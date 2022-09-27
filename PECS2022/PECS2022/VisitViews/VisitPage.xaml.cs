using PECS2022.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PECS2022.VisitViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VisitPage : MasterDetailPage
    {
        public static VisitPage CurrentVisitPage { get; set; }
        public Dictionary<string, NavigationPage> Pages = new Dictionary<string, NavigationPage>();
        // public List<QuestionnairePage> Pages = new List<QuestionnairePage>();
        private NavigationPage VisitPageDetail = null;
        private NavigationPage MembersDetail = null;
        public MembersDetails MembersDetails = null;
        public NavigationPage QuestionEvalPage = null;
        private VisitPageMenuItem CurrentItem { get; set; }
        private bool _IsFirstVisit { get; set; }

        private string CurrentBuildingNo { get; set; } = "";
        private string SampleIDSAM { get; set; } = "";

        public bool IsEnableFillDetails { get { return _IsFirstVisit; } set { _IsFirstVisit = value;
                try
                {
                    if (MasterPage != null)
                    {
                        MasterPage.ListView.IsEnabled = value;
                    }
                }
                catch { }  } }

        public VisitPage(SampleInfo sample)
        {
            CurrentVisitPage = this;
            CurrentBuildingNo = sample.GetBuildingFullCode();
            sample.LoadVisit();
            SampleIDSAM = sample.ID00;
            var visit = QuestionnaireManager.StartVisit(sample, CurrentBuildingNo, SampleIDSAM);
            InitializeComponent();
            Initilalize(visit);
        }

      
        private async void Initilalize(Visit visit)
        {
            IsEnableFillDetails =(visit.QC2.ToString() == "1" || visit.QC2.ToString() == "2");
            VisitPageDetail = (NavigationPage)Detail;
            VisitPageDetail.FlowDirection = FlowDirection.RightToLeft;

            if (MasterPage.MenuItems == null)
            {
                await DisplayAlert("", GeneralMessages.LoadSurveyERROR, GeneralMessages.Ok);
                await Navigation.PopAsync();
                return;
            }

            MasterPage.ListView.SelectedItem = MasterPage.MenuItems[0];
            CurrentItem = MasterPage.MenuItems[0];

            MasterPage.ListView.ItemSelected += ListView_ItemSelectedAsync;
        }

        internal void SetSecionStatus(CurrentStatus currentStatusId)
        {
            CurrentItem.CurrentStatus = currentStatusId;
         
            
        }

        internal void SetSecionStatus(int sectionId, CurrentStatus currentStatusId)
        {

            var mPage = this.Master as VisitPageMaster;

            var menu = mPage.MenuItems.FirstOrDefault(x => x.SectionId == sectionId);
            if (menu != null)
            {
                menu.CurrentStatus = currentStatusId;
            }
          


        }

        private async void ListView_ItemSelectedAsync(object sender, SelectedItemChangedEventArgs e)
        {


            var item = e.SelectedItem as VisitPageMenuItem;
            if (item == null || item.IsEnabled == false)
            {

                return;
            }
            CurrentItem = item;
            WizardTypeId wizardTypeId = item.WizardTypeId;
            NavigationPage page = null;
            switch (wizardTypeId)
            {
                case WizardTypeId.VisitForm:
                    if (VisitPageDetail == null)
                    {
                        VisitPageDetail = new NavigationPage(new VisitPageDetail());

                    }

                    page = VisitPageDetail;

                    break;


                case WizardTypeId.MembersForm:
                    if (MembersDetail == null)
                    {
                       MembersDetail = new NavigationPage(new MembersDetails());

                    }

                    page = MembersDetail;

                    break;

                case WizardTypeId.QuestionEvaluations:
                    if (QuestionEvalPage == null)
                    {
                        QuestionEvalPage = new NavigationPage(new CallLogListPage());

                    }

                    page = QuestionEvalPage;
                    break;

                case WizardTypeId.Sections:

                    string hr01 = item.HR01?.ToString() ?? "X";
                    string key = $"SEC_{item.SectionId.Value}_HR_{hr01}";
                    // QuestionnairePage secPage = Pages.Where(x => x.CurrentSectionId == item.SectionId.Value && x.HR01==item.HR01).FirstOrDefault();
                    if (!Pages.ContainsKey(key))
                    {
                        var secPage = new NavigationPage(new QuestionnairePage(item.SectionId.Value, item.HR01) /*{ Title=item.Title }*/);
                        Pages.Add(key, secPage);
                    }

                    page = Pages[key];
                    break;
            }


            if (page == null)
            {
                MasterPage.ListView.SelectedItem = null;
                return;

            }
            if (page != null && Detail != page)
            {
                Detail = page;
                //
                if (Device.RuntimePlatform == Device.Android)
                {
                    await Task.Delay(100);
                }
                 

                IsPresented = false;

                //this.Title = item.Title;
            }


          
           
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
            //return base.OnBackButtonPressed();
        }
    }
}