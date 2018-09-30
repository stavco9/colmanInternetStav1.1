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
        private static bool devMode = false;

        /*
        // DBCONTEXT - NOT IN USE, DELETE THIS LATER
        private static ColmanInternetiotContext dbContext;
        static Account()
        {
            dbContext = new ColmanInternetiotContext();
        }
        */

        public static bool isLoggedIn(ClaimsPrincipal principal)
        {
            if (devMode)
            {
                return true;
            }

            return (principal.Identity.IsAuthenticated);
        }

        public static bool isAdmin(ClaimsPrincipal principal)
        {
            if (devMode)
            {
                return true;
            }

            if (isLoggedIn(principal))
            {
                return (new ColmanInternetiotContext().Users.SingleOrDefault(
                    u => u.NameId == principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value
                    ).IsAdmin);
            }

            return (false);
        }

        public static Dictionary<string, string> getDetails(ClaimsPrincipal principal)
        {
            if (devMode)
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
            }

            Dictionary<string, string> d = new Dictionary<string, string>();
            d.Add("nameid", principal.Claims.FirstOrDefault(c => c.Type.Contains("nameidentifier")).Value);
            d.Add("fullname", principal.Claims.FirstOrDefault(c => c.Type.EndsWith("/name")).Value);
            d.Add("firstname", principal.Claims.FirstOrDefault(c => c.Type.Contains("givenname")).Value);
            d.Add("emailaddress", principal.Claims.FirstOrDefault(c => c.Type.Contains("emailaddress")).Value);
            d.Add("lastname", principal.Claims.FirstOrDefault(c => c.Type.Contains("surname")).Value);
            d.Add("gender", "male");

            return (d);
        }
    }
}
