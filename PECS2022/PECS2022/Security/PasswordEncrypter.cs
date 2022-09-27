using System;
using System.Collections.Generic;
using System.Text;

namespace PECS2022.Security
{
   public class PasswordEncrypter
    {
        public static string EncryptPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password)) return string.Empty;
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(password);
            return System.Convert.ToBase64String(plainTextBytes);
        }
    }
}
