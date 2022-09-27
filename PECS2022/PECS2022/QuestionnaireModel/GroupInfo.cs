using System.Collections.Generic;

namespace PECS2022
{
    public class GroupInfo
    {

    
        public GroupInfo()
        {
            this.Questions = new HashSet<QuestionInfo>();
        }

        public int Id { get; set; }
        public int OrderId { get; set; }
        public string Description { get; set; }
        public string Comments { get; set; }
        public bool IsActive { get; set; }


   
       
        public virtual ICollection<QuestionInfo> Questions { get; set; }
    }
}
