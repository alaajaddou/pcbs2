using PECS2022.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace PECS2022.General
{
   public static class Expressions
    {
        public static Expression<Func<SampleInfo, bool>> SamplesNotInBuildings = x => x.ID01 == GeneralApplicationSettings.LocationForm.Governorate.Code && x.ID02 == GeneralApplicationSettings.LocationForm.Locality.Code && (x.ID03 == null || x.ID05 == null);
    }
}
