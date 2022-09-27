using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace PECS2022.Util
{
    public static class Validator
    {
        public static bool IsDigit(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return false;

            return Regex.IsMatch(value, @"^\d+$");
        }

        public static bool IsInRange(int value , int max , int min)
        {
            return  (value >= min && value <= max) ;
        }

        public static bool iSAlphanumeric(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return false;
            Boolean valid = false;
            //^[\u0621-\u064Aa-zA-Z\d\-_\s]+$
            valid = Regex.IsMatch(value, @"^[\u0621-\u064Aa-zA-Z\d\s]+$"); 
            return valid;
        }

        public static bool iSAlpha(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return false;
            Boolean valid = false;
            //  if (Regex.IsMatch(value, @"^([a-zA-Z]+|[a-zA-Z]+\s[a-zA-Z]+)$"))
            if (Regex.IsMatch(value, @"^(\s*[a-zA-Z]+\s*|\s*[a-zA-Z]+\s*[a-zA-Z]+\s*)$"))
                valid = true;
            else if(Regex.IsMatch(value, @"^[\u0621-\u064A ]+$"))
                valid = true;
            return (valid);
        }
        public static bool IsNullable(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return true;

            return false;
        }

        public static bool IsEmail(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return false;

            try
            {
                var addr = new System.Net.Mail.MailAddress(value);

                var host = addr.Host;
                string[] domain = host.Split('.');
                if (domain.Count() == 1)
                {
                    return false;
                }
                else
                {
                    if (domain.Last().Length < 2)
                    {
                        return false;
                    }
                }

            }
            catch (FormatException)
            {
                return false;
            }

            return true;
        }

        public static bool IsWebsite(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return false;

            try
            {
                return Regex.IsMatch(value, @"^(?:http(s)?:\/\/)?[\w.-]+(?:\.[\w\.-]+)+[\w\-\._~:/?#[\]@!\$&'\(\)\*\+,;=.]+$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            }
            catch (FormatException)
            {
                return false;
            }

        }

        public static bool IsTelephone(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return false;

            if (value == "99")
            {
                return true;
            }
            else if (value.Length == 9 && value.Substring(0, 2).EqualsOneOf("02", "04", "08", "09"))
            {
                return true;
            }
        
            return false;
        }

        public static bool IsMobile(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return false;
            if (value == "99")
            {
                return true;
            }
            else if (value.Length != 10 || !value.Substring(0, 2).EqualsOneOf("05"))
            {
                return false;
            }

            return true;
        }

        public static bool IsID(string ID)
        {
            if (string.IsNullOrWhiteSpace(ID) || ID.Length != 9) return false;

            int multipleNum, sum = 0;

            for (int x = 0; x < ID.Length; x++)
            {
                if (x % 2 == 0)
                {
                    multipleNum = int.Parse(ID[x].ToString()) * 1;
                }
                else
                {
                    multipleNum = int.Parse(ID[x].ToString()) * 2;
                }

                sum += CheckSumDigit(multipleNum);
            }

            return sum % 10 == 0;
        }

        private static int CheckSumDigit(int num)
        {
            var temp = num.ToString();
            int y = 0;
            for (int x = 0; x < temp.Length; x++)
            {
                y += int.Parse(temp[x].ToString());
            }
            return y;
        }
    }
}
