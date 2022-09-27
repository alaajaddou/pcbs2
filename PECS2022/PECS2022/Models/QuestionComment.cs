using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace PECS2022.Models
{
  public  class QuestionComment
    {
        [PrimaryKey]
        public Guid ID { get; set; } = Guid.NewGuid();
        public string ASID { get; set;}
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public string CreatedBy { get; set; }
        public string Code { get; set; }
        public string Comments { get; set; }
        public bool IsSent { get; set; } = false;
    }
}
