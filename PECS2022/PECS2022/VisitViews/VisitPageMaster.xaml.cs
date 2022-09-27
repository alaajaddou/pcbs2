using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PECS2022.VisitViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VisitPageMaster : ContentPage
    {
        public ListView ListView;
        public ObservableCollection<VisitPageMenuItem> MenuItems;


        public VisitPageMaster()
        {
              InitializeComponent();
              var x = new VisitPageMasterViewModel(QuestionnaireManager.CurrentVisit.ID00);
              MenuItems = x.MenuItems;
            BuildServeySections();
            BindingContext = x;
             // lblIDSAM.Text =$"{QuestionnaireManager.CurrentVisit.IDH07}"; // maram
              ListView = MenuItemsListView;

           
        }

        class VisitPageMasterViewModel : INotifyPropertyChanged
        {
            public ObservableCollection<VisitPageMenuItem> MenuItems { get; set; }
            
            public VisitPageMasterViewModel(string ID00)
            {

              var  surveyInfo=  ApplicationMainSettings.GetSurveyInfo();

                var statusList = QuestionnaireManager.GetSectionStatusList(ID00);

                CurrentStatus mainStatus = CurrentStatus.None;
                CurrentStatus indivStatus = CurrentStatus.None;


                var main = statusList.Where(x => x.SectionId == -1).FirstOrDefault();
                var indiv = statusList.Where(x => x.SectionId == 0).FirstOrDefault();

                if (main != null)
                {
                    mainStatus =(CurrentStatus) main.CurrentStatusId;
                }

                if (indiv != null)
                {
                    indivStatus = (CurrentStatus)indiv.CurrentStatusId;
                }




                MenuItems = new ObservableCollection<VisitPageMenuItem>(new[]
                {
                    new VisitPageMenuItem { Id = 0, Title = "البيانات التعريفية",  WizardTypeId= WizardTypeId.VisitForm, CurrentStatus=mainStatus },
                    new VisitPageMenuItem { Id = 1, Title = "بيانات أفراد الأسرة", WizardTypeId=WizardTypeId.MembersForm, IsEnabled=true, CurrentStatus=indivStatus }
                   


                });
                // BuildServeySections();


                //if (QuestionnaireManager.CurrentSample.SurveyTypeId == 1)
                //{
                //    sections =sections.Where(x => x.Id != 113  && x.Id!= 1122 && x.Id!=117);
                //}

                //else if(QuestionnaireManager.CurrentSample.SurveyTypeId==2 )                                                                                                                                                                                            
                //{
                //    sections = sections.Where(x => x.Id != 114 && x.Id != 117);
                //}




               // MenuItems.Add(new VisitPageMenuItem { Id = 4, Title = "سجل الاتصال", WizardTypeId = WizardTypeId.QuestionEvaluations, SectionId = -1000, IsEnabled = true, CurrentStatus = CurrentStatus.Edititing });

                // MenuItems.Add(new VisitPageMenuItem { Id = 4, Title = "الملاحظات على الاستمارة", WizardTypeId = WizardTypeId.QuestionEvaluations, SectionId = -1000, IsEnabled = true, CurrentStatus =   CurrentStatus.Edititing });


            }



            #region INotifyPropertyChanged Implementation
            public event PropertyChangedEventHandler PropertyChanged;
            void OnPropertyChanged([CallerMemberName] string propertyName = "")
            {
                if (PropertyChanged == null)
                    return;

                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            #endregion
        }

        internal void BuildServeySections()
        {
            var ID00 = QuestionnaireManager.CurrentVisit.ID00;
            var surveyInfo = ApplicationMainSettings.GetSurveyInfo();

            var statusList = QuestionnaireManager.GetSectionStatusList(ID00);
            int id = 2;
            var sections = surveyInfo.Sections.Where(x => x.Id != 1124 && x.Id != 1125);

          
                var itemsToRemove = MenuItems.Where(x => x.SectionId>2000).ToList();
                foreach (var item in itemsToRemove)
                {
                    MenuItems.Remove(item);

                }


            
            foreach (var sec in sections)
            {
                
              
                CurrentStatus status = CurrentStatus.None;
                var secStatus = statusList.Where(x => x.SectionId == sec.Id).FirstOrDefault();

                if (secStatus != null)
                {
                    status = (CurrentStatus)secStatus.CurrentStatusId;
                }

                MenuItems.Add(new VisitPageMenuItem { Id = id++, Title = sec.Description, WizardTypeId = WizardTypeId.Sections, SectionId = sec.Id, IsEnabled = true, CurrentStatus = status });

            }
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            Navigation.PopToRootAsync();
        }

        private async void btnHome_Clicked(object sender, EventArgs e)
        {
            if (QuestionnaireManager.CurrentVisit.QC2.ToString() == LookUpManager.DefaultLookupValue.Code)
            {
                CloseScreen();

                return;
            }

                bool allSectionSaved = true;
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("هل انت متاكد من عدم حفظ الاقسام التالية");

            var currentVisitPage = this.Parent as VisitPage;


            foreach (var nPage in currentVisitPage.Pages)
            {
                var page = (QuestionnairePage)nPage.Value.RootPage;
                if (!page.IsSaved)
                {

                    allSectionSaved = false;
                    stringBuilder.AppendLine("-" + page.CurrentSurvey.Sections.Where(x => x.Id == page.CurrentSectionId).First().Description);
                }
            }


            if (!allSectionSaved)
            {
                allSectionSaved= await DisplayAlert(GeneralMessages.Error, stringBuilder.ToString(), GeneralMessages.Yes, GeneralMessages.No);
            }


            if (allSectionSaved)
            {



                bool result = await DisplayAlert(GeneralMessages.Question, "هل انت متاكد من عملية الخروج؟", GeneralMessages.Yes, GeneralMessages.No);

                if (result)
                {
                    var items = MenuItems.Where(x => x.WizardTypeId != WizardTypeId.QuestionEvaluations).ToList();
                    bool isComplete = items.All(x=>x.CurrentStatus== CurrentStatus.Complete);


                    //if (QuestionnaireManager.CurrentVisit.IR03 == 2 && isComplete == false)
                    //{
                    //    bool notCompleteReult = await DisplayAlert(GeneralMessages.Question, "نتيجة المقابلة مكتمل جزئي. هل تم  الانتهاء من العمل مع هذه الاسرة؟ ", GeneralMessages.Yes, GeneralMessages.No);

                    //    if (notCompleteReult)
                    //    {
                    //        isComplete = true;
                    //    }
                    //}

                    //else if (QuestionnaireManager.CurrentVisit.IR03 == 2 && isComplete == true)
                    //{

                    //    await DisplayAlert(GeneralMessages.Error, " جميع  الاقسام  مكتملة ونتيجة المقابلة مكتمل جزئي. يجب تحويل نتيجة المقابلة الى اكتملت أولا", GeneralMessages.Cancel);
                    //    return;
                    //}

                    if(!(QuestionnaireManager.CurrentVisit.QC2.ToString() == "1" || QuestionnaireManager.CurrentVisit.QC2.ToString() == "6"))
                    {
                        isComplete = true;
                    }
                    else
                    {
                        foreach(var item in MenuItems.ToList())
                        {
                            SectionPageData data = await QuestionnaireManager.GetSectionMaster(QuestionnaireManager.CurrentVisit.ID00, item.SectionId.HasValue ? item.SectionId.Value : -1, item.HR01, false);

                            if (data != null)
                            {
                                if (!data.SectionVisit.IsComplete)
                                {
                                    isComplete = false;
                                    break;
                                }
                            }
                        }

                    }


                    QuestionnaireManager.CurrentVisit.IsComplete = isComplete;
                    bool saveResult = QuestionnaireManager.SaveCurrentVisit();

                    if (saveResult)
                    {
                        ToastManager.ShortAlert(GeneralMessages.DataSavedSuccessfully);
                        CloseScreen();
                    }
                    else
                    {
                        await DisplayAlert(GeneralMessages.Error, GeneralMessages.SaveNotSuccess, GeneralMessages.Cancel);
                    }

                }

            }

           
        }


        private async void CloseScreen()
        {

            QuestionnaireManager.ClearCurrentSettings();
            await Navigation.PopAsync();
        }
    }
}