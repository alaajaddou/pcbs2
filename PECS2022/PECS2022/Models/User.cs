using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace PECS2022.Models
{
    [Table("Users")]
    public class User
    {  //UserName TEXT PRIMARY KEY ,Password,GovCode,FullName,AreaId
        [PrimaryKey]
        public string UserName { get; set; }
        [NotNull]
        public string FullName { get; set; }

        [NotNull]
        public string Password { get; set; }
        public long? AreaId { get; set; }

        [NotNull]
        public string GovCode { get; set; }
    }
}
