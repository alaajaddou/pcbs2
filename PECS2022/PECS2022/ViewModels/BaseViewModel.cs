using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PECS2022.ViewModels
{
    public abstract class BaseViewModel : ValidationBase, INotifyPropertyChanged
    {



        public event OnSaveSuccess OnSaveSuccess;
        public event OnSaveFailure OnSaveFailure;
        public event OnDeleteSuccess OnDeleteSuccess;
        public event OnDeleteFailure OnDeleteFailure;

        public event OnRestoreSuccess OnRestoreSuccess;
        public event OnRestoreFailure OnRestoreFailure;

        public string SaveMessage { get; set; }
        public string SaveFailureMessage { get; set; }

        public string DeleteSuccessMessage { get; set; }
        public string DeleteNotSuccess { get; set; }

        public BaseViewModel(bool isDeleted = false)
        {
            IsDeleted = isDeleted;
            SaveMessage = GeneralMessages.DataSavedSuccessfully;
            SaveFailureMessage = GeneralMessages.SaveNotSuccess;
            DeleteSuccessMessage = GeneralMessages.DeleteSuccess;
            DeleteNotSuccess = GeneralMessages.DeleteNotSuccess;
            Save = new Command(async () =>
            {


                bool saveResult = await SaveChanges();






            });

            Delete = new Command(async () => { await DeleteData(); });
            Restore = new Command(async () => { await RestoreData(); });


            SetActionStatus();
        }


        public virtual void SetBusy()
        {
            IsBusy = true;
            //NavigationPage.SetHasBackButton(App.CurrentPage, false);
        }

        public virtual void SetNotBusy()
        {
            IsBusy = false;
            //NavigationPage.SetHasBackButton(App.CurrentPage, true);
        }


        public virtual string GetMessageString()
        {
            var message = GetErrors(null);
            List<string> values = new List<string>();
            foreach (string x in message)
            {
                values.Add(x);
            }
            var _values = values.Select(x => x);
            var msgStr = string.Join(Environment.NewLine, _values.ToArray());

            return msgStr;
        }


        bool isBusy = false;
        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); NotifyPropertyChanged(nameof(IsNotBusy)); }
        }

        public bool IsNotBusy { get { return !isBusy; } }


        private bool _DeleteEnabled;
        private bool _SaveEnabled;
        private bool _RestorEnabled;
        public bool DeleteEnabled { get { return _DeleteEnabled; } set { _DeleteEnabled = value; NotifyPropertyChanged(); } }
        public bool SaveEnabled { get { return _SaveEnabled; } set { _SaveEnabled = value; NotifyPropertyChanged(); } }

        public bool RestorEnabled { get { return _RestorEnabled; } set { _RestorEnabled = value; NotifyPropertyChanged(); } }


        public bool IsDeleted { get; set; }
        public Command Save { get; private set; }

        public Command Delete { get; private set; }

        public Command Restore { get; private set; }


        private void SetActionStatus()
        {
            DeleteEnabled = !IsDeleted;
            SaveEnabled = !IsDeleted;
            RestorEnabled = IsDeleted;
        }


        public async Task<bool> SaveChanges()
        {

            bool result = false;
            if (!IsBusy)
            {
                IsBusy = true;
                if (await IsValid())
                {
                    result = await DoSave();
                    if (result)
                    {
                        OnSaveSuccess?.Invoke();
                        ToastManager.LongAlert(SaveMessage);
                    }
                    else
                    {
                        OnSaveFailure?.Invoke();
                        ToastManager.LongAlert(SaveFailureMessage);
                    }

                }

                else
                {
                    string msgStr = GetMessageString();
                    if (!string.IsNullOrWhiteSpace(msgStr))
                    {
                        await App.Current.MainPage.DisplayAlert(GeneralMessages.Error, msgStr, GeneralMessages.Cancel);
                    }
                }
                IsBusy = false;

            }
            return result;
        }
        public virtual void RefreshUI()
        {

        }
        public virtual async Task<bool> DoSave()
        {
            return await Task.Run<bool>(() => { return true; });

        }

        public virtual void SetObjectToScreen(object o)
        {

        }

        public virtual async Task<bool> DoDelete()
        {
            return await Task.Run<bool>(() => { return true; });

        }
        public virtual async Task<bool> DoRestore()
        {
            return await Task.Run<bool>(() => { return true; });
        }



        public virtual async Task<bool> CustomValidate()
        {
            return await Task.Run<bool>(() => { return true; });
        }


        public virtual async Task<bool> IsValid()
        {


            Validate(GetSkipedFields().ToArray());



            ScrollToControlProperty(GetFirstInvalidPropertyName);

            bool result = !HasErrors;

            if (result)
            {
                result = await CustomValidate();
            }

            return result;
        }

        public virtual List<string> GetSkipedFields()
        {
            List<string> skippedFields = new List<string>();
            return skippedFields;
        }


        string title = string.Empty;
        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        protected bool SetProperty<T>(ref T backingStore, T value,
            [CallerMemberName]string propertyName = "",
            Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            ValidateProperty(value, propertyName);
            NotifyPropertyChanged(propertyName);
            return true;
        }



        public async Task<bool> RestoreData()
        {
            if (!IsBusy)
            {
                IsBusy = true;
                bool conf = await Application.Current.MainPage.DisplayAlert(GeneralMessages.Question, "هل  تريد  الاستمرار في عملية الاستعاده؟", GeneralMessages.Yes, GeneralMessages.No);

                if (conf)
                {
                    bool restoreResult = await DoRestore();
                    if (restoreResult)
                    {
                        IsDeleted = false;
                        SetActionStatus();


                        OnRestoreSuccess?.Invoke();
                        IsBusy = false;
                        return true;
                    }
                    else
                    {
                        OnRestoreFailure?.Invoke();
                    }
                    IsBusy = false;
                    return false;

                }

                IsBusy = false;

            }
            return true;
        }

        public async Task<bool> DeleteData()
        {

            if (!IsBusy)
            {
                IsBusy = true;


                bool conf = await Application.Current.MainPage.DisplayAlert(GeneralMessages.Question, "هل  تريد  الاستمرار في عملية الحذف؟", GeneralMessages.Yes, GeneralMessages.No);

                if (conf)
                {
                    bool deleteResult = await DoDelete();
                    if (deleteResult)
                    {
                        IsDeleted = true;
                        SetActionStatus();
                        OnDeleteSuccess?.Invoke(this);
                        IsBusy = false;
                        return true;
                    }
                    else
                    {
                        OnDeleteFailure?.Invoke();
                    }
                    IsBusy = false;
                    return false;

                }
                IsBusy = false;
            }

            return true;
        }



        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        //protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        //{
        //    var changed = PropertyChanged;
        //    if (changed == null)
        //        return;

        //    changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        //}


        protected void NotifyPropertyChanged([CallerMemberName]string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
