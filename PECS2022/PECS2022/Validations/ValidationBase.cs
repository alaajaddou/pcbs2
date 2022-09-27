using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;

namespace PECS2022
{
    public class ValidationBase : INotifyScrollToProperty, INotifyDataErrorInfo
    {
        #region Properties
        // Dictionary to collect errors of model
        private Dictionary<string, List<string>> _errors = new Dictionary<string, List<string>>();
        // INotifyDataErrorInfo - Event
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public void ClearError(string code, bool forceOnErrorsChanged = false)
        {
            if (_errors != null)
            {
                _errors.Remove(code);
            }

            if (forceOnErrorsChanged)
                OnErrorsChanged(code);
        }

        public void AddError(string code, string msg, bool forceOnErrorsChanged = false)
        {
            if (_errors == null)
            {
                _errors = new Dictionary<string, List<string>>();
            }

            var temp = _errors.Where(x => x.Key == code).FirstOrDefault().Value;

            if (temp == null)
            {
                _errors.Add(code, new List<string>() { msg });
            }
            else
            {
                temp.Clear();
                temp.Add(msg);
            }

            if (forceOnErrorsChanged)
                OnErrorsChanged(code);
        }

        // Scroll To Event Property
        public event ScrollToPropertyHandler ScrollToProperty;

        // Lock for async work
        private object _lock = new object();
        public bool HasErrors
        {
            get
            {
                return _errors.Any(propErrors => propErrors.Value != null && propErrors.Value.Count > 0);
            }
        }

        // Return first invalid property name
        public string GetFirstInvalidPropertyName
        {
            get
            {
                if (!this.HasErrors)
                    return string.Empty;

                return _errors.Select(x => x.Key).FirstOrDefault();
            }
        }

        #endregion

        public IEnumerable GetErrors(string propertyName)
        {
            if (!string.IsNullOrEmpty(propertyName))
            {
                if (_errors.ContainsKey(propertyName) && (_errors[propertyName] != null) && _errors[propertyName].Count > 0)
                    return _errors[propertyName].ToList();
                else
                    return null;
            }
            else
            {

                // var errors = _errors.Select(x => x.Key).ToList();
                return _errors.SelectMany(err => err.Value.ToList()).ToList();
            }

        }




        public void OnErrorsChanged(string propertyName)
        {
            if (ErrorsChanged != null)
                ErrorsChanged(this, new DataErrorsChangedEventArgs(propertyName));
        }

        public void ValidateProperty(object value, [CallerMemberName] string propertyName = null)
        {
            lock (_lock)
            {

                var validationContext = new ValidationContext(this, null);
                validationContext.MemberName = propertyName;
                var validationResults = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
                try
                {
                    Validator.TryValidateProperty(value, validationContext, validationResults);
                }
                catch
                {
                    validationResults.Add(new System.ComponentModel.DataAnnotations.ValidationResult("برجاء التاكد من القيمة المدخلة", new string[] { propertyName }));
                }

                //clear previous _errors from tested property  
                if (_errors.ContainsKey(propertyName))
                    _errors.Remove(propertyName);
                OnErrorsChanged(propertyName);
                HandleValidationResults(validationResults);
            }
        }

        public void Validate(params string[] skipedFields)
        {
            lock (_lock)
            {
                var validationContext = new ValidationContext(this, null);

                var validationResults = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
                Validator.TryValidateObject(this, validationContext, validationResults, true);

                //clear all previous _errors  
                var propNames = _errors.Keys.ToList();
                _errors.Clear();
                //propNames.ForEach(pn => OnErrorsChanged(pn));
                foreach (var propertyName in propNames)
                {
                    if (skipedFields != null)
                    {
                        OnErrorsChanged(propertyName);
                    }



                }
                HandleValidationResults(validationResults);
                propNames = _errors.Keys.Where(x => skipedFields.Contains(x)).ToList();
                foreach (var propertyName in propNames)
                {
                    if (_errors.ContainsKey(propertyName))
                    {
                        _errors.Remove(propertyName);
                    }


                }
            }
        }


        public void ClearErrors()
        {
            //lock (_lock)
            //{
            //    var validationContext = new ValidationContext(this, null);

            //    var validationResults = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
            //    Validator.TryValidateObject(this, validationContext, validationResults, true);

            //    //clear all previous _errors  
            //    var propNames = _errors.Keys.ToList();
            //    var skipedFields = propNames;
            //    _errors.Clear();
            //    //propNames.ForEach(pn => OnErrorsChanged(pn));
            //    foreach (var propertyName in propNames)
            //    {
            //        if (skipedFields != null)
            //        {
            //            OnErrorsChanged(propertyName);
            //        }



            //    }
            //    HandleValidationResults(validationResults);
            //    propNames = _errors.Keys.Where(x => skipedFields.Contains(x)).ToList();
            //    foreach (var propertyName in propNames)
            //    {
            //        if (_errors.ContainsKey(propertyName))
            //        {
            //            _errors.Remove(propertyName);
            //        }


            //    }
            //}
        }

        private void HandleValidationResults(List<System.ComponentModel.DataAnnotations.ValidationResult> validationResults)
        {
            //Group validation results by property names  
            var resultsByPropNames = from res in validationResults
                                     from mname in res.MemberNames
                                     group res by mname into g
                                     select g;

            //add _errors to dictionary and inform binding engine about _errors  
            foreach (var prop in resultsByPropNames)
            {
                var messages = prop.Select(r => r.ErrorMessage).ToList();
                _errors.Add(prop.Key, messages);
                OnErrorsChanged(prop.Key);
            }
        }

        #region Invoke Scroll To Property
        /// <summary>
        /// Invoke scroll to property event
        /// </summary>
        /// <param name="PropertyName">PropertyName</param>
        protected void InvokeScrollToProperty(string PropertyName)
        {
            ScrollToProperty?.Invoke(PropertyName);
        }

        public void ScrollToControlProperty(string PropertyName)
        {
            InvokeScrollToProperty(PropertyName);
        }
        #endregion

    }
}
