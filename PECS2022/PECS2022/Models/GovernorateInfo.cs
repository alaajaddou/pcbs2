using System;
using System.Collections.Generic;
using System.Text;

namespace PECS2022.Models
{
   public class GovernorateInfo
    {
        private static GovernorateInfo _Default;
        public static GovernorateInfo Default
        {
            get
            {
                if (_Default == null)
                {
                    _Default = new GovernorateInfo() { Code = "", Description = "أختار المحافظة" };
                }

                return _Default;
            }

        }
        public string Code { get; set; }
        public string Description { get; set; }

    }
}
