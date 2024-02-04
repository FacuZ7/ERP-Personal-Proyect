using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Microsoft.EntityFrameworkCore;
using ERP_D.Helpers;

namespace ERP_D.Models
{
    public class Empleado:Persona
    {   
        [Required(ErrorMessage="El campo {0} es requerido")]
        [Display(Name = Alias.PosicionId)] 
        public int PosicionId { get; set; }
        public Posicion Posicion{ get; set; }
        
        [Display(Name = Alias.ObraSocial)] 
        public ObraSocial ObraSocial { get; set; }

        [Display(Name = Alias.EstadoEmpleado)]
        public Boolean EstadoEmpleado { get; set; }

        
        
    }
}
