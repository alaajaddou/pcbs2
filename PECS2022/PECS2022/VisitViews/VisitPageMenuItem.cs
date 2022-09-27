using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PECS2022.VisitViews
{

    public class VisitPageMenuItem : INotifyPropertyChanged
    {
        public VisitPageMenuItem()
        {
            WizardTypeId = WizardTypeId.VisitForm;
            IsEnabled = true;
        }
        public int Id { get; set; }
        public int? HR01 { get; set; }
        private string title;
        public string Title { get { return title; } set { title = value; OnPropertyChanged(); } }

        public WizardTypeId WizardTypeId { get; set; }


        private CurrentStatus _CurrentStatus;
        public CurrentStatus CurrentStatus
        {
            get
            {
                return _CurrentStatus;
            }

            set
            {
                _CurrentStatus = value;
             ImageURL =  $"state_{_CurrentStatus.ToString().ToLower()}.png";

               

            }
        }


        private string imageURL;
        public string ImageURL
        {
            get
            {

                return imageURL;

            }

          private  set
            {
                imageURL = value;
                OnPropertyChanged();
            }
        }
        public bool IsEnabled { get; set; }

        public int? SectionId { get; set; }

        #region INotifyPropertyChanged Implementation
        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged == null)
                return;

            PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }


    public enum WizardTypeId
    {

        VisitForm =1,
        MembersForm=2,
        Sections=3,
       QuestionEvaluations = 4
    }


  
}