using PECS2022.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Globalization;
using PECS2022.ViewModels;

namespace PECS2022.VisitViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
   
    public partial class MembersDetails : ContentPage
    {

        public static MembersDetails Current;
        private SectionStatus CurrentSecStatus;
        private bool IsComplete { get; set; }

        public MembersDetails()
        {
            Current = this;
            InitializeComponent();
            Initialize();
        }


        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (CurrentSecStatus == null)
            {
                CurrentSecStatus = QuestionnaireManager.GetSectionStatus(0, null);
                //  CurrentSecStatus.CurrentStatusId = (int)CurrentStatus.Edititing;


                UpdateSecStatus();
            }
        }

        private void UpdateSecStatus()
        {
            VisitPage page = (VisitPage)this.Parent.Parent;
            QuestionnaireManager.UpdateSectionStatus(CurrentSecStatus.ID00, CurrentSecStatus.SectionId, (CurrentStatus)CurrentSecStatus.CurrentStatusId, null);
            page.SetSecionStatus((CurrentStatus)CurrentSecStatus.CurrentStatusId);

        }


        public List<Individual> Individuals;
        private int CurrentIndex = 0;
        public Individual CurrentIndividual = null;

        private async void Initialize()
        {

            var db = await DataBase.GetAsyncConnection();

            var ID00 = QuestionnaireManager.CurrentSample.ID00;
            Individuals = await db.Table<Individual>().Where(x => x.ID00 == ID00).OrderBy(i => i.D1).ToListAsync();

            if (Individuals.Count() > 0)
            {
                SetIndividualToScreen(Individuals[0]);
            }
            else
            {
                CurrentIndex = 0;
                AddNewIndivisual();
            }


            var genderList = await LookUpManager.GetLookupVals("D4");
            cmbSex2.ItemsSource = genderList;
            cmbSex2.ItemDisplayBinding = new Binding("Description");

        }

        MemberViewModel viewModel;

        private void SetIndividualToScreen(Individual individual)
        {


            ClearScreen();
            if (individual != null)
            {
                CurrentIndividual = individual;
                Individuals = Individuals.OrderBy(i => i.D1).ToList();
                if (Individuals.Contains(individual))
                {
                    CurrentIndex = Individuals.IndexOf(individual);
                    lblNumberOfIndiv.Text = $"{CurrentIndex + 1}/{Individuals.Count()}";
                    btnDelete.IsEnabled = true;
                }

                else
                {
                    btnDelete.IsEnabled = false;
                    lblNumberOfIndiv.Text = $"{CurrentIndex + 1}/{Individuals.Count() + 1}";
                }


                btnNext.IsEnabled = !(CurrentIndex + 1 >= Individuals.Count);
                btnPrvious.IsEnabled = CurrentIndex != 0;

                btnAddNew.IsEnabled = (CurrentIndex + 1 >= Individuals.Count);
                // btnSaveAndComplete.IsEnabled = (CurrentIndex + 1 >= Individuals.Count);

                viewModel = new MemberViewModel(CurrentIndividual, Individuals);
                MemberView.BindingContext = null;
                MemberView.BindingContext = viewModel;


            }
        }



        private async Task FillLookup(Picker picker, string code)
        {


            var data = await LookUpManager.GetLookupVals(code);

            picker.ItemDisplayBinding = new Binding("Description");
            picker.ItemsSource = data;

        }

        private void ClearScreen()
        {

            MemberView.BindingContext = null;

        }




        private async Task<SaveDataStatus> DoSave(bool allowResturnwithoutSave = false)
        {

            bool saved = false;

            bool isValid = await IsValid(!allowResturnwithoutSave);



            if (!isValid && allowResturnwithoutSave)
            {

                bool result = await DisplayAlert(GeneralMessages.Question, "جميع معلومات هذا الفرد غير معبئة؟ هل تريد الاستمرار في الانتقال وعدم الحفظ؟", GeneralMessages.Yes, GeneralMessages.No);

                if (result)
                {


                    return SaveDataStatus.Skipped;
                }
                else
                {

                    isValid = await IsValid();
                }


            }



            if (isValid)
            {
                await viewModel.DoSave();
                var ind = CurrentIndividual;
                try
                {
                    var db = DataBase.GetConnection();



                    int i = db.InsertOrReplace(ind);

                    if (!Individuals.Contains(ind))
                    {

                        Individuals.Add(ind);
                        CurrentIndex++;
                    }

                    saved = true;
                }
                catch
                {
                    saved = false;
                }




            }

            else
            {

                var errors = viewModel.GetErrors(null);
                List<string> errorMsgs = new List<string>();
                foreach (var e in errors)
                {
                    errorMsgs.Add(e.ToString());
                }

                string msg = string.Join(Environment.NewLine, errorMsgs.ToArray());
                msg = "برجاء تصحيح الاخطاء التالية والمحاولة مرة أخرى" + Environment.NewLine + msg;
                await DisplayAlert(GeneralMessages.Error, msg, GeneralMessages.Ok);


                return SaveDataStatus.NotValid;
            }
            if (saved)
            {
                return SaveDataStatus.Saved;
            }
            return SaveDataStatus.Error;
        }


        private async Task<bool> IsValid(bool showMessage = true)
        {
            return await viewModel.IsValid();
        }

        private async void btnNext_Clicked(object sender, EventArgs e)
        {
            SaveDataStatus status = await DoSave(false);
            if (status == SaveDataStatus.Saved)
            {
                CurrentSecStatus.CurrentStatusId = (int)CurrentStatus.NotComplete;
                UpdateSecStatus();
                ToastManager.ShortAlert(GeneralMessages.DataSavedSuccessfully);
                Individuals = Individuals.OrderBy(i => i.D1).ToList();
                CurrentIndex = Individuals.IndexOf(CurrentIndividual);
                SetIndividualToScreen(Individuals[CurrentIndex + 1]);
            }

        }

        private async void btnPrvious_Clicked(object sender, EventArgs e)
        {
            SaveDataStatus status = await DoSave(true);
            if (status == SaveDataStatus.Saved)
            {
                ToastManager.ShortAlert(GeneralMessages.DataSavedSuccessfully);
                Individuals = Individuals.OrderBy(i => i.D1).ToList();
                CurrentIndex = Individuals.IndexOf(CurrentIndividual);
                SetIndividualToScreen(Individuals[CurrentIndex - 1]);
            }

            else if (status == SaveDataStatus.Skipped)
            {
                Individuals = Individuals.OrderBy(i => i.D1).ToList();
                CurrentIndex = Individuals.IndexOf(CurrentIndividual);
                if (CurrentIndex == -1)
                {
                    if (Individuals != null && Individuals.Count > 0)
                    {
                        Individuals = Individuals.OrderBy(i => i.D1).ToList();
                        CurrentIndex = Individuals.Count;
                    }
                }
                SetIndividualToScreen(Individuals[CurrentIndex - 1]);
            }



        }
        private async void btnSaveAndComplete_Clicked(object sender, EventArgs e)
        {

            SaveDataStatus status = await DoSave(false);
            if (status == SaveDataStatus.Saved)
            {
                if (Individuals.Where(x => x.D3 == 1).Count() == 0)
                {
                    ToastManager.ShortAlert(GeneralMessages.DataSavedSuccessfully);

                    await DisplayAlert(GeneralMessages.Warning, "لم  يتم تعين رب اسرة لهذه الاسرة. برجاء تعيين رب اسرة لتغيير حالة هذا القسم لمكتمل", GeneralMessages.Cancel);
                }
                else
                {

                    //QuestionnaireManager.CurrentVisit.QC3 = Individuals.Count();
                    // QuestionnaireManager.CurrentVisit.QC5 = Individuals.Where(x => x.D4 == 2).Count();
                    // QuestionnaireManager.CurrentVisit.QC4 = Individuals.Where(x => x.D4 == 1).Count();
                    QuestionnaireManager.SaveCurrentVisit();

                    if (true)
                    {
                        if (Individuals.Count == QuestionnaireManager.CurrentVisit.QC3)
                            CurrentSecStatus.CurrentStatusId = (int)CurrentStatus.Complete;
                        else
                            CurrentSecStatus.CurrentStatusId = (int)CurrentStatus.NotComplete;
                        UpdateSecStatus();
                        ToastManager.ShortAlert(GeneralMessages.DataSavedSuccessfully);
                    }
                }
            }

        }

        private async void btnAddNew_Clicked(object sender, EventArgs e)
        {

            SaveDataStatus status = await DoSave(false);
            if (status == SaveDataStatus.Saved)
            {
                ToastManager.ShortAlert(GeneralMessages.DataSavedSuccessfully);
                if (await DisplayAlert(GeneralMessages.Question, "هل تريد الاستمرار في اضافة  فرد جديد؟", GeneralMessages.Yes, GeneralMessages.No))
                {

                    AddNewIndivisual();

                }

            }

        }


        private void AddNewIndivisual()
        {
            Individual individual = new Individual();
            individual.ID = Guid.NewGuid();
            if (Individuals.Count() == QuestionnaireManager.CurrentVisit.QC3)
            {
                DisplayAlert(GeneralMessages.Warning, "لا يمكن اضافة فرد جديد .. عدد الافراد مكتمل", GeneralMessages.Cancel);
            }
            else
            {
                if (Individuals.Count() > 0)
                {
                    CurrentIndex = Individuals.Count();
                    CurrentSecStatus.CurrentStatusId = (int)CurrentStatus.NotComplete;

                    UpdateSecStatus();

                    var db = DataBase.GetConnection();

                    var ID00 = QuestionnaireManager.CurrentVisit.ID00;
                    var maxD1 = db.Table<Individual>().Where(x => x.ID00 == ID00).OrderByDescending(x => x.D1).Select(x => x.D1).ToList();
                    if (maxD1.Count() > 0)
                    {
                        individual.D1 = maxD1[0] + 1;
                    }
                    else
                    {
                        individual.D1 = 1;
                    }

                    //individual.CreatedBy = Security.CurrentUserSettings.CurrentUser.UserName;
                    //individual.CreatedDate = DateTime.Now;
                    individual.ID00 = QuestionnaireManager.CurrentSample.ID00;
                    individual.IsNew = true;
                    //individual.D2A = "2";

                    SetIndividualToScreen(individual);
                }

                else
                {

                    CurrentIndex = 0;

                    var db = DataBase.GetConnection();

                    var ID00 = QuestionnaireManager.CurrentVisit.ID00;
                    var maxD1 = db.Table<Individual>().Where(x => x.ID00 == ID00).OrderByDescending(x => x.D1).Select(x => x.D1).ToList();
                    if (maxD1.Count() > 0)
                    {
                        individual.D1 = maxD1[0] + 1;
                    }
                    else
                    {
                        individual.D1 = 1;
                    }


                    //individual.CreatedBy = Security.CurrentUserSettings.CurrentUser.UserName;
                    //individual.CreatedDate = DateTime.Now;
                    individual.ID00 = QuestionnaireManager.CurrentVisit.ID00;
                    individual.D2 = QuestionnaireManager.CurrentVisit.QC1_1;
                    individual.D3 = 1;
                    //  individual.D2A = "2";
                    individual.IsNew = true;

                    SetIndividualToScreen(individual);
                }
            }

        }



        private void Button_Clicked(object sender, EventArgs e)
        {
            pnlMembersOrder.IsVisible = false;
            pnlMembersStatistics.IsVisible = !pnlMembersStatistics.IsVisible;

            if (pnlMembersStatistics.IsVisible)
            {
                lblIR0X.Text = Individuals.Count().ToString();
                lblIR04.Text = Individuals.Where(x => x.D4 == 1 && x.D6 >= 18).Count().ToString();
                lblIR05.Text = Individuals.Where(x => x.D4 == 2 && x.D6 >= 18).Count().ToString();

            }

        }

        private void btnClose_Clicked(object sender, EventArgs e)
        {
            pnlMembersStatistics.IsVisible = false;
            pnlMembersOrder.IsVisible = false;


        }

        private void txt05A_DateSelected(object sender, DateChangedEventArgs e)
        {
            // txtHR05.Text = CalculateAge(txtHR05A.Date).ToString();


        }














        private void cmbHL16_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cmbHL17_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void BtnMemberOrder_Clicked(object sender, EventArgs e)
        {
            pnlMembersStatistics.IsVisible = false;
            pnlMembersOrder.IsVisible = !pnlMembersOrder.IsVisible;

            if (pnlMembersOrder.IsVisible)
            {


                LoadMembersOrder();
            }
        }

        private void LoadMembersOrder()
        {
            lstSex.Children.Clear();

            if (cmbSex2.SelectedIndex >= 0)
            {
                var genCode = (cmbSex2?.SelectedItem as LookupVal)?.Code?.ToInt();
                var inds = Individuals.FindAll(x => x.D6 >= 18 && x.D4 == genCode).OrderByDescending(x => x.D6);

                foreach (var ind in inds)
                {
                    Label label = new Label() { BackgroundColor = Color.White, Text = $"{ind.D1} - {ind.D2} - ({ind.D6})" };

                    lstSex.Children.Add(label);
                }
            }
        }

        private async void BtnDelete_Clicked(object sender, EventArgs e)
        {
            bool result = await DisplayAlert(GeneralMessages.Question, $"هل انت متاكد من عملية حذف هذا الفرد ({viewModel.D2})?", GeneralMessages.Yes, GeneralMessages.No);

            if (result)
            {
                Individuals.Remove(CurrentIndividual);
                var db = await DataBase.GetAsyncConnection();
                if (CurrentIndividual.IsNew.HasValue && CurrentIndividual.IsNew.Value == true)
                {


                    int i = await db.DeleteAsync(CurrentIndividual);
                }
                else
                {
                    //CurrentIndividual.D2A = "3";
                    await db.InsertOrReplaceAsync(CurrentIndividual);

                }
                if (CurrentIndex != 0)
                {
                    SetIndividualToScreen(Individuals[CurrentIndex - 1]);

                    UpdateIndSection();
                }

                else
                {

                    if (Individuals.Count() > 0)
                    {
                        SetIndividualToScreen(Individuals[0]);
                        UpdateIndSection();
                    }
                    else
                    {
                        CurrentIndex = 0;
                        AddNewIndivisual();
                        UpdateIndSection();
                    }
                }


            }
        }


        private void UpdateIndSection()
        {
            if (Individuals.Count == QuestionnaireManager.CurrentVisit.QC3)
                CurrentSecStatus.CurrentStatusId = (int)CurrentStatus.Complete;
            else
                CurrentSecStatus.CurrentStatusId = (int)CurrentStatus.NotComplete;
            UpdateSecStatus();
            ToastManager.ShortAlert(GeneralMessages.DeleteSuccess);
        }
        private void CmbSex2_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadMembersOrder();
        }
    }


    public enum SaveDataStatus
    {
        Skipped = 0,
        Saved = 1,
        Error = 2,
        NotValid = 3
    }
}

