using Newtonsoft.Json;
using PECS2022.Interfaces;
using PECS2022.Managers;
using PECS2022.VisitViews;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using PECS2022.ViewModels;

namespace PECS2022.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SupportPage : ContentPage
    {
        private SupportViewModel model;

        public SupportPage()
        {
            InitializeComponent();
            IsBusy = false;

            model = new SupportViewModel();
            this.BindingContext = model;

            //cmbGovernorate.ItemsSource = ApplicationMainSettings.GovernoratesByUser;
            //cmbMonth.ItemsSource = ApplicationMainSettings.Months;
            //cmbVisitNo.ItemsSource = ApplicationMainSettings.VisitsNo;

            //cmbGovernorate.SelectedIndex = cmbMonth.SelectedIndex = cmbVisitNo.SelectedIndex = 0;
        }


        private void btnLogin_Clicked(object sender, EventArgs e)
        {

            if (!string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                if (txtPassword.Text == "20!*Jus@2018!")
                {
                    pnlDownloadData.IsVisible = false;
                    pnlActions.IsVisible = true;
                   
                }

                else
                {
                    DisplayAlert(GeneralMessages.Error, "كلمة المرور غير صحيحة", GeneralMessages.Cancel);
                }

            }
            else
            {
                DisplayAlert(GeneralMessages.Error, "كلمة  مرور الدعم الفني حقل متطلب", GeneralMessages.Cancel);
            }

        }

        private void btnCancel_Clicked(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }

        private async void btnDownloadData_Clicked(object sender, EventArgs e)
        {

            if (await DisplayAlert("", "هل تريد الاستمرار في ارسال البيانات ؟", GeneralMessages.Yes, GeneralMessages.No))
            {
                loadPage.IsVisible = true;

                await DownloadData();

                loadPage.IsVisible = false;
            }
        }

        private async Task<bool> DownloadData(bool standAlone = true)
        {
            //IsBusy = true;

            bool isSuccess = false;
            bool needSendData = false;
            try
            {

                if (standAlone)
                {


                }
               // aiDownloadData.IsRunning = true;

                var db = DataBase.GetConnection();

               // var samplesToSend = db.Table<Sample>().Where(x => x.NeedSend == true).Count();
                var visits = db.Table<Visit>().Where(x => x.NeedSend == true).Count();

                if ( visits > 0)
                {
                    needSendData = true;
                    await DisplayAlert(GeneralMessages.Error, "يجب ارسال  البيانات أولا", GeneralMessages.Cancel);
                }

                else
                {
                    var visitResults = await WebApiDataManager.GetListAsync<Visit>("/PECSServices/api/Visit/GetVisits", true);
                    var forms = await WebApiDataManager.GetListAsync<SectionMaster>("/PECSServices/api/Visit/DownloadForms", true);
                   //var individuals = await WebApiDataManager.GetListAsync<Individual>("/PECSServices/api/Visit/DownloadIndividuals", true);




                    if (visitResults.IsSuccess  && forms.IsSuccess )
                    {


                        //string tempPath = await DependencyService.Get<IDatabaseSettings>().SampleSectionsFolderBackUpFile();
                        //string folderPath = await DependencyService.Get<IDatabaseSettings>().SampleSectionsFolder();
                        //string dbPath = DependencyService.Get<IDatabaseSettings>().DatabasePath;
                        //string dbBackupPath = DependencyService.Get<IDatabaseSettings>().BackupDatabasePath;
                        
                     
                        string folderPath = await DependencyService.Get<IDatabaseSettings>().SampleSectionsFolder();
                        string dbPath = DependencyService.Get<IDatabaseSettings>().DatabasePath;
                        string dbBackupPath = DependencyService.Get<IDatabaseSettings>().BackupDatabasePath;


                        try
                        {
                            string tempPath = await DependencyService.Get<IDatabaseSettings>().SampleSectionsFolderBackUpFile();
                            if (Directory.Exists(tempPath))
                            {
                                Directory.Delete(tempPath);
                            }
                            Directory.Move(folderPath, tempPath);

                            File.Copy(dbPath, dbBackupPath);

                        }
                        catch { }

                        if (Directory.Exists(folderPath))
                        {
                            Directory.Delete(folderPath);
                        }


                        DataBase.CloseCurrentConnection();





                        db = DataBase.GetConnection();

                        db.Execute("Delete from Visits");
                        db.Execute("Delete from SectionStatus");
                        //db.Execute("Delete from  Individual");

                        db.InsertAll(visitResults.Data);

                       
                        folderPath = await DependencyService.Get<IDatabaseSettings>().SampleSectionsFolder();


                        var statusList = new List<SectionStatus>();

                        foreach (var v in visitResults.Data)
                        {
                            statusList.Add(new SectionStatus() { CurrentStatusId = (int)CurrentStatus.Complete, ID = Guid.NewGuid(), ID00 = v.ID00, SectionId = -1 });


                            int statusId = (int)CurrentStatus.Complete;

                            if (!v.IsComplete)
                            {

                                statusId= (int)CurrentStatus.NotComplete;

                            }
                            statusList.Add(new SectionStatus() { CurrentStatusId = statusId, ID = Guid.NewGuid(), ID00 = v.ID00, SectionId = 0 });

                        }
                        foreach (var sec in forms.Data)
                        {

                            SectionStatus sectionStatus = new SectionStatus() { ID = Guid.NewGuid(), ID00 = sec.ID00, SectionId = sec.SectionId, CurrentStatusId = (int)CurrentStatus.Complete };



                            if (!sec.IsComplete) sectionStatus.CurrentStatusId = (int)CurrentStatus.NotComplete;

                            statusList.Add(sectionStatus);
                            SectionPageData sectionPageData = new SectionPageData();
                            sectionPageData.OldAnswerList = new List<QuestionAnswer>();
                            sectionPageData.SectionVisit = sec;

                            string filename = await QuestionnaireManager.GetFileName(sec.ID00, sec.SectionId,sec.HR01);

                            JsonSerializer serializer = new JsonSerializer();
                            // serializer.Converters.Add(new JavaScriptDateTimeConverter());
                            // serializer.NullValueHandling = NullValueHandling.Ignore;
                            string result = JsonConvert.SerializeObject(sectionPageData, Formatting.Indented);

                            if (File.Exists(filename)) File.Delete(filename);


                            using (StreamWriter objStreamWriter = new StreamWriter(filename, true))
                            {
                                objStreamWriter.Write(result);
                                objStreamWriter.Close();

                            }

                        }

                        db.InsertAll(statusList);
                        isSuccess = true;

                    }

                }


                isSuccess = true;
            }
            catch
            {


            }

            finally
            {


                if (standAlone)
                {
                    if (isSuccess)
                    {
                        if (!needSendData)
                        {
                            GeneralApplicationSettings.NeedUpdateLocs = true;
                            ToastManager.LongAlert(GeneralMessages.DataDownloadFromServer);
                        }

                    }
                    else
                    {
                        if (!needSendData)
                        {
                            ToastManager.LongAlert("حدث خطاء اثناء تحميل البيانات");
                        }


                    }



                }
                //aiDownloadData.IsRunning = false;
              //  IsBusy = false;
            }


            return isSuccess;

        }

        private async void btnBackupData_Clicked(object sender, EventArgs e)
        {
            if (await DisplayAlert(GeneralMessages.Question, "هل تريد الاستمرار في عمل نسخة احتياطية ؟", GeneralMessages.Yes, GeneralMessages.No))
            {
                await BackupData();
            }
        }

        private async Task<bool> BackupData(bool standAlone = true)
        {

           // IsBusy = true;

            bool isSuccess = false;
            bool needSendData = false;
            try
            {

                if (standAlone)
                {


                }
           //     aiDownloadData.IsRunning = true;



                // string tempPath = await DependencyService.Get<IFileStorage>().SelectBackUpFolder();
                string folderPath = await DependencyService.Get<IDatabaseSettings>().SampleSectionsFolder();
                string dbPath = DependencyService.Get<IDatabaseSettings>().DatabasePath;
                string dbBackupPath = DependencyService.Get<IDatabaseSettings>().BackupDatabasePath;
                string dbName = Path.GetFileName(dbPath);
                //   string dbBackupPath = Path.Combine(tempPath, dbName);

                string tmpPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), $"Backup_{DateTime.Now.ToString("ddMMyyyyhhmmss")}.zip");

                File.Copy(dbPath, dbBackupPath);
                var files = Directory.GetFiles(folderPath).ToList();
                files.Add(dbBackupPath);


             DependencyService.Get<IZipManager>().QuickZip(files.ToArray(), tmpPath);


                await DependencyService.Get<IFileStorage>().SaveBackupFile(tmpPath);
                //var surveyData = Path.Combine(folderPath, "SurveyData");
                //    Directory.CreateDirectory(surveyData);

                //foreach(var  filePath in files)
                //{
                //    var  filename=   Path.GetFileName(filePath);
                //    var backUp = Path.Combine(tempPath, filename);
                //    File.Copy(filePath, backUp);
                //}


                // File.Copy(dbPath, dbBackupPath);








                isSuccess = true;
            }
            catch
            {


            }

            finally
            {


                if (standAlone)
                {
                    if (isSuccess)
                    {
                        if (!needSendData)
                        {
                            GeneralApplicationSettings.NeedUpdateLocs = true;
                            ToastManager.LongAlert("تم  إنشاء نسخة الاحتياط  بنجاح");
                        }

                    }
                    else
                    {
                        if (!needSendData)
                        {
                            ToastManager.LongAlert("حدث خطاء اثناء انشاء النسخة الاحتياطية");
                        }


                    }



                }
              //  aiDownloadData.IsRunning = false;
               // IsBusy = false;
            }


            return isSuccess;

        }



        private void btnDownloadPanelBack_Clicked(object sender, EventArgs e)
        {
            pnlActions.IsVisible = true;
          //  pnlDownloadData.IsVisible = false;
        }

        private void btnShowDownloadDataPanel_Clicked(object sender, EventArgs e)
        {
            pnlActions.IsVisible = false;
          //  pnlDownloadData.IsVisible = true;
        }

    }
}