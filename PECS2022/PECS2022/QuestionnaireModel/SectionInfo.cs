using System.Collections.Generic;

namespace PECS2022
{
    public class SectionInfo
    {
      
        public SectionInfo()
        {
            this.Groups = new HashSet<GroupInfo>();
        }

        public int Id { get; set; }
        public int OrderId { get; set; }
      
        public string Description { get; set; }
        public string Comments { get; set; }
        public bool IsActive { get; set; }
    
   

      
        public virtual ICollection<GroupInfo> Groups { get; set; }
        
    }
}
