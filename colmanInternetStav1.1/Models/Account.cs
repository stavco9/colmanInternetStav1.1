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
        public string NameID { get; }

        public string EmailAddress { get; }

        public string FirstName { get; }

        public string LastName { get; }

        public string FullName { get; }

        public string Gender { get; set; }

        private readonly ColmanInternetiotContext _context;

        public Account()
        {

        }

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

        public static bool IsCurrUserAdmin(IEnumerable<Claim> userClaims, ColmanInternetiotContext context)
        {
            Users currUserFromDB = context.Users.SingleOrDefault(m => m.NameId == GetCurrAccount(userClaims).NameID);

            return currUserFromDB.IsAdmin;
            //return bool.Parse(context.Users.SingleOrDefault(m => m.NameId == GetCurrAccount(userClaims).NameID).IsAdmin.ToString());
        }

        public static Account GetCurrAccount(IEnumerable<Claim> userClaims)
        {
            return new Account(userClaims);
        }


    }
}
