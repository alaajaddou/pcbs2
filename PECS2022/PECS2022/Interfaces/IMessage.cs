using System;
using System.Collections.Generic;
using System.Text;

namespace PECS2022.Interfaces
{
   public interface IMessage
    {

        void LongAlert(string message);
        void ShortAlert(string message);
    }
}
