using PECS2022.Models;
using PECS2022.SurveyModel;
using PECS2022.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PECS2022.ViewModels
{

    public delegate Task OnContinuePressed();
  public  class CallLogViewModel: BaseViewModel
    {


        private readonly SampleInfo _sample;
        private readonly CallLogInfo _callLog;


        public  Command Continue { get; private set; }

        public event OnContinuePressed OnContinuePressed;



        public CallLogViewModel(SampleInfo sample, string tel )
        {

            _sample = sample;

            LoadSettings();

            CallLogInfo logInfo = new CallLogInfo() { ID = Guid.NewGuid(), ID00 = sample.ID00, C3 = DateTime.Now, C1 = 1  };
            _callLog = logInfo;


            if (!string.IsNullOrWhiteSpace(tel))
            {
                C2 = tel;
            }
            C3 = DateTime.Now.ToString();
            QuestionnaireManager.CurrentCall = _callLog;

            Continue=  new Command(async () =>
            {
              await  OnContinuePressed?.Invoke();
            });
            C2IsEnabled = string.IsNullOrWhiteSpace(tel);
            IsEnabled = true;
        }


        public CallLogViewModel(SampleInfo sample, CallLogInfo  logInfo)
        {

            _sample = sample;
            _callLog = logInfo;

            
            QuestionnaireManager.CurrentCall = _callLog;
        }

     


        private int _C1;
            private string _C2;
            private string _C3;
            private LookUpValueInfo _C4;
            private LookUpValueInfo _C5;
            private LookUpValueInfo _C6;
            private string _C6_Name;
            private LookUpValueInfo _C7;
            private LookUpValueInfo _C8;
            private string _Comments;
        //private string _CreatedBy;

        private List<LookUpValueInfo> _C4List;
        private List<LookUpValueInfo> _C5List;
        private List<LookUpValueInfo> _C6List;
        private List<LookUpValueInfo> _C7List;
        private List<LookUpValueInfo> _C8List;


        private  void LoadSettings()
        {
            C4List = LookUpManager.GetSurveyLookupById(4506);
            C5List = LookUpManager.GetSurveyLookupById(4507);
           // C6List = LookUpManager.GetSurveyLookupById(4508);
            C7List = LookUpManager.GetSurveyLookupById(4509);
            C8List = LookUpManager.GetSurveyLookupById(4510);


            var db =  DataBase.GetConnection();
            var inds =  db.Table<Individual>().Where(x => x.ID00 == _sample.ID00 ).ToList();

            var lookupValues2 = inds.Select(x => new LookUpValueInfo() { AnswerCode = x.D1.ToString(), Description = x.D2, AnswerId = x.D1 }).ToList();




           
            lookupValues2.Add(new LookUpValueInfo() { AnswerCode = "0", AnswerId=0, Description = "فرد اخر غير مدرج ضمن قائمة الافراد" });

            C6List =lookupValues2;
           
        }


        public List<LookUpValueInfo> C4List { get { return _C4List; } set { SetProperty(ref _C4List, value); } }
        public List<LookUpValueInfo> C5List { get { return _C5List; } set { SetProperty(ref _C5List, value); } }
        public List<LookUpValueInfo> C6List { get { return _C6List; } set { SetProperty(ref _C6List, value); } }
        public List<LookUpValueInfo> C7List { get { return _C7List; } set { SetProperty(ref _C7List, value); } }
        public List<LookUpValueInfo> C8List { get { return _C8List; } set { SetProperty(ref _C8List, value); } }


        //public string ID00{ get { return _D7; } set { SetProperty(ref _D7, value); } }
           public int C1 { get { return _C1; } set { _ = SetProperty(ref _C1, value); } }

        [Display(Name = "C2", ResourceType = typeof(Resources.ModelResources))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        [MaxLength(10, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Resources.ModelResources))]        
        [TelMobile(ErrorMessageResourceName = "TELNotValid", ErrorMessageResourceType = typeof(Resources.ModelResources))] 
        public string C2 { get { return _C2; } set { SetProperty(ref _C2, value); } }

        [Display(Name = "C3", ResourceType = typeof(Resources.ModelResources))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        public string C3{ get { return _C3; } set { SetProperty(ref _C3, value); } }
        [Display(Name = "C4", ResourceType = typeof(Resources.ModelResources))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        public LookUpValueInfo C4{ get { return _C4; } set { SetProperty(ref _C4, value); SetControlSettings(); } }
        [Display(Name = "C5", ResourceType = typeof(Resources.ModelResources))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        public LookUpValueInfo C5 { get { return _C5; } set { SetProperty(ref _C5, value); SetControlSettings(); } }
        [Display(Name = "C6", ResourceType = typeof(Resources.ModelResources))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        public LookUpValueInfo C6 { get { return _C6; } set { SetProperty(ref _C6, value); SetControlSettings(); } }

        [Display(Name = "C6_Name", ResourceType = typeof(Resources.ModelResources))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        [ArabicOnly(ErrorMessageResourceName = "ArabicOnly", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        public string C6_Name{ get { return _C6_Name; } set { SetProperty(ref _C6_Name, value); } }

        [Display(Name = "C7", ResourceType = typeof(Resources.ModelResources))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        public LookUpValueInfo C7 { get { return _C7; } set { SetProperty(ref _C7, value); SetControlSettings(); } }

        [Display(Name = "C8", ResourceType = typeof(Resources.ModelResources))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        public LookUpValueInfo C8 { get { return _C8; } set { SetProperty(ref _C8, value); } }

        [Display(Name = "Comments", ResourceType = typeof(Resources.ModelResources))]
       // [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        public string Comments{ get { return _Comments; } set { SetProperty(ref _Comments, value); } }
        // public string CreatedBy{ get { return _D7; } set { SetProperty(ref _D7, value); } }



        private bool _IsEnabled;
        private bool _C5IsEnabled;
        private bool _C6IsEnabled;
        private bool _C6_CMNTIsEnabled;
        private bool _C7IsEnabled;
        private bool _C8IsEnabled;
        private bool _C2IsEnabled;

        public bool IsEnabled { get { return _IsEnabled; } set { SetProperty(ref _IsEnabled, value); } }
        public bool C2IsEnabled { get { return _C2IsEnabled; } set { SetProperty(ref _C2IsEnabled, value); } }
        public bool C5IsEnabled { get { return _C5IsEnabled; } set { SetProperty(ref _C5IsEnabled, value); } }
        public bool C6IsEnabled { get { return _C6IsEnabled; } set { SetProperty(ref _C6IsEnabled, value); } }
        public bool C6_CMNTIsEnabled { get { return C6IsEnabled && _C6_CMNTIsEnabled; } set { SetProperty(ref _C6_CMNTIsEnabled, value); } }
        public bool C7IsEnabled { get { return _C7IsEnabled; } set { SetProperty(ref _C7IsEnabled, value); } }
        public bool C8IsEnabled { get { return _C8IsEnabled; } set { SetProperty(ref _C8IsEnabled, value); } }
        


        private void SetControlSettings()
        {
            C5IsEnabled = C4?.AnswerCode == "1";

            C6IsEnabled = C5?.AnswerCode == "2";

            C6_CMNTIsEnabled = C6?.AnswerCode == "0";

            C7IsEnabled = C4?.AnswerCode == "1";
            C8IsEnabled = C7?.AnswerCode == "2" || C7?.AnswerCode == "3";
        }


        public override List<string> GetSkipedFields()
        {

            List<string> skippedFields = new List<string>();
            if (!C2IsEnabled)
                skippedFields.Add(nameof(C2));
            if (!C5IsEnabled)
                skippedFields.Add(nameof(C5));
            if (!C6IsEnabled)
                skippedFields.Add(nameof(C6));
            if (!C6_CMNTIsEnabled)
                skippedFields.Add(nameof(C6_Name));
            if (!C7IsEnabled)
                skippedFields.Add(nameof(C7));
            if (!C8IsEnabled)
                skippedFields.Add(nameof(C8));
            return skippedFields;
        }


        public override async Task<bool> DoSave()
        {
            bool result = false;

            try
            {



                var diffInSeconds = (DateTime.Now - _callLog.C3).TotalSeconds;
                _callLog.C9 = Convert.ToInt32(diffInSeconds);

                _callLog.C2 = C2;

                _callLog.C4 = C4.AnswerId;

                if (C5IsEnabled)
                {
                    _callLog.C5 = C5.AnswerId;
                }
                else
                {
                    _callLog.C5 = null;
                }
                if (C6IsEnabled)
                {
                    _callLog.C6 = C6.AnswerId;
                }
                else
                {
                    _callLog.C6 = null;
                }
                if (C6_CMNTIsEnabled)
                {
                    _callLog.C6_Name = C6_Name;
                }
                else
                {
                    _callLog.C6_Name = null;
                }
                if (C7IsEnabled)
                {
                    _callLog.C7 = C7.AnswerId;
                }
                else
                {
                    _callLog.C7 = null;
                }
                if (C8IsEnabled)
                {
                    _callLog.C8 = C8.AnswerId;
                }
                else
                {
                    _callLog.C8 = null;
                }


                _callLog.Comments = Comments;

                var db = await DataBase.GetAsyncConnection();
                await db.InsertOrReplaceAsync(_callLog);

                result = true;




            }

            catch
            {
                result = false;
            }


            return result;
        }




    }
}
