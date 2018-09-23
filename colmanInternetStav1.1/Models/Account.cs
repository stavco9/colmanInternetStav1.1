using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Security.Principal;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.AspNetCore.Mvc;

namespace colmanInternetStav1._1.Models
{
    public class Account
    {
        private static ColmanInternetiotContext dbContext;

        static Account()
        {
            dbContext = new ColmanInternetiotContext();
        }

        public static bool isLoggedIn(ClaimsPrincipal principal)
        {
            return true;
            return (principal.Identity.IsAuthenticated);
        }

        public static bool isAdmin(ClaimsPrincipal principal)
        {
            return true;
            if (isLoggedIn(principal))
            {
                return (dbContext.Users.SingleOrDefault(
                    u => u.NameId == principal.FindFirst("nameidentifier").Value
                    ).IsAdmin);
            }

            return (false);
        }

        public static Dictionary<string, string> getDetails(ClaimsPrincipal principal)
        {
            return new Dictionary<string, string>()
            {
                { "nameid","0" },
                {"fullname","Diana Prince" },
                {"firstname","Diana" },
                {"lastname","Prince" },
                {"gender","female" },
                {"emailaddress","dianap9@gmail.com" }
            };
            Dictionary<string, string> d = new Dictionary<string, string>();
            d.Add("nameid", principal.Claims.FirstOrDefault(c => c.Type.Contains("nameidentifier")).Value);
            d.Add("fullname", principal.Claims.FirstOrDefault(c => c.Type.Contains("/name")).Value);
            d.Add("firstname", principal.Claims.FirstOrDefault(c => c.Type.Contains("givenname")).Value);
            d.Add("emailaddress", principal.Claims.FirstOrDefault(c => c.Type.Contains("emailaddress")).Value);
            d.Add("lastname", principal.Claims.FirstOrDefault(c => c.Type.Contains("surname")).Value);
            d.Add("gender", "male");

            return (d);
        }
    }
}
