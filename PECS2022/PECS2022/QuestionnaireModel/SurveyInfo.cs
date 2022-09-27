using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PECS2022
{
    public class SurveyInfo
    {

        public SurveyInfo()
        {
            this.FormLists = new HashSet<FormListInfo>();
            this.LookUps = new HashSet<LookUpInfo>();
            this.Reps = new HashSet<RepInfo>();
            this.Sections = new HashSet<SectionInfo>();
        }

        public int ID { get; set; }
        public string Description { get; set; }
        public string Intsractions { get; set; }
        public string ThankYouMessage { get; set; }
        public string Comments { get; set; }
        public int BaseYear { get; set; }
      
        public string CustomLibary { get; set; }
        public int SampleTypeId { get; set; }
        public bool EnableGIS { get; set; }
        public bool EnableGovernorate { get; set; }
        public bool EnableLocality { get; set; }
        public bool SampleGroupEnabled { get; set; }
        public Nullable<int> SampleGroupLookUpId { get; set; }
        public bool FieldOneEnabled { get; set; }
        public bool FieldTwoEnabled { get; set; }
        public bool FieldThreeEnabled { get; set; }
        public bool FieldFourEnabled { get; set; }
        public bool FieldFiveEnabled { get; set; }
        public string FieldOneCaption { get; set; }
        public string FieldTwoCaption { get; set; }
        public string FieldThreeCaption { get; set; }
        public string FieldFourCaption { get; set; }
        public string FieldFiveCaption { get; set; }
        public string SampleGroupCaption { get; set; }
        public bool IsActive { get; set; }
   
        public System.DateTime LastModifiedDate { get; set; }

       
        public virtual ICollection<FormListInfo> FormLists { get; set; }
       
        public virtual ICollection<LookUpInfo> LookUps { get; set; }
      
        public virtual ICollection<RepInfo> Reps { get; set; }
               
        public virtual ICollection<SectionInfo> Sections { get; set; }

        public virtual ICollection<DisplayConditions> DisplayConditions { get; set; }
    }
}