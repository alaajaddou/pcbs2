using PECS2022.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PECS2022.Models;
using PECS2022.Util;
using System.Globalization;

using Xamarin.Forms;

namespace PECS2022.ViewModels
{
    public class MemberViewModel : BaseViewModel
    {

        private readonly Individual _individual;
        private readonly List<Individual> Individuals;
        public MemberViewModel(Individual individual, List<Individual> individuals)
        {
            this._individual = individual;
            this.Individuals = individuals;


            SetObjectToScreen(individual);
            IsEnabled = true;

        }
        // to do
        public override void SetObjectToScreen(object o)
        {
            var indv = o as Individual;
            LoadSettings(indv);

        }

        public string _ID00;
        public int _D1;
        public string _D2;// اسم الفرد
        public LookupVal _D3; //العلاقة برب الاسرة
        public LookupVal _D4; // الجنس
        public string _D5; // تاريخ الميلاد
        public OptionInfo _D5_A;
        public OptionInfo _D5_B;
        public OptionInfo _D5_C;
        public int? _D6; // العمر
        public LookupVal _D7;//  حالة اللجوء
                             // public string _D7_CMNT;
        public LookupVal _D8;//  حالة الزواجية
        public LookupVal _D9;// تامين صحي
        public LookupVal _D10_A; // حكومي
        public LookupVal _D10_B;//وكالة
        public LookupVal _D10_C;//خاص
        public LookupVal _D10_D;//اسرائيلي
        public LookupVal _D10_E;//اخرى
                                // public string _D10_E_CMNT;
        public LookupVal _D11;
        public LookupVal _D12_A; // النظر
        public LookupVal _D12_B;//السمع
        public LookupVal _D12_C;//الحركة واستخدام الاصابع
        public LookupVal _D12_D;//التذكر والتركيز
        public LookupVal _D12_E;//التواصل
        public LookupVal _D12_F;//العناية
        public int? _D13;
        public int? _D14;
        public int? _D15;
        public LookupVal _D16;
        public LookupVal _D17;
        public string _D17_CMNT;
        public LookupVal _D18;
        public LookupVal _D19;
        public LookupVal _D20;
        public int? _D21;
        public LookupVal _D22;
        public int? _D23;
        public LookupVal _D24;
        public LookupVal _D25;
        public LookupVal _D26;
        public string _D26_CMNT;
        public LookupVal _L1;
        public string _L1_CMNT;
        public LookupVal _L2;
        public LookupVal _L3_A;
        public LookupVal _L3_B;
        public LookupVal _L3_C;
        public LookupVal _L4;
        public LookupVal _L5;
        public LookupVal _L6;
        public LookupVal _L7;
        public LookupVal _L8;
        public LookupVal _L9;
        //  public string _L9_CMNT;
        public int? _L10;
        public LookupVal _L11;

        public string _L12_Code;
        public string _L12_Desc;
        public string _L13_Code;
        public string _L13_Desc;
        //public string _D17;
        //public string _D17_Desc;
        public LookupVal _L14_A;
        public LookupVal _L14_B;
        public LookupVal _L14_C;
        public LookupVal _L14_D;
        public LookupVal _L14_E;
        //public LookUpValueInfo _D18_1;
        //public int? _D18_2;
        //public LookUpValueInfo _D18_3;

        //public LookUpValueInfo _D19;
        //public int? _D19_1;
        //public LookUpValueInfo _D19_2_1;
        //public LookUpValueInfo _D19_2_2;
        //public LookUpValueInfo _D19_2_3;
        //public LookUpValueInfo _D19_2_4;
        //public LookUpValueInfo _D19_2_5;
        //public LookUpValueInfo _D21;
        public bool _IsNew;


