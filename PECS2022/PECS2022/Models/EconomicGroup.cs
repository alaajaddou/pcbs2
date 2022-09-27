using SQLite;

using System;
using System.Collections.Generic;
using System.Text;

namespace PECS2022.Models
{

    [Table("EconomicGroup")]
    public class EconomicGroup
    {

        [PrimaryKey]
        public int ID { get; set; }

        [NotNull]
        public string Description { get; set; }
        public string ActivityGroup { get; set; }


        public int OrderId { get; set; }
    }
}
