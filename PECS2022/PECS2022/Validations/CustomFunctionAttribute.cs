using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PECS2022.Validations
{

    public delegate bool CustomFunctionDelegate(out string msg);
    public class CustomFunctionAttribute : ValidationAttribute
    {

      private string _other { get; set; }
     

       public CustomFunctionAttribute(string MethodName)
       {
          this._other = MethodName;
       }

      

        protected override System.ComponentModel.DataAnnotations.ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var method = validationContext.ObjectType.GetMethod(_other);
            if (method == null)
            {
                return new System.ComponentModel.DataAnnotations.ValidationResult(
                    string.Format("Unknown Method: {0}", _other)
                );
            }
            object[] parameters = new object[] { null };
            var otherValue = method.Invoke(validationContext.ObjectInstance, parameters);

            // at this stage you have "value" and "otherValue" pointing
            // to the value of the property on which this attribute
            // is applied and the value of the other property respectively
            // => you could do some checks

            if (!Convert.ToBoolean(otherValue))
            {
                // here we are verifying whether the 2 values are equal
                // but you could do any custom validation you like
                var result = new System.ComponentModel.DataAnnotations.ValidationResult(parameters[0].ToString(), new List<string>() { validationContext.MemberName });
                return result;
            }
            return System.ComponentModel.DataAnnotations.ValidationResult.Success;
        }



    }
}
