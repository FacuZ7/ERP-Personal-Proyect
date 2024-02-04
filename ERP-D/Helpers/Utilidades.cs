using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ERP_D.Helpers
{
    public class Utilidades
    {
        public static int GetUserId(ClaimsPrincipal user)
        {
            return Int32.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier));
        }

    }
}
