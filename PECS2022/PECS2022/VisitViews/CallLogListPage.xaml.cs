using PECS2022.SurveyModel;
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
    public partial class CallLogListPage : ContentPage
    {
        public CallLogListPage()
        {
            InitializeComponent();

            Intitiate();
        }

        private async void Intitiate()
        {

            var db = await DataBase.GetAsyncConnection();
            var idsam = QuestionnaireManager.CurrentSample.ID00;

            var logs = await db.Table<CallLogInfo>().Where(x => x.ID00 == idsam).OrderBy(x => x.C3).ToListAsync();
            var Individuals = await db.Table<Individual>().Where(x => x.ID00 == idsam).OrderBy(i => i.D1).ToListAsync();
            var c4Lookup = LookUpManager.GetSurveyLookupById(4506);
            var c5Lookup = LookUpManager.GetSurveyLookupById(4507);
            var c7Lookup = LookUpManager.GetSurveyLookupById(4509);
            var c8Lookup = LookUpManager.GetSurveyLookupById(4510);


          
            int count = 1;
            List<CallsView> logsView = new List<CallsView>();
            foreach (var l in logs)
            {
                var log = new CallsView() { C1 = count, C2 = l.C2, C3 = l.C3.ToString("dd-MM-yyyy hh:mm tt"), C9 = l.Comments };
                logsView.Add(log);
                count++;
                if (l.C4.HasValue)
                {
                    log.C4 = c4Lookup.FirstOrDefault(x => x.AnswerId == l.C4)?.FullDescription;
                }
                if (l.C5.HasValue)
                {
                    log.C5 = c5Lookup.FirstOrDefault(x => x.AnswerId == l.C5)?.FullDescription;
                }
                if (l.C6.HasValue)
                {

                    if (l.C6 == 0)
                    {
                        log.C6 = l.C6_Name;
                    }
                    else
                    {
                        log.C6 = Individuals.FirstOrDefault(x => x.D1 == l.C6)?.D2;
                    }

                }
                else
                {
                    log.C6 = l.C6_Name;
                }

                if (l.C7.HasValue)
                {
                    log.C7 = c7Lookup.FirstOrDefault(x => x.AnswerId == l.C7)?.FullDescription;
                }

                if (l.C8.HasValue)
                {
                    log.C8 = c8Lookup.FirstOrDefault(x => x.AnswerId == l.C8)?.FullDescription;
                }





            }

            dataGrid.ItemsSource = logsView.OrderByDescending(x=>x.C1);
        }
    }


    public class CallsView
    {

        public int C1 { get; set; }
        public string C2 { get; set; }
        public string C3 { get; set; }
        public string C4 { get; set; }
        public string C5 { get; set; }
        public string C6 { get; set; }
        public string C7 { get; set; }
        public string C8 { get; set; }
        public string C9 { get; set; }


    }
}