using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Security.Principal;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace colmanInternetStav1._1.Models
{
    public class Account
    {
        private readonly IHttpContextAccessor _contextAccessor;
        
        public string NameID { get; }

        public string EmailAddress { get; }

        public string FirstName { get; }

        public string LastName { get; }

        public string FullName { get; }

        public string Gender { get; set; }

        public Account(IEnumerable<Claim> userClaims)
        {
           foreach(var claim in userClaims)
            {
                if (claim.Type.Contains("nameidentifier"))
                {
                    this.NameID = claim.Value;
                }

                if (claim.Type.Contains("emailaddress"))
                {
                    this.EmailAddress = claim.Value;
                }

                if (claim.Type.Contains("givenname"))
                {
                    this.FirstName = claim.Value;
                }

                if (claim.Type.Contains("surname"))
                {
                    this.LastName = claim.Value;
                }

                if (claim.Type.Contains("/name"))
                {
                    this.FullName = claim.Value;
                }
            }

            this.Gender = "male";
        }

        public static Account GetCurrAccount(IEnumerable<Claim> userClaims)
        {
            return new Account(userClaims);
        }
    }
}
