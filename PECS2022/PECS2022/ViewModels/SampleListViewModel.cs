using PECS2022.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PECS2022.ViewModels
{
    public class SampleListViewModel : BaseViewModel
    {

	
        public SampleListViewModel(SampleInfo sampleInfo, bool isEnabled=false)
        {
         
            IsEnabled = isEnabled;
            SetObjectToScreen(sampleInfo);
           // Title = "قائمة العينات";
        }

        private string _ID00;
        private GovernorateInfo _ID1Val;
        private Locality _ID2Val;
        private int _ID3;
        private int _ID4;
        private int _ID5;
        private int _ID6;
        private LookUpValueInfo _ID7;
        private LookUpValueInfo _QC2Val;
        private string _QC2_CMNT;
        private string _QC3_1;
        private string _QC3_2;
        private string _QC3_3;
        private string _QC4_1;
        private string _QC4_2;

        private string _IndivID;
        private string _IndivName;



        public string IndivID { get { return _IndivID; } set { SetProperty(ref _IndivID, value); } }

        public string IndivName { get { return _IndivName; } set { SetProperty(ref _IndivName, value); } }



        [Display(Name = "ID00", ResourceType = typeof(Resources.ModelResources))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        [MaxLength(8, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        public string ID00 { get { return _ID00; } set { SetProperty(ref _ID00, value); } }

        [Display(Name = "ID3", ResourceType = typeof(Resources.ModelResources))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        [Range(1, 999, ErrorMessageResourceName = "Range", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        public int ID3 { get { return _ID3; } set { SetProperty(ref _ID3, value); } }

        [Display(Name = "ID4", ResourceType = typeof(Resources.ModelResources))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        [Range(1, 999, ErrorMessageResourceName = "Range", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        public int ID4 { get { return _ID4; } set { SetProperty(ref _ID4, value); } }

        [Display(Name = "ID5", ResourceType = typeof(Resources.ModelResources))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        [Range(1, 999, ErrorMessageResourceName = "Range", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        public int ID5 { get { return _ID5; } set { SetProperty(ref _ID5, value); } }

        [Display(Name = "ID6", ResourceType = typeof(Resources.ModelResources))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        [Range(1, 100, ErrorMessageResourceName = "Range", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        public int ID6 { get { return _ID6; } set { SetProperty(ref _ID6, value); } }


        [Display(Name = "ID1", ResourceType = typeof(Resources.ModelResources))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        public GovernorateInfo ID1Val { get { return _ID1Val; } set { _ID1Val = value; ValidateProperty(value); NotifyPropertyChanged(); LoadLocalities(); } }

        [Display(Name = "ID2", ResourceType = typeof(Resources.ModelResources))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        public Locality ID2Val { get { return _ID2Val; } set { _ID2Val = value; ValidateProperty(value); NotifyPropertyChanged();  } }


        [Display(Name = "QC2", ResourceType = typeof(Resources.ModelResources))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        
        public LookUpValueInfo QC2Val { get { return _QC2Val; } set { _QC2Val = value; ValidateProperty(value); NotifyPropertyChanged(); SetModelSettings(); } }

        [Display(Name = "ID7", ResourceType = typeof(Resources.ModelResources))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ModelResources))]

        public LookUpValueInfo ID7Val { get { return _ID7; } set { _ID7 = value; ValidateProperty(value); NotifyPropertyChanged(); SetModelSettings(); } }



        [Display(Name = "QC2_CMNT", ResourceType = typeof(Resources.ModelResources))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        [MaxLength(100, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        public string QC2_CMNT { get { return _QC2_CMNT; } set { SetProperty(ref _QC2_CMNT, value); } }

        [Display(Name = "QC3_1", ResourceType = typeof(Resources.ModelResources))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        [MaxLength(100, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        public string QC3_1 { get { return _QC3_1; } set { SetProperty(ref _QC3_1, value); } }


        [Display(Name = "QC3_2", ResourceType = typeof(Resources.ModelResources))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        [MaxLength(10, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        [RegularExpression(@"^\d+$", ErrorMessageResourceName ="TELNotValid",  ErrorMessageResourceType = typeof(Resources.ModelResources))]
        public string QC3_2 { get { return _QC3_2; } set { SetProperty(ref _QC3_2, value); } }
        [Display(Name = "QC3_3", ResourceType = typeof(Resources.ModelResources))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        [MaxLength(10, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        [RegularExpression(@"^\d+$", ErrorMessageResourceName = "TELNotValid", ErrorMessageResourceType = typeof(Resources.ModelResources))]

        public string QC3_3 { get { return _QC3_3; } set { SetProperty(ref _QC3_3, value); } }


        [Display(Name = "QC4_1", ResourceType = typeof(Resources.ModelResources))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        [MaxLength(100, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Resources.ModelResources))]
       

        public string QC4_1 { get { return _QC4_1; } set { SetProperty(ref _QC4_1, value); } }


       
        [Display(Name = "QC4_2", ResourceType = typeof(Resources.ModelResources))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        [MaxLength(10, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        [RegularExpression(@"^\d+$", ErrorMessageResourceName = "TELNotValid", ErrorMessageResourceType = typeof(Resources.ModelResources))]

        public string QC4_2 { get { return _QC4_2; } set { SetProperty(ref _QC4_2, value); } }


        private List<LookUpValueInfo> _QC2List;

        public List<LookUpValueInfo> QC2List
        {
            get { return _QC2List; }
            set
            {
                SetProperty(ref _QC2List, value);
            }
        }

        private List<LookUpValueInfo> _ID7List;

        public List<LookUpValueInfo> ID7List
        {
            get { return _ID7List; }
            set
            {
                SetProperty(ref _ID7List, value);
            }
        }

        private List<GovernorateInfo> _ID1List;

        public List<GovernorateInfo> ID1List
        {
            get { return _ID1List; }
            set
            {
                SetProperty(ref _ID1List, value);
            }
        }

        private List<Locality> _ID2List;

        public List<Locality> ID2List
        {
            get { return _ID2List; }
            set
            {
                SetProperty(ref _ID2List, value);
            }
        }

        public override void SetObjectToScreen(object o)
        {
            var sample = o as SampleInfo;
            LoadSettings();
          
            if (sample.Visit != null)
            {
                ID00 = sample.ID00;
                ID1Val = ID1List.FirstOrDefault(x => x.Code == sample.ID01);
                ID2Val = ID2List.FirstOrDefault(x => x.Code == sample.ID02);
                ID3 = sample.Visit.ID03;
                ID4 = sample.Visit.ID04;
                ID5 = sample.Visit.ID05 ;
                ID6 = sample.Visit.ID06;
                ID7Val = ID7List.FirstOrDefault(x => x.AnswerId == sample.Visit.ID07);
                QC3_1 = sample.Visit.QC1_1;
                QC3_2 = sample.Visit.QC1_2;
                QC3_3 = sample.Visit.QC1_3;
                //QC4_1 = sample.Visit.QC4_1;
                //QC4_2 = sample.Visit.QC4_2;
                QC2Val = QC2List.FirstOrDefault(x => x.AnswerId == sample.Visit.QC2);
                QC2_CMNT = sample.Visit.QC2_txt;

               

            }
            else
            {
                QC2Val = null;
                QC2_CMNT = null;
                ID00 = sample.ID00;
                ID1Val = ID1List.FirstOrDefault(x => x.Code == sample.ID01);
                ID2Val = ID2List.FirstOrDefault(x => x.Code == sample.ID02);
                ID3 = sample.ID03;
                ID4 = sample.ID04 ?? 0;
                ID5 = sample.ID05 ?? 0;
                ID6 = sample.ID06 ??0;
                
                QC3_1 = sample.QC1_1;
                //QC3_2 = sample.QC1_2;
                //QC3_3 = sample.QC1_3;

              

            }
        }


        public override async Task<bool> CustomValidate()
        {
            bool isValid = true;
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("برجاء تعبئة الحقول التالية أولا");


            bool warning = false;

            StringBuilder warningString = new StringBuilder();
            warningString.AppendLine("- هل انت متاكد من عدم تعبئة الحقول التالية:");


         

            if (Util.Validator.IsNullable(QC3_2))
            {
                warning = true;

                warningString.AppendLine("- رقم تلفون أرضي");
            }
            else if (!Util.Validator.IsDigit(QC3_2))
            {
                isValid = false;

                stringBuilder.AppendLine("- رقم تلفون أرضي يجب ان يكون ارقام فقط");
            }
            else if (!Util.Validator.IsTelephone(QC3_2))
            {
                isValid = false;

                stringBuilder.AppendLine("-رقم تلفون أرضي يجب ان يكون 9  ارقام فقط ان يبدأ بأحد الارقام التالية (02,04,08,09) فقط");
            }

            if (Util.Validator.IsNullable(QC3_3))
            {
                warning = true;

                warningString.AppendLine("- رقم تلفون نقال");
            }
            else if (!Util.Validator.IsDigit(QC3_3))
            {
                isValid = false;

                stringBuilder.AppendLine("- رقم تلفون نقال يجب ان يكون ارقام فقط");
            }
            else if (!Util.Validator.IsTelephone(QC3_3))
            {
                isValid = false;

                stringBuilder.AppendLine("-رقم تلفون نقال يجب ان يكون 9  ارقام فقط ان يبدأ بأحد الارقام التالية (02,04,08,09) فقط");
            }

            if (Util.Validator.IsNullable(QC4_2))
            {
                warning = true;

                warningString.AppendLine("- رقم تلفون نقال للمعرفين عن الاسرة");
            }
            else if (!Util.Validator.IsDigit(QC4_2))
            {
                isValid = false;

                stringBuilder.AppendLine("- رقم تلفون نقال للمعرفين عن الاسرة يجب ان يكون ارقام فقط");
            }
            else if (!Util.Validator.IsTelephone(QC4_2))
            {
                isValid = false;

                stringBuilder.AppendLine("-رقم تلفون نقال للمعرفين عن الاسرة يجب ان يكون 9  ارقام فقط ان يبدأ بأحد الارقام التالية (02,04,08,09) فقط");
            }




            if (!isValid)
            {

                await Application.Current.MainPage.DisplayAlert(GeneralMessages.Error, stringBuilder.ToString(), GeneralMessages.Cancel);
            }
            else if (warning)
            {
                isValid = await Application.Current.MainPage.DisplayAlert(GeneralMessages.Question, warningString.ToString(), GeneralMessages.Yes, GeneralMessages.No);
            }

            return await Task.Run(() => isValid);
        }

        #region Settings
        private void LoadSettings()
        {
            QC2List =  LookUpManager.GetSurveyLookupById(15632);
            ID7List = LookUpManager.GetSurveyLookupById(15641);
            ID1List = ApplicationMainSettings.GovernorateList;
            LoadLocalities();
           


        }

        private void LoadLocalities()
        {
            if (ID1Val == null)
            {
                ID2List = ApplicationMainSettings.LocalityList;
            }
            else
            {
                ID2List = ApplicationMainSettings.GetLocalityByGovCode(ID1Val.Code);
            }
        }


      

        private void SetModelSettings()
        {
            if (QC2Val != null)
            {
                IsEnabledQC2_CMNT = QC2Val.NeedComments;




            }
            else
            {
                IsEnabledQC2_CMNT = false;
            }
        }


        private bool _IsEnabledQC2_CMNT;

        public bool IsEnabledQC2_CMNT { get { return _IsEnabledQC2_CMNT; } set { SetProperty(ref _IsEnabledQC2_CMNT, value); } }

        private bool _IsEnabled;
        public bool IsEnabled { get { return _IsEnabled; } set { SetProperty(ref _IsEnabled, value); } }

        #endregion





    }
}
