using ERP_D.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ERP_D.ViewModels
{
    public class Login
    {
        public string UserName { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }
        public Boolean RememberMe { get; set; }

        [Display(Name = Alias.DNI)] 
        public string DNI { get; set; }
    }
}
