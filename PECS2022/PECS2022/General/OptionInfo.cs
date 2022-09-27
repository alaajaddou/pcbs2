using System;
using System.Collections.Generic;
using System.Text;

namespace PECS2022
{
   public  class OptionInfo
    {

        public int Id { get; set; }
        public string Description { get; set; }
        public bool NeedComments { get; set; }


        private static OptionInfo _default;

        public static OptionInfo Default
        {
            get
            {
                if (_default == null)
                {
                    _default = new OptionInfo() { Id = -1, Description = "اختار" };
                }

                return _default;
            }
        }



    }
}
