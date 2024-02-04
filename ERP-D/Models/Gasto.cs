using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using ERP_D.Helpers;

namespace ERP_D.Models
{
    public class Gasto
    {
        [Display(Name = Alias.GastoId)]
        public int GastoId { get; set; }

        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(120,ErrorMessage = "El campo {0}, no puede superar {1} caracteres")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Range(1,double.MaxValue, ErrorMessage = "El monto debe estar comprendido entre {1} y {2}")]
        public double Monto { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Display(Name = Alias.FechaGasto)]
        [DataType(DataType.Date)]
        public DateTime FechaGasto { get; set; }

        public Empleado Empleado { get; set; }

        [Required]
        [Display(Name = Alias.EmpleadoId)]
        public int EmpleadoID { get; set; }
 
        public CentroDeCosto CentroDeCosto { get; set; }
        
        [Required]
        [Display(Name = Alias.CentroDeCostoId)]
        public int CentroDeCostoId { get; set; }

        public string DescripcionGasto { get { return $"Descripción: {Descripcion} // Fecha: {FechaGasto.Day}/{FechaGasto.Month}/{FechaGasto.Year} // Monto: $ {Monto}"; } }



    }
}
