using SQLite;

using System;
using System.Collections.Generic;
using System.Text;

namespace PECS2022.Models
{

    [Table("Governorates")]
    public class Governorate
    {

        [PrimaryKey]
        public Guid ID { get; set; }

        [NotNull]
        [MaxLength(2)]
        public string Code { get; set; }

        [NotNull]
        [MaxLength(2)]
        public string OldCode { get; set; }


        [NotNull]
        public string Description { get; set; }

        public int? AreaId { get; set; }

    }
}
