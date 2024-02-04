using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using ERP_D.Helpers;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP_D.Models
{
    public class Telefono
        
    {
        [Display(Name = Alias.EmpleadoId)]
        public int TelefonoId { get; set; }
     
        [Required(ErrorMessage="El campo {0} es requerido")]
        [StringLength(21, MinimumLength = 8, ErrorMessage = "El campo {0} admite un maximo de {1} caracteres y un minimo de {2}")]
        public string Numero { get; set; }

        [Required(ErrorMessage="El campo {0} es requerido")]
        [Display(Name = Alias.TipoTelefono)] 
        public TipoTelefono TipoTelefono { get; set; }
        
        
        [Required]
        [Display(Name = Alias.EmpleadoId)] 
        public int EmpleadoId{get; set;}

        public Empleado Empleado {get; set;}

        public string NumeroCompleto { get { return $"({TipoTelefono}) - {Numero}"; } }
     
       


    }
}
