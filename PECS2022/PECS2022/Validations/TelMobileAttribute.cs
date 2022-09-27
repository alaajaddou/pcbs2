using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PECS2022.Validations
{
   public class TelMobileAttribute : ValidationAttribute
    {

        public override bool IsValid(object value)
        {
            bool isValid = true;
            string phone = value?.ToString();
            if (!string.IsNullOrWhiteSpace(phone))
            {
                if (!Util.Validator.IsDigit(phone))
                {
                    isValid = false;
                    ErrorMessageResourceName = "DigitOnly";

                }
                else if (Util.Validator.IsTelephone(phone) )
                {
                    isValid = true;
                }
                else if (!Util.Validator.IsMobile(phone))
                {
                    isValid = false;

                    ErrorMessageResourceName = "TelFormat";

                }
                   
                
            }

            return isValid;
        }
    }
}
