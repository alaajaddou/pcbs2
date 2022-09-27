using System;
using System.Collections.Generic;
using System.Text;

namespace PECS2022
{
   public class ValidationResult
    {

        private ValidationResult()
        {

        }

        public static ValidationResult CreateSuccess()
        {
            return new ValidationResult() { Result = ValidationResultEnum.Success };
        }

        public static ValidationResult CreateError(string errorMessage)
        {
            return new ValidationResult() { Result = ValidationResultEnum.HasError, Message = errorMessage };
        }

        public static ValidationResult CreateWarning(string warningMessage)
        {
            return new ValidationResult() { Result = ValidationResultEnum.HasWarning, Message = warningMessage };
        }

        public ValidationResultEnum Result { get; private set; }
       

        public string Message { get; private set; }
    }

    public enum ValidationResultEnum
    {
        HasError,
        HasWarning,
        Success
    }
}
