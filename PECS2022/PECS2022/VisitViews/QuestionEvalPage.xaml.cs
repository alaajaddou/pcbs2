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
    public partial class QuestionEvalPage : ContentPage
    {
        public QuestionEvalPage()
        {
            InitializeComponent();

            Initialize();
        }

        private async void Initialize()
        {
            var db = await DataBase.GetAsyncConnection();

            dataGrid.ItemsSource = await db.Table<Models.QuestionComment>().OrderByDescending(x => x.CreatedDate).ToListAsync();
            ClearInput();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            if (IsValid())
            {
                var db = await DataBase.GetAsyncConnection();
                Models.QuestionComment questionComment = new Models.QuestionComment() { Code = txtQCode.Text, Comments = txtComments.Text, CreatedBy = Security.CurrentUserSettings.CurrentUser.UserName };

                await db.InsertAsync(questionComment);

                Initialize();
            }
        }

        private bool IsValid()
        {
            bool valid = true;
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("برجاء تعبئة الحقول التالية أولا");
            if (string.IsNullOrWhiteSpace(txtQCode.Text))
            {
                stringBuilder.AppendLine("- رمز السؤال");
                valid = false;
            }

            if (string.IsNullOrWhiteSpace(txtComments.Text))
            {
                stringBuilder.AppendLine("- الملاحظات");
                valid = false;
            }


            if (!valid)
            {
                DisplayAlert(GeneralMessages.Error, stringBuilder.ToString(), GeneralMessages.Cancel);
            }


            return valid;

        }

        private void ClearInput()
        {
            txtQCode.Text = "";
            txtComments.Text = "";
        }
    }
}