using ERP_D.Helpers;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ERP_D.Models
{
    public class Rol:IdentityRole<int>
    {
        //SON LOS DOS CONSTRUCTORES QUE POSEE LA DEFINICION DE IDENTITYROLE
        //CONSTRUCTOR VACIO. SE MANTENGA UN ROL POR DEFECTO Y USE LA CLASE BASE iDENTITYROLE
        public Rol():base()
        {

        }

        //CONSTRUCTOR PARAMETRIZDO. CREA DIRECTAMENTE UN ROL EN BASE AL sTRING Q RECIBE
        public Rol(string rolName): base(rolName)
        {

        }

        [Display(Name=Alias.RolName)]
        public override string Name
        {
            get { return base.Name; }
            set { base.Name = value; }
        }

    }
}
