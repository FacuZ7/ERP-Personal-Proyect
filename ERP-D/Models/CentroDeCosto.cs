using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using ERP_D.Helpers;

namespace ERP_D.Models
{
    public class CentroDeCosto
    {    
        [Display(Name = Alias.CentroDeCostoId)]
        public int CentroDeCostoId { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(120, ErrorMessage = "El campo {0}, no puede superar {1} caracteres")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Range(1,double.MaxValue, ErrorMessage = "El monto debe estar comprendido entre {1} y {2}")]
        [Display(Name = Alias.MontoMaximo)]
        public double MontoMaximo { get; set; }

        public List<Gasto> Gastos { get; set; } 

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Display(Name = Alias.GerenciaId)]
        public int GerenciaId { get; set; }
        public Gerencia Gerencia { get; set; }

        
    }
}
