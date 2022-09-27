using System.Collections;
using System.Collections.Generic;

namespace PECS2022
{
    public class LookUpInfo
    {
        
        public LookUpInfo()
        {
           
        }

        public int Id { get; set; }
        public int SurveyId { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
     
       public ICollection<LookUpValueInfo> LookUpValues { get; set; }



    }
}
