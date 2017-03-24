using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;

namespace LexiconLMS.Extensions
{
    public static class ExtensionMethods
    {
        public static string GetName(this IIdentity identity)
        {
            return ((ClaimsIdentity)identity).FindFirstValue("Namn");
        }
    }
}