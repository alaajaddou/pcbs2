using SQLite;

using System;
using System.Collections.Generic;
using System.Text;

namespace PECS2022.Models
{

    [Table("EconomicActivities")]
    public class EconomicActivity
    {


        [PrimaryKey]
        public int Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string ActivityGroup { get; set; }

        public int? ParentId { get; set; }
    }
}
