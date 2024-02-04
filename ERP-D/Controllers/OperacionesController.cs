using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERP_D.Controllers
{
    [Authorize]
    public class OperacionesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
