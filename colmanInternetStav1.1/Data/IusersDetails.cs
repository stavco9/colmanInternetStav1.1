using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using colmanInternetStav1._1.Models;
using colmanInternetStav1._1.Controllers;

namespace colmanInternetStav1._1.Data
{
    internal interface IusersDetails : IDisposable
    {
        IEnumerable<Users> GetUsers();
    }
}