using Syncfusion.SfAutoComplete.XForms;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Reflection;
using System.Text;
using Xamarin.Forms;

namespace PECS2022
{
    public class MPAutoComplete : SfAutoComplete, IControlValidation
    {
        #region Properties
        public BaseControlValidation<MPAutoComplete> _Validate;
        Boolean _disableNestedCalls;

        //public static readonly new BindableProperty ItemsSourceProperty =
        //    BindableProperty.Create("ItemsSource", typeof(IEnumerable), typeof(MpPicker),
        //        null, propertyChanged: OnItemsSourceChanged);

        //public static readonly new BindableProperty SelectedItemProperty =
        //    BindableProperty.Create("SelectedItem", typeof(Object), typeof(MpPicker),
        //        null, BindingMode.TwoWay, propertyChanged: OnSelectedItemChanged);

        //public static readonly BindableProperty SelectedValueProperty =
        //    BindableProperty.Create("SelectedValue", typeof(Object), typeof(MpPicker),
        //        null, BindingMode.TwoWay, propertyChanged: OnSelectedValueChanged);

        //public String DisplayMemberPath { get; set; }

        //public new IList ItemsSource
        //{
        //    get { return (IList)GetValue(ItemsSourceProperty); }
        //    set { SetValue(ItemsSourceProperty, value); base.ItemsSource = value; }
        //}

        //public virtual new object SelectedItem
        //{
        //    get { return GetValue(SelectedItemProperty); }
        //    set
        //    {
        //        if (this.SelectedItem != value)
        //        {
        //            base.SelectedItem = value;
        //            SetValue(SelectedItemProperty, value);

        //        }
        //    }
        //}

        //public Object SelectedValue
        //{
        //    get { return GetValue(SelectedValueProperty); }
        //    set
        //    {
        //        SetValue(SelectedValueProperty, value);
        //        InternalSelectedValueChanged();
        //    }
        //}

        //public String SelectedValuePath { get; set; }
        #endregion

        #region Constructor
        public MPAutoComplete()
        {

            //this.ItemSelected += OnSelectedIndexChanged;

            AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            TextHighlightMode = OccurrenceMode.MultipleOccurrence;
            HighlightedTextColor = Color.Red;
            SuggestionMode = SuggestionMode.Contains;
            NoResultsFoundText = "لا يوجد نتائج لبحثك";
            ShowSuggestionsOnFocus = true;
            this.ValueChanged += MPAutoComplete_ValueChanged;
          

            _Validate = new BaseControlValidation<MPAutoComplete>(
                this,
                SelectedItemProperty.PropertyName, SelectedItemProperty,
                this.SetPrivateFields);
        }

        private string LastValue;
        private void MPAutoComplete_ValueChanged(object sender, Syncfusion.SfAutoComplete.XForms.ValueChangedEventArgs e)
        {
            if (SelectedItem != null)
            {
                var type = SelectedItem.GetType();
                var prop = type.GetRuntimeProperty(this.DisplayMemberPath);
              var  val=  prop.GetValue(SelectedItem) as string;

                var value = e.Value;
                if (val != value)
                {
                    this.SetValue(SelectedItemProperty, null);
                    LastValue = e.Value;
                    Text = LastValue;
                    //this.SetValue(SelectedValueProperty, value);

                }
            }
            else if(string.IsNullOrWhiteSpace(e.Value) && !string.IsNullOrWhiteSpace(LastValue))
            {
                Text = LastValue;
             
                LastValue = null;
            }
        }

      

        private void SetPrivateFields(bool _hasError, string _errorMessage)
        {
            this.HasError = _hasError;
            this.ErrorMessage = _errorMessage;
        }

        #endregion

       

        #region Validations Properties        

        #region Has Error
        public static readonly BindableProperty HasErrorProperty =
            BindableProperty.Create("HasError", typeof(bool), typeof(MpPicker), false, defaultBindingMode: BindingMode.TwoWay);

        public bool HasError
        {
            get { return (bool)GetValue(HasErrorProperty); }
            private set { SetValue(HasErrorProperty, value); }
        }
        #endregion

        #region ErrorMessage

        public static readonly BindableProperty ErrorMessageProperty =
           BindableProperty.Create("ErrorMessage", typeof(string), typeof(MpPicker), string.Empty);

        public string ErrorMessage
        {
            get { return (string)GetValue(ErrorMessageProperty); }
            set { SetValue(ErrorMessageProperty, value); }
        }
        #endregion

        #region ShowErrorMessage

        public static readonly BindableProperty ShowErrorMessageProperty =
           BindableProperty.Create("ShowErrorMessage", typeof(bool), typeof(MpPicker), false, propertyChanged: OnShowErrorMessageChanged, defaultBindingMode: BindingMode.TwoWay);

        private static void OnShowErrorMessageChanged(BindableObject bindable, object oldValue, object newValue)
        {
            // execute on bindable context changed method
            MpPicker control = bindable as MpPicker;
            if (control != null && control.BindingContext != null)
            {
                control._Validate.CheckValidation();
            }
        }

        public bool ShowErrorMessage
        {
            get { return (bool)GetValue(ShowErrorMessageProperty); }
            set { SetValue(ShowErrorMessageProperty, value); }
        }
        #endregion

        #endregion

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();


           // this._Validate.CheckValidation();
        }
    }
}
