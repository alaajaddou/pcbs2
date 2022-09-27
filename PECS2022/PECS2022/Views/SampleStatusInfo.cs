using PECS2022.Models;
using PECS2022.VisitViews;
using System;
using System.Collections.Generic;
using System.Text;

namespace PECS2022.Views
{
  public  class SampleStatusInfo
    {

        public SampleStatusInfo()
        {

            CurrentStatus = CurrentStatus.NotFilled;

        }
        public SampleInfo Sample { get; set; }
        public CurrentStatus CurrentStatus { get; set; }

        public string ImageURL
        {
            get
            {

                return  $"state_{CurrentStatus.ToString().ToLower()}.png";

            }

            
        }


        public string Description
        {
            get
            {
                if (Sample == null) return string.Empty;

               

                return Sample.Description;

            }

           
        }
    }
}
