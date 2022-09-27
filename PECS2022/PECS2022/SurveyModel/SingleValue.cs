using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace PECS2022
{
  public  class SingleValue
    {

       
       
        public int SEQId { get; set; }



        public string Value { get; set; }
        public string Comments { get; set; }

        public bool HasComments { get; set; }
    }
}
