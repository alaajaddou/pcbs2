using System;
using System.Collections.Generic;

namespace PECS2022
{
    public class FormListInfo
    {

       
        public FormListInfo()
        {
            this.FormListFields = new HashSet<FormListFieldInfo>();
        
        }

        public System.Guid ID { get; set; }
        public string Description { get; set; }
        public string Comments { get; set; }
        public int SurveyId { get; set; }
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public int LastModifiedBy { get; set; }
        public System.DateTime LastModifiedDate { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FormListFieldInfo> FormListFields { get; set; }
      
     
    }
}
