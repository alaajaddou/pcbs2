using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace PECS2022.Interfaces
{
    public interface ILocalize
    {
        CultureInfo GetCurrentCultureInfo();
        void SetLocale(CultureInfo ci);
    }
}
