using PECS2022.Models;
using PECS2022.Util;
using PECS2022.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PECS2022.VisitViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VisitPageDetail : ContentPage
    {
        public static VisitPageDetail Current { get; private set; }
        private SectionStatus CurrentSecStatus;
        private VisitViewModel VisitViewModel { get; set; }
        public VisitPageDetail()
        {
            InitializeComponent();
            Current = this;
            Initialize();       
        }


        private async void Initialize()
        {
            
          
            VisitViewModel = new VisitViewModel(QuestionnaireManager.CurrentVisit,true);
            sampleView.BindingContext = VisitViewModel;
           

            try
            {
                VisitPage page = VisitPage.CurrentVisitPage;

                
                    page.IsEnableFillDetails = (QuestionnaireManager.CurrentVisit.QC2 == 1 || QuestionnaireManager.CurrentVisit.QC2 == 2);
                
            }
            catch
            {

            }

            CurrentSecStatus = QuestionnaireManager.GetSectionStatus(-1, null);
        }


        private void UpdateSecStatus()
        {
            VisitPage page = (VisitPage)this.Parent.Parent;
            page.SetSecionStatus((CurrentStatus)CurrentSecStatus.CurrentStatusId);
            
        }
     


        private async Task FillLookups()
        {
            await FillVisitResultId();
            
        }

        private async Task FillVisitResultId()
        {

        }

        

     

        
      

        private async void btnSaveChanges_Clicked(object sender, EventArgs e)
        {
            if (await VisitViewModel.IsValid())
            {
                var visit = QuestionnaireManager.CurrentVisit;
                var resultId = VisitViewModel.QC2Val.AnswerCode;



               await VisitViewModel.DoSave();

                bool isComplete = !(resultId == "1" || resultId == "2");

                visit.IsComplete = isComplete;
              
             
               CurrentSecStatus.CurrentStatusId = (int)CurrentStatus.Complete;
               
                QuestionnaireManager.UpdateSectionStatus(CurrentSecStatus.ID00, CurrentSecStatus.SectionId, CurrentStatus.Complete,null);
                UpdateSecStatus();

                bool result = QuestionnaireManager.SaveCurrentVisit();


                if (result)
                {
                    ToastManager.ShortAlert(GeneralMessages.DataSavedSuccessfully);

                    VisitPage page = VisitPage.CurrentVisitPage;
                    page.IsEnableFillDetails =(visit.QC2.ToString() == "1" || visit.QC2.ToString() == "2");


                }

                else
                {
                   await DisplayAlert(GeneralMessages.Error, GeneralMessages.SaveNotSuccess, GeneralMessages.Cancel);
                }
            }
        }

      
    }
}