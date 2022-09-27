using PECS2022.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace PECS2022
{
    public static class ExtensionMethods
    {

        public static string GetAnswerCode(this int?  value)
        {
            if (value == null) return string.Empty;
            return value.ToString();
        }
        public static bool EqualsOneOf(this string str,params string []values)
        {
             return values.Contains(str);
        }

        public static void SetAnswerCode(this Picker picker, string answerCode)
        {

            if (!string.IsNullOrWhiteSpace(answerCode))
            {
                var values = picker.ItemsSource as List<LookupVal>;

                if (values != null)
                {
                    var val = values.Where(x => x.Code == answerCode).FirstOrDefault();

                    if (val != null)
                    {
                        picker.SelectedItem = val;
                    }
                }

            }
            else
            {
                picker.SelectedIndex = 0;
            }


        }
      
        public static string GetAnswerCode(this Picker picker)
        {
            string selectedVal = null;

            var val = picker.SelectedItem as LookupVal;

            if (val != null)
            {

                if (!string.IsNullOrWhiteSpace(val.Code))
                {
                    selectedVal = val.Code;
                }

            }


            return selectedVal;
        }

        public static int? GetAnswerId(this Picker picker)
        {
            int? selectedVal = null;

            var val = picker.SelectedItem as LookupVal;

            if (val != null)
            {

                if (!string.IsNullOrWhiteSpace(val.Code))
                {
                    selectedVal = val.Id;
                }

            }


            return selectedVal;
        }


        public static int?  ToInt(this string val)
        {
            if(int.TryParse(val,out int value))
            {
                return value;
            }

            return null;
        }
    }
}
