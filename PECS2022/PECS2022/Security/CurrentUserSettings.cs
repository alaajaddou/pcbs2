using PECS2022.Models;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;

namespace PECS2022.Security
{
    public class CurrentUserSettings
    {
        public static bool IsUserLoggedIn { get; set; }


        public static User CurrentUser { get; set; }


        public static AuthenticationHeaderValue GetAuthorizationKey()
        {
          
            if (CurrentUser != null)
            {
                var byteArray = Encoding.ASCII.GetBytes(string.Format("{0}:{1}", CurrentUser.UserName, CurrentUser.Password));
                return new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
            }


            return new AuthenticationHeaderValue("Basic", string.Empty);
        }


       
    }
}
