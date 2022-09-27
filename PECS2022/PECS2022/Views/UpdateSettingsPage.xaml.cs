using Newtonsoft.Json;
using PECS2022.Interfaces;
using PECS2022.Managers;
using PECS2022.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using PECS2022.Security;
using PECS2022.ViewModels;
using Plugin.Connectivity;
using PECS2022.SurveyModel;

namespace PECS2022.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UpdateSettingsPage : ContentPage
    {
        private UpdateSettingsViewModel model;

        public UpdateSettingsPage()
        {
            InitializeComponent();

            lblResearcher.Text = Security.CurrentUserSettings.CurrentUser.FullName;

            model = new UpdateSettingsViewModel();
            this.BindingContext = model;
        }

        private async Task DownloadImages(string imageName)
        {

            try
            {
                var data = await WebApiDataManager.DownloadImageAsync($"/PortalServices/images/{imageName}", false);

                if (data != null)
                {
                    var path = DependencyService.Get<IDatabaseSettings>().DatabaseFolderPath;
                    string filename = Path.Combine(path, $"{imageName}");
                    File.WriteAllBytes(filename, data);
                }

            }
            catch
            {

            }
        }



        private async Task<bool> DownloadSurvey(int surveyId, bool standAlone = true)
        {
            string surveyLink = $"/ESurveyServices/api/survey/{surveyId}";



            bool isSuccess = false;
            try
            {

                if (standAlone)
                {
                    // btnUpdateSurvey.IsEnabled = false;

                }
                //   ActivityIndicator1.IsRunning = true;
                model.SetBusy();
                var client = WebApiDataManager.GetHttpClient(true);
                var result = await client.GetStringAsync(surveyLink);


                var path = DependencyService.Get<IDatabaseSettings>().DatabaseFolderPath;
                string filename = Path.Combine(path, $"EsurveyLast_{surveyId}.json");

                if (File.Exists(filename)) File.Delete(filename);


                using (StreamWriter objStreamWriter = new StreamWriter(filename, true))
                {
                    objStreamWriter.Write(result);
                    objStreamWriter.Close();

                }

                ApplicationMainSettings.ResetSurvey();
                isSuccess = true;
                // SurveyInfo account = JsonConvert.DeserializeObject<SurveyInfo>(result);



            }
            catch
            {


            }

            finally
            {
                SampleUpdated = isSuccess;

                if (standAlone)
                {
                    if (isSuccess)
                    {
                        ToastManager.LongAlert(GeneralMessages.DownloadSurevySuccess);

                    }
                    else
                    {
                        ToastManager.LongAlert(GeneralMessages.DownloadSurveyERROR);


                    }

                    //btnUpdateSurvey.IsEnabled = true;

                }
                //     ActivityIndicator1.IsRunning = false;
                model.SetNotBusy();
            }

            return isSuccess;

        }



        private async void btnSendData_Clicked(object sender, EventArgs e)
        {

            //await DisplayAlert(GeneralMessages.Warning, "ارسال البيانات غير مفعل", GeneralMessages.Cancel);


            //return;
            bool networkAvailable =  GeneralFunctions.NetworkAvailable();
            if (!networkAvailable)
            {
                await DisplayAlert(GeneralMessages.Warning, GeneralMessages.ConnectError, GeneralMessages.Cancel);
                model.SetNotBusy();
                return;
            }


            // btnSendData.IsEnabled = false;
            // ActivityIndicator1.IsRunning = true;

            bool allSuccess = true;
            try
            {
                var db = DataBase.GetConnection();

                //send samples 
                var samplesList = db.Table<SampleInfo>().Where(x => x.NeedSend == true).ToList();

                if (samplesList.Count() > 0)
                {
                    samplesList.ForEach(sample =>
                    {
                        var samplesResult = WebApiDataManager.Upload<SampleInfo>(WebApiSettings.FullPortalServiceURL, "/PECSServices/api/Sample/Upload", sample, true);

                        if (samplesResult)
                        {
                            sample.NeedSend = false;
                            db.Update(sample);
                        }

                    });
                }

                //send Building 

                var buildingToSend = db.Table<Building>().Where(x => x.IsSent == false).ToList();

                if (buildingToSend.Count() > 0)
                {
                    var buildingResult = WebApiDataManager.UploadList<Building>(WebApiSettings.FullPortalServiceURL, "/PECSServices/api/Visit/UploadBuilding", buildingToSend, true);

                    if (buildingResult)
                    {
                        buildingToSend.ForEach(x => x.IsSent = true);
                        db.UpdateAll(buildingToSend);
                    }
                    else
                    {
                        allSuccess = false;
                    }
                }

                // send Visits

                var visitList = db.Table<Visit>().Where(x => x.NeedSend == true).ToList();

                if (visitList.Count() > 0)
                {
                    var survey = ApplicationMainSettings.GetSurveyInfo();
                    foreach (var visit in visitList)
                    {
                        var visitResult = WebApiDataManager.Upload<Visit>(WebApiSettings.FullPortalServiceURL, "/PECSServices/api/Visit/Upload", visit, true);

                        if (visitResult)
                        {
                            // load individuls 
                            var ID00 = visit.ID00;
                            var indivList = db.Table<Individual>().Where(x => x.ID00 == ID00).ToList();
                            bool indResult = true;
                            bool allSecSuccess = true;
                            if (indivList.Count() > 0)
                            {
                                indResult = WebApiDataManager.UploadList<Individual>(WebApiSettings.FullPortalServiceURL, "/PECSServices/api/Visit/UploadIndividuals", indivList, true);
                            }


                            if (indResult)
                            {
                                foreach (var sec in survey.Sections)
                                {

                                    //if (((visit.IV01 > 0 || visit.IV02 > 0) && sec.Id== 2174) || ((visit.IV03 > 0 || visit.IV04 > 0) && sec.Id == 2176))
                                    //{
                                    var secData = await QuestionnaireManager.GetSectionMaster(ID00, sec.Id, null, false);


                                    if (secData != null && secData.SectionVisit != null)
                                    {

                                        if (secData.SectionVisit.HR01 == null)
                                        {
                                            secData.SectionVisit.HR01 = -1;
                                        }
                                        var secResult = WebApiDataManager.Upload<SectionMaster>(WebApiSettings.FullPortalServiceURL, "/PECSServices/api/Visit/UploadForms", secData.SectionVisit, true);


                                        if (!secResult)
                                        {
                                            allSecSuccess = false;
                                            break;
                                        }
                                    }

                                }

                            }

                            if (indResult && allSecSuccess)
                            {
                                visit.NeedSend = false;
                                db.Update(visit);
                            }
                            else
                            {
                                allSuccess = false;
                            }


                            //addressLog.ForEach(x => x.NeedSend = false);
                            //db.UpdateAll(addressLog);
                        }

                    }


                }


                // send visitLog 

                var visitLog = db.Table<VisitLog>().Where(x => x.NeedSend == true).ToList();

                if (visitLog.Count() > 0)
                {
                    var logResult = WebApiDataManager.UploadList<VisitLog>(WebApiSettings.FullPortalServiceURL, "/PECSServices/api/Visit/UploadVisitHistory", visitLog, true);

                    if (logResult)
                    {
                        visitLog.ForEach(x => x.NeedSend = false);
                        db.UpdateAll(visitLog);
                    }
                    else
                    {
                        allSuccess = false;
                    }
                }

                //send Question Comments 

                var comments = db.Table<QuestionComment>().Where(x => x.IsSent == false).ToList();

                if (comments.Count() > 0)
                {
                    var buildingResult = WebApiDataManager.UploadList<QuestionComment>(WebApiSettings.FullPortalServiceURL, "/PECSServices/api/Visit/UploadQuestionComments", comments, true);

                    if (buildingResult)
                    {
                        comments.ForEach(x => x.IsSent = true);
                        db.UpdateAll(comments);
                    }
                    else
                    {
                        allSuccess = false;
                    }
                }


                var callLogInfos = db.Table<CallLogInfo>().Where(x => x.IsSent == false).ToList();

                if (callLogInfos.Count() > 0)
                {
                    var buildingResult = WebApiDataManager.UploadList(WebApiSettings.FullPortalServiceURL, "/PECSServices/api/Visit/UploadCallLogs", callLogInfos, true);

                    if (buildingResult)
                    {
                        callLogInfos.ForEach(x => x.IsSent = true);
                        db.UpdateAll(callLogInfos);
                    }
                    else
                    {
                        allSuccess = false;
                    }
                }



            }

            catch
            {
                allSuccess = false;

            }


            if (allSuccess)
            {
                ToastManager.LongAlert("تم  ارسال البيانات بنجاح");
            }
            else
            {
                await DisplayAlert(GeneralMessages.Error, "حدث خطاء اثناء ارسال  البيانات برجاء المحاولة مرة  أخرى", GeneralMessages.Cancel);
            }

            //  btnSendData.IsEnabled = true;
            // ActivityIndicator1.IsRunning = false;
            model.SetNotBusy();

        }

        private async void btnUpdateSample_Clicked(object sender, EventArgs e)
        {
            bool networkAvailable =  GeneralFunctions.NetworkAvailable();
            if (networkAvailable)
            {
                await UpdateSamples();
            }
            else
            {
                await DisplayAlert(GeneralMessages.Warning, GeneralMessages.ConnectError, GeneralMessages.Cancel);
                model.SetNotBusy();
            }




        }

        private async Task<bool> UpdateSamples(bool standAlone = true)
        {
            bool isSuccess = false;
            bool needSendData = false;
            try
            {

                if (standAlone)
                {
                    btnUpdateSample.IsEnabled = false;

                }
                // ActivityIndicator1.IsEnabled = true;
                model.SetBusy();
                var db = DataBase.GetConnection();

                // var  samplesToSend= db.Table<Sample>().Where(x => x.NeedSend == true).Count();
                var visits = db.Table<Visit>().Where(x => x.NeedSend == true).Count();

                if (visits > 0)
                {
                    needSendData = true;
                    await DisplayAlert(GeneralMessages.Error, "يجب ارسال  البيانات أولا", GeneralMessages.Cancel);
                }

                else
                {
                  
                    if( await GeneralFunctions.UpdateSamples())
                    {
                        isSuccess = true;
                        GeneralApplicationSettings.NeedUpdateLocs = true;
                    }

                }



            }
            catch
            {


            }

            finally
            {
                SampleUpdated = isSuccess;

                if (standAlone)
                {
                    if (isSuccess)
                    {
                        if (!needSendData)
                        {
                            GeneralApplicationSettings.NeedUpdateLocs = true;
                            GeneralApplicationSettings.NeedUpdateMap = true;
                            ToastManager.LongAlert(GeneralMessages.DownloadSampleSuccess);
                        }

                    }
                    else
                    {
                        if (!needSendData)
                        {
                            ToastManager.LongAlert(GeneralMessages.DownloadSampleERROR);
                        }


                    }

                    btnUpdateSample.IsEnabled = true;

                }
                //  ActivityIndicator1.IsEnabled = false;
                model.SetNotBusy();
            }

            return isSuccess;

        }

        private async void btnUpdateSurvey_Clicked(object sender, EventArgs e)
        {
            await DownloadSurvey(ApplicationMainSettings.SurveyId, true);



        }

        private async void btnUpdateGoves_Clicked(object sender, EventArgs e)
        {
            await UpdateGovernorates();
        }

        private async void btnUpdateProfission_Clicked(object sender, EventArgs e)
        {

            await UpdateProffissionList();
        }

        private async void btnUpdateActivity_Clicked(object sender, EventArgs e)
        {
            await UpdateActivityList();
        }

        private bool GovUpdated { get; set; }
        private bool ProfUpdated { get; set; }
        private bool SampleUpdated { get; set; }

        private bool ActivityUpdated { get; set; }

        private bool LookupsUpdated { get; set; }
        private bool RepsUpdated { get; set; }



        private bool CountryUpdated { get; set; }
        private bool ScientificSpecializationUpdated { get; set; }


        private async Task<bool> UpdateActivityList(bool standAlone = true)
        {
            bool isSuccess = false;
            try
            {
                //   ActivityIndicator1.IsRunning = true;
                model.SetBusy();
                var detailResult = await WebApiDataManager.GetEconomicActivityListAsync();
                var groupResult = await WebApiDataManager.GetEconomicGroupListAsync();

                if (detailResult.IsSuccess && groupResult.IsSuccess)
                {
                    // ActivityIndicator1.IsRunning = true;
                    model.SetBusy();
                    var db = DataBase.GetConnection();

                    db.Table<EconomicGroup>().Delete(X => X.ID != -9999);
                    db.Table<EconomicActivity>().Delete(X => X.Id != -9999);
                    var details = detailResult.Data;
                    var economicGroups = groupResult.Data;


                    db.InsertAll(details);
                    db.InsertAll(economicGroups);
                    isSuccess = true;

                }
            }
            catch
            {


            }

            finally
            {
                ActivityUpdated = isSuccess;

                if (standAlone)
                {
                    if (isSuccess)
                    {
                        ToastManager.LongAlert(GeneralMessages.DownloadEcoActivitySuccess);

                    }
                    else
                    {
                        ToastManager.LongAlert(GeneralMessages.DownloadEcoActivityERROR);
                    }
                }
                //  ActivityIndicator1.IsRunning = false;
                model.SetNotBusy();
            }

            return isSuccess;
        }

        private async Task<bool> UpdateProffissionList(bool standAlone = true)
        {
            bool isSuccess = false;
            try
            {

                //  ActivityIndicator1.IsRunning = true;
                model.SetBusy();
                var profResult = await WebApiDataManager.GetProfessionListAsync();


                if (profResult.IsSuccess)
                {
                    //  ActivityIndicator1.IsRunning = true;
                    model.SetBusy();
                    var db = DataBase.GetConnection();

                    db.Table<Profession>().Delete(X => X.Code != "-999999999999");

                    var profs = profResult.Data;

                    db.InsertAll(profs);

                    isSuccess = true;

                }
            }
            catch
            {
            }

            finally
            {
                ProfUpdated = isSuccess;

                if (standAlone)
                {
                    if (isSuccess)
                    {
                        ToastManager.LongAlert(GeneralMessages.DownloadProfSuccess);

                    }
                    else
                    {
                        ToastManager.LongAlert(GeneralMessages.DownloadProfERROR);
                    }

                }
                // ActivityIndicator1.IsVisible = false;
                model.SetNotBusy();
            }

            return isSuccess;
        }

        private async Task<bool> UpdateGovernorates(bool standAlone = true)
        {
            bool isSuccess = false;
            try
            {
                //  ActivityIndicator1.IsRunning = true;
                model.SetBusy();
                var govResult = await WebApiDataManager.GetGovernorateListAsync();
                var locResult = await WebApiDataManager.GetLocalityListAsync();

                if (govResult.IsSuccess && locResult.IsSuccess)
                {
                    //  ActivityIndicator1.IsEnabled = true;
                    model.SetBusy();
                    var db = DataBase.GetConnection();

                    db.Table<Governorate>().Delete(X => X.Code != "00");
                    db.Table<Locality>().Delete(X => X.Code != "00");
                    var govs = govResult.Data;
                    var locs = locResult.Data;

                    govs.ForEach((g) => { g.ID = Guid.NewGuid(); });

                    db.InsertAll(govs);
                    db.InsertAll(locs);
                    isSuccess = true;
                }
            }
            catch
            {

            }

            finally
            {
                GovUpdated = isSuccess;

                if (standAlone)
                {
                    if (isSuccess)
                    {
                        ToastManager.LongAlert(GeneralMessages.DownloadGovsSuccess);

                    }
                    else
                    {
                        ToastManager.LongAlert(GeneralMessages.DownloadGovsERROR);
                    }

                }
                // ActivityIndicator1.IsRunning = false;
                model.SetNotBusy();
            }

            return isSuccess;
        }

        private async void btnUpdateAllSettings_Clicked(object sender, EventArgs e)
        {
            bool networkAvailable =  GeneralFunctions.NetworkAvailable();
            if (networkAvailable)
            {
                UpdateAllSettings();
            }
            else
            {
                await DisplayAlert(GeneralMessages.Warning, GeneralMessages.ConnectError, GeneralMessages.Cancel);
                model.SetNotBusy();
            }



        }

        private async Task<bool> UpdateLookups(bool standAlone = true)
        {
            bool isSuccess = false;
            try
            {
                //  ActivityIndicator1.IsRunning = true;
                model.SetBusy();
                var lookupResults = await WebApiDataManager.GetLookUpValueListAsync();


                if (lookupResults.IsSuccess)
                {
                    //  ActivityIndicator1.IsEnabled = true;
                    model.SetBusy();
                    var db = DataBase.GetConnection();

                    db.Table<LookupVal>().Delete(X => X.Code != "00");

                    var lookups = lookupResults.Data;

                    lookups.ForEach((g) => { g.KID = Guid.NewGuid(); });

                    db.InsertAll(lookups);
                    LookUpManager.LookupVals = null;
                    isSuccess = true;
                }

            }
            catch
            {
            }

            finally
            {
                LookupsUpdated = isSuccess;

                if (standAlone)
                {
                    if (isSuccess)
                    {
                        ToastManager.LongAlert(GeneralMessages.DownloadSuccess);
                    }
                    else
                    {
                        ToastManager.LongAlert(GeneralMessages.DownloadError);
                    }

                }
                // ActivityIndicator1.IsRunning = false;
                model.SetNotBusy();
            }

            return isSuccess;
        }

        private async Task<bool> UpdateCountry(bool standAlone = true)
        {
            bool isSuccess = false;
            try
            {
                // ActivityIndicator1.IsRunning = true;
                model.SetBusy();
                var countryResult = await WebApiDataManager.GetListAsync<CountryInfo>(WebApiSettings.FullPortalServiceURL, "/PortalServices/api/LookUps/Countries");


                if (countryResult.IsSuccess)
                {
                    //  ActivityIndicator1.IsEnabled = true;
                    model.SetBusy();
                    var countries = countryResult.Data;
                    var db = await DataBase.GetAsyncConnection();
                    await db.Table<CountryInfo>().DeleteAsync(X => X.Code != "-0000");
                    await db.InsertAllAsync(countries);
                    isSuccess = true;
                }
            }
            catch
            {

            }

            finally
            {
                CountryUpdated = isSuccess;

                if (standAlone)
                {
                    if (isSuccess)
                    {
                        ToastManager.LongAlert(GeneralMessages.DownloadSuccess);
                    }
                    else
                    {
                        ToastManager.LongAlert(GeneralMessages.DownloadError);
                    }

                }
                //  ActivityIndicator1.IsRunning = false;
                model.SetNotBusy();
            }

            return isSuccess;
        }

        private async Task<bool> UpdateScientificSpecialization(bool standAlone = true)
        {
            bool isSuccess = false;
            try
            {
                //  ActivityIndicator1.IsRunning = true;
                model.SetBusy();
                var countryResult = await WebApiDataManager.GetListAsync<ScientificSpecialization>(WebApiSettings.FullPortalServiceURL, "/PortalServices/api/LookUps/ScientificSpecialization");


                if (countryResult.IsSuccess)
                {
                    //  ActivityIndicator1.IsEnabled = true;
                    model.SetBusy();
                    var countries = countryResult.Data;
                    var db = await DataBase.GetAsyncConnection();
                    await db.Table<ScientificSpecialization>().DeleteAsync(X => X.Code != "-0000");
                    await db.InsertAllAsync(countries);
                    isSuccess = true;
                }
            }
            catch
            {

            }

            finally
            {
                ScientificSpecializationUpdated = isSuccess;

                if (standAlone)
                {
                    if (isSuccess)
                    {
                        ToastManager.LongAlert(GeneralMessages.DownloadSuccess);
                    }
                    else
                    {
                        ToastManager.LongAlert(GeneralMessages.DownloadError);
                    }

                }
                // ActivityIndicator1.IsRunning = false;
                model.SetNotBusy();
            }

            return isSuccess;
        }

        private async void UpdateAllSettings()
        {
            btnUpdateAllSettings.IsEnabled = false;
            bool updateSurvey = await DownloadSurvey(ApplicationMainSettings.SurveyId, true);
            //  bool updateSample= await UpdateSamples();

            await DownloadImages("balloon.png");
            await DownloadImages("redstickpin.png");

            await UpdateGovernorates(false);
            await UpdateProffissionList(false);
            await UpdateActivityList(false);
            await UpdateLookups(false);

            await UpdateCountry(false);

            await UpdateScientificSpecialization(false);


            btnUpdateAllSettings.IsEnabled = true;

            if (updateSurvey && ActivityUpdated && GovUpdated && ProfUpdated && LookupsUpdated && CountryUpdated && ScientificSpecializationUpdated)
            {
                ToastManager.ShortAlert(GeneralMessages.UpdateAllSettingsSuccess);

                if (!GeneralApplicationSettings.IsFirstTimeSyncDone)
                {
                    await UpdateSamples();
                }

                GeneralApplicationSettings.IsFirstTimeSyncDone = true;

                //  btnClosePage.IsVisible = true;

            }
            else
            {
                ToastManager.ShortAlert(GeneralMessages.UpdateAllSettingsError);
            }
            model.SetNotBusy();
        }

        private void btnClosePage_Clicked(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }

        private async void btnSupport_Clicked(object sender, EventArgs e)
        {
            SupportPage supportPage = new SupportPage();

            // await Navigation.PushModalAsync(supportPage,true);
            //NavigationPage nav = new NavigationPage(new SupportPage() );
            await Navigation.PushAsync(new SupportPage());

        }

        private async void LogOutClicked(object sender, EventArgs e)
        {
            //LogoutButton.IsEnabled = false;
            //var db = DataBase.GetConnection();
            //int affectedRows = db.Table<User>().Delete(x => x.UserName == CurrentUserSettings.CurrentUser.UserName);

            //if (affectedRows == 1)
            //{


            //    Navigation.InsertPageBefore(new LoginPage(), MainPage.instance);

            //    await Navigation.PopToRootAsync(true);
            //}
            //else
            //{
            //    LogoutButton.IsEnabled = true;
            //}
        }
    }
}