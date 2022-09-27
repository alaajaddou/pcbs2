using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace PECS2022
{

    [Table("SectionStatus")]
   public class SectionStatus
    {

        [PrimaryKey]
        public Guid ID { get; set; }
       
        public string ID00 { get; set; }


        public int SectionId { get; set; }


        public int? HR01 { get; set; }

        public int CurrentStatusId { get; set; }
    }
}
