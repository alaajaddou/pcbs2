using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace PECS2022.Models
{

    [Table("Building")]
     public class Building
    {


        [PrimaryKey]
        public string ID4 { get; set; }

        public string Owner { get; set; }
        public int DomainId { get; set; }

       public int NumberOfFloor { get; set; }


        public double GPS_X { get; set; }
        public double GPS_Y { get; set; }

        

        public bool IsSent { get; set; }
        public string CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