        [Display(Name = "ID", ResourceType = typeof(Resources.ModelResources))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        public System.Guid ID { get; set; }

        [Display(Name = "ID00", ResourceType = typeof(Resources.ModelResources))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        [MaxLength(12, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        public string ID00 { get { return _ID00; } set { SetProperty(ref _ID00, value); ValidateProperty(value); NotifyPropertyChanged(); } }

        [Display(Name = "D1", ResourceType = typeof(Resources.ModelResources))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        public int D1 { get { return _D1; } set { SetProperty(ref _D1, value); } }

        [Display(Name = "D2", ResourceType = typeof(Resources.ModelResources))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        [MaxLength(50, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        [ArabicOnly(ErrorMessageResourceName = "ArabicOnly", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        public string D2 { get { return _D2; } set { SetProperty(ref _D2, value); ValidateProperty(value); NotifyPropertyChanged(); } }

        [Display(Name = "D3", ResourceType = typeof(Resources.ModelResources))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        [CustomFunction(MethodName: nameof(ValidateD3), ErrorMessageResourceName = "InValidValue", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        public LookupVal D3 { get { return _D3; } set { SetProperty(ref _D3, value); D3Changed(); ValidateProperty(value); NotifyPropertyChanged(); } }

        public bool ValidateD11(out string msg)
        {
            msg = string.Empty;
            bool valid = true;
            if (D11 == null && D11.Code == "-1")
            {
                msg = "الرجاء اختيار اجابة";
                valid = false;
            }
            return valid;
        }
        public bool ValidateD12_A(out string msg)
        {
            msg = string.Empty;
            bool valid = true;
            if (D12_A != null && D12_A.Code == "-1")
            {
                msg = "الرجاء اختيار اجابة";
                valid = false;
            }
            return valid;
        }
        public bool ValidateD12_B(out string msg)
        {
            msg = string.Empty;
            bool valid = true;
            if (D12_B != null && D12_B.Code == "-1")
            {
                msg = "الرجاء اختيار اجابة";
                valid = false;
            }
            return valid;
        }
        public bool ValidateD12_C(out string msg)
        {
            msg = string.Empty;
            bool valid = true;
            if (D12_C != null && D12_C.Code == "-1")
            {
                msg = "الرجاء اختيار اجابة";
                valid = false;
            }
            return valid;
        }

        public bool ValidateD12_D(out string msg)
        {
            msg = string.Empty;
            bool valid = true;
            if (D12_D != null && D12_D.Code == "-1")
            {
                msg = "الرجاء اختيار اجابة";
                valid = false;
            }
            return valid;
        }
        public bool ValidateD12_E(out string msg)
        {
            msg = string.Empty;
            bool valid = true;
            if (D12_E != null && D12_E.Code == "-1")
            {
                msg = "الرجاء اختيار اجابة";
                valid = false;
            }
            return valid;
        }
        public bool ValidateD12_F(out string msg)
        {
            msg = string.Empty;
            bool valid = true;
            if (D12_F != null && D12_F.Code == "-1")
            {
                msg = "الرجاء اختيار اجابة";
                valid = false;
            }
            return valid;
        }
        public bool ValidateD5_C(out string msg)
        {
            msg = string.Empty;
            bool valid = true;
            if (D5_C == null && D5_C.Id == 0)
            {
                msg = "الرجاء اختيار اجابة";
                valid = false;
            }
            return valid;
        }
        public bool ValidateD5_B(out string msg)
        {
            msg = string.Empty;
            bool valid = true;
            if (D5_B == null && D5_B.Id == 0)
            {
                msg = "الرجاء اختيار اجابة";
                valid = false;
            }
            return valid;
        }
        public bool ValidateD5_A(out string msg)
        {
            msg = string.Empty;
            bool valid = true;
            if (D5_A == null && D5_A.Id == 0)
            {
                msg = "الرجاء اختيار اجابة";
                valid = false;
            }
            return valid;
        }
        public bool ValidateD3(out string msg)
        {

            msg = string.Empty;
            bool valid = true;

            if (D3 == null) return true;

            if (D3 != null && D3.Code == "-1")
            {
                msg = "الرجاء اختيار اجابة";
                valid = false;
            }
            if (D1 == 1 && D3.Code != "1")
            {
                msg = "الفرد الاول في الاسرة يجب ان  يكون رب الاسرة";
                valid = false;
            }

            var d1 = Individuals.Where(i => i.D3 == 1 && i.D1 < _individual.D1).ToList();
            if (d1.Count() > 0 && D3.Code == "1")
            {
                msg = "لا يمكن تكرار رب الاسرة";
                valid = false;
            }
            if (D3 != null && (D3.Code == "3" || D3.Code == "7" || D3.Code == "2" || D3.Code == "8"))
            {
                if (d1 != null && d1.Count >= 1 && (d1[0].D8 == 1 || d1[0].D8 == 1 || d1[0].D8 == 2))
                {
                    msg = "رب الاسرة لم يتزوج ولا يجوز ان يكون له ابن/ابنة حفيد/حفيدة زوجة ابن/زوج ابن";
                    valid = false;
                }
            }
            return valid;


        }

        [Display(Name = "D4", ResourceType = typeof(Resources.ModelResources))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        [CustomFunction(MethodName: nameof(ValidateD4), ErrorMessageResourceName = "InValidValue", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        public LookupVal D4 { get { return _D4; } set { SetProperty(ref _D4, value); D4Changed(); ValidateProperty(value); NotifyPropertyChanged(); D4Changed(); } }

        [Display(Name = "D5", ResourceType = typeof(Resources.ModelResources))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        // [Range(0, 99, ErrorMessageResourceName = "Range", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        //    [CustomFunction(MethodName: nameof(ValidateD5), ErrorMessageResourceName = "InValidValue", ErrorMessageResourceType = typeof(Resources.ModelResources))]

        public string D5 { get { return _D5; } set { SetProperty(ref _D5, value); ValidateProperty(value); NotifyPropertyChanged(); D5Changed(); } }

        [Display(Name = "D5_A", ResourceType = typeof(Resources.ModelResources))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        // [Range(1, 31, ErrorMessageResourceName = "Range", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        [CustomFunction(MethodName: nameof(ValidateD5_A), ErrorMessageResourceName = "InValidValue", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        public OptionInfo D5_A { get { return _D5_A; } set { SetProperty(ref _D5_A, value); ValidateProperty(value); NotifyPropertyChanged(); D5_AChanged(); } }

        [Display(Name = "D5_B", ResourceType = typeof(Resources.ModelResources))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        // [Range(1, 12, ErrorMessageResourceName = "Range", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        [CustomFunction(MethodName: nameof(ValidateD5_B), ErrorMessageResourceName = "InValidValue", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        public OptionInfo D5_B { get { return _D5_B; } set { SetProperty(ref _D5_B, value); ValidateProperty(value); NotifyPropertyChanged(); D5_BChanged(); } }

        [Display(Name = "D5_C", ResourceType = typeof(Resources.ModelResources))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        // [Range(0, 9999, ErrorMessageResourceName = "Range", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        [CustomFunction(MethodName: nameof(ValidateD5_C), ErrorMessageResourceName = "InValidValue", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        public OptionInfo D5_C { get { return _D5_C; } set { SetProperty(ref _D5_C, value); ValidateProperty(value); NotifyPropertyChanged(); D5_CChanged(); } }


        [Display(Name = "D6", ResourceType = typeof(Resources.ModelResources))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        [Range(0, 99, ErrorMessageResourceName = "Range", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        [CustomFunction(MethodName: nameof(ValidateD6), ErrorMessageResourceName = "InValidValue", ErrorMessageResourceType = typeof(Resources.ModelResources))]

        public int? D6 { get { return _D6; } set { SetProperty(ref _D6, value); ValidateProperty(value); NotifyPropertyChanged(); SetControlStatus(); } }

        private void D3Changed()
        {
            string s = "";
            ValidateD6(out s);
        }
        public bool ValidateD6(out string msg)
        {
            msg = string.Empty;
            bool valid = true;

            if (D6 != null && D3 != null)
            {
                if (D3.Code == "1" && D6 < 14)
                {
                    msg = "عمر رب الاسرة يجب ان يكون  اكبر أو يساوي  14";
                    valid = false;
                }
                else if (D3.Code == "8" && D6 < 14)
                {
                    msg = " زوجة ابن/ زوج بنت والعمر أقل من 14 سنة  ";
                    valid = false;
                }
                else if (D3.Code == "2" && D6 < 14)
                {
                    msg = "زوجة رب الأسرة والعمر أقل من 14 سنة";
                    valid = false;
                }

                else if (D3.Code == "4" && D6 < 30)
                {
                    msg = "عمر أب/ أم لرب الأسرة وعمره أقل من 30 سنة";
                    valid = false;
                }


                else if (D3.Code == "6" && D6 < 40)
                {
                    msg = "عمر جد/ جدة لرب الأسرة وعمره أقل من 40 سنة";
                    valid = false;
                }
                else if (D3.Code == "3")
                {

                    int? d1Age = Individuals.Where(x => x.D3 == 1).FirstOrDefault()?.D6;
                    if (d1Age != null)
                    {

                        if (d1Age - D6 < 14)
                        {
                            msg = "الفرق بالعمر بين الابن/البنت وبين رب الأسرة أقل من 14 سنة  ";
                            valid = false;
                        }

                    }

                    else
                    {
                        var d3Age = Individuals.Where(x => x.D3 == 3 && x.D6 == D6 && x.D1 != _individual.D1).FirstOrDefault();

                        if (d3Age != null)
                        {


                            msg = $"أخوة ضمن الاسرة  والفرق في العمر أقل من سنة - {d3Age.D2}";
                        }
                    }
                }

                else if (D3.Code == "7")
                {

                    int? d1Age = Individuals.Where(x => x.D3 == 1).FirstOrDefault()?.D6;
                    if (d1Age != null)
                    {

                        if (d1Age - D6 < 40)
                        {
                            msg = "الفرق بالعمر بين حفيد/حفيدة وبين رب الأسرة أقل من 40 سنة  ";
                            valid = false;
                        }

                    }
                }

                else if (D3.Code == "4")
                {
                    int? d1Age = Individuals.Where(x => x.D3 == 1).FirstOrDefault()?.D6;
                    if (d1Age != null)
                    {

                        if (D6 - d1Age < 14)
                        {
                            msg = "الفرق بالعمر بين أب/أم وبين رب الأسرة أقل من 14 سنة";
                            valid = false;
                        }

                    }
                }
            }


            return valid;
        }

        //[Display(Name = "D6", ResourceType = typeof(Resources.ModelResources))]
        //[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        ////[CustomFunction(MethodName: nameof(ValidateD6), ErrorMessageResourceName = "InValidValue", ErrorMessageResourceType = typeof(Resources.ModelResources))]

        //public LookUpValueInfo D6 { get { return _D6; } set { SetProperty(ref _D6, value); ValidateProperty(value); NotifyPropertyChanged(); } }

        public bool ValidateD7(out string msg)
        {
            msg = string.Empty;
            bool valid = true;
            //&& x.D4 == 1
            var d1 = Individuals.Where(x => x.D3 == 1).FirstOrDefault();
            var d7 = D7.Code;
            if (D7 != null && D7.Code == "-1")
            {
                msg = "الرجاء اختيار اجابة ";
                valid = false;
            }
            if (D3.Code == "3")
            {
                if (d1 != null && (d1.D7 == 1 || d1.D7 == 2) && D7.Code == "3")
                {
                    msg = "رب الاسرة (الأب) لاجئ مسجل أو لاجئ غير مسجل والفرد ليس لاجئ ";
                    valid = false;
                }
            }
            else
            {
                msg = null;
                valid = true;
            }

            return valid;

        }
        [Display(Name = "D7", ResourceType = typeof(Resources.ModelResources))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        [CustomFunction(MethodName: nameof(ValidateD7), ErrorMessageResourceName = "InValidValue", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        public LookupVal D7 { get { return _D7; } set { SetProperty(ref _D7, value); ValidateProperty(value); NotifyPropertyChanged(); } }

        public bool ValidateD9(out string msg)
        {
            bool valid = true;
            msg = string.Empty;
            if (D9 != null && D9.Code == "-1")
            {
                msg = "الرجاء اختيار اجابة ";
                valid = false;
            }
            if (D7 != null && D7.Code == "1" && D9 != null && D9.Code == "2")
            {

                msg = "فلسطيني ولاجئ مسجل وليس لديه تامين وكالة ";
                valid = false;

            }

            return valid;
        }

        //[Display(Name = "D7_CMNT", ResourceType = typeof(Resources.ModelResources))]
        //[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        //[MaxLength(50, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        //public string D7_CMNT { get { return _D7_CMNT; } set { SetProperty(ref _D7_CMNT, value); ValidateProperty(value); NotifyPropertyChanged(); } }

        [Display(Name = "D8", ResourceType = typeof(Resources.ModelResources))]
        //   [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        [CustomFunction(MethodName: nameof(ValidateD8), ErrorMessageResourceName = "InValidValue", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        public LookupVal D8 { get { return _D8; } set { SetProperty(ref _D8, value); ValidateProperty(value); NotifyPropertyChanged(); SetControlStatus(); } }


        [Display(Name = "D9", ResourceType = typeof(Resources.ModelResources))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        [CustomFunction(MethodName: nameof(ValidateD9), ErrorMessageResourceName = "InValidValue", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        public LookupVal D9 { get { return _D9; } set { SetProperty(ref _D9, value); D9Changed(); ValidateProperty(value); NotifyPropertyChanged(); SetControlStatus(); } }

        [Display(Name = "D10_A", ResourceType = typeof(Resources.ModelResources))]
        //[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        [CustomFunction(MethodName: nameof(ValidateD10_A), ErrorMessageResourceName = "InValidValue", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        public LookupVal D10_A { get { return _D10_A; } set { SetProperty(ref _D10_A, value); D10Changed(); ValidateProperty(value); NotifyPropertyChanged(); } }

        [Display(Name = "D10_B", ResourceType = typeof(Resources.ModelResources))]
        //   [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        [CustomFunction(MethodName: nameof(ValidateD10B), ErrorMessageResourceName = "InValidValue", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        public LookupVal D10_B { get { return _D10_B; } set { SetProperty(ref _D10_B, value); D10Changed(); ValidateProperty(value); NotifyPropertyChanged(); } }

        [Display(Name = "D10_C", ResourceType = typeof(Resources.ModelResources))]
        //    [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        [CustomFunction(MethodName: nameof(ValidateD10_C), ErrorMessageResourceName = "InValidValue", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        public LookupVal D10_C { get { return _D10_C; } set { SetProperty(ref _D10_C, value); D10Changed(); ValidateProperty(value); NotifyPropertyChanged(); } }

        [Display(Name = "D10_D", ResourceType = typeof(Resources.ModelResources))]
        //   [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        [CustomFunction(MethodName: nameof(ValidateD10_D), ErrorMessageResourceName = "InValidValue", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        public LookupVal D10_D { get { return _D10_D; } set { SetProperty(ref _D10_D, value); D10Changed(); ValidateProperty(value); NotifyPropertyChanged(); } }

        [Display(Name = "D10_E", ResourceType = typeof(Resources.ModelResources))]
        //   [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        [CustomFunction(MethodName: nameof(ValidateD10_E), ErrorMessageResourceName = "InValidValue", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        public LookupVal D10_E { get { return _D10_E; } set { SetProperty(ref _D10_E, value); D10Changed(); ValidateProperty(value); NotifyPropertyChanged(); } }

        //[Display(Name = "D10_E_CMNT", ResourceType = typeof(Resources.ModelResources))]
        //[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        //public string D10_E_CMNT { get { return _D10_E_CMNT; } set { SetProperty(ref _D10_E_CMNT, value); ValidateProperty(value); NotifyPropertyChanged(); } }


        public bool ValidateD10B(out string msg)
        {
            msg = string.Empty;
            bool valid = true;
            if (D10_B != null && D10_B.Code == "-1")
            {
                msg = "الرجاء اختيار اجابة ";
                valid = false;
            }

            if (D7 != null && D7.Code == "1" && D9 != null && D9.Code == "1" && D10_B != null && D10_B.Code == "2")
            {
                msg = "فلسطيني ولاجئ وليس لديه تامين  وكالة ";
                valid = false;
            }
            return valid;
        }
        public bool ValidateD16(out string msg)
        {
            msg = string.Empty;
            bool valid = true;

            if (D16 != null && D16.Code == "-1")
            {
                msg = "الرجاء اختيار اجابة ";
                valid = false;
            }
            if (D6 > 8 && D16 != null && D16.Code == "0")
            {
                msg = "ملتحق في رياض الاطفال وعمره اكبر من 8 سنوات";
                valid = false;
            }

            else if (D6 < 4 && D16 != null && D16.Code != "0" && D16.Code != "4")
            {
                msg = "العمر لا يتناسب مع الالتحاق بالتعليم";
                valid = false;

            }

            else if (D6 == 5 && D16 != null && D16.Code == "2")
            {
                msg = "العمر لا يتناسب مع الالتحاق بالتعليم";
                valid = false;

            }


            return valid;
        }

        [Display(Name = "D11", ResourceType = typeof(Resources.ModelResources))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        // [Range(0, 31, ErrorMessageResourceName = "Range", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        [CustomFunction(MethodName: nameof(ValidateD11), ErrorMessageResourceName = "InValidValue", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        public LookupVal D11 { get { return _D11; } set { SetProperty(ref _D11, value); ValidateProperty(value); NotifyPropertyChanged(); } }
        //public bool ValidateD11(out string msg)
        //{
        //    bool valid = true;
        //    msg = string.Empty;

        //    if (D11.HasValue)
        //    {
        //        if (D11 < 0 || D11 > 31)
        //        {
        //            msg="عدد سنوات الدراسة يجب ان تكون رقم بين 0 و 31";
        //            valid = false;
        //        }

        //        else if (D5 - D11 < 4)
        //        {
        //            msg="الفرق بين العمر وعدد سنوات الدراسة أقل من 5 سنوات";
        //            valid = false;
        //        }



        //    }


        //    return valid;
        //}


        [Display(Name = "D12_A", ResourceType = typeof(Resources.ModelResources))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        [CustomFunction(MethodName: nameof(ValidateD12_A), ErrorMessageResourceName = "InValidValue", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        public LookupVal D12_A { get { return _D12_A; } set { SetProperty(ref _D12_A, value); ValidateProperty(value); NotifyPropertyChanged(); } }

        [Display(Name = "D12_B", ResourceType = typeof(Resources.ModelResources))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        [CustomFunction(MethodName: nameof(ValidateD12_B), ErrorMessageResourceName = "InValidValue", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        public LookupVal D12_B { get { return _D12_B; } set { SetProperty(ref _D12_B, value); ValidateProperty(value); NotifyPropertyChanged(); } }

        [Display(Name = "D12_C", ResourceType = typeof(Resources.ModelResources))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        [CustomFunction(MethodName: nameof(ValidateD12_C), ErrorMessageResourceName = "InValidValue", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        public LookupVal D12_C { get { return _D12_C; } set { SetProperty(ref _D12_C, value); ValidateProperty(value); NotifyPropertyChanged(); } }

        [Display(Name = "D12_D", ResourceType = typeof(Resources.ModelResources))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        [CustomFunction(MethodName: nameof(ValidateD12_D), ErrorMessageResourceName = "InValidValue", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        public LookupVal D12_D { get { return _D12_D; } set { SetProperty(ref _D12_D, value); ValidateProperty(value); NotifyPropertyChanged(); } }


        [Display(Name = "D12_E", ResourceType = typeof(Resources.ModelResources))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        [CustomFunction(MethodName: nameof(ValidateD12_E), ErrorMessageResourceName = "InValidValue", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        public LookupVal D12_E { get { return _D12_E; } set { SetProperty(ref _D12_E, value); ValidateProperty(value); NotifyPropertyChanged(); } }

        [Display(Name = "D12_F", ResourceType = typeof(Resources.ModelResources))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        [CustomFunction(MethodName: nameof(ValidateD12_F), ErrorMessageResourceName = "InValidValue", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        public LookupVal D12_F { get { return _D12_F; } set { SetProperty(ref _D12_F, value); ValidateProperty(value); NotifyPropertyChanged(); } }

        private void D9Changed()
        {
            if (D9 != null)
            {
                if (D9.Id == 2)
                {
                    D10_A = D10_B = D10_C = D10_D = D10_E = null;
                    SetControlStatus();
                }
            }
        }



        [Display(Name = "D13", ResourceType = typeof(Resources.ModelResources))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        [Range(14, 99, ErrorMessageResourceName = "Range", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        public int? D13 { get { return _D13; } set { SetProperty(ref _D13, value); D13Changed(); ValidateProperty(value); NotifyPropertyChanged(); } }


        [Display(Name = "D14", ResourceType = typeof(Resources.ModelResources))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        [Range(0, 9, ErrorMessageResourceName = "Range", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        public int? D14 { get { return _D14; } set { SetProperty(ref _D14, value); D14Changed(); } }

        [Display(Name = "D15", ResourceType = typeof(Resources.ModelResources))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        [Range(0, 99, ErrorMessageResourceName = "Range", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        public int? D15 { get { return _D15; } set { SetProperty(ref _D15, value); } }

        [Display(Name = "D16", ResourceType = typeof(Resources.ModelResources))]
        // [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        [CustomFunction(MethodName: nameof(ValidateD16), ErrorMessageResourceName = "InValidValue", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        public LookupVal D16 { get { return _D16; } set { SetProperty(ref _D16, value); D16Changed(); ValidateProperty(value); NotifyPropertyChanged(); } }

        [Display(Name = "D17", ResourceType = typeof(Resources.ModelResources))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        [CustomFunction(MethodName: nameof(ValidateD17), ErrorMessageResourceName = "InValidValue", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        public LookupVal D17 { get { return _D17; } set { SetProperty(ref _D17, value); D17Changed(); ValidateProperty(value); NotifyPropertyChanged(); SetControlStatus(); } }


        [Display(Name = "D17_CMNT", ResourceType = typeof(Resources.ModelResources))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        [ArabicOnly(ErrorMessageResourceName = "ArabicOnly", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        [MaxLength(50, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        public string D17_CMNT { get { return _D17_CMNT; } set { SetProperty(ref _D17_CMNT, value); } }

        [Display(Name = "D26_CMNT", ResourceType = typeof(Resources.ModelResources))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        [ArabicOnly(ErrorMessageResourceName = "ArabicOnly", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        [MaxLength(50, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        public string D26_CMNT { get { return _D26_CMNT; } set { SetProperty(ref _D26_CMNT, value); } }

        [Display(Name = "D18", ResourceType = typeof(Resources.ModelResources))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        [CustomFunction(MethodName: nameof(ValidateD18), ErrorMessageResourceName = "InValidValue", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        public LookupVal D18 { get { return _D18; } set { SetProperty(ref _D18, value); D18Changed(); ValidateProperty(value); NotifyPropertyChanged(); } }

        [Display(Name = "D19", ResourceType = typeof(Resources.ModelResources))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        [CustomFunction(MethodName: nameof(ValidateD19), ErrorMessageResourceName = "InValidValue", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        public LookupVal D19 { get { return _D19; } set { SetProperty(ref _D19, value); D19Changed(); ValidateProperty(value); NotifyPropertyChanged(); } }

        [Display(Name = "D20", ResourceType = typeof(Resources.ModelResources))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        [CustomFunction(MethodName: nameof(ValidateD20), ErrorMessageResourceName = "InValidValue", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        public LookupVal D20 { get { return _D20; } set { SetProperty(ref _D20, value); ValidateProperty(value); NotifyPropertyChanged(); } }

        [Display(Name = "D21", ResourceType = typeof(Resources.ModelResources))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        [Range(1, 99, ErrorMessageResourceName = "Range", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        public int? D21 { get { return _D21; } set { SetProperty(ref _D21, value); } }

        [Display(Name = "D22", ResourceType = typeof(Resources.ModelResources))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        [CustomFunction(MethodName: nameof(ValidateD22), ErrorMessageResourceName = "InValidValue", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        public LookupVal D22 { get { return _D22; } set { SetProperty(ref _D22, value); ValidateProperty(value); NotifyPropertyChanged(); } }

        [Display(Name = "D23", ResourceType = typeof(Resources.ModelResources))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        [Range(1, 99, ErrorMessageResourceName = "Range", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        [CustomFunction(MethodName: nameof(ValidateD23), ErrorMessageResourceName = "InValidValue", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        public int? D23 { get { return _D23; } set { SetProperty(ref _D23, value); } }


        [Display(Name = "D24", ResourceType = typeof(Resources.ModelResources))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        [CustomFunction(MethodName: nameof(ValidateD24), ErrorMessageResourceName = "InValidValue", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        public LookupVal D24 { get { return _D24; } set { SetProperty(ref _D24, value); ValidateProperty(value); NotifyPropertyChanged(); } }


        [Display(Name = "D25", ResourceType = typeof(Resources.ModelResources))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        [CustomFunction(MethodName: nameof(ValidateD25), ErrorMessageResourceName = "InValidValue", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        public LookupVal D25 { get { return _D25; } set { SetProperty(ref _D25, value); D25Changed(); ValidateProperty(value); NotifyPropertyChanged(); } }


        [Display(Name = "D26", ResourceType = typeof(Resources.ModelResources))]
        // [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        [CustomFunction(MethodName: nameof(ValidateD26), ErrorMessageResourceName = "InValidValue", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        public LookupVal D26 { get { return _D26; } set { SetProperty(ref _D26, value); D26Changed(); ValidateProperty(value); NotifyPropertyChanged(); } }


        [Display(Name = "L1", ResourceType = typeof(Resources.ModelResources))]
        //[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        [CustomFunction(MethodName: nameof(ValidateL1), ErrorMessageResourceName = "InValidValue", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        public LookupVal L1 { get { return _L1; } set { SetProperty(ref _L1, value); L1Changed(); ValidateProperty(value); NotifyPropertyChanged(); SetControlStatus(); L1Changed(); } }

        [Display(Name = "L1_CMNT", ResourceType = typeof(Resources.ModelResources))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        [MaxLength(50, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        [ArabicOnly(ErrorMessageResourceName = "ArabicOnly", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        public string L1_CMNT { get { return _L1_CMNT; } set { SetProperty(ref _L1_CMNT, value); ValidateProperty(value); NotifyPropertyChanged(); } }

        [Display(Name = "L2", ResourceType = typeof(Resources.ModelResources))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        [CustomFunction(MethodName: nameof(ValidateL2), ErrorMessageResourceName = "InValidValue", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        public LookupVal L2 { get { return _L2; } set { SetProperty(ref _L2, value); L2Changed(); ValidateProperty(value); NotifyPropertyChanged(); SetControlStatus(); L2Changed(); } }

        [Display(Name = "L3_A", ResourceType = typeof(Resources.ModelResources))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        [CustomFunction(MethodName: nameof(ValidateL3_A), ErrorMessageResourceName = "InValidValue", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        public LookupVal L3_A { get { return _L3_A; } set { SetProperty(ref _L3_A, value); L3Changed(); ValidateProperty(value); NotifyPropertyChanged(); SetControlStatus(); } }

        [Display(Name = "L3_B", ResourceType = typeof(Resources.ModelResources))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        [CustomFunction(MethodName: nameof(ValidateL3_B), ErrorMessageResourceName = "InValidValue", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        public LookupVal L3_B { get { return _L3_B; } set { SetProperty(ref _L3_B, value); L3Changed(); ValidateProperty(value); NotifyPropertyChanged(); SetControlStatus(); } }

        [Display(Name = "L3_C", ResourceType = typeof(Resources.ModelResources))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        [CustomFunction(MethodName: nameof(ValidateL3_C), ErrorMessageResourceName = "InValidValue", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        public LookupVal L3_C { get { return _L3_C; } set { SetProperty(ref _L3_C, value); L3Changed(); ValidateProperty(value); NotifyPropertyChanged(); SetControlStatus(); } }

        [Display(Name = "L4", ResourceType = typeof(Resources.ModelResources))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        [CustomFunction(MethodName: nameof(ValidateL4), ErrorMessageResourceName = "InValidValue", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        public LookupVal L4 { get { return _L4; } set { SetProperty(ref _L4, value); ValidateProperty(value); L4Changed(); NotifyPropertyChanged(); SetControlStatus(); } }


        [Display(Name = "L5", ResourceType = typeof(Resources.ModelResources))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        [CustomFunction(MethodName: nameof(ValidateL5), ErrorMessageResourceName = "InValidValue", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        public LookupVal L5 { get { return _L5; } set { SetProperty(ref _L5, value); L5Changed(); ValidateProperty(value); NotifyPropertyChanged(); SetControlStatus(); } }


        [Display(Name = "L6", ResourceType = typeof(Resources.ModelResources))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        [CustomFunction(MethodName: nameof(ValidateL6), ErrorMessageResourceName = "InValidValue", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        public LookupVal L6 { get { return _L6; } set { SetProperty(ref _L6, value); ValidateProperty(value); NotifyPropertyChanged(); SetControlStatus(); } }

        [Display(Name = "L7", ResourceType = typeof(Resources.ModelResources))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        [CustomFunction(MethodName: nameof(ValidateL7), ErrorMessageResourceName = "InValidValue", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        public LookupVal L7 { get { return _L7; } set { SetProperty(ref _L7, value); ValidateProperty(value); NotifyPropertyChanged(); SetControlStatus(); } }

        [Display(Name = "L8", ResourceType = typeof(Resources.ModelResources))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        [CustomFunction(MethodName: nameof(ValidateL8), ErrorMessageResourceName = "InValidValue", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        public LookupVal L8 { get { return _L8; } set { SetProperty(ref _L8, value); ValidateProperty(value); NotifyPropertyChanged(); SetControlStatus(); } }

        [Display(Name = "L9", ResourceType = typeof(Resources.ModelResources))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        [CustomFunction(MethodName: nameof(ValidateL9), ErrorMessageResourceName = "InValidValue", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        public LookupVal L9 { get { return _L9; } set { SetProperty(ref _L9, value); ValidateProperty(value); NotifyPropertyChanged(); SetControlStatus(); } }

        //[Display(Name = "L9_CMNT", ResourceType = typeof(Resources.ModelResources))]
        //[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        //[MaxLength(50, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        //[ArabicOnly(ErrorMessageResourceName = "ArabicOnly", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        //public string L9_CMNT { get { return _L9_CMNT; } set { SetProperty(ref _L9_CMNT, value); ValidateProperty(value); NotifyPropertyChanged(); } }

        [Display(Name = "L10", ResourceType = typeof(Resources.ModelResources))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        [Range(1, 12, ErrorMessageResourceName = "Range", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        public int? L10 { get { return _L10; } set { SetProperty(ref _L10, value); ValidateProperty(value); NotifyPropertyChanged(); SetControlStatus(); } }

        [Display(Name = "L11", ResourceType = typeof(Resources.ModelResources))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        [CustomFunction(MethodName: nameof(ValidateL11), ErrorMessageResourceName = "InValidValue", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        public LookupVal L11 { get { return _L11; } set { SetProperty(ref _L11, value); ValidateProperty(value); NotifyPropertyChanged(); SetControlStatus(); } }

        //[Display(Name = "L12_Code", ResourceType = typeof(Resources.ModelResources))]
        //[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        //public string L12_Code { get { return _L12_Code; } set { SetProperty(ref _L12_Code, value); ValidateProperty(value); NotifyPropertyChanged(); } }

        [Display(Name = "L12_Desc", ResourceType = typeof(Resources.ModelResources))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        [MaxLength(50, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        [ArabicOnly(ErrorMessageResourceName = "ArabicOnly", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        public string L12_Desc { get { return _L12_Desc; } set { SetProperty(ref _L12_Desc, value); ValidateProperty(value); NotifyPropertyChanged(); } }

        //[Display(Name = "L13_Code", ResourceType = typeof(Resources.ModelResources))]
        //[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        //public string L13_Code { get { return _L13_Code; } set { SetProperty(ref _L13_Code, value); ValidateProperty(value); NotifyPropertyChanged(); } }

        [Display(Name = "L13_Desc", ResourceType = typeof(Resources.ModelResources))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        [MaxLength(50, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        [ArabicOnly(ErrorMessageResourceName = "ArabicOnly", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        public string L13_Desc { get { return _L13_Desc; } set { SetProperty(ref _L13_Desc, value); ValidateProperty(value); NotifyPropertyChanged(); } }


        [Display(Name = "L14_A", ResourceType = typeof(Resources.ModelResources))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        [CustomFunction(MethodName: nameof(ValidateL14_A), ErrorMessageResourceName = "InValidValue", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        public LookupVal L14_A { get { return _L14_A; } set { SetProperty(ref _L14_A, value); ValidateProperty(value); NotifyPropertyChanged(); SetControlStatus(); } }


        [Display(Name = "L14_B", ResourceType = typeof(Resources.ModelResources))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        [CustomFunction(MethodName: nameof(ValidateL14_B), ErrorMessageResourceName = "InValidValue", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        public LookupVal L14_B { get { return _L14_B; } set { SetProperty(ref _L14_B, value); ValidateProperty(value); NotifyPropertyChanged(); SetControlStatus(); } }

        [Display(Name = "L14_C", ResourceType = typeof(Resources.ModelResources))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        [CustomFunction(MethodName: nameof(ValidateL14_C), ErrorMessageResourceName = "InValidValue", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        public LookupVal L14_C { get { return _L14_C; } set { SetProperty(ref _L14_C, value); ValidateProperty(value); NotifyPropertyChanged(); SetControlStatus(); } }

        [Display(Name = "L14_D", ResourceType = typeof(Resources.ModelResources))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        [CustomFunction(MethodName: nameof(ValidateL14_D), ErrorMessageResourceName = "InValidValue", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        public LookupVal L14_D { get { return _L14_D; } set { SetProperty(ref _L14_D, value); ValidateProperty(value); NotifyPropertyChanged(); SetControlStatus(); } }

        [Display(Name = "L14_E", ResourceType = typeof(Resources.ModelResources))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        [CustomFunction(MethodName: nameof(ValidateL14_E), ErrorMessageResourceName = "InValidValue", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        public LookupVal L14_E { get { return _L14_E; } set { SetProperty(ref _L14_E, value); ValidateProperty(value); NotifyPropertyChanged(); SetControlStatus(); } }

        //    public string D17_Desc { get { return _D17_Desc; } set { SetProperty(ref _D17_Desc, value); } }
        //[Display(Name = "D18", ResourceType = typeof(Resources.ModelResources))]
        //[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        //[CustomFunction(MethodName: nameof(ValidateD18), ErrorMessageResourceName = "InValidValue", ErrorMessageResourceType = typeof(Resources.ModelResources))]

        //public LookUpValueInfo D18 { get { return _D18; } set { SetProperty(ref _D18, value); } }

        //[Display(Name = "D18_1", ResourceType = typeof(Resources.ModelResources))]
        //[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        //public LookUpValueInfo D18_02_1 { get { return _D18_1; } set { SetProperty(ref _D18_1, value); } }

        //[Display(Name = "D18_2", ResourceType = typeof(Resources.ModelResources))]
        //[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        //[Range(1, 99999, ErrorMessageResourceName = "Range", ErrorMessageResourceType = typeof(Resources.ModelResources))]

        //public int? D18_02_2 { get { return _D18_2; } set { SetProperty(ref _D18_2, value); } }

        //[Display(Name = "D18_3", ResourceType = typeof(Resources.ModelResources))]
        //[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ModelResources))]

        //public LookUpValueInfo D18_02_3 { get { return _D18_3; } set { SetProperty(ref _D18_3, value); } }


        //public bool ValidateD18(out string msg)
        //{
        //    bool valid = true;
        //    msg = string.Empty;

        //    var d18 = D18?.AnswerCode;
        //    var d15 = D15?.AnswerCode;
        //    var d14 = D14?.AnswerCode;
        //    if (d15 == "1" && d18 != "1")
        //    {
        //       msg="القطاع  الذي يعمل فيه لايتناسب مع مكان العمل";
        //        valid = false;

        //    }

        //    else if ((d15.ToInt() >= 5 && d15.ToInt() <= 7) && (d18 != "3" && d18 != "4"))
        //    {
        //        msg= "القطاع  الذي يعمل فيه لايتناسب مع مكان العمل";
        //        valid = false;

        //    }
        //    if ((d14 == "1" || d14 == "2" || d14 == "3") && d18.ToInt() > 4)
        //    {
        //        msg="القطاع  الذي يعمل فيه لايتناسب مع  الحالة العملية الرئيسة";
        //        valid = false;
        //    }

        //    return valid;

        //}


        //[Display(Name = "D21", ResourceType = typeof(Resources.ModelResources))]
        //[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        //[CustomFunction(MethodName: nameof(ValidateD21), ErrorMessageResourceName = "InValidValue", ErrorMessageResourceType = typeof(Resources.ModelResources))]

        //public LookUpValueInfo D21 { get { return _D21; } set { SetProperty(ref _D21, value); } }


        public bool ValidateD18(out string msg)
        {
            msg = string.Empty;
            bool valid = true;
            if (D18 != null && D18.Code == "-1")
            {

                msg = "الرجاء اختيار اجابة ";
                valid = false;
            }





            if (D16 != null && (D16.Code == "4" || D16.Code == "5"))
            {
                if (D18 != null && D18.Code == "0")
                {
                    valid = true;
                }
                else
                {
                    msg = "لا يوجد توافق بين الالتحاق بالتعليم والمستوى التعليمي";
                    valid = false;
                }

            }

            else if (D16 != null && (D16.Code == "3"))
            {
                if (D18 != null && (D18.Code == "1" || D18.Code == "2" || D18.Code == "3" || D18.Code == "4" || D18.Code == "5" || D18.Code == "6" || D18.Code == "7" || D18.Code == "8" || D18.Code == "9" || D18.Code == "10" || D18.Code == "11" || D18.Code == "12"))
                {
                    if (D18.Code == "1" && D6 < 6)
                    {
                        msg = "الصف الاول لا يتناسب مع العمر";
                        valid = false;
                    }


                    if (D18.Code == "2" && D6 < 7)
                    {
                        msg = "الصف الثاني لا يتناسب مع العمر";
                        valid = false;
                    }

                    if (D18.Code == "3" && D6 < 8)
                    {
                        msg = "الص الثالث لا يتناسب مع العمر";
                        valid = false;
                    }


                    if (D18.Code == "4" && D6 < 9)
                    {
                        msg = "الصف الرابع لا يتناسب مع العمر";
                        valid = false;
                    }


                    if (D18.Code == "5" && D6 < 10)
                    {
                        msg = "الصف الخامس لا يتناسب مع العمر";
                        valid = false;
                    }
                    if (D18.Code == "6" && D6 < 11)
                    {
                        msg = "الصف السادس لا يتناسب مع العمر";
                        valid = false;
                    }
                    if (D18.Code == "7" && D6 < 12)
                    {
                        msg = "الصف السابع لا يتناسب مع العمر";
                        valid = false;
                    }
                    if (D18.Code == "8" && D6 < 13)
                    {
                        msg = "الصف الثامن لا يتناسب مع العمر";
                        valid = false;
                    }
                    if (D18.Code == "9" && D6 < 14)
                    {
                        msg = "الصف التاسع لا يتناسب مع العمر";
                        valid = false;
                    }
                    if (D18.Code == "10" && D6 < 15)
                    {
                        msg = "الصف العاشر لا يتناسب مع العمر";
                        valid = false;
                    }
                    if (D18.Code == "11" && D6 < 16)
                    {
                        msg = "الصف الحادي عشر لا يتناسب مع العمر";
                        valid = false;
                    }
                    if (D18.Code == "12" && D6 < 17)
                    {
                        msg = "الصف الثاني عشر لا يتناسب مع العمر";
                        valid = false;
                    }
                }
                else
                {
                    msg = "لا يوجد توافق بين الالتحاق بالتعليم والمستوى التعليمي";
                    valid = false;
                }
            }
            else if (D16 != null && D16.Code == "1")
            {
                if (D18 != null && (D18.Code == "13" || D18.Code == "14" || D18.Code == "15" || D18.Code == "16" || D18.Code == "17" || D18.Code == "18"))
                {
                    if (D18.Code == "13" && D6 < 19)
                    {
                        msg = "دبلوم متوسط لا يتناسب مع العمر";
                        valid = false;
                    }


                    if (D18.Code == "14" && D6 < 21)
                    {
                        msg = "بكالوريوس لا يتناسب مع العمر";
                        valid = false;
                    }

                    if (D18.Code == "15" && D6 < 22)
                    {
                        msg = "دبلوم عالي لا يتناسب مع العمر";
                        valid = false;
                    }


                    if (D18.Code == "16" && D6 < 24)
                    {
                        msg = "ماجستير لا يتناسب مع العمر";
                        valid = false;
                    }


                    if (D18.Code == "17" && D6 < 27)
                    {
                        msg = "دكتوراه لا يتناسب مع العمر";
                        valid = false;
                    }
                }
                else if (D18 != null && (D18.Code == "1" || D18.Code == "2" || D18.Code == "3" || D18.Code == "4" || D18.Code == "5" || D18.Code == "6" || D18.Code == "7" || D18.Code == "8" || D18.Code == "9" || D18.Code == "10" || D18.Code == "11" || D18.Code == "12"))
                {
                    if (D18.Code == "1" && D6 != 6)
                    {
                        msg = "الصف الاول لا يتناسب مع العمر";
                        valid = false;
                    }


                    if (D18.Code == "2" && D6 != 7)
                    {
                        msg = "الصف الثاني لا يتناسب مع العمر";
                        valid = false;
                    }

                    if (D18.Code == "3" && D6 != 8)
                    {
                        msg = "الص الثالث لا يتناسب مع العمر";
                        valid = false;
                    }


                    if (D18.Code == "4" && D6 != 9)
                    {
                        msg = "الصف الرابع لا يتناسب مع العمر";
                        valid = false;
                    }


                    if (D18.Code == "5" && D6 != 10)
                    {
                        msg = "الصف الخامس لا يتناسب مع العمر";
                        valid = false;
                    }
                    if (D18.Code == "6" && D6 != 11)
                    {
                        msg = "الصف السادس لا يتناسب مع العمر";
                        valid = false;
                    }
                    if (D18.Code == "7" && D6 != 12)
                    {
                        msg = "الصف السابع لا يتناسب مع العمر";
                        valid = false;
                    }
                    if (D18.Code == "8" && D6 != 13)
                    {
                        msg = "الصف الثامن لا يتناسب مع العمر";
                        valid = false;
                    }
                    if (D18.Code == "9" && D6 != 14)
                    {
                        msg = "الصف التاسع لا يتناسب مع العمر";
                        valid = false;
                    }
                    if (D18.Code == "10" && D6 != 15)
                    {
                        msg = "الصف العاشر لا يتناسب مع العمر";
                        valid = false;
                    }
                    if (D18.Code == "11" && D6 != 16)
                    {
                        msg = "الصف الحادي عشر لا يتناسب مع العمر";
                        valid = false;
                    }
                    if (D18.Code == "12" && D6 != 17)
                    {
                        msg = "الصف الثاني عشر لا يتناسب مع العمر";
                        valid = false;
                    }
                }
                else
                {
                    msg = "لا يوجد توافق بين الالتحاق بالتعليم والمستوى التعليمي";
                    valid = false;
                }
            }
            else if (D16 != null && (D16.Code == "2"))
            {
                if (D18 != null && (D18.Code == "13" || D18.Code == "14" || D18.Code == "15" || D18.Code == "16" || D18.Code == "17" || D18.Code == "18"))
                {
                    if (D18.Code == "13" && D6 < 19)
                    {
                        msg = "دبلوم متوسط لا يتناسب مع العمر";
                        valid = false;
                    }


                    if (D18.Code == "14" && D6 < 21)
                    {
                        msg = "بكالوريوس لا يتناسب مع العمر";
                        valid = false;
                    }

                    if (D18.Code == "15" && D6 < 22)
                    {
                        msg = "دبلوم عالي لا يتناسب مع العمر";
                        valid = false;
                    }


                    if (D18.Code == "16" && D6 < 24)
                    {
                        msg = "ماجستير لا يتناسب مع العمر";
                        valid = false;
                    }


                    if (D18.Code == "17" && D6 < 27)
                    {
                        msg = "دكتوراه لا يتناسب مع العمر";
                        valid = false;
                    }
                }
                else
                {
                    msg = "لا يوجد توافق بين الالتحاق بالتعليم والمستوى التعليمي";
                    valid = false;
                }

            }




            return valid;
        }
        public bool ValidateD8(out string msg)
        {
            msg = string.Empty;
            bool valid = true;
            // int? d8 = D8.Code;
            //var d3 = D3.Code;
            //  var d4 = D4.Code;

            int? d8 = D8?.Code.ToInt();
            var d3 = D3?.Code.ToInt();
            var d4 = D4?.Code.ToInt();
            if (D8 != null && D8.Code == "-1")
            {
                msg = "الرجاء اختيار اجابة ";
                valid = false;
            }

            if (d3.HasValue && (new List<int> { 2, 4, 6, 8 }).Contains(d3.Value) && d8 < 3)
            {
                msg = "لا يوجد توافق بين  الحالة الزواجية والعلاقة برب الاسرة";
                valid = false;
            }
            if (d3 == 2 && d4.HasValue)
            {
                int d4ToCheck = d4 == 1 ? 2 : 1;

                var d1 = Individuals.Where(x => x.D3 == 1 && x.D1 < _individual.D1 && x.D4 == d4ToCheck).FirstOrDefault();

                if (d1 != null)
                {


                    if (d1.D8 != 3 && d8 == 3)
                    {
                        msg = "لا يوجد توافق بين  الحالة الزواجية والحالة الزواجية لرب الاسرة";
                        valid = false;
                    }

                    else if (d1.D8 == 3 && d8 != 3)
                    {
                        msg = "لا يوجد توافق بين  الحالة الزواجية والحالة الزواجية لرب الاسرة يجب تسجيل متزوج/ة";
                        valid = false;
                    }


                }



            }

            return valid;
        }




        #region Settings       

        private bool _IsEnabled;

        public bool IsEnabled { get { return _IsEnabled; } set { SetProperty(ref _IsEnabled, value); NotifyPropertyChanged(); } }


        private bool _IsEnabledD7_CMNT;

        public bool IsEnabledD7_CMNT { get { return _IsEnabledD7_CMNT; } set { SetProperty(ref _IsEnabledD7_CMNT, value); NotifyPropertyChanged(); } }

        private bool _IsEnabledDs;

        public bool IsEnabledDs { get { return _IsEnabledDs; } set { SetProperty(ref _IsEnabledDs, value); NotifyPropertyChanged(); } }



        private bool _IsEnabledD11;

        public bool IsEnabledD11 { get { return _IsEnabledDs && _IsEnabledD11; } set { SetProperty(ref _IsEnabledD11, value); NotifyPropertyChanged(); } }

        private bool _IsEnabledD12;

        public bool IsEnabledD12 { get { return _IsEnabledDs && _IsEnabledD12; } set { SetProperty(ref _IsEnabledD12, value); NotifyPropertyChanged(); } }



        private bool _IsEnabledD12_01;

        public bool IsEnabledD12_01 { get { return IsEnabledD12 && _IsEnabledD12_01; } set { SetProperty(ref _IsEnabledD12_01, value); NotifyPropertyChanged(); } }

        private bool _IsEnabledD12_02;

        public bool IsEnabledD12_02 { get { return IsEnabledD12 && _IsEnabledD12_02; } set { SetProperty(ref _IsEnabledD12_02, value); NotifyPropertyChanged(); } }



        private bool _IsEnabledD14_20;

        public bool IsEnabledD14_20 { get { return IsEnabledD13 && _IsEnabledD14_20; } set { SetProperty(ref _IsEnabledD14_20, value); NotifyPropertyChanged(); } }

        private bool _IsEnabledD19_X;
        public bool IsEnabledD19_X { get { return IsEnabledD14_20 && _IsEnabledD19_X; } set { SetProperty(ref _IsEnabledD19_X, value); NotifyPropertyChanged(); NotifyPropertyChanged(nameof(IsEnabledD19_4)); } }

        private bool _IsEnabledD18_02_X;
        public bool IsEnabledD18_02_X { get { return IsEnabledD14_20 && _IsEnabledD18_02_X; } set { SetProperty(ref _IsEnabledD18_02_X, value); NotifyPropertyChanged(); } }


        private bool _IsEnabledD19_4;

        public bool IsEnabledD19_4 { get { return IsEnabledD19_X && _IsEnabledD19_4; } set { SetProperty(ref _IsEnabledD19_4, value); NotifyPropertyChanged(); } }





        //IsEnabledD12_1_CMNT
        private bool _IsEnabledD12_1_CMNT;

        public bool IsEnabledD12_01_CMNT { get { return _IsEnabledD12_1_CMNT; } set { SetProperty(ref _IsEnabledD12_1_CMNT, value); NotifyPropertyChanged(); } }

        // maram enabled
        private bool _IsEnabledD8;

        public bool IsEnabledD8 { get { return _IsEnabledD8; } set { SetProperty(ref _IsEnabledD8, value); NotifyPropertyChanged(); } }


        private bool _IsEnabledD10;

        public bool IsEnabledD10 { get { return _IsEnabledD10; } set { SetProperty(ref _IsEnabledD10, value); NotifyPropertyChanged(); } }

        private bool _IsEnabledD13;

        public bool IsEnabledD13 { get { return _IsEnabledD13; } set { SetProperty(ref _IsEnabledD13, value); NotifyPropertyChanged(); } }

        private bool _IsEnabledD14;

        public bool IsEnabledD14 { get { return _IsEnabledD14; } set { SetProperty(ref _IsEnabledD14, value); NotifyPropertyChanged(); } }

        private bool _IsEnabledD15;

        public bool IsEnabledD15 { get { return _IsEnabledD15; } set { SetProperty(ref _IsEnabledD15, value); NotifyPropertyChanged(); } }

        private bool _IsEnabledD16;

        public bool IsEnabledD16 { get { return _IsEnabledD16; } set { SetProperty(ref _IsEnabledD16, value); NotifyPropertyChanged(); } }

        private bool _IsEnabledD17;

        public bool IsEnabledD17 { get { return _IsEnabledD17; } set { SetProperty(ref _IsEnabledD17, value); NotifyPropertyChanged(); } }

        private bool _IsEnabledD18;

        public bool IsEnabledD18 { get { return _IsEnabledD18; } set { SetProperty(ref _IsEnabledD18, value); NotifyPropertyChanged(); } }


        private bool _IsEnabledD19;

        public bool IsEnabledD19 { get { return _IsEnabledD19; } set { SetProperty(ref _IsEnabledD19, value); NotifyPropertyChanged(); } }


        private bool _IsEnabledD20;

        public bool IsEnabledD20 { get { return _IsEnabledD20; } set { SetProperty(ref _IsEnabledD20, value); NotifyPropertyChanged(); } }

        private bool _IsEnabledD21;

        public bool IsEnabledD21 { get { return _IsEnabledD21; } set { SetProperty(ref _IsEnabledD21, value); NotifyPropertyChanged(); } }


        private bool _IsEnabledD22;
        public bool IsEnabledD22 { get { return _IsEnabledD22; } set { SetProperty(ref _IsEnabledD22, value); NotifyPropertyChanged(); } }

        private bool _IsEnabledD23;
        public bool IsEnabledD23 { get { return _IsEnabledD23; } set { SetProperty(ref _IsEnabledD23, value); NotifyPropertyChanged(); } }

        private bool _IsEnabledD24;
        public bool IsEnabledD24 { get { return _IsEnabledD24; } set { SetProperty(ref _IsEnabledD24, value); NotifyPropertyChanged(); } }


        private bool _IsEnabledD26;
        public bool IsEnabledD26 { get { return _IsEnabledD26; } set { SetProperty(ref _IsEnabledD26, value); NotifyPropertyChanged(); } }


        private bool _IsEnabledL;
        public bool IsEnabledL { get { return _IsEnabledL; } set { SetProperty(ref _IsEnabledL, value); NotifyPropertyChanged(); } }

        private bool _D17_CMNTVisible;
        public bool D17_CMNTVisible { get { return _D17_CMNTVisible; } set { SetProperty(ref _D17_CMNTVisible, value); NotifyPropertyChanged(); } }

        private bool _D26_CMNTVisible;
        public bool D26_CMNTVisible { get { return _D26_CMNTVisible; } set { SetProperty(ref _D26_CMNTVisible, value); NotifyPropertyChanged(); } }

        private bool _L1_CMNTVisible;
        public bool L1_CMNTVisible { get { return _L1_CMNTVisible; } set { SetProperty(ref _L1_CMNTVisible, value); NotifyPropertyChanged(); } }

        private bool _IsEnabledL1;
        public bool IsEnabledL1 { get { return _IsEnabledL1; } set { SetProperty(ref _IsEnabledL1, value); NotifyPropertyChanged(); } }


        private bool _IsEnabledL2;
        public bool IsEnabledL2 { get { return _IsEnabledL2; } set { SetProperty(ref _IsEnabledL2, value); NotifyPropertyChanged(); } }

        private bool _IsEnabledL3;
        public bool IsEnabledL3 { get { return _IsEnabledL3; } set { SetProperty(ref _IsEnabledL3, value); NotifyPropertyChanged(); } }

        private bool _IsEnabledL4;
        public bool IsEnabledL4 { get { return _IsEnabledL4; } set { SetProperty(ref _IsEnabledL4, value); NotifyPropertyChanged(); } }

        private bool _IsEnabledL5;
        public bool IsEnabledL5 { get { return _IsEnabledL5; } set { SetProperty(ref _IsEnabledL5, value); NotifyPropertyChanged(); } }

        private bool _IsEnabledL6;
        public bool IsEnabledL6 { get { return _IsEnabledL6; } set { SetProperty(ref _IsEnabledL6, value); NotifyPropertyChanged(); } }

        private bool _IsEnabledL7;
        public bool IsEnabledL7 { get { return _IsEnabledL7; } set { SetProperty(ref _IsEnabledL7, value); NotifyPropertyChanged(); } }

        private bool _IsEnabledL8;
        public bool IsEnabledL8 { get { return _IsEnabledL8; } set { SetProperty(ref _IsEnabledL8, value); NotifyPropertyChanged(); } }

        private bool _IsEnabledL9_13;
        public bool IsEnabledL9_13 { get { return _IsEnabledL9_13; } set { SetProperty(ref _IsEnabledL9_13, value); NotifyPropertyChanged(); } }

        private bool _IsEnabledL14;
        public bool IsEnabledL14 { get { return _IsEnabledL14; } set { SetProperty(ref _IsEnabledL14, value); NotifyPropertyChanged(); } }

        private void D7Changed()
        {
            //IsEnabledD7_CMNT = D7?.NeedComments?? false;
        }
        private void D12_1Changed()
        {
            //IsEnabledD12_01_CMNT = D12_01?.NeedComments ?? false;
        }

        private void L1Changed()
        {
            if (L1 == null) return;
            if (L1.Code == "3")
            {
                L2 = null;
                L3_A = null;
                L3_B = L3_C = null;
            }
            if (L1.Code == "4")
            {
                L2 = null;
                L3_A = null;
                L3_B = L3_C = null;
                L5 = L6 = L7 = L8 = L9 = L11 = L14_A = L14_B = L14_C = L14_D = L14_E = null;
                L12_Desc = L13_Desc = string.Empty;
                L10 = null;
            }
            if (L1.Code == "5" || L1.Code == "6" || L1.Code == "7" || L1.Code == "8" ||
                L1.Code == "9" || L1.Code == "10" || L1.Code == "11")
            {
                L2 = L3_C = L3_A = L3_B = L4 = L5 = L6 = L7 = L8 = L9 = L11 = L14_A = L14_B = L14_C = L14_D = L14_E = null;
                L12_Desc = L13_Desc = string.Empty;
                L10 = null;
            }
            if (L1.Code == "12")
            {
                L2 = L3_C = L3_A = L3_B = L4 = L5 = L6 = L7 = L8 = L9 = L11 = L14_A = L14_B = L14_C = L14_D = L14_E = null;
                L12_Desc = L13_Desc = string.Empty;
                L10 = null;
                L1_CMNTVisible = true;
            }
            SetControlStatus();
        }
        private void L2Changed()
        {
            if (L2 == null) return;
            if (L2.Code == "3")
            {
                L3_C = L3_A = L3_B = null;
                L4 = null;
            }
            SetControlStatus();
        }

        public void L4Changed()
        {
            if (L1 != null && L1.Code == "4")
            {
                L5 = L6 = L7 = L8 = L9 = L11 = L14_A = L14_B = L14_C = L14_D = L14_E = null;
                L12_Desc = L13_Desc = null;
                L10 = null;
            }

            SetControlStatus();
        }
        public void L3Changed()
        {

            if (L3_A != null && L3_A.Code != "-1")
            {
                L4 = null;

            }
            if (L3_B != null && L3_B.Code != "-1")
            {
                L4 = null;

            }
            if (L3_C != null && L3_C.Code != "-1")
            {
                L4 = null;

            }
            SetControlStatus();
        }


        private void L5Changed()
        {
            if (L5 == null) return;
            if (L5.Code != "4" && L5.Code != "5")
            {
                L8 = L7 = L14_A = L14_B = L14_C = L14_D = L14_E = null;

            }
            if (!IsEnabledL9_13)
            {
                L9 = L11 = null;
                L10 = null;
                L13_Desc = L12_Desc = null;
            }
            SetControlStatus();
        }

        private void D18Changed()
        {
            if (D18 != null)
            {
                if (D18.Code == "0")
                {
                    D19 = null;
                    D20 = null;
                    D21 = null;
                    D22 = null;
                }
                else if (D18.Code == "13" || D18.Code == "14" || D18.Code == "15" || D18.Code == "16" || D18.Code == "17" || D18.Code == "18")
                {
                    D19 = null;
                    D20 = null;
                    D21 = null;
                }

            }
            SetControlStatus();
        }
        private void D19Changed()
        {
            SetControlStatus();
        }
        private void D25Changed()
        {
            if (D25 != null && D25.Code == "2")
                D26 = null;
            SetControlStatus();
        }
        private void D17Changed()
        {
            if (D17 == null) return;
            if (D17.Code == "15")
            {
                D17_CMNTVisible = true;

            }
            else
                D17_CMNTVisible = false;
            SetControlStatus();
        }

        private void D26Changed()
        {
            if (D26 == null) return;
            if (D26.Code == "6")
            {
                D26_CMNTVisible = true;

            }
            else
                D26_CMNTVisible = false;
            SetControlStatus();
        }
        private void D16Changed()
        {
            if (D16 != null)
            {
                if (D16.Code == "0")
                {
                    D17 = D18 = D19 = D20 = null;
                    D21 = null;
                }
                if (D16.Code == "1" || D16.Code == "2")
                {
                    D17 = null;

                }
                if (D16.Code == "6")
                {
                    D17 = D18 = D19 = D20 = D22 = null;
                    D21 = null;

                }

            }

            SetControlStatus();
        }
        private void D13Changed()
        {
            //IsEnabledD13_CMNT = D13?.NeedComments ?? false;
            //IsEnabledD14_20 = D13?.AnswerCode.ToInt() < 6;
            //IsEnabledD19 = D13?.AnswerCode.ToInt() < 5;
            //D14Changed();
            //D4Changed();
        }
        private void D14Changed()
        {
            //IsEnabledD15 = IsEnabledD14_20 &&( D14?.AnswerCode == "4" || D14?.AnswerCode == "5");

            //IsEnabledD19_X= IsEnabledD14_20 && (D14?.AnswerCode == "4" || D14?.AnswerCode == "5");


            //IsEnabledD18_02_X = IsEnabledD14_20 && (D14?.AnswerCode == "4" || D14?.AnswerCode == "5");

        }
        private void D10Changed()
        {
            //IsEnabledDs = D10?.AnswerCode!="1" && D10?.AnswerCode != "6";
            //IsEnabledD11 = (D10?.AnswerCode != "5" && D10?.AnswerCode != "6") && D5>=5;

            //IsEnabledD12_01 = D10?.AnswerCode == "3" || D10?.AnswerCode == "5";
            //IsEnabledD12_02 = D10?.AnswerCode == "2" ||  D10?.AnswerCode == "3" || D10?.AnswerCode == "4";
        }

        private void CalculateAge()
        {

            if (D5_A == null || D5_B == null || D5_C == null
                || D5_A.Id == 0 || D5_B.Id == 0 || D5_C.Id == 0) return;

            int year = 0;
            int month = 0;
            int day = 0;

            year = D5_C.Id;
            month = D5_B.Id;
            day = D5_A.Id;

            if (year > 0 && year <= DateTime.Now.Year)
            {
                DateTime dateTime = new DateTime(year, 1, 1);

                if (month > 0 && month <= 12)
                {
                    dateTime = dateTime.AddMonths(month - 1);
                }
                else
                {
                    month = 1;
                }

                if (day > 0 && day <= 31)
                {
                    if (day <= DateTime.DaysInMonth(year, month))
                    {
                        dateTime = dateTime.AddDays(day);
                    }
                }


                int age = 0;
                age = QuestionnaireManager.CurrentVisit.CreatedDate.Year - dateTime.Year;
                if (QuestionnaireManager.CurrentVisit.CreatedDate.DayOfYear < dateTime.DayOfYear)
                    age = age - 1;

                if (age > 98) age = 98;
                D6 = age;

                SetControlStatus();
            }
            else
            {

            }

        }

        private void D5_AChanged()
        {
            // day

            CalculateAge();

        }
        private void D5_BChanged()
        {
            // month
            CalculateAge();
        }
        private void SetControlStatus()
        {


            IsEnabledD8 = D6 >= 14;
            IsEnabledD8 = D6 >= 14;
            IsEnabledD13 = IsEnabledD14 = IsEnabledD15 = D6 >= 15 && D6 <= 49 && D4 != null && D4.Code == "2" && IsEnabledD8 && D8 != null && (D8.Code == "3" || D8.Code == "4" || D8.Code == "5");
            IsEnabledD10 = D9 != null && D9.Code == "1";
            IsEnabledD16 = D6 >= 4;
            IsEnabledD17 = D6 >= 4 && D16 != null && (D16.Code == "3" || D16.Code == "4" || D16.Code == "5");
            IsEnabledD18 = D6 >= 4 && (D16 != null && (D16.Code == "1" || D16.Code == "2" || D16.Code == "3" || D16.Code == "4" || D16.Code == "5"));
            IsEnabledD19 = D6 >= 4 && IsEnabledD18 && D18 != null &&
                (D18.Code == "1" || D18.Code == "2" || D18.Code == "3" || D18.Code == "4" || D18.Code == "5" || D18.Code == "5" ||
                D18.Code == "7" || D18.Code == "8" || D18.Code == "9" || D18.Code == "10" || D18.Code == "11" || D18.Code == "12");
            IsEnabledD20 = D6 >= 4 && IsEnabledD19 && D19 != null && D19.Code == "1";
            IsEnabledD21 = D6 >= 4 && IsEnabledD20;
            IsEnabledD22 = D6 >= 4 && ((IsEnabledD19 && D19 != null) || (IsEnabledD18 && D18 != null && (D18.Code == "13" || D18.Code == "14" || D18.Code == "15" || D18.Code == "16" || D18.Code == "17" || D18.Code == "18")) || (IsEnabledD16 && D16 != null && D16.Code == "0"));
            IsEnabledD23 = D6 >= 5 && D18 != null && (D18.Code == "1" || D18.Code == "2" || D18.Code == "3" || D18.Code == "4" || D18.Code == "5" || D18.Code == "5" ||
                D18.Code == "7" || D18.Code == "8" || D18.Code == "9" || D18.Code == "10" || D18.Code == "11" || D18.Code == "12");
            IsEnabledD24 = D6 >= 6 && D6 <= 18 && (D18 != null && (D18.Code == "1" || D18.Code == "2" || D18.Code == "3" || D18.Code == "4" || D18.Code == "5" || D18.Code == "5" ||
                D18.Code == "7" || D18.Code == "8" || D18.Code == "9" || D18.Code == "10" || D18.Code == "11" || D18.Code == "12"));
            IsEnabledD26 = D25 != null && D25.Code == "1";
            IsEnabledL = D6 >= 15 && L1 != null && L1.Code != "5" && L1.Code != "6" && L1.Code != "7" && L1.Code != "8" && L1.Code != "9" && L1.Code != "10" && L1.Code != "11" && L1.Code != "12";
            IsEnabledL1 = D6 >= 15;
            IsEnabledL2 = IsEnabledL && L1 != null && (L1.Code == "1" || L1.Code == "2");
            IsEnabledL3 = IsEnabledL2 && L2 != null && L2.Code != "3";
            IsEnabledL4 = IsEnabledL && L1 != null && (L1.Code == "3" || L1.Code == "4");
            //IsEnabledL5 = IsEnabledL && L1 != null && L1.Code!="4" && (L2 != null && L2.Code == "3" || (L3_A != null && L3_B != null && L3_C != null));
            IsEnabledL5 = IsEnabledL && (IsEnabledL2 || IsEnabledL3 || (IsEnabledL4 && L4 != null && L1.Code == "3"));
            IsEnabledL6 = IsEnabledL5;
            IsEnabledL7 = IsEnabledL5 && L5 != null && (L5.Code == "5" || L5.Code == "4");
            IsEnabledL8 = IsEnabledL7;
            IsEnabledL9_13 = IsEnabledL1 && L1 != null && (L1.Code == "1" || L1.Code == "2" || L1.Code == "3");
            IsEnabledL14 = IsEnabledL && L5 != null && (L5.Code == "4" || L5.Code == "5") && IsEnabledL9_13;

        }
        private void D5_CChanged()
        {
            // year
            if (D5_C != null && D5_C.Id > 0)
            {
                int yearId = D5_C.Id;

                if (yearId == 9999)
                { // to do
                  // txtHR06.IsEnabled = true;
                }
                else
                {
                    //txtHR06.IsEnabled = false;
                    CalculateAge();
                }
            }
        }
        private void D5Changed()
        {
            //IsEnabledD21 = D5 >= 14;
            //IsEnabledD10 = D5 >= 3;
            //IsEnabledD12 = D5 >= 5; //5+
            //IsEnabledD13 = D5 >= 7;
        }

        public bool ValidateD10_D(out string msg)
        {
            msg = string.Empty;
            bool valid = true;
            if (D10_D != null && D10_D.Code == "-1")
            {
                msg = "الرجاء اختيار اجابة";
                valid = false;
            }
            return valid;
        }

        public bool ValidateD17(out string msg)
        {
            msg = string.Empty;
            bool valid = true;
            if (D17 != null && D17.Code == "-1")
            {
                msg = "الرجاء اختيار اجابة";
                valid = false;
            }
            return valid;
        }
        public bool ValidateD20(out string msg)
        {
            msg = string.Empty;
            bool valid = true;
            if (D20 != null && D20.Code == "-1")
            {
                msg = "الرجاء اختيار اجابة";
                valid = false;
            }
            return valid;
        }

        public bool ValidateL1(out string msg)
        {
            msg = string.Empty;
            bool valid = true;
            if (L1 != null && L1.Code == "-1")
            {
                msg = "الرجاء اختيار اجابة";
                valid = false;
            }
            return valid;
        }

        public bool ValidateL2(out string msg)
        {
            msg = string.Empty;
            bool valid = true;
            if (L2 != null && L2.Code == "-1")
            {
                msg = "الرجاء اختيار اجابة";
                valid = false;
            }
            return valid;
        }

        public bool ValidateL3_A(out string msg)
        {
            msg = string.Empty;
            bool valid = true;
            if (L3_A != null && L3_A.Code == "-1")
            {
                msg = "الرجاء اختيار اجابة";
                valid = false;
            }
            return valid;
        }
        public bool ValidateL3_B(out string msg)
        {
            msg = string.Empty;
            bool valid = true;
            if (L3_B != null && L3_B.Code == "-1")
            {
                msg = "الرجاء اختيار اجابة";
                valid = false;
            }
            return valid;
        }
        public bool ValidateL3_C(out string msg)
        {
            msg = string.Empty;
            bool valid = true;
            if (L3_C != null && L3_C.Code == "-1")
            {
                msg = "الرجاء اختيار اجابة";
                valid = false;
            }
            return valid;
        }

        public bool ValidateL4(out string msg)
        {
            msg = string.Empty;
            bool valid = true;
            if (L4 != null && L4.Code == "-1")
            {
                msg = "الرجاء اختيار اجابة";
                valid = false;
            }
            return valid;
        }
        public bool ValidateL5(out string msg)
        {
            msg = string.Empty;
            bool valid = true;
            if (L5 != null && L5.Code == "-1")
            {
                msg = "الرجاء اختيار اجابة";
                valid = false;
            }
            return valid;
        }
        public bool ValidateL6(out string msg)
        {
            msg = string.Empty;
            bool valid = true;
            if (L6 != null && L6.Code == "-1")
            {
                msg = "الرجاء اختيار اجابة";
                valid = false;
            }
            return valid;
        }

        public bool ValidateL7(out string msg)
        {
            msg = string.Empty;
            bool valid = true;
            if (L7 != null && L7.Code == "-1")
            {
                msg = "الرجاء اختيار اجابة";
                valid = false;
            }
            return valid;
        }
        public bool ValidateL8(out string msg)
        {
            msg = string.Empty;
            bool valid = true;
            if (L8 != null && L8.Code == "-1")
            {
                msg = "الرجاء اختيار اجابة";
                valid = false;
            }
            return valid;
        }
        public bool ValidateL9(out string msg)
        {
            msg = string.Empty;
            bool valid = true;
            if (L9 != null && L9.Code == "-1")
            {
                msg = "الرجاء اختيار اجابة";
                valid = false;
            }
            return valid;
        }
        public bool ValidateL11(out string msg)
        {
            msg = string.Empty;
            bool valid = true;
            if (L11 != null && L11.Code == "-1")
            {
                msg = "الرجاء اختيار اجابة";
                valid = false;
            }
            return valid;
        }

        public bool ValidateL14_A(out string msg)
        {
            msg = string.Empty;
            bool valid = true;
            if (L14_A != null && L14_A.Code == "-1")
            {
                msg = "الرجاء اختيار اجابة";
                valid = false;
            }
            return valid;
        }
        public bool ValidateL14_B(out string msg)
        {
            msg = string.Empty;
            bool valid = true;
            if (L14_B != null && L14_B.Code == "-1")
            {
                msg = "الرجاء اختيار اجابة";
                valid = false;
            }
            return valid;
        }
        public bool ValidateL14_C(out string msg)
        {
            msg = string.Empty;
            bool valid = true;
            if (L14_C != null && L14_C.Code == "-1")
            {
                msg = "الرجاء اختيار اجابة";
                valid = false;
            }
            return valid;
        }
        public bool ValidateL14_D(out string msg)
        {
            msg = string.Empty;
            bool valid = true;
            if (L14_D != null && L14_D.Code == "-1")
            {
                msg = "الرجاء اختيار اجابة";
                valid = false;
            }
            return valid;
        }
        public bool ValidateL14_E(out string msg)
        {
            msg = string.Empty;
            bool valid = true;
            if (L14_E != null && L14_E.Code == "-1")
            {
                msg = "الرجاء اختيار اجابة";
                valid = false;
            }
            return valid;
        }
        public bool ValidateD23(out string msg)
        {
            msg = string.Empty;
            bool valid = true;
            if (D23 != null)
            {
                if (D18 != null && D18.Code == "0" && D23 > 5)
                {
                    msg = " المستوى التعليمي أمي ولا يتناسب مع عدد سنوات الدراسة";
                    valid = false;
                }
                //6
                if (D18 != null && D18.Code == "1" && (D23 < 1 || D23 > 1))
                {
                    msg = "المستوى التعليمي صف اول لا يتناسب مع عدد سنوات الدراسة ";
                    valid = false;
                }
                //7
                if (D18 != null && D18.Code == "2" && (D23 < 2 || D23 > 2))
                {
                    msg = "المستوى التعليمي صف ثاني لا يتناسب مع عدد سنوات الدراسة ";
                    valid = false;
                }
                //8
                if (D18 != null && D18.Code == "3" && (D23 < 3 || D23 > 3))
                {
                    msg = "المستوى التعليمي صف ثالث لا يتناسب مع عدد سنوات الدراسة ";
                    valid = false;
                }
                //9
                if (D18 != null && D18.Code == "4" && (D23 < 4 || D23 > 4))
                {
                    msg = "المستوى التعليمي صف رابع لا يتناسب مع عدد سنوات الدراسة ";
                    valid = false;
                }
                //10
                if (D18 != null && D18.Code == "5" && (D23 < 5 || D23 > 5))
                {
                    msg = "المستوى التعليمي صف خامس لا يتناسب مع عدد سنوات الدراسة ";
                    valid = false;
                }
                //11
                if (D18 != null && D18.Code == "6" && (D23 < 6 || D23 > 6))
                {
                    msg = "المستوى التعليمي صف سادس لا يتناسب مع عدد سنوات الدراسة ";
                    valid = false;
                }
                //12
                if (D18 != null && D18.Code == "7" && (D23 < 7 || D23 > 7))
                {
                    msg = "المستوى التعليمي صف سابع لا يتناسب مع عدد سنوات الدراسة ";
                    valid = false;
                }
                //13
                if (D18 != null && D18.Code == "8" && (D23 < 8 || D23 > 8))
                {
                    msg = "المستوى التعليمي صف ثامن لا يتناسب مع عدد سنوات الدراسة ";
                    valid = false;
                }
                //14
                if (D18 != null && D18.Code == "9" && (D23 < 9 || D23 > 9))
                {
                    msg = "المستوى التعليمي صف تاسع لا يتناسب مع عدد سنوات الدراسة ";
                    valid = false;
                }
                //15
                if (D18 != null && D18.Code == "10" && (D23 < 10 || D23 > 10))
                {
                    msg = "المستوى التعليمي صف العاشر لا يتناسب مع عدد سنوات الدراسة ";
                    valid = false;
                }
                //16
                if (D18 != null && D18.Code == "11" && (D23 < 11 || D23 > 11))
                {
                    msg = "المستوى التعليمي صف الحادي عشر لا يتناسب مع عدد سنوات الدراسة ";
                    valid = false;
                }
                //17
                if (D18 != null && D18.Code == "12" && (D23 < 12 || D23 > 12))
                {
                    msg = "المستوى التعليمي صف الثاني عشر لا يتناسب مع عدد سنوات الدراسة ";
                    valid = false;
                }
                if (D18 != null && D18.Code == "13" && (D23 < 13 || D23 > 18))
                {
                    msg = "دبلوم متوسط لا يتناسب مع عدد سنوات الدراسة والالتحاق بالتعليم   ";
                    valid = false;
                }
                if (D18 != null && D18.Code == "14" && (D23 < 15 || D23 > 24))
                {
                    msg = "بكالوريوس لا يتناسب مع عدد سنوات الدراسة والالتحاق بالتعليم";
                    valid = false;
                }
                if (D18 != null && D18.Code == "15" && (D23 < 16 || D23 > 24))
                {
                    msg = "دبلوم عالي لا يتناسب مع عدد سنوات الدراسة والالتحاق بالتعليم";
                    valid = false;
                }
                if (D18 != null && D18.Code == "16" && (D23 < 17 || D23 > 30))
                {
                    msg = "ماجستير لا يتناسب مع عدد سنوات الدراسة والالتحاق بالتعليم";
                    valid = false;
                }
                if (D18 != null && D18.Code == "17" && (D23 < 19 || D23 > 31))
                {
                    msg = "دكتوراه لا يتناسب مع عدد سنوات الدراسة والالتحاق بالتعليم";
                    valid = false;
                }
            }
            return valid;

        }
        public bool ValidateD22(out string msg)
        {
            msg = string.Empty;
            bool valid = true;
            if (D22 != null && D22.Code == "-1")
            {
                msg = "الرجاء اختيار اجابة";
                valid = false;
            }
            return valid;
        }
        public bool ValidateD25(out string msg)
        {
            msg = string.Empty;
            bool valid = true;
            if (D25 != null && D25.Code == "-1")
            {
                msg = "الرجاء اختيار اجابة";
                valid = false;
            }
            return valid;
        }
        public bool ValidateD24(out string msg)
        {
            msg = string.Empty;
            bool valid = true;
            if (D24 != null && D24.Code == "-1")
            {
                msg = "الرجاء اختيار اجابة";
                valid = false;
            }
            return valid;
        }
        public bool ValidateD26(out string msg)
        {
            msg = string.Empty;
            bool valid = true;
            if (D26 != null && D26.Code == "-1")
            {
                msg = "الرجاء اختيار اجابة";
                valid = false;
            }
            return valid;
        }
        public bool ValidateD19(out string msg)
        {
            msg = string.Empty;
            bool valid = true;
            if (D19 != null && D19.Code == "-1")
            {
                msg = "الرجاء اختيار اجابة";
                valid = false;
            }
            return valid;
        }
        public bool ValidateD10_E(out string msg)
        {
            msg = string.Empty;
            bool valid = true;
            if (D10_E != null && D10_E.Code == "-1")
            {
                msg = "الرجاء اختيار اجابة";
                valid = false;
            }
            return valid;
        }
        public bool ValidateD10_A(out string msg)
        {
            msg = string.Empty;
            bool valid = true;
            if (D10_A != null && D10_A.Code == "-1")
            {
                msg = "الرجاء اختيار اجابة";
                valid = false;
            }


            return valid;
        }
        public bool ValidateD10_C(out string msg)
        {
            msg = string.Empty;
            bool valid = true;
            if (D10_C != null && D10_C.Code == "-1")
            {
                msg = "الرجاء اختيار اجابة";
                valid = false;
            }
            return valid;
        }
        public bool ValidateD4(out string msg)
        {
            msg = string.Empty;
            bool valid = true;
            if (D4 != null && D4.Code == "-1")
            {
                msg = "الرجاء اختيار اجابة";
                valid = false;
            }
            if (D4 != null)
            {
                if (!_IsNew)
                {
                    if (Individuals != null && Individuals.Count() > 0)
                    {
                        int count = 0;
                        var tempIndividuals = Individuals;
                        Individual removedInd = Individuals.Where(x => x.ID00 == _individual.ID00 && x.D1 == _individual.D1).Distinct().FirstOrDefault();
                        tempIndividuals.Remove(removedInd);

                        if (D4.Code == "1")
                        {
                            var temp = tempIndividuals.Where(x => x.D4 == 1 && x.ID00 == _individual.ID00).ToList();
                            count = temp.Count() + 1;
                        }
                        if (D4.Code == "2")
                        {
                            var temp = tempIndividuals.Where(x => x.D4 == 2 && x.ID00 == _individual.ID00).ToList();
                            count = temp.Count() + 1;
                        }
                        if (D4.Code == "1" && QuestionnaireManager.CurrentVisit.QC4 < count)
                        {
                            msg = "لا يوجد توافق بين جنس الفرد وعدد الذكور بالاسرة";
                            valid = false;
                        }
                        if (D4.Code == "2" && QuestionnaireManager.CurrentVisit.QC5 < count)
                        {
                            msg = "لا يوجد توافق بين جنس الفرد وعدد الاناث بالاسرة";
                            valid = false;
                        }
                        if (D3.Code == "2")
                        {
                            var ind = Individuals.Where(x => x.ID00 == _individual.ID00 && x.D3 == 1).Distinct().FirstOrDefault();
                            if (ind.D4 == 1 && D4.Code == "1")
                            {
                                msg = "لا يوجد توافق بجنس زوجة/زوج مع جنس رب الاسرة ";
                                valid = false;
                            }
                            if (ind.D4 == 2 && D4.Code == "2")
                            {
                                msg = "لا يوجد توافق بجنس زوجة/زوج مع جنس رب الاسرة ";
                                valid = false;
                            }
                        }
                    }
                    if (Individuals != null && Individuals.Count() == 0)
                    {
                        if (D4.Code == "1" && QuestionnaireManager.CurrentVisit.QC4 == 0)
                        {
                            msg = "لا يوجد توافق بين جنس الفرد وعدد الذكور بالاسرة";
                            valid = false;
                        }
                        if (D4.Code == "2" && QuestionnaireManager.CurrentVisit.QC5 == 0)
                        {
                            msg = "لا يوجد توافق بين جنس الفرد وعدد الاناث بالاسرة";
                            valid = false;
                        }

                    }
                }
                else
                {
                    int sixCount = Individuals.Select(x => x.D4 == D4.Code.ToInt() && x.ID00 == _individual.ID00).ToList().Count();
                    if (D4 != null && D4.Code == "1" && sixCount > QuestionnaireManager.CurrentVisit.QC4)
                    {
                        msg = "لا يوجد توافق بين جنس الفرد وعدد الذكور بالاسرة";
                        valid = false;

                    }
                    if (D4 != null && D4.Code == "2" && sixCount > QuestionnaireManager.CurrentVisit.QC5)
                    {
                        msg = "لا يوجد توافق بين جنس الفرد وعدد الاناث بالاسرة";
                        valid = false;
                    }
                }
            }
            return valid;
        }
        private void D4Changed()
        {

        }
        private async Task FillMonths()
        {
            List<OptionInfo> options = new List<OptionInfo>();
            options.Add(OptionInfo.Default);
            for (int i = 1; i <= 12; i++)
            {
                string monthName = new DateTime(2010, i, 1).ToString("MMM", new CultureInfo("ar-JO"));
                options.Add(new OptionInfo() { Id = i, Description = monthName, NeedComments = false });
            }

            options.Add(new OptionInfo() { Id = 99, Description = "لا اعرف", NeedComments = false });
            D5_BList = options;
            // cmbHR05_M.ItemsSource = options;
            //cmbHR05_M.ItemDisplayBinding = new Binding("Description");
            //cmbHR05_M.SelectedIndex = 0;
        }

        private void FillDays()
        {
            List<OptionInfo> options = new List<OptionInfo>();
            options.Add(OptionInfo.Default);
            for (int i = 1; i <= 31; i++)
            {

                options.Add(new OptionInfo() { Id = i, Description = i.ToString(), NeedComments = false });
            }

            options.Add(new OptionInfo() { Id = 99, Description = "لا اعرف", NeedComments = false });
            D5_AList = options;
            //   cmbHR05_D.ItemsSource = options;
            //   cmbHR05_D.ItemDisplayBinding = new Binding("Description");
            //   cmbHR05_D.SelectedIndex = 0;

        }

        private void FillYears()
        {
            List<OptionInfo> options = new List<OptionInfo>();
            options.Add(OptionInfo.Default);
            for (int i = DateTime.Now.Year; i > 1900; i--)
            {

                options.Add(new OptionInfo() { Id = i, Description = i.ToString(), NeedComments = false });
            }


            options.Add(new OptionInfo() { Id = 9999, Description = "لا اعرف", NeedComments = false });
            D5_CList = options;
            //   cmbHR05_Y.ItemsSource = options;
            //cmbHR05_Y.ItemDisplayBinding = new Binding("Description");
            //  cmbHR05_Y.SelectedIndex = 0;

        }

        private async Task LoadSettings(Individual individual)
        {
            FillYears();
            FillMonths();
            FillDays();
            D3List = await LookUpManager.GetLookupVals("D3");
            D4List = await LookUpManager.GetLookupVals("D4");
            D7List = await LookUpManager.GetLookupVals("D7");
            D8List = await LookUpManager.GetLookupVals("D8");
            D9List = await LookUpManager.GetLookupVals("yes/no");
            D10_AList = await LookUpManager.GetLookupVals("yes/no");
            D10_BList = await LookUpManager.GetLookupVals("yes/no");
            D10_CList = await LookUpManager.GetLookupVals("yes/no");
            D10_DList = await LookUpManager.GetLookupVals("yes/no");
            D10_EList = await LookUpManager.GetLookupVals("yes/no");
            D11List = await LookUpManager.GetLookupVals("yes/no");
            D12_AList = D12_BList = D12_CList = D12_DList = D12_EList = D12_FList = await LookUpManager.GetLookupVals("D12");
            D16List = await LookUpManager.GetLookupVals("D16");
            D17List = await LookUpManager.GetLookupVals("D17");
            D18List = await LookUpManager.GetLookupVals("D18");
            D19List = await LookUpManager.GetLookupVals("yes/no");
            D20List = await LookUpManager.GetLookupVals("yes/no");
            D22List = await LookUpManager.GetLookupVals("D22");
            D24List = await LookUpManager.GetLookupVals("D24");
            D25List = await LookUpManager.GetLookupVals("yes/no");
            D26List = await LookUpManager.GetLookupVals("D26");
            L1List = await LookUpManager.GetLookupVals("L1");
            L2List = await LookUpManager.GetLookupVals("L2");
            L3_AList = L3_BList = L3_CList = await LookUpManager.GetLookupVals("yes/no");

            L4List = await LookUpManager.GetLookupVals("L4");
            L5List = await LookUpManager.GetLookupVals("L5");
            L6List = await LookUpManager.GetLookupVals("L6");
            L7List = await LookUpManager.GetLookupVals("L7");
            L8List = await LookUpManager.GetLookupVals("L8");

            L9List = await LookUpManager.GetLookupVals("L9");

            L11List = await LookUpManager.GetLookupVals("yes/no");
            L14List = await LookUpManager.GetLookupVals("L14");
            await SetVisitToScreen(individual);
        }
        private async Task SetVisitToScreen(Individual Individual)
        {
            if (Individual != null && Individual is Individual individual)
            {
                ID00 = individual.ID00;
                D1 = individual.D1;
                D2 = individual.D2;
                D3 = D3List.Find(x => x.Code.ToInt() == individual.D3);
                D4 = D4List.Find(x => x.Code.ToInt() == individual.D4);
                D5 = individual.D5_D + "/" + individual.D5_M + "/" + individual.D5_Y;
                D5_A = individual.D5_D.HasValue ? D5_AList.Find(x => x.Id == individual.D5_D) : null;
                D5_B = individual.D5_M.HasValue ? D5_BList.Find(x => x.Id == individual.D5_M) : null;
                D5_C = individual.D5_Y.HasValue ? D5_CList.Find(x => x.Id == individual.D5_Y) : null;
                D6 = individual.D6;//.HasValue ? D6List.Find(x => x.AnswerCode.ToInt() == individual.D6) : null;
                D7 = individual.D7.HasValue ? D7List.Find(x => x.Code.ToInt() == individual.D7) : null;
                D8 = individual.D8.HasValue ? D8List.Find(x => x.Code.ToInt() == individual.D8) : null;
                // D7_CMNT = individual.D7_CMNT;
                D9 = individual.D9.HasValue ? D9List.Find(x => x.Code.ToInt() == individual.D9) : null;

                D10_A = individual.D10_1.HasValue ? D10_AList.Find(x => x.Code.ToInt() == individual.D10_1) : null;
                D10_B = individual.D10_2.HasValue ? D10_BList.Find(x => x.Code.ToInt() == individual.D10_2) : null;
                D10_C = individual.D10_3.HasValue ? D10_CList.Find(x => x.Code.ToInt() == individual.D10_3) : null;
                D10_D = individual.D10_4.HasValue ? D10_DList.Find(x => x.Code.ToInt() == individual.D10_4) : null;
                D10_E = individual.D10_5.HasValue ? D10_EList.Find(x => x.Code.ToInt() == individual.D10_2) : null;

                D11 = individual.D11.HasValue ? D11List.Find(x => x.Code.ToInt() == individual.D11) : null;
                D12_A = individual.D12_1.HasValue ? D12_AList.Find(x => x.Code.ToInt() == individual.D12_1) : null;
                D12_B = individual.D12_2.HasValue ? D12_BList.Find(x => x.Code.ToInt() == individual.D12_2) : null;
                D12_C = individual.D12_3.HasValue ? D12_CList.Find(x => x.Code.ToInt() == individual.D12_3) : null;
                D12_D = individual.D12_4.HasValue ? D12_DList.Find(x => x.Code.ToInt() == individual.D12_4) : null;
                D12_E = individual.D12_5.HasValue ? D12_EList.Find(x => x.Code.ToInt() == individual.D12_5) : null;
                D12_F = individual.D12_6.HasValue ? D12_FList.Find(x => x.Code.ToInt() == individual.D12_6) : null;
                D13 = individual.D13;
                D14 = individual.D14;
                D15 = individual.D15;
                D16 = individual.D16.HasValue ? D16List.Find(x => x.Code.ToInt() == individual.D16) : null;
                D17 = individual.D17.HasValue ? D17List.Find(x => x.Code.ToInt() == individual.D17) : null;
                D17_CMNT = individual.D17_CMNT;

                D18 = individual.D18.HasValue ? D18List.Find(x => x.Code.ToInt() == individual.D18) : null;
                D19 = individual.D19.HasValue ? D19List.Find(x => x.Code.ToInt() == individual.D19) : null;

                D20 = individual.D20.HasValue ? D20List.Find(x => x.Code.ToInt() == individual.D20) : null;
                D21 = individual.D21;
                D22 = individual.D22.HasValue ? D22List.Find(x => x.Code.ToInt() == individual.D22) : null;
                D23 = individual.D23;
                D24 = individual.D24.HasValue ? D24List.Find(x => x.Code.ToInt() == individual.D24) : null;
                D25 = individual.D25.HasValue ? D25List.Find(x => x.Code.ToInt() == individual.D25) : null;
                D26 = individual.D26.HasValue ? D26List.Find(x => x.Code.ToInt() == individual.D26) : null;
                D26_CMNT = individual.D26_CMNT;
                L1 = individual.L1.HasValue ? L1List.Find(x => x.Code.ToInt() == individual.L1) : null;
                L1_CMNT = individual.L1_CMNT;
                // D19_2_5 = individual.D19_2_5.HasValue ? D19_2_5List.Find(x => x.AnswerCode == individual.D19_2_5) : null;
                L2 = individual.L2.HasValue ? L2List.Find(x => x.Code.ToInt() == individual.L2) : null;


                L3_A = individual.L3_A.HasValue ? L3_AList.Find(x => x.Code.ToInt() == individual.L3_A) : null;
                L3_B = individual.L3_B.HasValue ? L3_BList.Find(x => x.Code.ToInt() == individual.L3_B) : null;
                L3_C = individual.L3_C.HasValue ? L3_CList.Find(x => x.Code.ToInt() == individual.L3_C) : null;

                L4 = individual.L4.HasValue ? L4List.Find(x => x.Code.ToInt() == individual.L4) : null;
                L5 = individual.L5.HasValue ? L5List.Find(x => x.Code.ToInt() == individual.L5) : null;
                L6 = individual.L6.HasValue ? L6List.Find(x => x.Code.ToInt() == individual.L6) : null;
                L7 = individual.L7.HasValue ? L7List.Find(x => x.Code.ToInt() == individual.L7) : null;
                L8 = individual.L8.HasValue ? L8List.Find(x => x.Code.ToInt() == individual.L8) : null;
                L9 = individual.L9.HasValue ? L9List.Find(x => x.Code.ToInt() == individual.L9) : null;
                L10 = individual.L10;
                L11 = individual.L11.HasValue ? L11List.Find(x => x.Code.ToInt() == individual.L11) : null;
                L12_Desc = individual.L12_Desc;
                L13_Desc = individual.L13_Desc;

                L14_A = individual.L14_1.HasValue ? L14List.Find(x => x.Code.ToInt() == individual.L14_1) : null;
                L14_B = individual.L14_2.HasValue ? L14List.Find(x => x.Code.ToInt() == individual.L14_2) : null;
                L14_C = individual.L14_3.HasValue ? L14List.Find(x => x.Code.ToInt() == individual.L14_3) : null;
                L14_D = individual.L14_4.HasValue ? L14List.Find(x => x.Code.ToInt() == individual.L14_4) : null;
                L14_E = individual.L14_5.HasValue ? L14List.Find(x => x.Code.ToInt() == individual.L14_5) : null;
            }
        }
        private List<LookupVal> _D3List;
        private List<LookupVal> _D4List;
        private List<OptionInfo> _D5_AList;
        private List<OptionInfo> _D5_BList;
        private List<OptionInfo> _D5_CList;
        // private List<LookUpValueInfo> _D6List;
        private List<LookupVal> _D7List;
        private List<LookupVal> _D8List;
        private List<LookupVal> _D9List;
        private List<LookupVal> _D10_AList;
        private List<LookupVal> _D10_BList;
        private List<LookupVal> _D10_CList;
        private List<LookupVal> _D10_DList;
        private List<LookupVal> _D10_EList;
        private List<LookupVal> _D11List;
        private List<LookupVal> _D12_AList;
        private List<LookupVal> _D12_BList;
        private List<LookupVal> _D12_CList;
        private List<LookupVal> _D12_DList;
        private List<LookupVal> _D12_EList;
        private List<LookupVal> _D12_FList;
        // private List<LookupVal> _D13List;
        //   private List<LookUpValueInfo> _D14List;
        //   private List<LookUpValueInfo> _D15List;
        private List<LookupVal> _D16List;
        private List<LookupVal> _D17List;
        private List<LookupVal> _D18List;
        private List<LookupVal> _D19List;
        private List<LookupVal> _D20List;
        private List<LookupVal> _D22List;
        private List<LookupVal> _D24List;
        private List<LookupVal> _D25List;
        private List<LookupVal> _D26List;
        private List<LookupVal> _L1List;
        private List<LookupVal> _L2List;
        private List<LookupVal> _L3_AList;
        private List<LookupVal> _L3_BList;
        private List<LookupVal> _L3_CList;
        private List<LookupVal> _L4List;
        private List<LookupVal> _L5List;
        private List<LookupVal> _L6List;
        private List<LookupVal> _L7List;
        private List<LookupVal> _L8List;
        private List<LookupVal> _L9List;
        private List<LookupVal> _L11List;
        private List<LookupVal> _L14_AList;
        private List<LookupVal> _L14_BList;
        private List<LookupVal> _L14_CList;
        private List<LookupVal> _L14_DList;
        private List<LookupVal> _L14_EList;
        public List<LookupVal> D3List { get { return _D3List; } set { SetProperty(ref _D3List, value); } }
        public List<LookupVal> D4List { get { return _D4List; } set { SetProperty(ref _D4List, value); } }
        public List<OptionInfo> D5_AList { get { return _D5_AList; } set { SetProperty(ref _D5_AList, value); } }
        public List<OptionInfo> D5_BList { get { return _D5_BList; } set { SetProperty(ref _D5_BList, value); } }
        public List<OptionInfo> D5_CList { get { return _D5_CList; } set { SetProperty(ref _D5_CList, value); } }
        public List<LookupVal> D7List { get { return _D7List; } set { SetProperty(ref _D7List, value); } }
        public List<LookupVal> D8List { get { return _D8List; } set { SetProperty(ref _D8List, value); } }
        public List<LookupVal> D9List { get { return _D9List; } set { SetProperty(ref _D9List, value); } }
        public List<LookupVal> D10_AList { get { return _D10_AList; } set { SetProperty(ref _D10_AList, value); } }
        public List<LookupVal> D10_BList { get { return _D10_BList; } set { SetProperty(ref _D10_BList, value); } }
        public List<LookupVal> D10_CList { get { return _D10_CList; } set { SetProperty(ref _D10_CList, value); } }
        public List<LookupVal> D10_DList { get { return _D10_DList; } set { SetProperty(ref _D10_DList, value); } }
        public List<LookupVal> D10_EList { get { return _D10_EList; } set { SetProperty(ref _D10_EList, value); } }

        public List<LookupVal> D11List { get { return _D11List; } set { SetProperty(ref _D11List, value); } }
        public List<LookupVal> D12_AList { get { return _D12_AList; } set { SetProperty(ref _D12_AList, value); } }
        public List<LookupVal> D12_BList { get { return _D12_BList; } set { SetProperty(ref _D12_BList, value); } }
        public List<LookupVal> D12_CList { get { return _D12_CList; } set { SetProperty(ref _D12_CList, value); } }
        public List<LookupVal> D12_DList { get { return _D12_DList; } set { SetProperty(ref _D12_DList, value); } }
        public List<LookupVal> D12_EList { get { return _D12_EList; } set { SetProperty(ref _D12_EList, value); } }
        public List<LookupVal> D12_FList { get { return _D12_FList; } set { SetProperty(ref _D12_FList, value); } }
        public List<LookupVal> D16List { get { return _D16List; } set { SetProperty(ref _D16List, value); } }
        public List<LookupVal> D17List { get { return _D17List; } set { SetProperty(ref _D17List, value); } }
        public List<LookupVal> D18List { get { return _D18List; } set { SetProperty(ref _D18List, value); } }
        public List<LookupVal> D19List { get { return _D19List; } set { SetProperty(ref _D19List, value); } }
        public List<LookupVal> D20List { get { return _D20List; } set { SetProperty(ref _D20List, value); } }
        public List<LookupVal> D22List { get { return _D22List; } set { SetProperty(ref _D22List, value); } }
        public List<LookupVal> D24List { get { return _D24List; } set { SetProperty(ref _D24List, value); } }
        public List<LookupVal> D25List { get { return _D25List; } set { SetProperty(ref _D25List, value); } }
        public List<LookupVal> D26List { get { return _D26List; } set { SetProperty(ref _D26List, value); } }
        public List<LookupVal> L1List { get { return _L1List; } set { SetProperty(ref _L1List, value); } }
        public List<LookupVal> L2List { get { return _L2List; } set { SetProperty(ref _L2List, value); } }
        public List<LookupVal> L3_AList { get { return _L3_AList; } set { SetProperty(ref _L3_AList, value); } }
        public List<LookupVal> L3_BList { get { return _L3_BList; } set { SetProperty(ref _L3_BList, value); } }
        public List<LookupVal> L3_CList { get { return _L3_CList; } set { SetProperty(ref _L3_CList, value); } }
        public List<LookupVal> L4List { get { return _L4List; } set { SetProperty(ref _L4List, value); } }
        public List<LookupVal> L5List { get { return _L5List; } set { SetProperty(ref _L5List, value); } }
        public List<LookupVal> L6List { get { return _L6List; } set { SetProperty(ref _L6List, value); } }
        public List<LookupVal> L7List { get { return _L7List; } set { SetProperty(ref _L7List, value); } }
        public List<LookupVal> L8List { get { return _L8List; } set { SetProperty(ref _L8List, value); } }
        public List<LookupVal> L9List { get { return _L9List; } set { SetProperty(ref _L9List, value); } }
        public List<LookupVal> L11List { get { return _L11List; } set { SetProperty(ref _L11List, value); } }
        public List<LookupVal> L14List { get { return _L14_AList; } set { SetProperty(ref _L14_AList, value); } }



        public override List<string> GetSkipedFields()
        {
            List<string> fields = new List<string>();
            if (!IsEnabledD8) fields.Add(nameof(D8));
            if (!IsEnabledD10) fields.Add(nameof(D10_A));
            if (!IsEnabledD10) fields.Add(nameof(D10_B));
            if (!IsEnabledD10) fields.Add(nameof(D10_C));
            if (!IsEnabledD10) fields.Add(nameof(D10_D));
            if (!IsEnabledD10) fields.Add(nameof(D10_E));
            if (!IsEnabledD13) fields.Add(nameof(D13));
            if (!IsEnabledD14) fields.Add(nameof(D14));
            if (!IsEnabledD15) fields.Add(nameof(D15));
            if (!IsEnabledD16) fields.Add(nameof(D16));
            if (!IsEnabledD17) fields.Add(nameof(D17));
            if (!IsEnabledD18) fields.Add(nameof(D18));
            if (!IsEnabledD19) fields.Add(nameof(D19));
            if (!IsEnabledD20) fields.Add(nameof(D20));
            if (!IsEnabledD21) fields.Add(nameof(D21));
            if (!IsEnabledD22) fields.Add(nameof(D22));
            if (!IsEnabledD23) fields.Add(nameof(D23));
            if (!IsEnabledD24) fields.Add(nameof(D24));
            if (!IsEnabledD26) fields.Add(nameof(D26));
            if (!IsEnabledL1) fields.Add(nameof(L1));
            if (!IsEnabledL2) fields.Add(nameof(L2));
            if (!IsEnabledL3) fields.Add(nameof(L3_A));
            if (!IsEnabledL3) fields.Add(nameof(L3_B));
            if (!IsEnabledL3) fields.Add(nameof(L3_C));
            if (!IsEnabledL4) fields.Add(nameof(L4));
            if (!IsEnabledL5) fields.Add(nameof(L5));
            if (!IsEnabledL6) fields.Add(nameof(L6));
            if (!IsEnabledL7) fields.Add(nameof(L7));
            if (!IsEnabledL8) fields.Add(nameof(L8));
            if (!IsEnabledL9_13) fields.Add(nameof(L9));
            if (!IsEnabledL9_13) fields.Add(nameof(L10));
            if (!IsEnabledL9_13) fields.Add(nameof(L11));
            if (!IsEnabledL9_13) fields.Add(nameof(L12_Desc));
            if (!IsEnabledL9_13) fields.Add(nameof(L13_Desc));
            if (!IsEnabledL14) fields.Add(nameof(L14_A));
            if (!IsEnabledL14) fields.Add(nameof(L14_B));
            if (!IsEnabledL14) fields.Add(nameof(L14_C));
            if (!IsEnabledL14) fields.Add(nameof(L14_D));
            if (!IsEnabledL14) fields.Add(nameof(L14_E));
            if (!L1_CMNTVisible) fields.Add(nameof(L1_CMNT));
            if (!D17_CMNTVisible) fields.Add(nameof(D17_CMNT));
            if (!D26_CMNTVisible) fields.Add(nameof(D26_CMNT));




            return fields;
        }

        public override async Task<bool> CustomValidate()
        {
            bool isValid = true;
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("برجاء تعبئة الحقول التالية أولا");


            bool warning = false;

            StringBuilder warningString = new StringBuilder();
            warningString.AppendLine("- هل انت متاكد من عدم تعبئة الحقول التالية:");

            if (IsEnabledD10 && D10_A == null)
            {
                AddError(nameof(D10_A), "تامين حكومي حقل متطلب", true);
                isValid = false;
            }
            if (IsEnabledD10 && D10_B == null)
            {
                AddError(nameof(D10_B), "تامين وكالة حقل متطلب", true);
                isValid = false;
            }
            if (IsEnabledD10 && D10_C == null)
            {
                AddError(nameof(D10_C), "تامين خاص حقل متطلب", true);
                isValid = false;
            }
            if (IsEnabledD10 && D10_D == null)
            {
                AddError(nameof(D10_D), "تامين اسرائيلي حقل متطلب", true);
                isValid = false;
            }
            if (IsEnabledD10 && D10_E == null)
            {
                AddError(nameof(D10_E), "تامين اخرى حقل متطلب", true);

                isValid = false;
            }
            if (IsEnabledD8 && D8 == null)
            {
                AddError(nameof(D8), "الحالة الزواجية حقل متطلب حقل متطلب", true);
                isValid = false;
            }
            if (IsEnabledD16 && D16 == null)
            {
                AddError(nameof(D8), "الحالة الزواجية حقل متطلب حقل متطلب", true);
                isValid = false;
            }
            if (IsEnabledL1 && L1 == null)
            {
                AddError(nameof(L1), "العلاقة بقوة العمل خلال الاسبوع الماضي حقل متطلب حقل متطلب", true);
                isValid = false;
            }
            if (IsEnabledD26 && D26 == null)
            {
                AddError(nameof(D26), "عدد الطلاب في الغرفة الصفية للمراحل الاساسية والثانوية حقل متطلب", true);
                isValid = false;
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

        public override async Task<bool> DoSave()
        {
            // _individual.ID = Guid.NewGuid();
            _individual.D1 = D1;
            _individual.D2 = D2;
            _individual.D3 = Convert.ToInt32(D3.Code);
            _individual.D4 = D4.Id;
            _individual.D5_D = D5_A.Id;
            _individual.D5_M = D5_B.Id;
            _individual.D5_Y = D5_C.Id;
            _individual.D6 = D6;
            _individual.D7 = D7.Id;
            //_individual.D7_CMNT = IsEnabledD7_CMNT ? D7_CMNT : string.Empty;

            _individual.D8 = IsEnabledD8 ? D8.Code.ToInt() : null;  //D8.Id;
            _individual.D9 = D9.Id;
            _individual.D10_1 = IsEnabledD10 ? D10_A.Code.ToInt() : null;  //D10_A.Id;
            _individual.D10_2 = IsEnabledD10 ? D10_B.Code.ToInt() : null;  //D10_B.Id;
            _individual.D10_3 = IsEnabledD10 ? D10_C.Code.ToInt() : null;  //D10_C.Id;
            _individual.D10_4 = IsEnabledD10 ? D10_D.Code.ToInt() : null;  //D10_D.Id;
            _individual.D10_5 = IsEnabledD10 ? D10_E.Code.ToInt() : null;  //D10_E.Id;
            _individual.D11 = D11.Id;
            _individual.D12_1 = D12_A.Code.ToInt();
            _individual.D12_2 = D12_B.Code.ToInt();
            _individual.D12_3 = D12_C.Code.ToInt();
            _individual.D12_4 = D12_D.Code.ToInt();
            _individual.D12_5 = D12_E.Code.ToInt();
            _individual.D12_6 = D12_F.Code.ToInt();
            _individual.D13 = IsEnabledD13 ? D13 : null;  //D13;
            _individual.D14 = IsEnabledD14 ? D14 : null;  //D14;
            _individual.D15 = IsEnabledD15 ? D15 : null;  // D15;
            //_individual.D16= IsEnabledD10 ? D10?.AnswerCode.ToInt() : null;
            _individual.D16 = IsEnabledD16 ? D16.Code.ToInt() : null;  //D16.Id;

            _individual.D17 = IsEnabledD17 ? D17.Code.ToInt() : null;  //D17.Id;
            _individual.D17_CMNT = D17_CMNTVisible && IsEnabledD17 ? D17_CMNT : string.Empty;
            _individual.D18 = IsEnabledD18 ? D18.Code.ToInt() : null;  //D18.Id;
            _individual.D19 = IsEnabledD19 ? D19.Code.ToInt() : null;  // D19.Id;
            _individual.D20 = IsEnabledD20 ? D20.Code.ToInt() : null;  //D20.Id;
            _individual.D21 = IsEnabledD21 ? D21 : null;  //D21;
            _individual.D22 = IsEnabledD22 ? D22.Code.ToInt() : null;  //D22.Id;
            _individual.D23 = IsEnabledD23 ? D23 : null;  // D15;
            _individual.D24 = IsEnabledD24 ? D24.Code.ToInt() : null;  //D22.Id;
            _individual.D25 = D25.Code.ToInt();  //D22.Id;
            _individual.D26 = IsEnabledD26 ? D26.Code.ToInt() : null;  //D22.Id;
            _individual.D26_CMNT = D26_CMNTVisible && IsEnabledD26 ? D26_CMNT : string.Empty;
            _individual.L1 = IsEnabledL1 ? L1.Code.ToInt() : null;
            _individual.L1_CMNT = L1_CMNTVisible && IsEnabledL1 ? L1_CMNT : string.Empty;
            _individual.L2 = IsEnabledL2 ? L2.Code.ToInt() : null;
            _individual.L3_A = IsEnabledL3 ? L3_A.Code.ToInt() : null;  //L3_A.Id;
            _individual.L3_B = IsEnabledL3 ? L3_B.Code.ToInt() : null;  //L3_B.Id;
            _individual.L3_C = IsEnabledL3 ? L3_C.Code.ToInt() : null;  //L3_C.Id;
            _individual.L4 = IsEnabledL4 ? L4.Code.ToInt() : null;  //L4.Id;
            _individual.L5 = IsEnabledL5 ? L5.Code.ToInt() : null;  //L5.Id;
            _individual.L6 = IsEnabledL6 ? L6.Code.ToInt() : null;  //L6.Id;
            _individual.L7 = IsEnabledL7 ? L7.Code.ToInt() : null;  //L7.Id;
            _individual.L8 = IsEnabledL8 ? L8.Code.ToInt() : null;  //L8.Id;

            _individual.L9 = IsEnabledL9_13 ? L9.Code.ToInt() : null;  //L9.Id;
            _individual.L10 = IsEnabledL9_13 ? L10 : null;  //L10;

            _individual.L11 = IsEnabledL9_13 ? L11.Code.ToInt() : null;  //L11.Id;
            _individual.L12_Desc = IsEnabledL9_13 ? L12_Desc : string.Empty;  //L12_Desc; 
            _individual.L13_Desc = IsEnabledL9_13 ? L13_Desc : string.Empty;  //L13_Desc;
            _individual.L14_1 = IsEnabledL14 ? L14_A.Code.ToInt() : null;  //L14_A.Id;
            _individual.L14_2 = IsEnabledL14 ? L14_B.Code.ToInt() : null;  //L14_B.Id;
            _individual.L14_3 = IsEnabledL14 ? L14_C.Code.ToInt() : null;  //L14_C.Id;
            _individual.L14_4 = IsEnabledL14 ? L14_D.Code.ToInt() : null;  //L14_D.Id;
            _individual.L14_5 = IsEnabledL14 ? L14_E.Code.ToInt() : null;  //L14_E.Id;
            _individual.CreatedBy = Security.CurrentUserSettings.CurrentUser.UserName;
            _individual.CreatedDate = DateTime.Now;
            if (_individual.D3 == 1 && _individual.D1 != 1)
            {
                _individual.D1 = 1;
            }
            return await base.DoSave();
        }


        #endregion

    }

}
