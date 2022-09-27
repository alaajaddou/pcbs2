using Newtonsoft.Json;
using PECS2022.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Syncfusion.XForms.PopupLayout;
using PECS2022.Util;

namespace PECS2022.VisitViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class QuestionnairePage : ContentPage
    {
        #region General

        #region Attributes
        private SectionStatus CurrentSecStatus;
        public int CurrentSectionId = 0;
        public int? HR01 = null;
        List<QuestionInfo> QuestionList = null;
        List<QuestionAnswer> OldAnswerList = new List<QuestionAnswer>();
        QuestionInfo CurrentQuestion = null;

        internal SurveyInfo CurrentSurvey = null;
        SectionMaster SectionVisit = null;
        public bool IsSaved { get; set; }
        private bool IsComplete { get; set; }
        private int CurrentIndex = 0;
        #endregion

        #region Init
        public QuestionnairePage(int sectionId, int? HR01)
        {

            if (!Security.CurrentUserSettings.CurrentUser.GovCode.EqualsOneOf(GeneralApplicationSettings.GazaGovs))
            {
                SkipedFields = new string[]{ "R1", "P1_Code" };
            }
            CurrentSectionId = sectionId;
            this.HR01 = HR01;
            InitializeComponent();

            Initialize();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (CurrentSecStatus == null)
            {
                CurrentSecStatus = QuestionnaireManager.GetSectionStatus(CurrentSectionId, HR01);
                //  CurrentSecStatus.CurrentStatusId = (int)CurrentStatus.Edititing;
                // IsComplete = false;

                UpdateSecStatus();

            }

            if (CurrentQuestion != null && (CurrentQuestion.Code == "GA2" || CurrentQuestion.Code == "QC6" || CurrentQuestion.Code == "I01"))
            {
                Picker picker2 = pnlContents.Children[0] as Picker;
                if (picker2 != null)
                {
                    var d1 = picker2.GetAnswerCode();
                    FillIndividual(CurrentQuestion.Code, picker2);
                    picker2.SetAnswerCode(d1);
                }

            }

        }

        #endregion

        #region Main

        private void UpdateSecStatus()
        {
            VisitPage page = (VisitPage)this.Parent.Parent;
            QuestionnaireManager.UpdateSectionStatus(CurrentSecStatus.ID00, CurrentSecStatus.SectionId, (CurrentStatus)CurrentSecStatus.CurrentStatusId, CurrentSecStatus.HR01);
            page.SetSecionStatus((CurrentStatus)CurrentSecStatus.CurrentStatusId);

        }

        private async void Initialize()
        {
            CurrentSurvey = ApplicationMainSettings.GetSurveyInfo();

            SectionPageData data = await QuestionnaireManager.GetSectionMaster(QuestionnaireManager.CurrentVisit.ID00, CurrentSectionId, HR01, true);
            SectionVisit = data.SectionVisit;
            OldAnswerList = data.OldAnswerList;
            IsComplete = data.SectionVisit.IsComplete;
            SectionInfo sectionInfo = CurrentSurvey.Sections.Where(x => x.Id == CurrentSectionId).FirstOrDefault();

            var groups = sectionInfo.Groups.OrderBy(x => x.OrderId).ToList();

            QuestionList = new List<QuestionInfo>();

            foreach (var g in groups)
            {
                QuestionList.AddRange(g.Questions.OrderBy(x => x.OrderId).ToList());
            }


            lblNoQuestions.IsVisible = false;
            pnlDesc.IsVisible = true;
            pnlComment.IsVisible = true;

            if (QuestionList.Count == 0)
            {
                lblNoQuestions.IsVisible = true;
                pnlDesc.IsVisible = false;
                pnlComment.IsVisible = false;
                return;
            }

            // 
            CurrentIndex = 0;
            if (SectionVisit.Answers != null && SectionVisit.Answers.Count() > 0)
            {
                Guid lastQId = SectionVisit.Answers.Last().QId;
                var q = QuestionList.Where(x => x.Id == lastQId).FirstOrDefault();

                if (q != null)
                {
                    CurrentIndex = QuestionList.IndexOf(q);

                    QuestionAnswer questionAnswer = SectionVisit.Answers.Where(x => x.QId == q.Id).FirstOrDefault();
                    if (questionAnswer != null)
                    {
                        OldAnswerList.Add(questionAnswer);
                        SectionVisit.Answers.Remove(questionAnswer);

                    }



                }

            }



            //btnPrvious.IsEnabled = false;
            CurrentQuestion = QuestionList[CurrentIndex];
            if (!IsFullValidQuestion(CurrentQuestion))
            {
                CurrentQuestion = GetNextQuestion();

            }
            SetQuestionToScreen();

            var oldAnswer = OldAnswerList.Where(x => x.QId == CurrentQuestion.Id).FirstOrDefault();
            if (oldAnswer != null)
            {
                SetAnswerToScreen(oldAnswer);

            }

        }

        private List<LookUpValueInfo> FilterLookups(List<LookUpValueInfo> columnLookUps)
        {

            return columnLookUps;
        }

        private void BuildQuestion()
        {
            btnPrvious.IsEnabled = CurrentIndex != 0;
            btnNext.IsEnabled = CurrentIndex + 1 != QuestionList.Count;

            if (btnNext.IsEnabled == false)
            {
                IsComplete = true;
            }

            ClearScreen();

            pnlHeader_ScrollControls.IsVisible = true;
            pnlContentsHeader.IsVisible = true;

            var colGestureRecognizer = new TapGestureRecognizer();
            colGestureRecognizer.NumberOfTapsRequired = 2;
            colGestureRecognizer.Tapped += LabelTapGestureRecognizer_Tapped;

            var rowGestureRecognizer = new TapGestureRecognizer();
            rowGestureRecognizer.NumberOfTapsRequired = 1;
            rowGestureRecognizer.Tapped += HightLightRow_Tapped;

            if (CurrentQuestion.DisplayTypeId == 1)
            {
                pnlHeader_ScrollControls.IsVisible = false;
                pnlContentsHeader.IsVisible = false;

                View view = BuildSimpleTypeControls(CurrentQuestion.TypeId.Value, CurrentQuestion.LookUpId);

                view.VerticalOptions = LayoutOptions.StartAndExpand;

                view.HeightRequest = 40;


                pnlContents.Children.Add(view);

                //if (CurrentQuestion.TypeId == 3)
                //{

                //    Label label = new Label() { Text = "ملاحظات" };
                //    pnlContents.Children.Add(label);

                //    Entry entry = new Label() {  W };
                //    pnlContents.Children.Add(label);

                //}
            }
            else if (CurrentQuestion.DisplayTypeId == 2)
            {
                Grid grid = new Grid();
                grid.RowSpacing = 5;
                grid.ColumnSpacing = 5;
                pnlContentsHeader.RowSpacing = grid.RowSpacing;
                pnlContentsHeader.ColumnSpacing = grid.ColumnSpacing;

                grid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });

                List<LookUpValueInfo> RowlookUpValueInfos = CurrentSurvey.LookUps.Where(x => x.Id == CurrentQuestion.RowLookUpId).SelectMany(x => x.LookUpValues).ToList();
                List<LookUpValueInfo> columnLookUps = new List<LookUpValueInfo>();

                if (CurrentQuestion.SingleValueMatrix)
                {
                    columnLookUps.Add(new LookUpValueInfo() { AnswerCode = "", Description = "الاجابة" });
                }
                else
                {
                    columnLookUps = CurrentSurvey.LookUps.Where(x => x.Id == CurrentQuestion.ColumnLookUpId).SelectMany(x => x.LookUpValues).ToList();

                    columnLookUps = FilterLookups(columnLookUps);
                }

                int columnLookUpsCount = columnLookUps.Count();


                Label lbl = new Label() { Text = "السؤال", TextColor = Color.Black };

                Grid subGrid = new Grid();
                subGrid.Children.Add(lbl);
                subGrid.Padding = 5;
                subGrid.BackgroundColor = Color.FromHex("#cbcbcb");

                pnlContentsHeader.Children.Add(subGrid);
                Grid.SetColumn(subGrid, 0);
                Grid.SetRow(subGrid, 0);

                //if(columnLookUpsCount == 1)
                //{
                //    btnExpandQuestion.IsVisible = false;
                //}
                //else
                //{
                //    btnExpandQuestion.IsVisible = true;
                //}

                int colCount = 1;
                foreach (var c in columnLookUps)
                {
                    Label lblCol = new Label() { VerticalOptions = LayoutOptions.FillAndExpand, HorizontalOptions = LayoutOptions.FillAndExpand, BackgroundColor = Color.FromHex("#cbcbcb"), TextColor = Color.Black };

                    lblCol.FormattedText = new FormattedString();
                    if (!string.IsNullOrWhiteSpace(c.AnswerCode))
                    {
                        lblCol.FormattedText.Spans.Add(new Span() { Text = "أ", TextColor = lblCol.BackgroundColor });
                        lblCol.FormattedText.Spans.Add(new Span() { Text = $"{c.AnswerCode} - ", FontAttributes = FontAttributes.Bold });
                    }
                    lblCol.FormattedText.Spans.Add(new Span() { Text = c.Description });
                    lblCol.GestureRecognizers.Add(colGestureRecognizer);

                    subGrid = new Grid();
                    subGrid.Children.Add(lblCol);
                    subGrid.Padding = 5;
                    subGrid.BackgroundColor = Color.FromHex("#cbcbcb");

                    pnlContentsHeader.Children.Add(subGrid);
                    Grid.SetColumn(subGrid, colCount++);
                    Grid.SetRow(subGrid, 0);

                }

                int rIndex = 1;

                foreach (var r in RowlookUpValueInfos)
                {
                    if (Device.RuntimePlatform == Device.Android)
                    {
                        grid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
                    }
                    else
                    {
                        grid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Star });
                    }

                    Label lblRow = new Label() { TextColor = Color.Black, VerticalOptions = LayoutOptions.FillAndExpand, VerticalTextAlignment = TextAlignment.Center };

                    lblRow.FormattedText = new FormattedString();
                    lblRow.FormattedText.Spans.Add(new Span() { Text = $"{r.AnswerCode} - ", FontAttributes = FontAttributes.Bold });
                    lblRow.FormattedText.Spans.Add(new Span() { Text = r.Description });

                    StackLayout stack = new StackLayout();
                    stack.Children.Add(lblRow);
                    stack.Padding = 5;
                    stack.BackgroundColor = Color.FromHex("#cbcbcb");
                    stack.GestureRecognizers.Add(rowGestureRecognizer);

                    grid.Children.Add(stack);
                    Grid.SetColumn(stack, 0);
                    Grid.SetRow(stack, rIndex);

                    int colIndex = 1;
                    foreach (var c in columnLookUps)
                    {
                        View view = BuildSimpleTypeControls(CurrentQuestion.TypeId.Value, CurrentQuestion.LookUpId);
                        grid.Children.Add(view);
                        Grid.SetColumn(view, colIndex++);
                        Grid.SetRow(view, rIndex);
                    }

                    rIndex++;
                }

                //  grid.HeightRequest = RowlookUpValueInfos.Count() * 40;

                pnlContents.Children.Add(grid);

            }
            else if (CurrentQuestion.DisplayTypeId == 3)
            {
                Grid grid = new Grid();
                grid.RowSpacing = 5;
                grid.ColumnSpacing = 5;
                pnlContentsHeader.RowSpacing = grid.RowSpacing;
                pnlContentsHeader.ColumnSpacing = grid.ColumnSpacing;

                FormListInfo formList = CurrentSurvey.FormLists.Where(x => x.ID == CurrentQuestion.ListId).FirstOrDefault();

                if (formList != null)
                {
                    var fields = formList.FormListFields.Where(x=> !SkipedFields.Contains(x.Code)).OrderBy(x => x.OrderId).ToList();

                    var primaryKey = formList.FormListFields.Where(x => !SkipedFields.Contains(x.Code)).Where(x => x.IsKey == true).FirstOrDefault();
                    int fCount = formList.FormListFields.Where(x => !SkipedFields.Contains(x.Code)).Count();
                    bool hasPK = !(primaryKey != null && primaryKey.TypeId == 3);

                    if (hasPK)
                    {
                        fCount++;
                    }

                    //if (fields.Count == 1)
                    //{
                    //    btnExpandQuestion.IsVisible = false;
                    //}
                    //else
                    //{
                    //    btnExpandQuestion.IsVisible = true;
                    //}

                    grid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });

                    int colIndex = 0;
                    foreach (var field in fields)
                    {
                        Label label = new Label() { VerticalOptions = LayoutOptions.FillAndExpand, HorizontalOptions = LayoutOptions.FillAndExpand, BackgroundColor = Color.FromHex("#cbcbcb"), TextColor = Color.Black };

                        label.FormattedText = new FormattedString();
                        label.FormattedText.Spans.Add(new Span() { Text = "أ", TextColor = label.BackgroundColor });
                        label.FormattedText.Spans.Add(new Span() { Text = $"{field.Code} - ", FontAttributes = FontAttributes.Bold });
                        label.FormattedText.Spans.Add(new Span() { Text = field.Description });

                        pnlContentsHeader.Children.Add(label);
                        Grid.SetColumn(label, colIndex++);
                        Grid.SetRow(label, 0);

                        label.GestureRecognizers.Add(colGestureRecognizer);
                    }

                    if (hasPK)
                    {
                        Label label = new Label() { VerticalOptions = LayoutOptions.FillAndExpand, HorizontalOptions = LayoutOptions.FillAndExpand, Text = $"إدارة", HorizontalTextAlignment = TextAlignment.Center, BackgroundColor = Color.FromHex("#cbcbcb"), TextColor = Color.Black };
                        pnlContentsHeader.Children.Add(label);
                        Grid.SetColumn(label, colIndex++);
                        Grid.SetRow(label, 0);
                    }

                    if (primaryKey != null && primaryKey.TypeId == 3)
                    {
                        var lookUpValues = CurrentSurvey.LookUps.Where(x => x.Id == primaryKey.LookUpId).FirstOrDefault().LookUpValues.ToList();
                        int rIndex = 1;

                        foreach (var lValue in lookUpValues)
                        {

                            if (Device.RuntimePlatform == Device.Android)
                            {
                                grid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Star });
                            }
                            else
                            {
                                grid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Star });
                            }

                            Dictionary<string, View> views = new Dictionary<string, View>();
                            colIndex = 0;
                            foreach (var filed in fields)
                            {
                                View view = null;
                                if (colIndex != 0)
                                {
                                    view = BuildSimpleTypeControls(filed.TypeId.Value, filed.LookUpId);
                                    views.Add(filed.Code, view);
                                }
                                else
                                {

                                    Label label = new Label() { VerticalOptions = LayoutOptions.FillAndExpand, VerticalTextAlignment = TextAlignment.Center, TextColor = Color.Black };
                                    label.FormattedText = new FormattedString();
                                    label.FormattedText.Spans.Add(new Span() { Text = $"{lValue.AnswerCode} - ", FontAttributes = FontAttributes.Bold });
                                    label.FormattedText.Spans.Add(new Span() { Text = lValue.Description });

                                    StackLayout stack = new StackLayout();
                                    stack.Children.Add(label);
                                    stack.Padding = 5;
                                    stack.BackgroundColor = Color.FromHex("#cbcbcb");
                                    stack.GestureRecognizers.Add(rowGestureRecognizer);

                                    view = stack;

                                }
                                grid.Children.Add(view);
                                Grid.SetColumn(view, colIndex++);
                                Grid.SetRow(view, rIndex);

                            }

                            ApplyListCustomActions(formList, views, true);

                            rIndex++;
                        }
                    }

                    else
                    {

                        Button button = new Button { Text = "إضافة" };
                        button.StyleClass = new List<string> { "Green" };
                        button.Clicked += ((s, e) =>
                        {
                            grid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
                            var children = grid.Children.ToList();
                            int index = children.Any() ? children.Select(x => Grid.GetRow(x)).Max() : 0;
                            index++;
                            colIndex = 0;

                            Dictionary<string, View> views = new Dictionary<string, View>();
                            foreach (var filed in fields)
                            {
                                // 
                                View view = BuildSimpleTypeControls(filed.TypeId.Value, filed.LookUpId);
                                grid.Children.Add(view);
                                views.Add(filed.Code, view);
                                Grid.SetColumn(view, colIndex++);
                                Grid.SetRow(view, index);
                            }

                            ApplyListCustomActions(formList, views);


                            if (hasPK)
                            {
                                Button btnDelete = new Button { Text = "حذف" };
                                btnDelete.StyleClass = new List<string> { "Red" };

                                btnDelete.Clicked += (async (s2, e2) =>
                                {
                                    bool result = await DisplayAlert(GeneralMessages.Question, "هل انت متاكد من عملية الحذف؟", GeneralMessages.Yes, GeneralMessages.No);
                                    if (result)
                                    {
                                        var last = grid.RowDefinitions.Last();
                                        grid.RowDefinitions.Remove(last);
                                        var row = Grid.GetRow((Button)s2);
                                        var children2 = grid.Children.ToList();
                                        foreach (var child in children2.Where(child => Grid.GetRow(child) == row))
                                        {
                                            grid.Children.Remove(child);
                                        }
                                        foreach (var child in children2.Where(child => Grid.GetRow(child) > row))
                                        {
                                            Grid.SetRow(child, Grid.GetRow(child) - 1);
                                        }
                                        // rIndex--;
                                    }

                                });
                                grid.Children.Add(btnDelete);
                                Grid.SetColumn(btnDelete, colIndex++);
                                Grid.SetRow(btnDelete, index);
                            }
                        });


                        pnlContents.Children.Add(button);
                        //Grid.SetRow(button,rIndex);
                        //Grid.SetColumn(button, 0);
                        //Grid.SetColumnSpan(button, fields.Count);
                    }

                    pnlContents.Children.Add(grid);

                }
            }

            if (CurrentQuestion.DisplayTypeId == 2 || CurrentQuestion.DisplayTypeId == 3)
            {
                if (questionExpanded)
                    OnExpandQuestion();
            }

            OnBuildQuestion();
        }


        private string[] SkipedFields = { /*"R1" , "P1_Code"*/ };

        private void ApplyListCustomActions(FormListInfo formList, Dictionary<string, View> views, bool byForce = false)
        {

            if (formList != null && views != null)
            {

                if(formList.ID== new Guid("4f64492e-b496-47e8-9dd6-f39f2eacaef3"))
                {
                    var newControlsToApplyAction = new List<View>();
                    newControlsToApplyAction.Add(views["BTQ7_C"]);
                   

                    var oldControlsToApplyAction = new List<View>();
                    oldControlsToApplyAction.Add(views["BTQ7_A"]);
                    oldControlsToApplyAction.Add(views["BTQ7_B"]);

                    if (GetGA2Individual()?.IsNew ?? false)
                    {
                        newControlsToApplyAction.ForEach(v => { v.IsEnabled = true; });

                        oldControlsToApplyAction.ForEach(v => { v.IsEnabled = false; if (v is Entry) { ((Entry)v).Text = null; } else if (v is Picker) { ((Picker)v).SelectedIndex = 0; } });

                    }
                    else
                    {
                        oldControlsToApplyAction.ForEach(v => { v.IsEnabled = true; });

                        newControlsToApplyAction.ForEach(v => { v.IsEnabled = false; if (v is Entry) { ((Entry)v).Text = null; } else if (v is Picker) { ((Picker)v).SelectedIndex = 0; } });

                    }
                }

                if (formList.ID == new Guid("efc86287-524e-4fc9-be75-e3377e5a5225"))
                {
                    var controlsToApplyAction = new List<View>();
                    controlsToApplyAction.Add(views["C02_F"]);
                    controlsToApplyAction.Add(views["C02_F_CMNT"]);


                    var controlsToApplyAction_2 = new List<View>();
                    controlsToApplyAction_2.Add(views["C02_A_CMNT"]);

                    var controlsToApplyAction_3 = new List<View>();
                    controlsToApplyAction_3.Add(views["C02_C_CMNT"]);

                    var controlsToApplyAction_4 = new List<View>();
                    controlsToApplyAction_4.Add(views["C02_F_CMNT"]);


                    View C02_E = views["C02_E"];

                    Picker pickerC02_E = C02_E as Picker;


                    View C02_A = views["C02_A"];

                    Picker pickerC02_A = C02_A as Picker;

                    View C02_C = views["C02_C"];

                    Picker pickerC02_C = C02_C as Picker;

                    View C02_F = views["C02_F"];

                    Picker pickerC02_F = C02_F as Picker;

                    if (pickerC02_E != null)
                    {
                        pickerC02_E.SelectedIndexChanged += ((e, s) =>
                        {

                            var C02_EVal = pickerC02_E.SelectedItem as LookUpValueInfo;

                            if (C02_EVal?.AnswerCode == "3" || C02_EVal?.AnswerCode == "4")
                            {

                                controlsToApplyAction.ForEach(v => { v.IsEnabled = true; });

                            }

                            else
                            {
                                controlsToApplyAction.ForEach(v => { v.IsEnabled = false; if (v is Entry) { ((Entry)v).Text = null; } else if (v is Picker) { ((Picker)v).SelectedIndex = 0; } });
                            }
                        });

                        if (byForce)
                        {
                            var C02_EVal = pickerC02_E.SelectedItem as LookUpValueInfo;

                            pickerC02_E.SelectedIndex = 0;

                            pickerC02_E.SelectedItem = C02_EVal;
                        }
                    }

                    if (pickerC02_A != null)
                    {
                        pickerC02_A.SelectedIndexChanged += ((e, s) =>
                        {

                            var C02_EVal = pickerC02_A.SelectedItem as LookUpValueInfo;

                            if (C02_EVal?.AnswerCode == "15")
                            {

                                controlsToApplyAction_2.ForEach(v => { v.IsEnabled = true; });

                            }

                            else
                            {
                                controlsToApplyAction_2.ForEach(v => { v.IsEnabled = false; if (v is Entry) { ((Entry)v).Text = null; } else if (v is Picker) { ((Picker)v).SelectedIndex = 0; } });
                            }
                        });

                        if (byForce)
                        {
                            var C02_EVal = pickerC02_A.SelectedItem as LookUpValueInfo;

                            pickerC02_A.SelectedIndex = 0;

                            pickerC02_A.SelectedItem = C02_EVal;
                        }
                    }

                    if (pickerC02_C != null)
                    {
                        pickerC02_C.SelectedIndexChanged += ((e, s) =>
                        {

                            var C02_EVal = pickerC02_C.SelectedItem as LookUpValueInfo;

                            if (C02_EVal?.AnswerCode == "14")
                            {

                                controlsToApplyAction_3.ForEach(v => { v.IsEnabled = true; });

                            }

                            else
                            {
                                controlsToApplyAction_3.ForEach(v => { v.IsEnabled = false; if (v is Entry) { ((Entry)v).Text = null; } else if (v is Picker) { ((Picker)v).SelectedIndex = 0; } });
                            }
                        });

                        if (byForce)
                        {
                            var C02_EVal = pickerC02_C.SelectedItem as LookUpValueInfo;

                            pickerC02_C.SelectedIndex = 0;

                            pickerC02_C.SelectedItem = C02_EVal;
                        }
                    }

                    if (pickerC02_F != null)
                    {
                        pickerC02_F.SelectedIndexChanged += ((e, s) =>
                        {

                            var C02_EVal = pickerC02_F.SelectedItem as LookUpValueInfo;

                            if (C02_EVal?.AnswerCode == "5")
                            {

                                controlsToApplyAction_4.ForEach(v => { v.IsEnabled = true; });

                            }

                            else
                            {
                                controlsToApplyAction_4.ForEach(v => { v.IsEnabled = false; if (v is Entry) { ((Entry)v).Text = null; } else if (v is Picker) { ((Picker)v).SelectedIndex = 0; } });
                            }
                        });

                        if (byForce)
                        {
                            var C02_EVal = pickerC02_F.SelectedItem as LookUpValueInfo;

                            pickerC02_F.SelectedIndex = 0;

                            pickerC02_F.SelectedItem = C02_EVal;
                        }
                    }

                }


                else if (formList.ID == new Guid("623378ae-d406-4bad-aa0a-5c28bdcc3a6a"))
                {
                    var controlsToApplyAction = new List<View>();
                    controlsToApplyAction.Add(views["C13_B"]);
                    controlsToApplyAction.Add(views["C13_C"]);

                    View C13_A = views["C13_A"];

                    Picker pickerC13_A = C13_A as Picker;

                    if (pickerC13_A != null)
                    {
                        pickerC13_A.SelectedIndexChanged += ((e, s) =>
                        {

                            var C02_EVal = pickerC13_A.SelectedItem as LookUpValueInfo;

                            if (C02_EVal?.AnswerCode == "1")
                            {

                                controlsToApplyAction.ForEach(v => v.IsEnabled = true);

                            }

                            else
                            {
                                controlsToApplyAction.ForEach(v => { v.IsEnabled = false; if (v is Entry) { ((Entry)v).Text = null; } else if (v is Picker) { ((Picker)v).SelectedIndex = 0; } });
                            }
                        });

                        if (byForce)
                        {
                            var C02_EVal = pickerC13_A.SelectedItem as LookUpValueInfo;

                            pickerC13_A.SelectedIndex = 0;

                            pickerC13_A.SelectedItem = C02_EVal;
                        }
                    }
                }


                else if (formList.ID == new Guid("83543db8-5678-46ab-9c6e-044cb6ab2a12"))
                {
                    var controlsToApplyAction = new List<View>();
                    controlsToApplyAction.Add(views["E801B"]);
                    controlsToApplyAction.Add(views["E801B_CMNT"]);


                    var controlsToApplyAction_2 = new List<View>();
                    controlsToApplyAction_2.Add(views["E801B_CMNT"]);


                    View E801A = views["E801A"];

                    Entry entryE801A = E801A as Entry;

                    View E801B = views["E801B"];

                    Picker entryE801B = E801B as Picker;

                    if (entryE801A != null)
                    {
                        entryE801A.TextChanged += ((e, s) =>
                        {


                            if (entryE801A.Text != "0")
                            {
                                controlsToApplyAction.ForEach(v => v.IsEnabled = true);
                            }
                            else
                            {
                                controlsToApplyAction.ForEach(v => { v.IsEnabled = false; if (v is Entry) { ((Entry)v).Text = null; } else if (v is Picker) { ((Picker)v).SelectedIndex = 0; } });

                            }

                        });
                    }

                    if (entryE801B != null)
                    {
                        entryE801B.SelectedIndexChanged += ((e, s) =>
                        {

                            var C02_EVal = entryE801B.SelectedItem as LookUpValueInfo;

                            if (C02_EVal?.AnswerCode == "9")
                            {

                                controlsToApplyAction_2.ForEach(v => v.IsEnabled = true);

                            }

                            else
                            {
                                controlsToApplyAction_2.ForEach(v => { v.IsEnabled = false; if (v is Entry) { ((Entry)v).Text = null; } else if (v is Picker) { ((Picker)v).SelectedIndex = 0; } });
                            }
                        });

                        if (byForce)
                        {
                            var C02_EVal = entryE801B.SelectedItem as LookUpValueInfo;

                            entryE801B.SelectedIndex = 0;

                            entryE801B.SelectedItem = C02_EVal;
                        }
                    }

                }


                else if (formList.ID == new Guid("f89ad024-5872-4a55-a045-d5b2b9139bc9"))
                {
                    var controlsToApplyAction = new List<View>();
                    controlsToApplyAction.Add(views["I06B"]);


                    View I06A = views["I06A"];

                    Picker entryI06A = I06A as Picker;

                    if (entryI06A != null)
                    {
                        entryI06A.SelectedIndexChanged += ((e, s) =>
                        {

                          var  val=  entryI06A.SelectedItem as LookUpValueInfo;
                            if (val?.AnswerCode == "1")
                            {
                                controlsToApplyAction.ForEach(v => v.IsEnabled = true);
                            }
                            else
                            {
                                controlsToApplyAction.ForEach(v => { v.IsEnabled = false; if (v is Entry) { ((Entry)v).Text = null; } else if (v is Picker) { ((Picker)v).SelectedIndex = 0; } });

                            }

                        });
                    }
                }

                else if (formList.ID == new Guid("40ef6223-c10d-43cc-9d83-c56c9ad7b6be"))
                {
                    var controlsToApplyAction = new List<View>();
                    controlsToApplyAction.Add(views["AG8_14_B"]);
                    controlsToApplyAction.Add(views["AG8_14_C"]);
                    controlsToApplyAction.Add(views["AG8_14_D"]);



                    View AG8_14_A = views["AG8_14_A"];

                    Entry entryAG8_14_A = AG8_14_A as Entry;

                    if (entryAG8_14_A != null)
                    {
                        entryAG8_14_A.TextChanged += ((e, s) =>
                        {


                            if (entryAG8_14_A.Text != "0")
                            {
                                controlsToApplyAction.ForEach(v => v.IsEnabled = true);
                            }
                            else
                            {
                                controlsToApplyAction.ForEach(v => { v.IsEnabled = false; if (v is Entry) { ((Entry)v).Text = null; } else if (v is Picker) { ((Picker)v).SelectedIndex = 0; } });

                            }

                        });
                    }
                }

                else if (formList.ID == new Guid("933a2dc7-916b-42b3-913c-df77f758f82c"))
                {
                    var controlsToApplyAction = new List<View>();
                    controlsToApplyAction.Add(views["T2_B_1"]);
                    controlsToApplyAction.Add(views["T2_B_2"]);
                    controlsToApplyAction.Add(views["T2_B_3"]);
                    controlsToApplyAction.Add(views["T2_B_4"]);
                    controlsToApplyAction.Add(views["T2_B_4_CMNT"]);
                    controlsToApplyAction.Add(views["T2_B_5"]);



                    var controlsToApplyAction_2 = new List<View>();
                    controlsToApplyAction_2.Add(views["T2_B_4_CMNT"]);



                    View I06A = views["T2_A"];

                    Picker entryI06A = I06A as Picker;

                    View T2_B4 = views["T2_B_4"];

                    Picker entryT2_B4 = T2_B4 as Picker;

                    if (entryI06A != null)
                    {
                        entryI06A.SelectedIndexChanged += ((e, s) =>
                        {
                            var val = entryI06A.SelectedItem as LookUpValueInfo;

                            if (val?.AnswerCode=="2" || val?.AnswerCode == "3")
                            {
                                controlsToApplyAction.ForEach(v => v.IsEnabled = true);
                            }
                            else
                            {
                                controlsToApplyAction.ForEach(v => { v.IsEnabled = false; if (v is Entry) { ((Entry)v).Text = null; } else if (v is Picker) { ((Picker)v).SelectedIndex = 0; } });

                            }

                        });
                    }

                    if (entryT2_B4 != null)
                    {
                        entryT2_B4.SelectedIndexChanged += ((e, s) =>
                        {
                            var val = entryT2_B4.SelectedItem as LookUpValueInfo;

                            if (val?.AnswerCode == "1" )
                            {
                                controlsToApplyAction_2.ForEach(v => v.IsEnabled = true);
                            }
                            else
                            {
                                controlsToApplyAction_2.ForEach(v => { v.IsEnabled = false; if (v is Entry) { ((Entry)v).Text = null; } else if (v is Picker) { ((Picker)v).SelectedIndex = 0; } });

                            }

                        });
                    }

                }

                else if (formList.ID == new Guid("81cd8445-acdd-44cd-8058-8356b5d2e964"))
                {
                    var controlsToApplyAction = new List<View>();
                    controlsToApplyAction.Add(views["T3_2_CMNT"]);

                    var controlsToApplyAction3 = new List<View>();
                    controlsToApplyAction3.Add(views["T3_3"]);


                    var controlsToApplyAction2 = new List<View>();
                    controlsToApplyAction2.Add(views["T3_2"]);
                    controlsToApplyAction2.Add(views["T3_3"]);
                    controlsToApplyAction2.Add(views["T3_2_CMNT"]);





                    View T3_1 = views["T3_1"];

                    View T3_2 = views["T3_2"];
                 
                    View T3_3 = views["T3_3"];



                    Picker entryT3_2 = T3_2 as Picker;
                    Picker entryT3_1 = T3_1 as Picker;



                    if (entryT3_1 != null)
                    {
                        entryT3_1.SelectedIndexChanged += ((e, s) =>
                        {
                            var val = entryT3_1.SelectedItem as LookUpValueInfo;

                            if (val == null || string.IsNullOrWhiteSpace(val?.AnswerCode) || val?.AnswerCode == "9")
                            {
                                controlsToApplyAction2.ForEach(v => { v.IsEnabled = false; if (v is Entry) { ((Entry)v).Text = null; } else if (v is Picker) { ((Picker)v).SelectedIndex = 0; } });

                            }
                            else
                            {
                                controlsToApplyAction2.ForEach(v => v.IsEnabled = true);

                            }

                        });
                    }

                    if (entryT3_2 != null)
                    {
                        entryT3_2.SelectedIndexChanged += ((e, s) =>
                        {
                            var val = entryT3_2.SelectedItem as LookUpValueInfo;

                            if (val?.AnswerCode == "5")
                            {
                                controlsToApplyAction.ForEach(v => v.IsEnabled = true);
                            }
                            else
                            {
                                controlsToApplyAction.ForEach(v => { v.IsEnabled = false; if (v is Entry) { ((Entry)v).Text = null; } else if (v is Picker) { ((Picker)v).SelectedIndex = 0; } });

                            }


                            if (val==null || string.IsNullOrWhiteSpace( val?.AnswerCode) || val?.AnswerCode == "9")
                            {
                                controlsToApplyAction3.ForEach(v => { v.IsEnabled = false; if (v is Entry) { ((Entry)v).Text = null; } else if (v is Picker) { ((Picker)v).SelectedIndex = 0; } });

                            }
                            else
                            {
                                controlsToApplyAction3.ForEach(v => v.IsEnabled = true);

                            }

                        });
                    }

             

                }

                else if (formList.ID == new Guid("bda771be-4a47-4718-9fc7-e516cc4b9018"))
                {
                    var controlsToApplyAction = new List<View>();
                    controlsToApplyAction.Add(views["GOS12"]);
                    controlsToApplyAction.Add(views["GOS13"]);

                    var controlsToApplyAction2 = new List<View>();                   
                    controlsToApplyAction2.Add(views["GOS13"] );
                    var controlsToApplyAction2_1 = new List<View>();
                    controlsToApplyAction2_1.Add(views["GOS13"]);
                    controlsToApplyAction2_1.Add(views["GOS13_CMNT"]);




                    var controlsToApplyAction3 = new List<View>();
                    controlsToApplyAction3.Add(views["GOS13_CMNT"]);
                    


                    View I06A = views["GOS11"];
                    View GOS12 = views["GOS12"];
                    View GOS13 = views["GOS13"];

                    Picker entryI06A = I06A as Picker;
                    Picker entryGOS12 = GOS12 as Picker;
                    Picker entryGOS13 = GOS13 as Picker;

                    if (entryI06A != null)
                    {
                        entryI06A.SelectedIndexChanged += ((e, s) =>
                        {
                            var val = entryI06A.SelectedItem as LookUpValueInfo;

                            if (val?.AnswerCode == "1")
                            {
                                controlsToApplyAction.ForEach(v => v.IsEnabled = true);

                            }
                            else
                            {
                                controlsToApplyAction.ForEach(v => { v.IsEnabled = false; if (v is Entry) { ((Entry)v).Text = null; } else if (v is Picker) { ((Picker)v).SelectedIndex = 0; } });

                            }

                        });
                    }

                    if (entryGOS12 != null)
                    {
                        entryGOS12.SelectedIndexChanged += ((e, s) =>
                        {
                            var val = entryGOS12.SelectedItem as LookUpValueInfo;

                            if (val?.AnswerCode == "2")
                            {
                               
                                controlsToApplyAction2.ForEach(v => v.IsEnabled = true);

                                //var oldVal = entryGOS13.GetAnswerCode();
                                //entryGOS13.SelectedIndex = 0;
                                //entryGOS13.SetAnswerCode(oldVal);

                            }
                            else
                            {
                                controlsToApplyAction2_1.ForEach(v => { v.IsEnabled = false; if (v is Entry) { ((Entry)v).Text = null; } else if (v is Picker) { ((Picker)v).SelectedIndex = 0; } });

                            }

                        });
                    }

                    if (entryGOS13 != null)
                    {
                        entryGOS13.SelectedIndexChanged += ((e, s) =>
                        {
                            var val = entryGOS13.SelectedItem as LookUpValueInfo;

                            if (val?.AnswerCode == "6")
                            {
                                controlsToApplyAction3.ForEach(v => v.IsEnabled = true);
                            }
                            else
                            {
                                controlsToApplyAction3.ForEach(v => { v.IsEnabled = false; if (v is Entry) { ((Entry)v).Text = null; } else if (v is Picker) { ((Picker)v).SelectedIndex = 0; } });

                            }

                        });
                    }

                }
            }
        }

        private void ClearScreen()
        {
            pnlContentsHeader.Children.Clear();
            pnlContents.Children.Clear();
            prevRowColors.Clear();
        }

        private string ReturnMessage(string value)
        {
            return "- " + value;
        }

        private View BuildSimpleTypeControls(int typeId, int? lookUpId)
        {
            View cntrl = null;

            switch (typeId)
            {
                //Number
                case 1:
                    Entry entry1 = new Entry() { Keyboard = Keyboard.Numeric };
                    entry1.TextChanged += EntryTextChanged;
                    cntrl = entry1;
                   
                    break;
                //Text = 2,
                case 2:
                    Entry entry2 = new Entry() { Keyboard = Keyboard.Text };
                    entry2.TextChanged += EntryTextChanged;
                    cntrl = entry2;
                    break;

                //Choice = 3,
                case 3:
                    Picker picker = new Picker();
                    picker.SelectedIndexChanged += PickerSelectedIndexChanged;

                    var lookupValues = CurrentSurvey.LookUps.Where(x => x.Id == lookUpId).First().LookUpValues.ToList();
                    lookupValues.Insert(0, new LookUpValueInfo() { AnswerCode = "", Description = "اختيار الاجابة", AnswerId = -1, Id = -1, IsActive = true, NeedComments = false });
                    picker.ItemsSource = lookupValues;
                    picker.ItemDisplayBinding = new Binding("FullDescription");
                    picker.SelectedIndex = 0;
                    cntrl = picker;
                    break;
                //ListItem = 4, 
                case 4:

                    Picker picker2 = new Picker();
                    picker2.SelectedIndexChanged += PickerSelectedIndexChanged;

                    FillIndividual(CurrentQuestion.Code, picker2);
                    cntrl = picker2;

                    break;

                //DateTime = 5
                case 5:
                    DatePicker datePicker = new DatePicker();
                    cntrl = datePicker;
                    break;
            }

            return cntrl;


        }

       
        private Individual GetGA2Individual()
        {

            var str = GetSingleAnswerValue("GA2");

            if (!string.IsNullOrWhiteSpace(str))
            {
                var db = DataBase.GetConnection();
                var ID00 = QuestionnaireManager.CurrentVisit.ID00;
                var d1Id = Convert.ToInt32(str);
                return  db.Table<Individual>().Where(x => x.ID00 == ID00 && x.D1== d1Id).FirstOrDefault();
            }


            return null;
        }

        private void FillIndividual(string code, Picker picker)
        {

            List<LookUpValueInfo> lookupValues2 = new List<LookUpValueInfo>();
            var db = DataBase.GetConnection();
            var ID00 = QuestionnaireManager.CurrentVisit.ID00;


            //if (code == "GA2" || code=="QC6" || code== "I01")
            //{
            //    var Individuals = db.Table<Individual>().Where(x => x.ID00 == ID00 && x.D2A!="3"  && x.D5>=18).ToList();
            //    var IND = Individuals.OrderBy(i => i.D1).ToList();
            //    lookupValues2 = IND.Select(x => new LookUpValueInfo() { AnswerCode = x.D1.ToString(), Description=x.D2, AnswerId = x.D1,  Id = x.D1, IsActive = true, NeedComments = false }).ToList();

            //}


            lookupValues2.Insert(0, new LookUpValueInfo() { AnswerCode = "", Description = "اختيار الاجابة", AnswerId = -1, Id = -1, IsActive = true, NeedComments = false });
            picker.ItemsSource = lookupValues2;
            picker.ItemDisplayBinding = new Binding("FullDescription");
            picker.SelectedIndex = 0;

        }


        //private int CalculateAge(int year, int month, int day)
        //{
        //    DateTime dateTime = new DateTime(year, 1, 1);

        //    if (month > 0 && month <= 12)
        //    {
        //        dateTime = dateTime.AddMonths(month - 1);
        //    }
        //    else
        //    {
        //        month = 1;
        //    }

        //    if (day > 0 && day <= 31)
        //    {
        //        if (day <= DateTime.DaysInMonth(year, month))
        //        {
        //            dateTime = dateTime.AddDays(day-1);
        //        }
        //    }


        //    int age = 0;
        //    age = QuestionnaireManager.CurrentVisit.HH5.Year - dateTime.Year;
        //    if (QuestionnaireManager.CurrentVisit.HH5.Month < dateTime.Month)
        //    {
        //        age = age - 1;

        //    }
        //    else if(QuestionnaireManager.CurrentVisit.HH5.Month == dateTime.Month  && dateTime.Day > QuestionnaireManager.CurrentVisit.HH5.Day)
        //    {
        //        age = age - 1;
        //    }


        //    return age;
        //}

        public async Task<Individual> GetIndividualAsync()
        {
            Individual ind = null;

            var qB00 = QuestionList.Where(x => x.Code.StartsWith("BP00")).Select(qu => qu.Id).FirstOrDefault();

            if (qB00 != null)
            {
                var valQB00 = Convert.ToInt32(SectionVisit.Answers.Where(x => qB00 == x.QId).SelectMany(x => x.SingleValues).FirstOrDefault().Value);


                var ID00 = QuestionnaireManager.CurrentVisit.ID00;
                //if (MembersDetails.Current != null && MembersDetails.Current.Individuals != null)
                //{
                //    ind = MembersDetails.Current.Individuals.FirstOrDefault(x => x.D1 == valQB00);
                //}
                //else
                //{
                //    var db = await DataBase.GetAsyncConnection();
                //    ind = await db.Table<Individual>().Where(x => x.ID00 == ID00 && x.D1 == valQB00).OrderBy(i => i.D1).FirstOrDefaultAsync();
                //}

            }

            return ind;
        }

        public Individual GetIndividual()
        {
            Individual ind = null;

            var qB00 = QuestionList.Where(x => x.Code.StartsWith("BP00")).Select(qu => qu.Id).FirstOrDefault();

            if (qB00 != null)
            {
                var valQB00 = Convert.ToInt32(SectionVisit.Answers.Where(x => qB00 == x.QId).SelectMany(x => x.SingleValues).FirstOrDefault().Value);


                var ID00 = QuestionnaireManager.CurrentVisit.ID00;
                //if (MembersDetails.Current != null && MembersDetails.Current.Individuals != null)
                //{
                //    ind = MembersDetails.Current.Individuals.FirstOrDefault(x => x.D1 == valQB00);
                //}
                if (false)
                {

                }
                //else
                //{
                //    var db = DataBase.GetConnection();
                //    ind = db.Table<Individual>().Where(x => x.ID00 == ID00 && x.D2A!="3" && x.D1 == valQB00).OrderBy(i => i.D1).FirstOrDefault();
                //}

            }

            return ind;
        }

        private async void SaveChanges()
        {
            SectionVisit.Answers.RemoveAll(x => x.QId == CurrentQuestion.Id);
            if (CurrentQuestion.DisplayTypeId == 1)
            {
                string val = GetViewValueFromScreen(pnlContents.Children[0], CurrentQuestion.TypeId.Value);
                //CurrentSingleValue = singleValue;
                SectionVisit.Answers.Add(new QuestionAnswer() { QId = CurrentQuestion.Id, SingleValues = new List<SingleValue>() { new SingleValue() { Value = val } } });


                //CustomSave();
            }

            else if (CurrentQuestion.DisplayTypeId == 2)
            {

                var matrixValue = GetMatrixValueFromScreen();
                SectionVisit.Answers.Add(new QuestionAnswer() { QId = CurrentQuestion.Id, MatrixValues = matrixValue });

            }

            if (CurrentQuestion.DisplayTypeId == 3)
            {

                var listValue = GetListValue();
                SectionVisit.Answers.Add(new QuestionAnswer() { QId = CurrentQuestion.Id, ListItems = listValue });
            }

        }

        //private  void CustomSave()
        //{
        //    if (CurrentQuestion.Code== "UB1_Y" || CurrentQuestion.Code== "UB1_M" || CurrentQuestion.Code=="UB2" || CurrentQuestion.Code == "WB3_Y" || CurrentQuestion.Code == "WB3_M" || CurrentQuestion.Code == "WB4" || CurrentQuestion.Code == "WB5" || CurrentQuestion.Code == "WB6_L" || CurrentQuestion.Code == "WB6_C" || CurrentQuestion.Code == "WB7")
        //    {
        //        var val = GetViewValueFromScreen(pnlContents.Children[0], CurrentQuestion.TypeId.Value);



        //        var ind =  GetIndividual();
        //        if (ind != null  && int.TryParse(val,out int answerVal))
        //        {
        //            string orgVal = null;

        //            switch (CurrentQuestion.Code)
        //            {
        //                case "WB3_Y":
        //                case "UB1_Y":
        //                    orgVal = ind.HL5_Year?.ToString();
        //                    ind.HL5_Year = answerVal;

        //                    break;
        //                case "WB3_M":
        //                case "UB1_M":
        //                    orgVal = ind.HL5_Month?.ToString();

        //                    ind.HL5_Month = answerVal;

        //                    break;
        //                case "WB4":
        //                case "UB2":
        //                    orgVal = ind.HL6?.ToString();

        //                    ind.HL6 = answerVal;

        //                    break;
        //                case "WB5":
        //                    orgVal = ind.ED4;

        //                    ind.ED4 = val;
        //                    if (val == "2")
        //                    {
        //                        ind.ED5_Level = null;
        //                        ind.ED5_NumberOfYears = null;
        //                        ind.ED6 = null;
        //                    }
        //                    break;
        //                case "WB6_L":

        //                    if (val == "000")
        //                    {
        //                        val = "0";
        //                    }
        //                    orgVal = ind.ED5_Level;
        //                    ind.ED5_Level = val;


        //                    if (val == "0")
        //                    {

        //                        ind.ED5_NumberOfYears = null;
        //                        ind.ED6 = null;
        //                    }
        //                    break;
        //                case "WB6_C":
        //                    orgVal = ind.ED5_NumberOfYears?.ToString();

        //                    ind.ED5_NumberOfYears = answerVal;

        //                    break;
        //                case "WB7":
        //                    orgVal = ind.ED6;

        //                    ind.ED6=val;

        //                    break;
        //            }

        //            if (orgVal != val)
        //            {
        //                var db = DataBase.GetConnection();
        //                db.InsertOrReplace(ind);

        //                if(MembersDetails.Current != null && MembersDetails.Current.CurrentIndividual == ind)
        //                {
        //                    MembersDetails.Current.RefreshScreen();
        //                }


        //            }



        //        }
        //    }
        //}

        private void SetQuestionToScreen()
        {

            //  lblCode.Text = CurrentQuestion.Code;
            //    lblDescription.Text = CurrentQuestion.Description; // $"{CurrentQuestion.Description} - {CurrentQuestion.Code}.";

            switch (CurrentQuestion.Code)
            {
                case "ECD30.1":
                case "ECD30.2":
                case "ECD37.1":
                case "ECD37.2":
                case "ECD37.3":
                case "ECD36.1":
                case "ECD36.2":
                case "ECD36.3":
                case "ECD35.1":
                case "ECD35.2":
                case "ECD35.3":
                case "UCD3":
                    pnlDesc.BackgroundColor = Color.FromHex("#e1e4c8");

                    break;
                default:

                    pnlDesc.BackgroundColor = Color.FromHex("#6fd0fb");
                    break;
            }

            lblDescription.FormattedText = new FormattedString();
            if (!string.IsNullOrWhiteSpace(CurrentQuestion.Code))
            {
                lblDescription.FormattedText.Spans.Add(new Span() { Text = "أ", TextColor = pnlDesc.BackgroundColor });
                lblDescription.FormattedText.Spans.Add(new Span() { Text = $"{CurrentQuestion.Code} - ", FontAttributes = FontAttributes.Bold });
            }

            lblDescription.FormattedText.Spans.Add(new Span() { Text = CurrentQuestion.Description });
            lblComments.Text = CurrentQuestion.Comments;

            //SimpleType = 1,
            //MatrixType = 2,
            //List = 3

            BuildQuestion();

            ShowPopUp();


            //lblQuestion.Text = $" {CurrentQuestion.Code} - {CurrentQuestion.Description}";
        }

        private QuestionInfo GetNextQuestion()
        {
            if (CurrentQuestion != null)
            {
                int index = QuestionList.IndexOf(CurrentQuestion) + 1;
                if (index == QuestionList.Count()) return null;

                bool valid = false;

                QuestionInfo questionInfo;
                do
                {
                    if (index == QuestionList.Count()) return null;
                    questionInfo = QuestionList[index];

                    valid = IsFullValidQuestion(questionInfo);

                    if (!valid)
                    {
                        index++;

                    }

                } while (questionInfo != null && valid == false);
                if (questionInfo != null)
                {
                    CurrentIndex = index;
                }
                return questionInfo;

            }

            return null;
        }

        private List<ListItem> GetListValue()
        {

            List<ListItem> items = new List<ListItem>();

            Grid grid = null;

            foreach (var view in pnlContents.Children)
            {
                if (view is Grid)
                {
                    grid = (Grid)view;
                    break;
                }
            }


            FormListInfo formList = CurrentSurvey.FormLists.Where(x => x.ID == CurrentQuestion.ListId).FirstOrDefault();


            if (formList != null)
            {
                var fields = formList.FormListFields.Where(x => !SkipedFields.Contains(x.Code)).OrderBy(x => x.OrderId).ToList();

                var primaryKey = formList.FormListFields.Where(x => !SkipedFields.Contains(x.Code)).Where(x => x.IsKey == true).FirstOrDefault();
                int fCount = formList.FormListFields.Where(x => !SkipedFields.Contains(x.Code)).Count();
                bool hasPK = !(primaryKey != null && primaryKey.TypeId == 3);
                if (hasPK)
                {
                    fCount++;

                }




                var children = grid.Children.ToList();
                if (primaryKey != null && primaryKey.TypeId == 3)
                {
                    var lookUpValues = CurrentSurvey.LookUps.Where(x => x.Id == primaryKey.LookUpId).FirstOrDefault().LookUpValues.ToList();

                    int rIndex = 1;


                    foreach (var lValue in lookUpValues)
                    {
                        ListItem listItem = new ListItem() { ItemId = rIndex };
                        listItem.ListValues = new List<ListItemValue>();
                        items.Add(listItem);
                        int colIndex = 0;
                        foreach (var field in fields)
                        {

                            if (field == primaryKey)
                            {
                                ListItemValue lv = new ListItemValue() { FieldId = field.Id, SingleValues = new List<SingleValue>() { new SingleValue() { Value = lValue.AnswerCode } } };
                                listItem.ListValues.Add(lv);
                            }
                            else
                            {
                                var child = children.Where(x => Grid.GetRow(x) == rIndex && Grid.GetColumn(x) == colIndex).First();
                                string val = GetViewValueFromScreen(child, field.TypeId.Value);
                                ListItemValue lv = new ListItemValue() { FieldId = field.Id, SingleValues = new List<SingleValue>() { new SingleValue() { Value = val } } };


                                listItem.ListValues.Add(lv);
                            }

                            colIndex++;
                        }

                        rIndex++;
                    }

                }

                else
                {
                    var noRows = children.Any() ? children.Select(x => Grid.GetRow(x)).Max() : 0;

                    for (int rIndex = 1; rIndex <= noRows; rIndex++)
                    {
                        ListItem listItem = new ListItem() { ItemId = rIndex };
                        listItem.ListValues = new List<ListItemValue>();
                        items.Add(listItem);
                        int colIndex = 0;
                        foreach (var field in fields)
                        {




                            var child = children.Where(x => Grid.GetRow(x) == rIndex && Grid.GetColumn(x) == colIndex).First();
                            string val = GetViewValueFromScreen(child, field.TypeId.Value);
                            ListItemValue lv = new ListItemValue() { FieldId = field.Id, SingleValues = new List<SingleValue>() { new SingleValue() { Value = val } } };


                            listItem.ListValues.Add(lv);

                            colIndex++;

                        }


                    }


                }





            }

            return items;
        }

        private List<MatrixValue> GetMatrixValueFromScreen()
        {
            List<MatrixValue> values = new List<MatrixValue>();

            Grid grid = null;

            foreach (var view in pnlContents.Children)
            {
                if (view is Grid)
                {
                    grid = (Grid)view;
                    break;
                }
            }


            List<LookUpValueInfo> RowlookUpValueInfos = CurrentSurvey.LookUps.Where(x => x.Id == CurrentQuestion.RowLookUpId).SelectMany(x => x.LookUpValues).ToList();
            List<LookUpValueInfo> columnLookUps = new List<LookUpValueInfo>();

            if (CurrentQuestion.SingleValueMatrix)
            {
                columnLookUps.Add(new LookUpValueInfo() { AnswerId = -1, AnswerCode = "", Description = "الاجابة" });
            }
            else
            {

                columnLookUps = CurrentSurvey.LookUps.Where(x => x.Id == CurrentQuestion.ColumnLookUpId).SelectMany(x => x.LookUpValues).ToList();
                columnLookUps = FilterLookups(columnLookUps);

            }






            int rIndex = 1;

            var children = grid.Children.ToList();






            foreach (var r in RowlookUpValueInfos)
            {
                int colIndex = 1;
                foreach (var c in columnLookUps)
                {
                    var child = children.Where(x => Grid.GetRow(x) == rIndex && Grid.GetColumn(x) == colIndex).First();
                    string val = GetViewValueFromScreen(child, CurrentQuestion.TypeId.Value);

                    MatrixValue singleValue = new MatrixValue() { SEQId = 1, Value = val, HasComments = false, Comments = null, ColumnCode = c.AnswerCode, RowCode = r.AnswerCode };

                    values.Add(singleValue);

                    colIndex++;
                }

                rIndex++;
            }



            return values;


        }


        private void SetAnswerToScreen(QuestionAnswer questionAnswer)
        {
            bool isReadOnly = false;
            if (CurrentQuestion != null && !string.IsNullOrWhiteSpace(CurrentQuestion.Code))
            {

                if (CurrentQuestion.Code == "BP01")
                {
                    var ind = GetIndividual();
                    if (ind != null)
                    {
                        if (questionAnswer == null)
                        {
                            questionAnswer = new QuestionAnswer();
                            questionAnswer.SingleValues.Add(new SingleValue() { });
                        }
                        isReadOnly = true;
                        questionAnswer.SingleValues[0].Value = "1";//ind.Gender.ToString();
                    }
                }
                else if (CurrentQuestion.Code == "BP02")
                {
                    var ind = GetIndividual();
                    if (ind != null)
                    {
                        if (questionAnswer == null)
                        {
                            questionAnswer = new QuestionAnswer();
                            questionAnswer.SingleValues.Add(new SingleValue() { });
                        }
                        isReadOnly = true;

                        questionAnswer.SingleValues[0].Value ="1"; //ind.Age.ToString();
                    }

                }
            }


            if (questionAnswer == null) return;
            if (CurrentQuestion.DisplayTypeId == 1)
            {
                if (questionAnswer.SingleValues == null || questionAnswer.SingleValues.Count() == 0) return;
                SingleValue singleValue = questionAnswer.SingleValues[0];
                View view = pnlContents.Children[0];

                SetViewValue(view, CurrentQuestion.TypeId.Value, singleValue.Value);
                view.IsEnabled = !isReadOnly;
            }
            else if (CurrentQuestion.DisplayTypeId == 2)
            {
                if (questionAnswer.MatrixValues == null || questionAnswer.MatrixValues.Count() == 0) return;
                var matrixValues = questionAnswer.MatrixValues;

                Grid grid = null;

                foreach (var view in pnlContents.Children)
                {
                    if (view is Grid)
                    {
                        grid = (Grid)view;
                        break;
                    }
                }


                List<LookUpValueInfo> RowlookUpValueInfos = CurrentSurvey.LookUps.Where(x => x.Id == CurrentQuestion.RowLookUpId).SelectMany(x => x.LookUpValues).ToList();
                List<LookUpValueInfo> columnLookUps = new List<LookUpValueInfo>();

                if (CurrentQuestion.SingleValueMatrix)
                {
                    columnLookUps.Add(new LookUpValueInfo() { AnswerId = -1, AnswerCode = "", Description = "الاجابة" });
                }
                else
                {
                    columnLookUps = CurrentSurvey.LookUps.Where(x => x.Id == CurrentQuestion.ColumnLookUpId).SelectMany(x => x.LookUpValues).ToList();

                    columnLookUps = FilterLookups(columnLookUps);
                }



                int rIndex = 1;

                var children = grid.Children.ToList();
                foreach (var r in RowlookUpValueInfos)
                {
                    int colIndex = 1;
                    foreach (var c in columnLookUps)
                    {
                        var child = children.Where(x => Grid.GetRow(x) == rIndex && Grid.GetColumn(x) == colIndex).First();
                        var val = matrixValues.Where(x => x.RowCode == r.AnswerCode && x.ColumnCode == c.AnswerCode).FirstOrDefault();
                        if (val != null)
                        {
                            SetViewValue(child, CurrentQuestion.TypeId.Value, val.Value);
                        }

                        colIndex++;
                    }

                    rIndex++;
                }


            }
            else if (CurrentQuestion.DisplayTypeId == 3)
            {
                if (questionAnswer.ListItems == null || questionAnswer.ListItems.Count() == 0) return;
                var listItems = questionAnswer.ListItems;
                Grid grid = null;

                foreach (var view in pnlContents.Children)
                {
                    if (view is Grid)
                    {
                        grid = (Grid)view;
                        break;
                    }
                }


                FormListInfo formList = CurrentSurvey.FormLists.Where(x => x.ID == CurrentQuestion.ListId).FirstOrDefault();


                if (formList != null)
                {
                    var fields = formList.FormListFields.Where(x => !SkipedFields.Contains(x.Code)).OrderBy(x => x.OrderId).ToList();

                    var primaryKey = formList.FormListFields.Where(x => !SkipedFields.Contains(x.Code)).Where(x => x.IsKey == true).FirstOrDefault();
                    int fCount = formList.FormListFields.Where(x => !SkipedFields.Contains(x.Code)).Count();
                    bool hasPK = !(primaryKey != null && primaryKey.TypeId == 3);
                    if (hasPK)
                    {
                        fCount++;

                    }

                    var children = grid.Children.ToList();
                    if (primaryKey != null && primaryKey.TypeId == 3)
                    {
                        var lookUpValues = CurrentSurvey.LookUps.Where(x => x.Id == primaryKey.LookUpId).FirstOrDefault().LookUpValues.ToList();
                        int rIndex = 1;


                        foreach (var lValue in lookUpValues)
                        {
                            int colIndex = 0;
                            ListItem item = null;//listItems.Where(x => x.ListValues.Any(i => i.FieldId == primaryKey.Id && i.SingleValues[0].Value==lValue.AnswerId)).FirstOrDefault();
                            foreach (ListItem i in listItems)
                            {
                                var lvs = i.ListValues.Where(lv => lv.FieldId == primaryKey.Id).FirstOrDefault();

                                if (lvs != null)
                                {
                                    foreach (var sv in lvs.SingleValues)
                                    {
                                        if (sv.Value == lValue.AnswerCode)
                                        {
                                            item = i;
                                            break;
                                        }
                                    }

                                    if (item != null) break;
                                }
                            }


                            if (item != null)
                            {
                                foreach (var field in fields)
                                {

                                    var xxxx = item.ListValues.Where(l => l.FieldId == field.Id).FirstOrDefault();

                                    if (field != primaryKey && xxxx != null)
                                    {
                                        var child = children.Where(x => Grid.GetRow(x) == rIndex && Grid.GetColumn(x) == colIndex).First();
                                        if (xxxx.SingleValues != null && xxxx.SingleValues.Count() > 0)
                                        {
                                            SetViewValue(child, field.TypeId.Value, xxxx.SingleValues[0].Value);
                                        }

                                    }

                                    colIndex++;
                                }

                            }

                            rIndex++;
                        }

                    }

                    else
                    {


                        foreach (ListItem item in listItems)
                        {
                            AddListItemToScreen(grid, formList, hasPK, item);
                        }


                    }





                }
            }
        }

        private void AddListItemToScreen(Grid grid, FormListInfo formList, bool hasPK, ListItem item)
        {

            grid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            var fields = formList.FormListFields.Where(x => !SkipedFields.Contains(x.Code)).OrderBy(x => x.OrderId).ToList();

            var children = grid.Children.ToList();

            int rindex = children.Any() ? children.Select(x => Grid.GetRow(x)).Max() : 0;

            rindex++;
            int colIndex = 0;
            Dictionary<string, View> views = new Dictionary<string, View>();
            Dictionary<string, string> vs = new Dictionary<string, string>();

            foreach (var filed in fields)
            {


                // 
                View view = BuildSimpleTypeControls(filed.TypeId.Value, filed.LookUpId);
                grid.Children.Add(view);
                views.Add(filed.Code, view);
                var lv = item.ListValues.Where(x => x.FieldId == filed.Id).FirstOrDefault(); ;

                if (lv != null && lv.SingleValues != null && lv.SingleValues.Count > 0)
                {
                    SetViewValue(view, filed.TypeId.Value, lv.SingleValues[0].Value);
                    vs.Add(filed.Code, lv.SingleValues[0].Value);
                }
                else
                {
                    vs.Add(filed.Code, null);

                }
                Grid.SetColumn(view, colIndex++);
                Grid.SetRow(view, rindex);

            }

            ApplyListCustomActions(formList, views, true);

            foreach (var filed in fields)
            {
                SetViewValue(views[filed.Code], filed.TypeId.Value, vs[filed.Code]);
            }

            if (hasPK)
            {
                Button btnDelete = new Button { Text = "حذف" };
                btnDelete.StyleClass = new List<string> { "Red" };

                btnDelete.Clicked += (async (s2, e2) =>
                {
                    bool result = await DisplayAlert(GeneralMessages.Question, "هل انت متاكد من عملية الحذف؟", GeneralMessages.Yes, GeneralMessages.No);
                    if (result)
                    {
                        var last = grid.RowDefinitions.Last();
                        grid.RowDefinitions.Remove(last);
                        var row = Grid.GetRow((Button)s2);
                        var children2 = grid.Children.ToList();
                        foreach (var child in children2.Where(child => Grid.GetRow(child) == row))
                        {
                            grid.Children.Remove(child);
                        }
                        foreach (var child in children2.Where(child => Grid.GetRow(child) > row))
                        {
                            Grid.SetRow(child, Grid.GetRow(child) - 1);
                        }
                        // rIndex--;
                    }

                });
                grid.Children.Add(btnDelete);
                Grid.SetColumn(btnDelete, colIndex++);
                Grid.SetRow(btnDelete, rindex);

                colIndex++;
            }

        }

        private void EnableView(View view)
        {
            if (view == null) return;

            view.IsEnabled = true;
        }

        private void DisableView(View view)
        {
            if (view == null) return;

            view.IsEnabled = false;

            if (view.GetType() == typeof(Entry))
            {
                (view as Entry).Text = string.Empty;
            }
            else if (view.GetType() == typeof(Picker))
            {
                (view as Picker).SelectedIndex = 0;
            }
        }

        private void SetViewValue(View view, int typeId, string val)
        {
            if (string.IsNullOrWhiteSpace(val))
            {
                if (typeId == 3 || typeId == 4)
                {
                    Picker picker = view as Picker;
                    picker.SelectedIndex = -1;
                    picker.SelectedIndex = 0;
                }
                return;
            }

            switch (typeId)
            {
                //Number
                case 1:
                    Entry txtNumber = view as Entry;
                    if (txtNumber.IsEnabled)
                        txtNumber.Text = val;
                    else
                        txtNumber.Text = string.Empty;

                    break;
                //Text = 2,
                case 2:
                    Entry txtText = view as Entry;

                    if (txtText.IsEnabled)
                        txtText.Text = val;
                    else
                        txtText.Text = string.Empty;

                    break;

                //Choice = 3,
                case 3:
                    Picker picker = view as Picker;

                    LookUpValueInfo value = ((List<LookUpValueInfo>)picker.ItemsSource).Where(x => x.AnswerCode == val).FirstOrDefault();
                    if (value != null)
                    {
                        if (picker.IsEnabled)
                            picker.SelectedItem = value;
                        else
                            picker.SelectedIndex = 0;

                    }

                    break;
                //ListItem = 4, 
                case 4:

                    Picker picker2 = view as Picker;
                    LookUpValueInfo value2 = ((List<LookUpValueInfo>)picker2.ItemsSource).Where(x => x.AnswerCode == val).FirstOrDefault();
                    if (value2 != null)
                    {
                        if (picker2.IsEnabled)
                            picker2.SelectedItem = value2;
                        else
                            picker2.SelectedIndex = 0;
                    }
                    break;

                //DateTime = 5
                case 5:
                    DatePicker datePicker = view as DatePicker;
                    DateTime theTime = DateTime.ParseExact(val,
                                        "dd-MM-yyyy", CultureInfo.InvariantCulture);

                    datePicker.Date = theTime;

                    break;
            }
        }

        private string GetViewValueFromScreen(View view, int typeId)
        {
            string val = string.Empty;
            switch (typeId)
            {
                //Number
                case 1:
                    Entry txtNumber = view as Entry;
                    val = txtNumber.Text;

                    break;
                //Text = 2,
                case 2:
                    Entry txtText = view as Entry;
                    val = txtText.Text;


                    break;

                //Choice = 3,
                case 3:
                    Picker picker = view as Picker;
                    val = ((LookUpValueInfo)picker.SelectedItem).AnswerCode;
                    break;
                //ListItem = 4, 
                case 4:

                    Picker picker2 = view as Picker;
                    val = ((LookUpValueInfo)picker2.SelectedItem).AnswerCode;
                    break;

                //DateTime = 5
                case 5:
                    DatePicker datePicker = view as DatePicker;
                    val = datePicker.Date.ToString("dd-MM-yyyy");
                    break;
            }

            return val;
        }

        private List<View> GetCQuestionViewsFromScreen()
        {
            if (CurrentQuestion != null)
            {
                if (CurrentQuestion.DisplayTypeId == 1)
                {
                    return pnlContents.Children.ToList();
                }
                else if (CurrentQuestion.DisplayTypeId == 2 || CurrentQuestion.DisplayTypeId == 3)
                {
                    Grid grid = null;

                    foreach (var view in pnlContents.Children)
                    {
                        if (view is Grid)
                        {
                            grid = (Grid)view;
                            break;
                        }
                    }

                    var children = grid.Children.ToList();

                    return children.Where(x => !(x is Layout<View>)).ToList();
                }

            }

            return null;
        }

        private List<View> GetCQuestionViewsFromScreenByRow(int row = 1)
        {
            if (CurrentQuestion != null)
            {
                if (CurrentQuestion.DisplayTypeId == 1)
                {
                    return pnlContents.Children.ToList();
                }
                else if (CurrentQuestion.DisplayTypeId == 2 || CurrentQuestion.DisplayTypeId == 3)
                {
                    Grid grid = null;

                    foreach (var view in pnlContents.Children)
                    {
                        if (view is Grid)
                        {
                            grid = (Grid)view;
                            break;
                        }
                    }

                    var children = grid.Children.ToList();

                    return children.Where(x => Grid.GetRow(x) == row && !(x is Layout<View>)).ToList();
                }

            }

            return null;
        }

        private List<View> GetCQuestionViewsFromScreenByColumn(int col = 1)
        {
            if (CurrentQuestion != null)
            {
                if (CurrentQuestion.DisplayTypeId == 1)
                {
                    return pnlContents.Children.ToList();
                }
                else if (CurrentQuestion.DisplayTypeId == 2 || CurrentQuestion.DisplayTypeId == 3)
                {
                    Grid grid = null;

                    foreach (var view in pnlContents.Children)
                    {
                        if (view is Grid)
                        {
                            grid = (Grid)view;
                            break;
                        }
                    }

                    var children = grid.Children.ToList();

                    return children.Where(x => Grid.GetColumn(x) == col && !(x is Layout<View>)).ToList();
                }

            }

            return null;
        }

        private View GetCQuestionViewFromScreenByRowColumn(int row = 1, int col = 1)
        {
            if (CurrentQuestion != null)
            {
                if (CurrentQuestion.DisplayTypeId == 1)
                {
                    return pnlContents.Children[0];
                }
                else if (CurrentQuestion.DisplayTypeId == 2 || CurrentQuestion.DisplayTypeId == 3)
                {
                    Grid grid = null;

                    foreach (var view in pnlContents.Children)
                    {
                        if (view is Grid)
                        {
                            grid = (Grid)view;
                            break;
                        }
                    }

                    var children = grid.Children.ToList();

                    return children.Where(x => Grid.GetRow(x) == row && Grid.GetColumn(x) == col).First();
                }

            }

            return null;
        }

        private string GetSingleAnswerValue(string questionCode)
        {
            if (string.IsNullOrEmpty(questionCode)) return null;

            var question = QuestionList.Where(x => x.Code == questionCode).FirstOrDefault();

            if (question != null)
            {
                var qAnswer = FindAnswer(question.Id);

                if (qAnswer != null)
                {
                    if (qAnswer.SingleValues.Any())
                    {
                        return qAnswer.SingleValues.First().Value;
                    }
                }

            }

            return null;
        }

        private List<MatrixValue> GetMatrixAnswerValues(string questionCode)
        {
            if (string.IsNullOrEmpty(questionCode)) return null;

            var question = QuestionList.Where(x => x.Code == questionCode).FirstOrDefault();

            if (question != null)
            {
                var qAnswer = FindAnswer(question.Id);

                if (qAnswer != null)
                {
                    return qAnswer.MatrixValues;
                }
            }

            return null;
        }

        private string GetMatrixAnswerValue(string questionCode, string row, string col)
        {
            if (string.IsNullOrEmpty(questionCode)) return null;

            var question = QuestionList.Where(x => x.Code == questionCode).FirstOrDefault();

            if (question != null)
            {
                var qAnswer = FindAnswer(question.Id);

                if (qAnswer != null)
                {
                    var matrixValue = qAnswer.MatrixValues.Where(x => x.RowCode == row.ToString() && (x.ColumnCode == col.ToString() || x.ColumnCode == string.Empty)).FirstOrDefault();
                    if (matrixValue != null)
                        return matrixValue.Value;
                }
            }

            return null;
        }

        private string GetListItemAnswerValue(string questionCode, int row, int col)
        {
            if (string.IsNullOrEmpty(questionCode)) return null;

            row -= 1;

            var question = QuestionList.Where(x => x.Code == questionCode).FirstOrDefault();

            if (question != null)
            {
                var qAnswer = FindAnswer(question.Id);

                if (qAnswer != null)
                {
                    if (row < qAnswer.ListItems.Count)
                    {
                        var listItem = qAnswer.ListItems[row];
                        if (listItem != null)
                        {
                            if (col < listItem.ListValues.Count)
                            {
                                if (listItem.ListValues[col].SingleValues.Any())
                                    return listItem.ListValues[col].SingleValues.First().Value;
                            }
                        }
                    }

                }
            }

            return null;
        }

        private QuestionAnswer FindAnswer(Guid id)
        {
            var answer = SectionVisit.Answers.Where(q => q.QId == id).FirstOrDefault();
            if (answer == null)
            {
                answer = OldAnswerList.Where(q => q.QId == id).FirstOrDefault();
            }

            return answer;
        }

        private string GetViewValue(View view)
        {
            string val = string.Empty;

            if (view.GetType() == typeof(Entry))
            {
                Entry txtText = view as Entry;
                val = txtText.Text;
            }
            else if (view.GetType() == typeof(Picker))
            {
                Picker picker = view as Picker;
                val = ((LookUpValueInfo)picker.SelectedItem).AnswerCode;
            }
            else if (view.GetType() == typeof(DateTime))
            {
                DatePicker datePicker = view as DatePicker;
                val = datePicker.Date.ToString("dd-MM-yyyy");
            }

            return val;
        }


        private void ShowCQuestionViewsByRow(int row)
        {
            if (CurrentQuestion != null)
            {
                List<View> views = GetCQuestionViewsFromScreenByRow(row);
                views.ForEach(x =>
                {
                    x.IsVisible = true;
                });
            }
        }

        private void ShowCQuestionViewsByColumn(int col)
        {
            if (CurrentQuestion != null)
            {
                List<View> views = GetCQuestionViewsFromScreenByColumn(col);
                views.ForEach(x =>
                {
                    x.IsVisible = true;
                });
            }
        }

        private void ShowCQuestionViewByRowColumn(int row, int col)
        {
            View view = GetCQuestionViewFromScreenByRowColumn(row, col);
            if (view != null)
                view.IsVisible = true;
        }

        private void HideCQuestionViewsByRow(int row)
        {
            if (CurrentQuestion != null)
            {
                List<View> views = GetCQuestionViewsFromScreenByRow(row);
                views.ForEach(x =>
                {
                    x.IsVisible = false;
                });
            }
        }

        private void HideCQuestionViewsByColumn(int col)
        {
            if (CurrentQuestion != null)
            {
                List<View> views = GetCQuestionViewsFromScreenByColumn(col);
                views.ForEach(x =>
                {
                    x.IsVisible = false;
                });
            }
        }

        private void HideCQuestionViewByRowColumn(int row, int col)
        {
            View view = GetCQuestionViewFromScreenByRowColumn(row, col);
            if (view != null)
                view.IsVisible = false;
        }

        private void CreatePickerExtraCustomEntryField(Picker picker)
        {
            if (picker == null) return;

            Grid grid = picker.Parent as Grid;
            if (grid == null) return;

            View view = BuildSimpleTypeControls(2, null);
            Entry viewEntry = view as Entry;
            viewEntry.Placeholder = "قم بتعبئة الحقل";
            viewEntry.HeightRequest = 60;

            view.Margin = new Thickness() { Bottom = 0, Left = 0, Right = 0, Top = viewEntry.HeightRequest * 1.5 };
            grid.Children.Add(view, Grid.GetColumn(picker), Grid.GetRow(picker));
        }

        private void RemovePickerExtraCustomEntryField(Picker picker)
        {
            if (picker == null) return;

            Grid grid = picker.Parent as Grid;
            if (grid == null) return;

            List<View> views = grid.Children.Where(x => Grid.GetRow(x) == Grid.GetRow(picker) && Grid.GetColumn(x) == Grid.GetColumn(picker)).ToList();

            if (views.Count == 1)
            {
                return;
            }

            views.ForEach(x =>
            {
                if (x.GetType() == typeof(Entry))
                {
                    grid.Children.Remove(x);
                }
            });
        }


        public bool IsSpecailNeeds(int HR01)
        {
            return true;
        }

        SfPopupLayout popupLayout = null;
        private void ShowPopUp()
        {
            if (popupLayout != null && popupLayout.IsOpen)
            {
                popupLayout.Dismiss();
            }

            bool showPopup = false;
            string txt = string.Empty;
            Color color = Color.White;
            if (CurrentQuestion.Code == "SDG02" || CurrentQuestion.Code == "SDG03")
            {
                color = Color.Orange;
                showPopup = true;
                txt = @"


            إظهار بطاقة الاهداف ال 17 للتنمية المستدامة 





            ";
            }
            if (CurrentQuestion.Code == "WZ")
            {
                color = Color.LightSkyBlue;
                showPopup = true;
                txt = @"


            البطاقة رقم (1)





            ";
            }

            if (CurrentQuestion.Code == "WL")
            {
                color = Color.LightGreen;
                showPopup = true;
                txt = @"


            البطاقة رقم (2)





            ";
            }

            if (CurrentQuestion.Code == "WA" || CurrentQuestion.Code == "WS" || CurrentQuestion.Code == "WG" || CurrentQuestion.Code == "ZA" || CurrentQuestion.Code == "DA" || CurrentQuestion.Code == "DBA" || CurrentQuestion.Code == "DBB" || CurrentQuestion.Code == "DBC" || CurrentQuestion.Code == "DBD" || CurrentQuestion.Code == "DBE" || CurrentQuestion.Code == "DBF")
            {
                color = Color.LightYellow;
                showPopup = true;
                txt = @"


            البطاقة رقم (4)





            ";
            }

            if (showPopup)
            {
                popupLayout = new SfPopupLayout();
                Label popupContent;
                var templateView = new DataTemplate(() =>
                {
                    popupContent = new Label();
                    popupContent.Text = txt;
                    popupContent.BackgroundColor = color;
                    popupContent.HorizontalTextAlignment = TextAlignment.Center;
                    return popupContent;
                });

                popupLayout.PopupView.ContentTemplate = templateView;
                popupLayout.PopupView.HeaderTitle = "برجاء اظهار البطاقة التالية";
                popupLayout.PopupView.AcceptButtonText = "موافق";
                popupLayout.Show();

            }
        }

        private bool AllMatrixAnswersEqualsOneOf(string questionCode, string[] values)
        {
            if (string.IsNullOrEmpty(questionCode) || values == null) return false;

            var question = QuestionList.Where(x => x.Code == questionCode).FirstOrDefault();

            if (question != null)
            {
                var qAnswer = FindAnswer(question.Id);

                if (qAnswer != null)
                {

                    foreach (var x in qAnswer.MatrixValues)
                    {
                        if (!values.Contains(x.Value))
                        {
                            return false;
                        }
                    }


                    return true;
                }

            }

            return false;
        }

        private bool SomeMatrixAnswersEqualsOneOf(string questionCode, string[] values)
        {
            if (string.IsNullOrEmpty(questionCode) || values == null) return false;

            var question = QuestionList.Where(x => x.Code == questionCode).FirstOrDefault();

            if (question != null)
            {
                var qAnswer = FindAnswer(question.Id);

                if (qAnswer != null)
                {
                    bool atLeastOneFound = false;

                    foreach (var x in qAnswer.MatrixValues)
                    {
                        if (values.Contains(x.Value))
                        {
                            atLeastOneFound = true;
                            break;
                        }
                    }


                    return atLeastOneFound;
                }

            }

            return false;
        }


        private bool GridViewChangedOfCell(View view, string questionCode, int? row = null, int? column = null)
        {
            if (CurrentQuestion == null || view == null || string.IsNullOrEmpty(questionCode))
                return false;

            int pickerRow = Grid.GetRow(view);
            int pickerColumn = Grid.GetColumn(view);

            if (CurrentQuestion.Code == questionCode)
            {
                if (row.HasValue && column.HasValue)
                {
                    return column == pickerColumn && row == pickerRow;
                }
                else if (row.HasValue && !column.HasValue)
                {
                    return row == pickerRow;
                }
                else if (!row.HasValue && column.HasValue)
                {
                    return column == pickerColumn;
                }

                return true;// a view is changed
            }

            return false;
        }

        private bool GridPickerChangedOfQuestion(Picker picker, string questionCode)
        {
            if (CurrentQuestion == null || picker == null || string.IsNullOrEmpty(questionCode))
                return false;

            if (CurrentQuestion.Code == questionCode)
            {
                return true;
            }
            return false;
        }


        #endregion

        #region UI Events

        private async void btnNext_Clicked(object sender, EventArgs e)
        {
            if (await IsValidAnswer())
            {

                if (CurrentSecStatus.CurrentStatusId != (int)CurrentStatus.NotComplete)
                {
                    CurrentSecStatus.CurrentStatusId = (int)CurrentStatus.NotComplete;
                    UpdateSecStatus();
                }
                SaveChanges();
                OldAnswerList.RemoveAll(c => c.QId == CurrentQuestion.Id);
                //CurrentIndex++;

                var q = GetNextQuestion();

                if (q == null)
                {

                    await DisplayAlert(GeneralMessages.Info, "تم انتهاء هذا  القسم. برجاء حفظ البيانات ", GeneralMessages.Cancel);
                    // btnNext.IsEnabled = false;
                    IsComplete = true;
                }
                else
                {
                    IsSaved = false;
                    CurrentQuestion = q;
                    IsComplete = false; ;

                    SetQuestionToScreen();

                    var oldAnswer = OldAnswerList.Where(x => x.QId == CurrentQuestion.Id).FirstOrDefault();
                    if (oldAnswer != null)
                    {
                        
                            SetAnswerToScreen(oldAnswer);
                        
                    }

                    else
                    {
                        if (CurrentQuestion.Code == "BP01" || CurrentQuestion.Code == "BP02")
                        {
                            SetAnswerToScreen(null);
                        }

                    }
                }
            }
        }
        private void btnPrvious_Clicked(object sender, EventArgs e)
        {


            if (CurrentIndex == 0) { return; }
            IsSaved = false;
            IsComplete = false;
            if (CurrentSecStatus.CurrentStatusId != (int)CurrentStatus.NotComplete)
            {
                CurrentSecStatus.CurrentStatusId = (int)CurrentStatus.NotComplete;
                UpdateSecStatus();
            }

            if (CurrentQuestion != null)
            {
                SaveChanges();
            }
            var questionAnswerCurrent = SectionVisit.Answers.LastOrDefault();
            if (questionAnswerCurrent != null)
            {
                SectionVisit.Answers.RemoveAll(c => c.QId == questionAnswerCurrent.QId);
                OldAnswerList.RemoveAll(c => c.QId == questionAnswerCurrent.QId);
                OldAnswerList.Add(questionAnswerCurrent);
            }



            var questionAnswer = SectionVisit.Answers.LastOrDefault();
            if (questionAnswer != null)
            {



                CurrentQuestion = QuestionList.Find(x => x.Id == questionAnswer.QId);
                SectionVisit.Answers.Remove(questionAnswer);
                OldAnswerList.RemoveAll(c => c.QId == questionAnswer.QId);
                OldAnswerList.Add(questionAnswer);
                if (CurrentQuestion != null)
                {
                    CurrentIndex = QuestionList.IndexOf(CurrentQuestion);
                    SetQuestionToScreen();
                    SetAnswerToScreen(questionAnswer);
                }

            }
        }
        private async void btnSaveSection_Clicked(object sender, EventArgs e)
        {
            try
            {

                if (CurrentQuestion != null)
                {
                    if (await IsValidAnswer())
                    {

                        SectionVisit.Answers.RemoveAll(c => c.QId == CurrentQuestion.Id);


                        if (IsComplete || CurrentIndex == QuestionList.Count() - 1)
                        {
                            CurrentSecStatus.CurrentStatusId = (int)CurrentStatus.Complete;
                            SectionVisit.IsComplete = true;
                        }
                        else
                        {
                            CurrentSecStatus.CurrentStatusId = (int)CurrentStatus.NotComplete;
                            SectionVisit.IsComplete = false;

                        }

                        if (SectionVisit.IsComplete)
                        {
                            CurrentSecStatus.CurrentStatusId = (int)CurrentStatus.Complete;
                            var ResultQ = QuestionList.Where(x => x.Code == "B01").FirstOrDefault();
                            if (ResultQ != null)
                            {
                                var answer = SectionVisit.Answers.Where(x => x.QId == ResultQ.Id).FirstOrDefault();
                                if (answer != null)
                                {
                                    //if (CurrentSectionId == 2174)
                                    //{
                                    //    var val = Convert.ToInt32(answer.SingleValues[0].Value);
                                    //    if ((val == 1 && QuestionnaireManager.CurrentVisit.IV01 == 0) || (val == 2 && QuestionnaireManager.CurrentVisit.IV02 == 0))
                                    //    {
                                    //        await DisplayAlert(GeneralMessages.Question, "الجنس لا يتناسب مع  الفئة العمرية والجنس في البيانات التعريفية", GeneralMessages.Cancel);
                                    //        CurrentSecStatus.CurrentStatusId = (int)CurrentStatus.NotComplete;
                                    //        SectionVisit.IsComplete = false;
                                    //        UpdateSecStatus();

                                    //        return;
                                    //    }
                                    //    QuestionnaireManager.CurrentVisit.B01Gender = val;
                                    //}
                                    //else
                                    //{
                                    //    // QuestionnaireManager.CurrentVisit.B01Gender = null;
                                    //}

                                    QuestionnaireManager.SaveCurrentVisit();
                                }
                                else
                                {

                                }

                            }


                        }
                        SaveChanges();
                        OldAnswerList.RemoveAll(c => c.QId == CurrentQuestion.Id);



                    }
                }

                SectionVisit.IsPartialComplete = false;
                SectionPageData sectionPageData = new SectionPageData();
                sectionPageData.OldAnswerList = OldAnswerList;
                sectionPageData.SectionVisit = SectionVisit;

                string filename = await QuestionnaireManager.GetFileName(QuestionnaireManager.CurrentVisit.ID00, CurrentSectionId, HR01);

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

                IsSaved = true;
                UpdateSecStatus();
                ToastManager.ShortAlert(GeneralMessages.DataSavedSuccessfully);
            }
            catch
            {
                await DisplayAlert(GeneralMessages.Error, GeneralMessages.SaveNotSuccess, GeneralMessages.Cancel);
            }

        }

        private async void btnSavePartialComplete_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (CurrentQuestion != null)
                {


                    //if (await IsValidAnswer())
                    //{

                    SectionVisit.Answers.RemoveAll(c => c.QId == CurrentQuestion.Id);


                    //if (IsComplete || CurrentIndex == QuestionList.Count() - 1)
                    //{
                    //    CurrentSecStatus.CurrentStatusId = (int)CurrentStatus.Complete;
                    //    SectionVisit.IsComplete = true;
                    //}
                    //else
                    //{
                    //    CurrentSecStatus.CurrentStatusId = (int)CurrentStatus.NotComplete;
                    //    SectionVisit.IsComplete = false;

                    //}

                    CurrentSecStatus.CurrentStatusId = (int)CurrentStatus.NotComplete;
                    SaveChanges();
                    OldAnswerList.RemoveAll(c => c.QId == CurrentQuestion.Id);



                    // }

                }
                bool saveData = true;


                //if (await DisplayAlert(GeneralMessages.Question, "هل تريد الاستمرار في تعيين هذا  القسم كمكتمل جزئي؟", GeneralMessages.Yes, GeneralMessages.No))
                //{
                // saveData = true;

                SectionVisit.IsPartialComplete = false;
                CurrentSecStatus.CurrentStatusId = (int)CurrentStatus.NotComplete;
                SectionVisit.IsComplete = false;

                // }



                if (saveData)
                {
                    //if (SectionVisit.IsPartialComplete)
                    //{
                    //    answer.SingleValues[0].Value = "04";
                    //}
                    //else
                    //{
                    //    answer.SingleValues[0].Value = "01";
                    //}

                    SectionPageData sectionPageData = new SectionPageData();
                    sectionPageData.OldAnswerList = OldAnswerList;
                    sectionPageData.SectionVisit = SectionVisit;

                    string filename = await QuestionnaireManager.GetFileName(QuestionnaireManager.CurrentVisit.ID00, CurrentSectionId, HR01);

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

                    IsSaved = true;
                    UpdateSecStatus();
                    ToastManager.ShortAlert(GeneralMessages.DataSavedSuccessfully);

                }

                else
                {
                    await DisplayAlert(GeneralMessages.Warning, "لم يتم حفظ  البيانات !!!", GeneralMessages.Cancel);
                }





                //}
                //else
                //{
                //    await DisplayAlert(GeneralMessages.Warning, "لا يمكن اختيار مكتمل جزئي", GeneralMessages.Cancel);
                //}


            }
            catch
            {
                await DisplayAlert(GeneralMessages.Error, GeneralMessages.SaveNotSuccess, GeneralMessages.Cancel);
            }
        }


        private const int pnlContentsHeaderExpandOffset = 20;
        private const int pnlContentsHeaderMin = 30, pnlContentsHeaderMax = 500;
        private bool questionExpanded;
        private void btnExpandHeader_Clicked(object sender, EventArgs e)
        {
            if (pnlContentsScroll.Orientation != ScrollOrientation.Vertical)
            {
                return;
            }

            if (pnlContentsHeader.Height + pnlContentsHeaderExpandOffset >= pnlContentsHeaderMax)
            {
                pnlContentsHeader.HeightRequest = pnlContentsHeaderMax;
                return;
            }

            pnlContentsHeader.HeightRequest = pnlContentsHeader.Height + pnlContentsHeaderExpandOffset;
        }

        private void btnDecreaseHeader_Clicked(object sender, EventArgs e)
        {
            if (pnlContentsScroll.Orientation != ScrollOrientation.Vertical)
            {
                return;
            }

            if (pnlContentsHeader.Height - pnlContentsHeaderExpandOffset <= pnlContentsHeaderMin)
            {
                pnlContentsHeader.HeightRequest = pnlContentsHeaderMin;
                return;
            }

            pnlContentsHeader.HeightRequest = pnlContentsHeader.Height - pnlContentsHeaderExpandOffset;
        }

        private void btnExpandQuestion_Clicked(object sender, EventArgs e)
        {
            if (pnlContentsScroll.Orientation == ScrollOrientation.Both)
            {
                questionExpanded = false;
                OnRevertExpandQuestion();

                pnlContentsScroll.Orientation = ScrollOrientation.Vertical;
                btnExpandQuestion.Text = "توسيع الاسئلة";
                List<string> listStyle = new List<string> { "Green" };
                btnExpandQuestion.StyleClass = listStyle;

                btnExpandHeader.IsVisible = btnDecreaseHeader.IsVisible = true;
            }
            else
            {
                questionExpanded = true;
                OnExpandQuestion();

                pnlContentsScroll.Orientation = ScrollOrientation.Both;
                btnExpandQuestion.Text = "تراجع توسيع الاسئلة";
                List<string> listStyle = new List<string> { "Red" };
                btnExpandQuestion.StyleClass = listStyle;

                btnExpandHeader.IsVisible = btnDecreaseHeader.IsVisible = false;
            }

            pnlContentsHeader.HeightRequest = pnlContentsHeader.Height + 1;
            pnlContentsHeader.HeightRequest = pnlContentsHeader.Height - 1;
        }

        private void OnExpandQuestion()
        {
            pnlContentsHeader.IsVisible = false;
            Grid grid = pnlContents.Children[0] as Grid;

            int column = 0;
            pnlContentsHeader.Children.ToList().ForEach(x =>
            {
                grid.Children.Add(x);
                Grid.SetRow(x, 0);
                Grid.SetColumn(x, column);
                column++;
            });
        }

        private void OnRevertExpandQuestion()
        {
            Grid grid = pnlContents.Children[0] as Grid;

            int column = 0;
            grid.Children.Where(x => Grid.GetRow(x) == 0).ToList().ForEach(x =>
            {
                pnlContentsHeader.Children.Add(x);
                Grid.SetRow(x, 0);
                Grid.SetColumn(x, column);
                column++;
            });
            pnlContentsHeader.IsVisible = true;
        }

        private Dictionary<string, Color> prevRowColors = new Dictionary<string, Color>();
        private int prevRowID = -1;

        private async void HightLightRow_Tapped(object sender, EventArgs e)
        {

            View view = sender as View;

            if (view == null) return;


            Color highlightColor = Color.Yellow;
            Grid grid = pnlContents.Children[0] as Grid;
            var currentRow = grid.Children.Where(x => Grid.GetRow(x) == Grid.GetRow(view)).ToList();
            var prevRow = grid.Children.Where(x => Grid.GetRow(x) == prevRowID).ToList();

            if (prevRow.Any() && prevRowColors.Any())
            {
                foreach (var x in prevRow)
                {
                    if (!x.IsEnabled)
                    {
                        x.BackgroundColor = (Color)App.Current.Resources["ControlDisabledColor"];
                    }
                    else
                    {
                        if (x.BackgroundColor != Color.Red)
                        {
                            if (x.BackgroundColor == Color.Default && prevRowColors[x.Id.ToString()] == Color.Red)
                                continue;
                            x.BackgroundColor = prevRowColors[x.Id.ToString()];
                        }
                    }
                }
            }

            prevRowColors.Clear();

            currentRow.ForEach(x =>
            {
                prevRowColors.Add(x.Id.ToString(), x.BackgroundColor);
                x.BackgroundColor = highlightColor;
            });

            prevRowID = Grid.GetRow(view);

            await pnlContentsScroll.ScrollToAsync(view, ScrollToPosition.Center, true);

        }

        private void LabelTapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            Label lbl = sender as Label;

            if (lbl == null) return;

            if (popupLayout != null && popupLayout.IsOpen)
            {
                popupLayout.Dismiss();
            }

            string text = null;

            if (!string.IsNullOrEmpty(lbl.Text))
            {
                text = lbl.Text;
            }
            else if (!string.IsNullOrEmpty(lbl.FormattedText?.ToString()))
            {
                var formattedText = new FormattedString();
                var formattedTextSpans = lbl.FormattedText.Spans.Where(x => x.Text != "أ").ToList();
                formattedTextSpans.ForEach(x =>
                {
                    formattedText.Spans.Add(x);
                });

                text = formattedText.ToString();
            }

            if (!string.IsNullOrWhiteSpace(text))
            {
                popupLayout = new SfPopupLayout();
                Label popupContent;
                StackLayout stackLayout;
                var templateView = new DataTemplate(() =>
                {
                    stackLayout = new StackLayout() { BackgroundColor = Color.White };
                    popupContent = new Label();
                    popupContent.Text = text;
                    popupContent.BackgroundColor = Color.White;
                    popupContent.FontAttributes = FontAttributes.Bold;
                    popupContent.FontSize = Device.GetNamedSize(NamedSize.Medium, popupContent);


                    popupContent.FlowDirection = FlowDirection.RightToLeft;
                    popupContent.HorizontalTextAlignment = TextAlignment.Center;
                    popupContent.VerticalTextAlignment = TextAlignment.Center;

                    stackLayout.Children.Add(popupContent);

                    ScrollView scrollView = new ScrollView();
                    scrollView.Content = stackLayout;

                    return scrollView;
                });

                popupLayout.PopupView.ContentTemplate = templateView;
                popupLayout.PopupView.HeaderTitle = "";
                popupLayout.PopupView.AcceptButtonText = "موافق";
                popupLayout.WidthRequest = this.Width;
                popupLayout.BackgroundColor = Color.White;
                try
                {
                    popupLayout.Show();
                }
                catch
                {

                }

            }
        }

        #endregion

        #region UI Input Validation

        private bool OptionalQuestionValidate(StringBuilder stringBuilder)
        {
            if (CurrentQuestion.DisplayTypeId != 1)
            {
                var views = GetCQuestionViewsFromScreen();

                var emptyViews = new List<View>();
                views.ForEach(x =>
                {
                    if (string.IsNullOrWhiteSpace(GetViewValue(x)))
                    {
                        emptyViews.Add(x);
                    }
                });

                if (emptyViews.Count != 0 && emptyViews.Count != views.Count())
                {
                    emptyViews.ForEach(x =>
                    {
                        x.BackgroundColor = Color.Red;
                    });

                    stringBuilder.AppendLine(ReturnMessage("يجب وجود اجابة على كل فرع من فروع السؤال"));

                    return false;
                }
            }

            return true;
        }

        private ValidationResult ValidateView(View view, QuestionInfo questionInfo, string prefixMessage)
        {
            StringBuilder stringBuilder = new StringBuilder();
            bool valid = true;
            bool isOptional = questionInfo.IsOptional;
            switch (questionInfo.TypeId)
            {
                //Number
                case 1:
                    Entry txtNumber = view as Entry;
                    txtNumber.BackgroundColor = Color.Default;
                    if (isOptional && string.IsNullOrEmpty(txtNumber.Text))
                    {
                        valid = true;
                    }
                    else
                    {
                        valid = float.TryParse(txtNumber.Text, out float value);
                        if (!valid)
                        {
                            stringBuilder.AppendLine($"{prefixMessage}- الرقم المدخل غير صحيح");
                        }

                        if (valid && questionInfo.MinValue.HasValue && value < questionInfo.MinValue.Value)
                        {
                            stringBuilder.AppendLine($"{prefixMessage}- الرقم المدخل يجب ان يكون أكبر أو يساوي {questionInfo.MinValue.Value} ");

                            valid = false;
                        }

                        if (valid && questionInfo.MaxValue.HasValue && value > questionInfo.MaxValue.Value)
                        {
                            stringBuilder.AppendLine($"{prefixMessage}- الرقم المدخل يجب ان يكون أقل أو يساوي {questionInfo.MaxValue.Value} ");

                            valid = false;
                        }
                        if (valid)
                        {
                            if (questionInfo.FormateId == 4 || questionInfo.FormateId == 5)
                            {
                                bool isValidInt = int.TryParse(txtNumber.Text, out int valueInt);

                                if (!(isValidInt && value == valueInt))
                                {
                                    stringBuilder.AppendLine($"{prefixMessage} - الرقم المدخل يجب ان يكون عدد صحيح{questionInfo.MinValue.Value} ");

                                    valid = false;
                                }


                            }

                        }

                        else
                        {
                            txtNumber.BackgroundColor = Color.Red;
                        }
                    }

                    break;
                //Text = 2,
                case 2:
                    Entry txtText = view as Entry;
                    txtText.BackgroundColor = Color.Default;
                    if (isOptional && string.IsNullOrWhiteSpace(txtText.Text)) { valid = true; }
                    else if (string.IsNullOrWhiteSpace(txtText.Text))
                    {
                        stringBuilder.AppendLine($"{prefixMessage}- يجب ادخال النص اولا");
                        valid = false;

                        txtText.BackgroundColor = Color.Red;
                    }



                    break;

                //Choice = 3,
                case 3:
                    Picker picker = view as Picker;
                    picker.BackgroundColor = Color.Default;
                    if (isOptional && picker.SelectedIndex <= 0)
                    {
                        valid = true;
                    }
                    else if (picker.SelectedIndex <= 0)
                    {
                        stringBuilder.AppendLine($"{prefixMessage}- يجب أختيار الاجابة اولا");
                        valid = false;
                        picker.BackgroundColor = Color.Red;

                    }
                    break;
                //ListItem = 4, 
                case 4:

                    Picker picker2 = view as Picker;
                    picker2.BackgroundColor = Color.Default;

                    if (isOptional && picker2.SelectedIndex <= 0)
                    {
                        valid = true;
                    }
                    else if (picker2.SelectedIndex <= 0)
                    {
                        stringBuilder.AppendLine($"{prefixMessage}- يجب أختيار الاجابة اولا");
                        valid = false;
                        picker2.BackgroundColor = Color.Red;

                    }

                    break;

                //DateTime = 5
                case 5:
                    DatePicker datePicker = view as DatePicker;
                    datePicker.BackgroundColor = Color.Default;
                    if (isOptional && (datePicker.Date == DateTime.MinValue || datePicker.Date == DateTime.MaxValue))
                    {
                        valid = true;
                    }

                    else if ((datePicker.Date == DateTime.MinValue || datePicker.Date == DateTime.MaxValue || datePicker.Date >= DateTime.Today))
                    {
                        stringBuilder.AppendLine($"{prefixMessage} - التاريخ المدخل غير صحيح");

                        valid = false;

                        datePicker.BackgroundColor = Color.Red;
                    }
                    break;


            }

            if (!valid) return ValidationResult.CreateError(stringBuilder.ToString());

            return ValidationResult.CreateSuccess();

        }
        private ValidationResult ValidateView(View view, FormListFieldInfo questionInfo, int itemIndex)
        {
            StringBuilder stringBuilder = new StringBuilder();
            bool valid = true;
            bool isOptional = questionInfo.IsOptional;
            switch (questionInfo.TypeId)
            {
                //Number
                case 1:
                    Entry txtNumber = view as Entry;
                    if (isOptional && string.IsNullOrEmpty(txtNumber.Text))
                    {
                        valid = true;
                    }
                    else
                    {
                        valid = int.TryParse(txtNumber.Text, out int value);
                        if (!valid)
                        {
                            stringBuilder.AppendLine($"{questionInfo.Description} [{itemIndex}]- الرقم المدخل غير صحيح");
                        }

                        if (valid && questionInfo.MinValue.HasValue && value < questionInfo.MinValue.Value)
                        {
                            stringBuilder.AppendLine($"{questionInfo.Description} [{itemIndex}] - الرقم المدخل يجب ان يكون أكبر أو يساوي {questionInfo.MinValue.Value} ");

                            valid = false;
                        }

                        if (valid && questionInfo.MaxValue.HasValue && value > questionInfo.MaxValue.Value)
                        {
                            stringBuilder.AppendLine($"{questionInfo.Description} [{itemIndex}] - الرقم المدخل يجب ان يكون أقل أو يساوي {questionInfo.MaxValue.Value} ");

                            valid = false;
                        }
                    }

                    break;
                //Text = 2,
                case 2:
                    Entry txtText = view as Entry;
                    txtText.BackgroundColor = Color.Default;
                    if (isOptional && string.IsNullOrWhiteSpace(txtText.Text)) { valid = true; }
                    else if (string.IsNullOrWhiteSpace(txtText.Text))
                    {
                        stringBuilder.AppendLine($"{questionInfo.Description} [{itemIndex}] - يجب ادخال النص اولا");

                        txtText.BackgroundColor = Color.Red;
                        valid = false;
                    }




                    break;

                //Choice = 3,
                case 3:
                    Picker picker = view as Picker;
                    picker.BackgroundColor = Color.Default;
                    if (isOptional && picker.SelectedIndex <= 0)
                    {
                        valid = true;
                    }
                    else if (picker.SelectedIndex <= 0)
                    {
                        stringBuilder.AppendLine($"{questionInfo.Description} [{itemIndex}]  يجب أختيار الاجابة اولا -");
                        valid = false;
                        picker.BackgroundColor = Color.Red;

                    }
                    break;
                //ListItem = 4, 
                case 4:

                    Picker picker2 = view as Picker;
                    picker2.BackgroundColor = Color.Default;
                    if (isOptional && picker2.SelectedIndex <= 0)
                    {
                        valid = true;
                    }
                    else if (picker2.SelectedIndex <= 0)
                    {
                        stringBuilder.AppendLine($"{questionInfo.Description} [{itemIndex}] - يجب أختيار الاجابة اولا");
                        valid = false;

                        picker2.BackgroundColor = Color.Red;
                    }

                    break;

                //DateTime = 5
                case 5:
                    DatePicker datePicker = view as DatePicker;

                    if (isOptional && (datePicker.Date == DateTime.MinValue || datePicker.Date == DateTime.MaxValue))
                    {
                        valid = true;
                    }

                    else if ((datePicker.Date == DateTime.MinValue || datePicker.Date == DateTime.MaxValue || datePicker.Date >= DateTime.Today))
                    {
                        stringBuilder.AppendLine($"{questionInfo.Description} [{itemIndex}] - التاريخ المدخل غير صحيح");

                        valid = false;
                    }
                    break;


            }

            if (!valid) return ValidationResult.CreateError(stringBuilder.ToString());

            return ValidationResult.CreateSuccess();

        }


        private bool IsValidQuestion(QuestionInfo questionInfo)
        {
            if (questionInfo == null) return false;

            bool valid = true;

            var conditions = CurrentSurvey.DisplayConditions.Where(x => x.QuestionId == questionInfo.Id);
            if (conditions.Any())
            {
                List<bool> vs = new List<bool>();
                foreach (var condition in conditions)
                {
                    var answer = SectionVisit.Answers.Where(x => x.QId == condition.ConditionQuestionId).FirstOrDefault();

                    if (answer == null)
                    {
                        vs.Add(false);
                    }

                    else
                    {
                        var singleValue = answer.SingleValues.FirstOrDefault();
                        if (singleValue == null)
                        {
                            vs.Add(false);
                        }
                        else
                        {
                            var val = singleValue.Value;
                            // Equal = 1,

                            if (condition.OperatorId == 1)
                            {
                                vs.Add(val == condition.Value);
                            }
                            //GreaterThan = 2,
                            else if (condition.OperatorId == 2)
                            {
                                bool valValid = int.TryParse(val, out int valInt);
                                bool newVal = int.TryParse(condition.Value, out int valCompInt);
                                if (!valValid || !newVal) vs.Add(false);
                                vs.Add(valInt > valCompInt);
                            }
                            //GreaterThanEqual = 3,
                            else if (condition.OperatorId == 3)
                            {
                                bool valValid = int.TryParse(val, out int valInt);
                                bool newVal = int.TryParse(condition.Value, out int valCompInt);
                                if (!valValid || !newVal) vs.Add(false);
                                vs.Add(valInt >= valCompInt);
                            }
                            //LessThan = 4,
                            if (condition.OperatorId == 4)
                            {
                                bool valValid = int.TryParse(val, out int valInt);
                                bool newVal = int.TryParse(condition.Value, out int valCompInt);
                                if (!valValid || !newVal) vs.Add(false);
                                vs.Add(valInt < valCompInt);
                            }
                            //LessThanEqual = 5,
                            if (condition.OperatorId == 5)
                            {
                                bool valValid = int.TryParse(val, out int valInt);
                                bool newVal = int.TryParse(condition.Value, out int valCompInt);
                                if (!valValid || !newVal) vs.Add(false);
                                vs.Add(valInt <= valCompInt);
                            }
                            //NotEqual = 6
                            if (condition.OperatorId == 6)
                            {
                                vs.Add(val != condition.Value);
                            }


                        }
                    }
                }
                // MatchAllConditions=1,

                if (questionInfo.ConditionGroupId == 1)
                {
                    valid = vs.All(x => x == true);
                }
                //   MatchOneConditionOrMore = 2
                else
                {
                    valid = vs.Any(x => x == true);
                }

            }

            return valid;
        }

        private bool IsFullValidQuestion(QuestionInfo questionInfo)
        {
            if (questionInfo == null) return false;

            bool valid = true;

            valid = IsValidQuestion(questionInfo);

            if (valid)
            {
                valid = !SkipQuestion(questionInfo);
            }

            return valid;
            //throw new NotImplementedException();
        }

        private async Task<bool> IsValidAnswer(bool showMessage = true)
        {
            bool valid = true;
            StringBuilder stringBuilder = new StringBuilder();
            List<ValidationResult> results = new List<ValidationResult>();
            if (CurrentQuestion != null)
            {

                if (CurrentQuestion.DisplayTypeId == 1)
                {
                    FormListFieldInfo info = new FormListFieldInfo();


                    View view = pnlContents.Children[0];
                    ValidationResult result = ValidateView(view, CurrentQuestion, string.Empty);

                    results.Add(result);
                }

                else if (CurrentQuestion.DisplayTypeId == 2)
                {

                    Grid grid = null;

                    foreach (var view in pnlContents.Children)
                    {
                        if (view is Grid)
                        {
                            grid = (Grid)view;
                            break;
                        }
                    }


                    List<LookUpValueInfo> RowlookUpValueInfos = CurrentSurvey.LookUps.Where(x => x.Id == CurrentQuestion.RowLookUpId).SelectMany(x => x.LookUpValues).ToList();
                    List<LookUpValueInfo> columnLookUps = new List<LookUpValueInfo>();

                    if (CurrentQuestion.SingleValueMatrix)
                    {
                        columnLookUps.Add(new LookUpValueInfo() { AnswerId = -1, AnswerCode = "", Description = "الاجابة" });
                    }
                    else
                    {

                        columnLookUps = CurrentSurvey.LookUps.Where(x => x.Id == CurrentQuestion.ColumnLookUpId).SelectMany(x => x.LookUpValues).ToList();

                        columnLookUps = FilterLookups(columnLookUps);
                    }
                    int rIndex = 1;

                    var children = grid.Children.ToList();






                    foreach (var r in RowlookUpValueInfos)
                    {
                        int colIndex = 1;

                        foreach (var c in columnLookUps)
                        {
                            var child = children.Where(x => Grid.GetRow(x) == rIndex && Grid.GetColumn(x) == colIndex).First();

                            if (child.IsEnabled)
                            {
                                results.Add(ValidateView(child, CurrentQuestion, r.Description));
                            }

                            colIndex++;

                        }

                        rIndex++;
                    }






                }

                else if (CurrentQuestion.DisplayTypeId == 3)
                {


                    Grid grid = null;

                    foreach (var view in pnlContents.Children)
                    {
                        if (view is Grid)
                        {
                            grid = (Grid)view;
                            break;
                        }
                    }


                    FormListInfo formList = CurrentSurvey.FormLists.Where(x => x.ID == CurrentQuestion.ListId).FirstOrDefault();


                    if (formList != null)
                    {
                        var fields = formList.FormListFields.Where(x => !SkipedFields.Contains(x.Code)).OrderBy(x => x.OrderId).ToList();




                        var primaryKey = formList.FormListFields.Where(x => !SkipedFields.Contains(x.Code)).Where(x => x.IsKey == true).FirstOrDefault();
                        int fCount = formList.FormListFields.Where(x => !SkipedFields.Contains(x.Code)).Count();
                        bool hasPK = !(primaryKey != null && primaryKey.TypeId == 3);
                        if (hasPK)
                        {
                            fCount++;

                        }




                        var children = grid.Children.ToList();
                        if (primaryKey != null && primaryKey.TypeId == 3)
                        {
                            var lookUpValues = CurrentSurvey.LookUps.Where(x => x.Id == primaryKey.LookUpId).FirstOrDefault().LookUpValues.ToList();

                            int rIndex = 1;


                            foreach (var lValue in lookUpValues)
                            {
                                int colIndex = 0;
                                foreach (var field in fields)
                                {

                                    if (field == primaryKey)
                                    {
                                        ListItemValue lv = new ListItemValue() { FieldId = field.Id, SingleValues = new List<SingleValue>() { new SingleValue() { Value = lValue.AnswerCode } } };

                                    }
                                    else
                                    {



                                        var child = children.Where(x => Grid.GetRow(x) == rIndex && Grid.GetColumn(x) == colIndex).First();

                                        if (child.IsEnabled)
                                        {
                                            results.Add(ValidateView(child, field, rIndex));
                                        }
                                    }

                                    colIndex++;
                                }

                                rIndex++;
                            }

                        }

                        else
                        {
                            var noRows = children.Any() ? children.Select(x => Grid.GetRow(x)).Max() : 0;

                            for (int rIndex = 1; rIndex <= noRows; rIndex++)
                            {

                                int colIndex = 0;
                                foreach (var field in fields)
                                {




                                    var child = children.Where(x => Grid.GetRow(x) == rIndex && Grid.GetColumn(x) == colIndex).First();
                                    if (child.IsEnabled)
                                    {
                                        results.Add(ValidateView(child, field, rIndex));

                                    }


                                    colIndex++;

                                }


                            }


                        }





                    }
                }

            }


            if (results.Any(x => x.Result == ValidationResultEnum.HasError))
            {

                valid = false;
                if (showMessage)
                {

                    var messages = results.Where(x => x.Result == ValidationResultEnum.HasError).Select(x => x.Message.ToString());
                    await DisplayAlert(GeneralMessages.Error, string.Join(Environment.NewLine, messages), GeneralMessages.Cancel);
                }

            }
            else if (results.Any(x => x.Result == ValidationResultEnum.HasWarning))
            {
                valid = false;
                // 

                if (showMessage)
                {
                    var messages = results.Where(x => x.Result == ValidationResultEnum.HasWarning).Select(x => x.Message.ToString());
                    valid = await DisplayAlert(GeneralMessages.Question, string.Join(Environment.NewLine, messages), GeneralMessages.Yes, GeneralMessages.No);
                }
                else
                {
                    valid = true;
                }
            }
            else
            {
                valid = true;
            }

            if (valid == true)
            {
                valid = await CustomQuestionValidation();
            }


            return valid;
        }

        #endregion

        #endregion

        #region Custom Validation/Skips Rules

        private bool SkipQuestion(QuestionInfo questionInfo)
        {
            //Questions Skips Rules (سكيبات)

            //bool skip = false;
            bool valid = true;

            if (questionInfo.Code.StartsWith("C11"))
            {
                //var q = QuestionList.Where(x => x.Code.StartsWith("C10") && x.Code.Contains("_A_") && !x.Code.EndsWith("CMNT")).Select(qu => qu.Id).ToList();

                //var values = SectionVisit.Answers.Where(x => q.Contains(x.QId)).SelectMany(x => x.SingleValues).ToList();


                //valid = values.Any(x => x.Value == "1" || x.Value == "2");
            }

           

            //else if (questionInfo.Code.StartsWith("H9_1"))
            //{
            //    string idsam = QuestionnaireManager.CurrentSample.ID00;
            //    var db = DataBase.GetConnection();
            //    var Individuals = db.Table<Individual>().Where(x => x.ID00 == idsam && x.D2A!="3" && x.D4==2 && (x.D5>=10 && x.D5<=19)).Count();
               

            //    valid = Individuals > 0;
            //}

            //else if (questionInfo.Code.StartsWith("H9_2"))
            //{
            //    string idsam = QuestionnaireManager.CurrentSample.ID00;
            //    var db = DataBase.GetConnection();
            //    var Individuals = db.Table<Individual>().Where(x => x.ID00 == idsam && x.D2A != "3" && x.D4 == 1 && (x.D5 >= 10 && x.D5 <= 19)).Count();


            //    valid = Individuals > 0;
            //}




            else if (questionInfo.Code.StartsWith("C12_2"))
            {
                var q = QuestionList.Where(x => x.Code.StartsWith("C12_1")).Select(qu => qu.Id).FirstOrDefault();

                var values = SectionVisit.Answers.Where(x => q == x.QId).SelectMany(x => x.MatrixValues).ToList();
                valid = values.Where(x => x.Value == "1").Count() >= 1;
            }
            //else if (questionInfo.Code.StartsWith("C12_2_2"))
            //{
            //    var q = QuestionList.Where(x => x.Code.StartsWith("C12_1")).Select(qu => qu.Id).FirstOrDefault();

            //    var values = SectionVisit.Answers.Where(x => q == x.QId).SelectMany(x => x.MatrixValues).ToList();
            //    valid = values.Where(x => x.Value == "1").Count() >= 2;
            //}
            //else if (questionInfo.Code.StartsWith("C12_2_3"))
            //{
            //    var q = QuestionList.Where(x => x.Code.StartsWith("C12_1")).Select(qu => qu.Id).FirstOrDefault();

            //    var values = SectionVisit.Answers.Where(x => q == x.QId).SelectMany(x => x.MatrixValues).ToList();
            //    valid = values.Where(x => x.Value == "1").Count() >= 3;
            //}

            //else if (questionInfo.Code.StartsWith("H23_A"))
            //{
            //    valid = GeneralApplicationSettings.GazaGovs.Contains(QuestionnaireManager.CurrentSample.ID1);
            //}

            else if (questionInfo.Code == "EQ14_CMNT")
            {
                var q = QuestionList.Where(x => x.Code == "EQ14").Select(qu => qu.Id).ToList();
                var values = SectionVisit.Answers.Where(x => q.Contains(x.QId)).SelectMany(x => x.SingleValues).ToList();
                valid = values.Any(x => x.Value != "0");
            }
            else if (questionInfo.Code == "AG15_16_CMNT")
            {
                valid = GetMatrixAnswerValue("AG15", "16", "") == "1";
            }
            else if (questionInfo.Code == "TA2_CMNT")
            {
                valid = GetMatrixAnswerValue("TA2", "X", "") == "1";
            }
            else if (questionInfo.Code == "TA8_CMNT")
            {
                valid = GetMatrixAnswerValue("TA8", "X", "") == "1";
            }

            else if(questionInfo.Code== "I06_CMNT")
            {
                valid = GetMatrixAnswerValue("I06", "13", "") == "1";
            }





            else if ((questionInfo.Code.StartsWith("GA") || questionInfo.Code.StartsWith("ICT")) && (questionInfo.Code != "GA2" && questionInfo.Code != "GA3" && questionInfo.Code != "GA3_CMNT"))
            {
                var q = QuestionList.Where(x => x.Code == "GA3").Select(qu => qu.Id).ToList();
                var values = SectionVisit.Answers.Where(x => q.Contains(x.QId)).SelectMany(x => x.SingleValues).ToList();
                valid = values.Any(x => x.Value == "1" || x.Value == "2");
            }

            else if (questionInfo.Code == "I06A_13_CMNT")
            {
                var qVal = QuestionList.Where(x => x.Code == "I06").Select(qu => qu).FirstOrDefault();

                if (qVal != null)
                {
                    var q = qVal.Id;
                    var formList = CurrentSurvey.FormLists.Where(x => x.ID == qVal.ListId).First();
                    var field = formList.FormListFields.Where(x => x.Code == "I06A").First();

                    // var matrixValues = SectionVisit.Answers.Last().MatrixValues;
                    var listAnswer = FindAnswer(q);
                    var vals = listAnswer.ListItems.SelectMany(x => x.ListValues).Where(x => x.FieldId == field.Id).SelectMany(x => x.SingleValues).ToList();
                    if (vals.Count > 12)
                    {
                        valid = vals[12].Value == "1";
                    }
                }

            }

            else if (questionInfo.Code == "GOS14_A" || questionInfo.Code == "GOS15" || questionInfo.Code == "GOS16")
            {
                valid = false;
                var qVal = QuestionList.Where(x => x.Code == "GOS").Select(qu => qu).FirstOrDefault();

                if (qVal != null)
                {
                    var q = qVal.Id;
                    var formList = CurrentSurvey.FormLists.Where(x => x.ID == qVal.ListId).First();
                    var field = formList.FormListFields.Where(x => x.Code == "GOS11").First();

                    // var matrixValues = SectionVisit.Answers.Last().MatrixValues;
                    var listAnswer = FindAnswer(q);
                    if (listAnswer != null)
                    {
                        var vals = listAnswer.ListItems.SelectMany(x => x.ListValues).Where(x => x.FieldId == field.Id).SelectMany(x => x.SingleValues).ToList();
                        valid = vals?.Any(x => x.Value == "1") ?? false;
                    }
                }




            }

            else if ((questionInfo.Code.StartsWith("HS") || questionInfo.Code.StartsWith("Q") || questionInfo.Code.StartsWith("ES") || questionInfo.Code.StartsWith("GOS") || questionInfo.Code.StartsWith("TA") || questionInfo.Code.StartsWith("CE") || questionInfo.Code.StartsWith("AGG") || questionInfo.Code.StartsWith("BTQ") || questionInfo.Code.StartsWith("PTSD") || questionInfo.Code.StartsWith("COVID") || questionInfo.Code.StartsWith("PB9") || questionInfo.Code.StartsWith("GA7") || questionInfo.Code.StartsWith("GA5") || questionInfo.Code.StartsWith("GA6")))
            {
                var q = QuestionList.Where(x => x.Code == "GA3").Select(qu => qu.Id).ToList();
                var values = SectionVisit.Answers.Where(x => q.Contains(x.QId)).SelectMany(x => x.SingleValues).ToList();
                valid = values.Any(x => x.Value == "1" || x.Value == "2");
            }

           

                if (valid)
            {
                //if (questionInfo.Code.StartsWith("ES"))
                //{
                //    var db = DataBase.GetConnection();
                //    string idsam = QuestionnaireManager.CurrentSample.ID00;
                    
                //    var Individuals = db.Table<Individual>().Where(x => x.ID00 == idsam && x.D2A != "3").ToList();
                //    var IND = Individuals.Where(x => x.D5 >= 5 && x.D5 <= 18  /*&& d2_aVals.Contains(x.D2_A_New.Value)*/).OrderBy(i => i.D1).ToList();

                //    valid = IND.Count() > 0;

                //}


                if (questionInfo.Code.StartsWith("CE"))
                {
                    valid= GeneralApplicationSettings.GazaGovs.Contains( QuestionnaireManager.CurrentVisit.ID01);
                 }

                if (questionInfo.Code.StartsWith("PTSD"))
                {

                    var question = QuestionList.Where(x => x.Code == "BTQ7").FirstOrDefault();

                    if (question != null)
                    {
                        var qAnswer = FindAnswer(question.Id);


                        var formList = CurrentSurvey.FormLists.Where(x => x.ID == question.ListId).First();
                        var fieldCodes = new string[] { "BTQ7_A", "BTQ7_B", "BTQ7_C" };
                        var fields = formList.FormListFields.Where(x => fieldCodes.Contains(x.Code)).Select(x => x.Id).ToList();

                        // var matrixValues = SectionVisit.Answers.Last().MatrixValues;

                        var vals = qAnswer.ListItems.SelectMany(x => x.ListValues).Where(x => fields.Contains(x.FieldId)).SelectMany(x => x.SingleValues).Select(x => x.Value).ToList();


                        return !vals.Any(z => z == "1");
                    }


                }
            }

            if (valid)
            {
                if (questionInfo.Code == "ES08A")
                {
                  valid=  GetMatrixAnswerValue("ES07_A_A", "3", string.Empty)=="1";
                }
                if (questionInfo.Code == "ES08B")
                {
                    valid = GetMatrixAnswerValue("ES07_A_B", "3", string.Empty) == "1";
                }

                if (questionInfo.Code == "ES09A" || questionInfo.Code == "ES10A")
                {
                    valid = GetMatrixAnswerValue("ES07_A_A", "1", string.Empty) == "1" || GetMatrixAnswerValue("ES07_A_A", "2", string.Empty) == "1" || GetMatrixAnswerValue("ES07_A_A", "4", string.Empty) == "1";
                }

                if (questionInfo.Code == "ES09B" || questionInfo.Code == "ES10B")
                {
                    valid = GetMatrixAnswerValue("ES07_A_B", "1", string.Empty) == "1" || GetMatrixAnswerValue("ES07_A_B", "2", string.Empty) == "1" || GetMatrixAnswerValue("ES07_A_B", "4", string.Empty) == "1";

                }

            }

            return !valid;
        }

        private async Task<bool> CustomQuestionValidation(bool showMessage = true)
        {
            //Custom Question Input Validation (قواعد  التدقيق)

            bool isValid = true;
            StringBuilder stringBuilder = new StringBuilder();



          

            if (CurrentQuestion.Code == "C02")
            {
                var listValue = GetListValue();

                if (listValue.Count() == 0)
                {
                    stringBuilder.AppendLine("يجب أن يكون  اجابة واحدة على الاقل");
                    isValid = false;

                }
            }

            else if(CurrentQuestion.Code== "QC9")
            {
                var val = GetViewValueFromScreen(pnlContents.Children[0], CurrentQuestion.TypeId.Value);


                if(Validator.IsMobile(val)==false && Validator.IsTelephone(val) == false)
                {
                    stringBuilder.AppendLine("رقم الهاتف أو الجوال غير صحيح");
                    isValid = false;
                }

            }

            else if (CurrentQuestion.Code== "H2_A")
            {

                var val = GetViewValueFromScreen(pnlContents.Children[0], CurrentQuestion.TypeId.Value);
                var H1 = GetSingleAnswerValue("H1");
                if (H1 == "5" || H1 == "6" || H1 == "7")
                {
                   

                    if(val!="7")
                    {
                        stringBuilder.AppendLine("نوع المسكن خيمة ،براكية/ كرفان/ بركس، أخرىومادة بناء الجدران ليست أخرى ");
                        isValid = false;
                    }

                }

            }

            else if(CurrentQuestion.Code== "H9")
            {
                var val = GetViewValueFromScreen(pnlContents.Children[0], CurrentQuestion.TypeId.Value);

                var H1 = GetSingleAnswerValue("H1");
                if (H1 == "4")
                {


                    if (val != "1")
                    {
                        stringBuilder.AppendLine("نوع المسكن غرفة مستقلة ويوجد فيها أكثر من غرفة");
                        isValid = false;
                    }

                }

                else if (isValid)
                {
                    
                    var q = QuestionList.Where(x => x.Code.StartsWith("H8")).Select(qu => qu.Id).FirstOrDefault();
                    var values = SectionVisit.Answers.Where(x => q == x.QId).SelectMany(x => x.SingleValues).FirstOrDefault();

                    int h8 = Convert.ToInt32(values.Value);
                    int h9 = Convert.ToInt32(val);

                    if (h9 > h8)
                    {
                        stringBuilder.AppendLine("عدد الغرف المخصصة للنوم في المسكن يجب ان  تكون أقل او تساوي عدد الغرف في المسكن");
                        isValid = false;
                    }
                }
            }



            else if(CurrentQuestion.Code== "H18_1" || CurrentQuestion.Code == "H18_2")
            {
                var val = GetViewValueFromScreen(pnlContents.Children[0], CurrentQuestion.TypeId.Value);
                if (val == "3")
                {
                    var H12_2 = GetSingleAnswerValue("H12_2");
                    if (H12_2 == "6")
                    {
                        stringBuilder.AppendLine("لا يمكن اختيار هذا الخيار المسكن غير متصل بالكهرباء");
                        isValid = false;
                    }
                }
            }
            else if (CurrentQuestion.Code == "H18_3")
            {
                var val = GetViewValueFromScreen(pnlContents.Children[0], CurrentQuestion.TypeId.Value);
                if (val == "2")
                {
                    var H12_2 = GetSingleAnswerValue("H12_2");
                    if (H12_2 == "6")
                    {
                        stringBuilder.AppendLine("لا يمكن اختيار هذا الخيار المسكن غير متصل بالكهرباء");
                        isValid = false;
                    }
                }
            }
            else if (CurrentQuestion.Code == "H18_4")
            {
                var val = GetViewValueFromScreen(pnlContents.Children[0], CurrentQuestion.TypeId.Value);
                if (val == "4")
                {
                    var H12_2 = GetSingleAnswerValue("H12_2");
                    if (H12_2 == "6")
                    {
                        stringBuilder.AppendLine("لا يمكن اختيار هذا الخيار المسكن غير متصل بالكهرباء");
                        isValid = false;
                    }
                }
            }
            else if (CurrentQuestion.Code == "H18_5")
            {
                var val = GetViewValueFromScreen(pnlContents.Children[0], CurrentQuestion.TypeId.Value);
                if (val == "2")
                {
                    var H12_2 = GetSingleAnswerValue("H12_2");
                    if (H12_2 == "1")
                    {
                        stringBuilder.AppendLine("لا يمكن اختيار هذا الخيار المسكن غير متصل بالكهرباء");
                        isValid = false;
                    }
                }
            }

            else if (CurrentQuestion.Code== "H22")
            {
                //H12_2

                var H13_A_1 = GetSingleAnswerValue("H13_A_2");
                var H18_4 = GetSingleAnswerValue("H18_4");

                string v = null;
               
                if (H13_A_1 == "4")
                {
                    v = "29";
                }
                else if (H13_A_1 == "5")
                {
                    v = "28";
                }

               

              

                if (!string.IsNullOrWhiteSpace(v))
                {
                    var vals = GetMatrixValueFromScreen();
                    var xVal = vals.Where(x => x.RowCode == v).Select(x => x.Value).FirstOrDefault();

                    if (xVal == "0")
                    {
                        stringBuilder.AppendLine($"يجب أن تكون الاجابة في البند  {v} أكبر من  صفر" );
                        isValid = false;
                    }

                }

                if(H18_4=="1")
                {
                    var vals = GetMatrixValueFromScreen();
                    var xVal = vals.Where(x => x.RowCode == "32").Select(x => x.Value).FirstOrDefault();

                    if (xVal == "0")
                    {
                        stringBuilder.AppendLine($"يجب أن تكون الاجابة في البند  32 أكبر من  صفر");
                        isValid = false;
                    }
                }



            }


            else if(CurrentQuestion.Code== "E703")
            {
                var E703 = GetViewValueFromScreen(pnlContents.Children[0], CurrentQuestion.TypeId.Value);

                var q = QuestionList.Where(x => x.Code.StartsWith("E702")).Select(qu => qu.Id).FirstOrDefault();
                var matrixValues = GetSingleAnswerValue("E704").ToInt(); //SectionVisit.Answers.FirstOrDefault(x=>x.QId==q)?.MatrixValues;

                var  total= matrixValues?? 0;

                if (E703.ToInt() < total)
                {
                    stringBuilder.AppendLine("هو معدل انفاق الأسرة الشهري الكلي يجب ان يكون اكبر او يساوي " + total);
                    isValid = false;
                }
            }


            else if (CurrentQuestion.Code == "C06_15")
            {

                var val =GetViewValueFromScreen(pnlContents.Children[0], CurrentQuestion.TypeId.Value);

                if (val == "2")
                {

                    var q = QuestionList.Where(x => x.Code.StartsWith("C06")).Select(qu => qu.Id).FirstOrDefault();
                    var matrixValues = SectionVisit.Answers.FirstOrDefault(x=>x.QId==q)?.MatrixValues;
                    var vals = matrixValues?.Where(x => x.ColumnCode == "").Select(x => x);

                    if (vals.All(x => x.Value == "2"))
                    {
                        stringBuilder.AppendLine("يجب أن يكون  اجابة واحدة نعم على الاقل");
                        isValid = false;
                    }
                    //SectionVisit.Answers.Last().ListItems.Find(x=>x.)



                }
                //  check  if all  is no in C06



            }


            else if (CurrentQuestion.Code == "TA2" || CurrentQuestion.Code== "TA8" || CurrentQuestion.Code== "ES07_A_A" || CurrentQuestion.Code == "ES07_A_B")
            {



                    var matrixValues = GetMatrixValueFromScreen();
                    var vals = matrixValues.Where(x => x.ColumnCode == "").Select(x => x);

                    if (vals.All(x => x.Value == "2"))
                    {
                        stringBuilder.AppendLine("يجب أن يكون  اجابة واحدة نعم على الاقل");
                        isValid = false;
                    }
                    //SectionVisit.Answers.Last().ListItems.Find(x=>x.)



                
                //  check  if all  is no in C06



            }

            else if (CurrentQuestion.Code.StartsWith("PTSD7")  && CurrentQuestion.Code.EndsWith("11"))
            {
                var question = QuestionList.Where(x => x.Code == "BTQ7").FirstOrDefault();

                if (question != null)
                {
                    var qAnswer = FindAnswer(question.Id);

                    var formList = CurrentSurvey.FormLists.Where(x => x.ID == question.ListId).First();
                    var fieldCodes = new string[] { "BTQ7_A", "BTQ7_B", "BTQ7_C" };
                    var fields = formList.FormListFields.Where(x => fieldCodes.Contains(x.Code)).Select(x => x.Id).ToList();

                    var fieldId = new string[] { "BTQ7"};


                    var field = formList.FormListFields.Where(x => fieldId.Contains(x.Code)).Select(x => x.Id).FirstOrDefault();

                    // var matrixValues = SectionVisit.Answers.Last().MatrixValues;
                    var selectedVal = GetViewValueFromScreen(pnlContents.Children[0], CurrentQuestion.TypeId.Value);
                    var vals = qAnswer.ListItems.Where(x=>x.ListValues.Any(z=>z.FieldId==field && (z.SingleValues.FirstOrDefault()?.Value== selectedVal) )).SelectMany(x => x.ListValues).Where(x => fields.Contains(x.FieldId)).SelectMany(x => x.SingleValues).Select(x => x.Value).ToList();

                  

                    if (!vals.Any(z => z == "1"))
                    {
                        stringBuilder.AppendLine("يجب ان يكون  الخيار من  ضمن الاحداث/الصدمات التي اجاب عليها بنعم");
                        isValid = false;
                    }
                }

            }


            else if (CurrentQuestion.Code.StartsWith("C12_2"))
            {
                var q = QuestionList.Where(x => x.Code.StartsWith("C12_1")).Select(qu => qu.Id).FirstOrDefault();

                var values = SectionVisit.Answers.Where(x => q == x.QId).SelectMany(x => x.MatrixValues).ToList();
                var yesValues = values.Where(x => x.Value == "1").Select(x => x.RowCode).ToList();

                var val = GetViewValueFromScreen(pnlContents.Children[0], CurrentQuestion.TypeId.Value);

                if (!yesValues.Contains(val))
                {
                    stringBuilder.AppendLine("يجب ان يكون  الخيار من  ضمن الاحداث/الصدمات/الافعال التي اجاب عليها بنعم");
                    isValid = false;
                }


                //if (CurrentQuestion.Code.StartsWith("C12_2_2"))
                //{
                //    var qC12_2 = QuestionList.Where(x => x.Code == "C12_2_1").Select(qu => qu.Id).ToList();
                //    var qC12_2values = SectionVisit.Answers.Where(x => qC12_2.Contains(x.QId)).SelectMany(x => x.SingleValues).ToList().Select(x => x.Value).ToList();

                //    if (qC12_2values.Contains(val))
                //    {

                //        stringBuilder.AppendLine("لا يمكن تكرار الاجابة. لقد تم  اختيارها مسبقا");
                //        isValid = false;
                //    }
                //}
                //if (CurrentQuestion.Code.StartsWith("C12_2_3"))
                //{
                //    var qC12_2 = QuestionList.Where(x => x.Code == "C12_2_1" || x.Code == "C12_2_2").Select(qu => qu.Id).ToList();
                //    var qC12_2values = SectionVisit.Answers.Where(x => qC12_2.Contains(x.QId)).SelectMany(x => x.SingleValues).ToList().Select(x => x.Value).ToList();

                //    if (qC12_2values.Contains(val))
                //    {

                //        stringBuilder.AppendLine("لا يمكن تكرار الاجابة. لقد تم  اختيارها مسبقا");
                //        isValid = false;
                //    }
                //}

            }

           
            else if (CurrentQuestion.Code == "H9_1" || CurrentQuestion.Code == "H9_2")
            {
                var val = GetViewValueFromScreen(pnlContents.Children[0], CurrentQuestion.TypeId.Value);
                var q = QuestionList.Where(x => x.Code.StartsWith("H9")).Select(qu => qu.Id).FirstOrDefault();
                var values = SectionVisit.Answers.Where(x => q == x.QId).SelectMany(x => x.SingleValues).FirstOrDefault();

                int h8 = Convert.ToInt32(values.Value);
                int h9 = Convert.ToInt32(val);

                if (h9 > h8)
                {
                    stringBuilder.AppendLine("عدد الغرف المخصصة للنوم لهذه الفئة  يجب ان  تكون أقل او تساوي عدد غرف النوم في المسكن");
                    isValid = false;
                }

            }


            else if (CurrentQuestion.Code.StartsWith("GA51_11"))
            {
                var selectedVal = GetViewValueFromScreen(pnlContents.Children[0], CurrentQuestion.TypeId.Value);

                if (selectedVal == "2")
                {
                    var qIds = QuestionList.Where(x => x.Code.StartsWith("GA51")).Select(qu => qu.Id).ToList();

                    var values = SectionVisit.Answers.Where(x => qIds.Contains(x.QId)).ToList().SelectMany(x => x.SingleValues).Select(x => x.Value).ToList();

                    if (values.All(x => x == "2"))
                    {
                        isValid = false;

                    }
                }

                if (!isValid)
                {
                    stringBuilder.AppendLine("يجب ان يكون  هناك سبب واحد على الاقل  تم  الاجابة عليه بنعم");
                }

            }
            else if (CurrentQuestion.Code.StartsWith("H13_1_8"))
            {
                var selectedVal = GetViewValueFromScreen(pnlContents.Children[0], CurrentQuestion.TypeId.Value);

                if (selectedVal == "2")
                {
                    var qIds = QuestionList.Where(x => x.Code== "H13_1").Select(qu => qu.Id).FirstOrDefault();

                    var values = SectionVisit.Answers.Where(x =>x.QId==qIds).ToList().SelectMany(x => x.MatrixValues).Select(x => x.Value).ToList();

                    if (values.All(x => x == "2"))
                    {
                        isValid = false;

                    }
                }

                if (!isValid)
                {
                    stringBuilder.AppendLine("يجب ان يكون  هناك مصدر واحد على الاقل  تمت  الاجابة عليه بنعم");
                }

            }

            else if (CurrentQuestion.Code == "I06")
            {
               // var formList = CurrentSurvey.FormLists.Where(x => x.ID == CurrentQuestion.ListId).First();
                //var field = formList.FormListFields.Where(x => x.Code == "I06A").First();

                var matrixValues = GetMatrixValueFromScreen();
                

                if (matrixValues.All(x => x.Value == "2"))
                {
                    stringBuilder.AppendLine("يجب أن يكون  اجابة واحدة نعم على الاقل");
                    isValid = false;
                }
            }

            else if (CurrentQuestion.Code == "I06_01")
            {
                var q = QuestionList.Where(x => x.Code=="I06").Select(qu => qu).FirstOrDefault();
                //var qI06_13 = QuestionList.Where(x => x.Code.StartsWith("I06A_13")).Select(qu => qu).FirstOrDefault();
                //var formList = CurrentSurvey.FormLists.Where(x => x.ID == q.ListId).First();
                //var field = formList.FormListFields.Where(x => x.Code == "I06A").First();
                //var values = SectionVisit.Answers.Where(x => q.Id == x.QId).First().ListItems;
                //var qI06_13Values = SectionVisit.Answers.Where(x => qI06_13.Id == x.QId).First().SingleValues[0].Value;

                var values = GetMatrixAnswerValues("I06");

                var selectedVal = GetViewValueFromScreen(pnlContents.Children[0], CurrentQuestion.TypeId.Value);
                isValid = false;


                if (values.Any(x => x.RowCode == selectedVal && x.Value == "1"))
                {
                    isValid = true;
                }
                

                if (!isValid)
                {
                    stringBuilder.AppendLine("يجب ان يكون  الخيار من  ضمن المصادر التي اجاب عليها بنعم");
                }
               
            }


            //else if (CurrentQuestion.Code == "I07")
            //{
            //    var q = QuestionList.Where(x => x.Code=="I06").Select(qu => qu).FirstOrDefault();
            //    //var qI06_13 = QuestionList.Where(x => x.Code.StartsWith("I06A_13")).Select(qu => qu).FirstOrDefault();
            //    var formList = CurrentSurvey.FormLists.Where(x => x.ID == q.ListId).First();
            //    var field = formList.FormListFields.Where(x => x.Code == "I06B").First();
            //    var values = SectionVisit.Answers.Where(x => q.Id == x.QId).First().ListItems;
            //    //var qI06_13Values = SectionVisit.Answers.Where(x => qI06_13.Id == x.QId).First().SingleValues[0].Value;


            //    var selectedVal = GetViewValueFromScreen(pnlContents.Children[0], CurrentQuestion.TypeId.Value);
               

            //    var total = values.SelectMany(x => x.ListValues).Where(x => x.FieldId == field.Id).SelectMany(x => x.SingleValues).Where(x => !string.IsNullOrWhiteSpace(x.Value)).Sum(x => x.Value.ToInt());

            //    if (total != selectedVal.ToInt())
            //    {
            //        stringBuilder.AppendLine("معدل الدخل الشهري للمصادر يجب ان  يساوي "+ total);
            //        isValid = false;
            //    }
              
            //}

            else if (CurrentQuestion.Code == "GOS14_A")
            {
                var q = QuestionList.Where(x => x.Code.StartsWith("GOS")).Select(qu => qu).FirstOrDefault();
                //var qI06_13 = QuestionList.Where(x => x.Code.StartsWith("I06A_13")).Select(qu => qu).FirstOrDefault();
                var formList = CurrentSurvey.FormLists.Where(x => x.ID == q.ListId).First();
                var field = formList.FormListFields.Where(x => x.Code == "GOS11").First();
                var values = SectionVisit.Answers.Where(x => q.Id == x.QId).First().ListItems;
                //var qI06_13Values = SectionVisit.Answers.Where(x => qI06_13.Id == x.QId).First().SingleValues[0].Value;


                var selectedVal = GetViewValueFromScreen(pnlContents.Children[0], CurrentQuestion.TypeId.Value);
                isValid = false;


                foreach (var lv in values)
                {
                    var vals = lv.ListValues[0].SingleValues;
                    if (vals.Any(x => x.Value == selectedVal))
                    {
                        var qVal = lv.ListValues.Where(x => x.FieldId == field.Id).FirstOrDefault().SingleValues;
                        if (qVal.Any(x => x.Value == "1"))
                        {
                            isValid = true;
                        }

                    }

                }



                if (!isValid)
                {
                    stringBuilder.AppendLine("يجب ان يكون  الخيار من  ضمن المصادر التي اجاب عليها بنعم");
                }
                //var vals = GetListValue().SelectMany(x => x.ListValues);
                // var yesValues = values.Where(x => x.vals == "1").Select(x => x.RowCode).ToList();

                //var val = GetValueFromScreen(pnlContents.Children[0], CurrentQuestion.TypeId.Value);

                //if (!yesValues.Contains(val))
                //{
                //    stringBuilder.AppendLine("يجب ان يكون  الخيار من  ضمن المصادر التي اجاب عليها بنعم");
                //    isValid = false;
                //}
            }


            else if (CurrentQuestion.Code == "AG03_3")
            {
                var val = GetViewValueFromScreen(pnlContents.Children[0], CurrentQuestion.TypeId.Value);
                if (val == "2")
                {
                    var qC12_2 = QuestionList.Where(x => x.Code == "AG03_1" || x.Code == "AG03_2").Select(qu => qu.Id).ToList();
                    var qC12_2values = SectionVisit.Answers.Where(x => qC12_2.Contains(x.QId)).SelectMany(x => x.SingleValues).ToList().Select(x => x.Value).ToList();

                    if (qC12_2values.All(x => x == "2"))
                    {

                        stringBuilder.AppendLine("يجب ان تكون هناك  اجابه  واحده  نعم ");
                        isValid = false;
                    }

                }

            }








            if (CurrentQuestion.IsOptional)
            {
                isValid &= OptionalQuestionValidate(stringBuilder);
            }

            if (showMessage && !isValid && stringBuilder.Length > 0)
            {
                var messages = stringBuilder.ToString();
                await DisplayAlert(GeneralMessages.Error, messages, GeneralMessages.Cancel);
            }
            else if (showMessage && stringBuilder.Length > 0)
            {
                var messages = stringBuilder.ToString();
                await DisplayAlert(GeneralMessages.Warning, messages, GeneralMessages.Ok);
            }




            return isValid;
        }

       
        private bool P6CustomValidate(StringBuilder stringBuilder)
        {
            var views = GetCQuestionViewsFromScreenByColumn().Cast<Entry>().ToList();

         

          //  string[] views = { P3, p4 };

            decimal sum = 0, parsedValue;

            foreach (var x in views)
            {
                if (decimal.TryParse(x.Text, out parsedValue))
                {
                    sum += parsedValue;
                }
            }


            if ((sum != 100))
            {
                stringBuilder.AppendLine(ReturnMessage("يجب ان يكون مجموع النسب  100%"));
                return false;
            }

            return true;
        }

        private bool E9_6CustomValidate(StringBuilder stringBuilder)
        {
            var answer = GetMatrixValueFromScreen();
            var totalEmployees = answer.Select(x=> Convert.ToInt32(x.Value)).Sum(); //DateTime.Now.Year;

            if (totalEmployees ==0)
            {
                stringBuilder.AppendLine(ReturnMessage("يجب وجود عامل واحد على الاقل "));
                return false;
            }

            return true;
        }

        private bool Cov3CustomValidate(StringBuilder stringBuilder)
        {
            var answer =Convert.ToInt32( GetViewValue(GetCQuestionViewFromScreenByRowColumn()));
            var cov2 = GetSingleAnswerValue("COV2");



            if (cov2=="1" && answer!=100)
            {
                stringBuilder.AppendLine(ReturnMessage("يجب تسجيل 100% لمن اجابوا  لم تتأثر"));
                return false;
            }

            if (cov2 == "2" && answer>=100)
            {
                stringBuilder.AppendLine(ReturnMessage("يجب تسجيل اقل من 100% لمن اجابوا  تأثر سلبا – انخفاض المبيعات/الانتاج"));
                return false;
            }
            if (cov2 == "3" && answer <= 100)
            {
                stringBuilder.AppendLine(ReturnMessage("يجب تسجيل أكثر من 100% لمن اجابوا تأثرت ايجابا – ارتفاع المبيعات/الانتاج"));
                return false;
            }

            return true;
        }

        private bool A0CustomValidate(StringBuilder stringBuilder)
        {
            var answer = GetMatrixValueFromScreen();
            var totalEmployees = answer.Select(x => Convert.ToInt32(x.Value)).Sum(); //DateTime.Now.Year;

            if (totalEmployees == 0)
            {
                stringBuilder.AppendLine(ReturnMessage("يجب وجود عامل واحد على الاقل باجر او يدون أجر"));
                return false;
            }

            return true;
        }

        private bool A4CustomValidate(StringBuilder stringBuilder)
        {

            var A1 = Convert.ToInt32(GetSingleAnswerValue("A1"));
            var A2 = Convert.ToInt32(GetSingleAnswerValue("A2"));
            var A3 = Convert.ToInt32(GetSingleAnswerValue("A3"));
            var A4 = Convert.ToInt32(GetViewValue(GetCQuestionViewFromScreenByRowColumn()));




            if (A3+A4 >A2+A1)
            {
                stringBuilder.AppendLine(ReturnMessage("مجموع المصروفات والمستلزمات اكبر من الانتاج"));
                return false;
            }

            return true;
        }

     

       

        private bool P4CustomValidate(StringBuilder stringBuilder)
        {
            //var views = GetCQuestionViewsFromScreenByColumn().Cast<Entry>().ToList();

            var P3 = GetSingleAnswerValue("P3");
            var p4 =( (Entry)GetCQuestionViewsFromScreen().First()).Text;

            string[] views = { P3, p4 };

            decimal sum = 0, parsedValue;

            foreach(var x in views)
            {
                if (decimal.TryParse(x, out parsedValue))
                {
                    sum += parsedValue;
                }
            }
          

            if (!(sum >= 0 && sum <= 100))
            {
                stringBuilder.AppendLine(ReturnMessage("يجب ان يكون مجموع النسب من 0 الى 100%"));
                return false;
            }

            return true;
        }


        private bool S1CustomValidate(StringBuilder stringBuilder)
        {
            var views = GetCQuestionViewsFromScreenByColumn().Cast<Entry>().ToList();

            decimal sum = 0, parsedValue;
            views.ForEach(x =>
            {
                if (decimal.TryParse(x.Text, out parsedValue))
                {
                    sum += parsedValue;
                }
            });

            if (sum != 100)
            {
                stringBuilder.AppendLine(ReturnMessage("يجب ان يكون مجموع النسب يساوي 100%"));
                return false;
            }

            return true;
        }


        private bool I1CustomValidate(StringBuilder stringBuilder)
        {
            if (!GetMatrixValueFromScreen().Where(x => x.Value == "1").Select(x => x.Value).ToList().Any())
            {
                stringBuilder.AppendLine(ReturnMessage("يجب ان اتكون اجابة واحدة على الاقل بنعم"));

                return false;
            }
            return true;
        }

        private bool I4CustomValidate(StringBuilder stringBuilder)
        {
            var I3 = GetSingleAnswerValue("I3"); // GetViewValue(GetCQuestionViewFromScreenByRowColumn());
            var I4 = ((Entry)GetCQuestionViewsFromScreen().First()).Text;

            int total = 0;

            if(int.TryParse(I3,out int i3Val)){
                total += i3Val;
            }
            if (int.TryParse(I4, out int i4Val)){
                total += i4Val;
            }

            if (total==0)
            {
                stringBuilder.AppendLine(ReturnMessage("يجب وجود قيمة اكبر من  صفر في الديزل او الغاز على الاقل"));//warning

                return false;
            }
            return true;
        }

        private bool T2CustomValidate(StringBuilder stringBuilder)
        {
            var answer = GetViewValue(GetCQuestionViewFromScreenByRowColumn());

            if (answer == "0")
            {
                stringBuilder.AppendLine(ReturnMessage("تم اختيار معدل كمية النفايات الصلبة 'صفر'"));//warning
            }
            return true;
        }

        private async Task<bool> E26CustomValidate(StringBuilder stringBuilder)
        {
            var answer = GetViewValue(GetCQuestionViewFromScreenByRowColumn());
            var opponent = GetSingleAnswerValue("E20");
            int parsed;

            if (!string.IsNullOrEmpty(answer))
            {
                if (answer.Length != 9 || !int.TryParse(answer, out parsed))
                {
                    stringBuilder.AppendLine(ReturnMessage("يجب وجود إجابة رقمية من 9 خانات"));
                    return false;
                }
                else if (!opponent.EqualsOneOf("1", "2", "10", "11") && !answer.StartsWith("5"))
                {
                    stringBuilder.AppendLine(ReturnMessage("يجب ان يبدأ رقم الهوية بالرقم 5"));
                    return false;
                }
                else if (!Validator.IsID(answer))
                {
                    return await DisplayAlert(GeneralMessages.Question, "هل انت متاكد من رقم الهوية؟", GeneralMessages.Yes, GeneralMessages.No);
                }

            }

            return true;
        }

        private bool L25L35CustomValidate(StringBuilder stringBuilder)
        {
            var lastRowViews = GetCQuestionViewsFromScreenByRow(GetCQuestionViewsFromScreenByColumn().Count()).Cast<Entry>().ToList();
            int sum = 0;
            int parsed;

            lastRowViews.ForEach(x =>
            {
                if( int.TryParse(x.Text, out parsed))
                {
                    sum += parsed;
                }
                
            });

            if (sum == 0)
            {
                stringBuilder.AppendLine(ReturnMessage("مجموع العمال يجب أن يساوي 1 أو أكثر"));
                return false;
            }

            return true;
        }


        #endregion

        #region Picker Changed Events

        private void PickerSelectedIndexChanged(object sender, EventArgs e)
        {
            //Question Picker Changed Event

            Picker picker = sender as Picker;
            LookUpValueInfo selectedItem = picker.SelectedItem as LookUpValueInfo;

            if (selectedItem == null)
            {
                return;
            }

            int selectedAnswer = picker.SelectedIndex;
            string value = selectedItem.Description;
        }

        #endregion

        #region Entry Text Changed Event

        private void EntryTextChanged(object sender, TextChangedEventArgs e)
        {
            //Question Entry Changed Event

            Entry entry = sender as Entry;
            string oldValue = e.OldTextValue;
            string value = entry.Text;

            if (GridViewChangedOfCell(entry, "L1") || GridViewChangedOfCell(entry, "L2"))
            {
                var rowViews = GetCQuestionViewsFromScreenByColumn(Grid.GetColumn(entry)).Cast<Entry>().ToList();
                var lastView = rowViews.Last();
                rowViews.Remove(lastView);

                int sum = 0, parsedValue;
                rowViews.ForEach(x =>
                {
                    if (int.TryParse(x.Text, out parsedValue))
                    {
                        sum += parsedValue;
                    }
                });

                lastView.Text = sum.ToString();
            }
        }

        #endregion

        #region On Build Question

        private void OnBuildQuestion()
        {
            if (CurrentQuestion == null) return;

            if (CurrentQuestion.Code == "L1" || CurrentQuestion.Code == "L2")
            {
                var rowViews = GetCQuestionViewsFromScreenByRow(GetCQuestionViewsFromScreenByColumn().Count()).ToList();
                rowViews.ForEach(x =>
                {
                    x.IsEnabled = false;
                });
            }
        }
        #endregion

    }
}