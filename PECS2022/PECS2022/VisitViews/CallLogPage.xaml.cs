using PECS2022.Models;
using PECS2022.SurveyModel;
using PECS2022.ViewModels;
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
    public partial class CallLogPage : ContentPage
    {
        public SampleInfo CurrentSample { get; set; }
        public CallLogPage(SampleInfo sampleInfo, string telNo)
        {

            InitializeComponent();
            CurrentSample = sampleInfo;

            CallLogViewModel vm = new CallLogViewModel(sampleInfo,telNo);
            vm.OnContinuePressed += Vm_OnContinuePressed;
            vm.OnSaveSuccess += Vm_OnSaveSuccess;
            vwCallView.BindingContext = vm;


            // txtC2.Text = telNo;



            //var db = DataBase.GetConnection();


            //var newTels = callLogs.Select(x => x.C2).Distinct().ToList();
            //tels.AddRange(newTels);
            //cmbC2.DataSource = tels;
            ////   cmbC2.Text = telNo;
            //CurrentTelNo = telNo;
            //txtC3.Text = DateTime.Now.ToString();
            //StartDate = DateTime.Now;
            //LoadSettings();

            //cmbC4.IsEnabled = !string.IsNullOrWhiteSpace(telNo);
            //btnCall.IsEnabled = string.IsNullOrWhiteSpace(telNo);
            //cmbC2.IsEnabled = string.IsNullOrWhiteSpace(telNo);

            //btnContinue.IsEnabled = string.IsNullOrWhiteSpace(telNo);
        }

        private void Vm_OnSaveSuccess()
        {
            Navigation.InsertPageBefore(new VisitViews.VisitPage(CurrentSample), this);
            Navigation.PopAsync();
        }

        private async Task  Vm_OnContinuePressed()
        {
            var db = await DataBase.GetAsyncConnection();
            var callLog = await db.Table<CallLogInfo>().Where(x => x.ID00 == CurrentSample.ID00).CountAsync();

            if (callLog != 0)
            {
                if (await DisplayAlert("", $"هل تريد الاستمرار في الذهاب الى بيانات الاسرة ", GeneralMessages.Yes, GeneralMessages.No))
                {
                    QuestionnaireManager.CurrentCall = null;
                    Navigation.InsertPageBefore(new VisitViews.VisitPage(CurrentSample), this);
                    await Navigation.PopAsync();
                }
            }
            else
            {
                await DisplayAlert("", $"لا يمكن الاستمرار للذهاب الى بيانات الاسرة", GeneralMessages.Ok);
            }
        }
    }
}