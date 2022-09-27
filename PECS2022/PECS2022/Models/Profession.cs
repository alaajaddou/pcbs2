using SQLite;

using System;
using System.Collections.Generic;
using System.Text;

namespace PECS2022.Models
{

    [Table("Professions")]
 public  class Profession
    {


        [PrimaryKey]
        public string Code
        {
            get;
            set;
        }


        public string Description
        {
            get;
            set;
        }
    }
}
