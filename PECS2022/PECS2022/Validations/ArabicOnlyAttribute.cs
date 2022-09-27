using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.RegularExpressions;

namespace PECS2022.Validations
{
   public class ArabicOnlyAttribute:ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            Regex regex = new Regex("^[\u0621-\u064A ]+$");

            return regex.IsMatch(value?.ToString());
        }
    }
}
