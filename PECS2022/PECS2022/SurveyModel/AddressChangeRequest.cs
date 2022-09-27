using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace PECS2022
{

    [Table("AddressChangeRequests")]
    public class AddressChangeRequest
    {


        [PrimaryKey]
        public Guid Id { get; set; }
        public string ID00 { get; set; }
      
      

        public string ID1 { get; set; }

        public string ID2 { get; set; }

        public int? ID3 { get; set; }

        public int? ID4 { get; set; }

        public int? ID5 { get; set; }

        public string Address { get; set; }

        public string QC03_1 { get; set; }
        public string QC3_2 { get; set; }

        public string QC3_3 { get; set; }

       
        public string QC4_1 { get; set; }

        public string QC4_2 { get; set; }

        public bool NeedSend { get; set; }
        public string CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }
        public string LastModifiedBy { get; set; }

        public DateTime LastModifiedDate { get; set; }
      
    }
}
