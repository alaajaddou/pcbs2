using SQLite;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PECS2022.Models
{
    [Table("Localitie")]
    public class Locality
    {
        //Code PRIMARY KEY,GovCode, Description
        [PrimaryKey]
        public string Code { get; set; }

        [NotNull]
        [MaxLength(2)]
        public string GovCode { get; set; }

        [NotNull]
        public string Description { get; set; }

        public string GetLocalityFullCode()
        {
            string prefix = GeneralFunctions.GetGovPrefix(GovCode);

            string locCode = $"{prefix}{Code}";

            return locCode;
        }
    }
}
