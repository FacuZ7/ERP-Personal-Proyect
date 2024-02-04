using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using ERP_D.Helpers;

namespace ERP_D.Models
{
    public class Posicion
    {

        [Display(Name = Alias.PosicionId)]
        [Key] //RELACION 1 A 1 CON EMPLEADO
        public int PosicionId { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(100, ErrorMessage = "El campo {0} admite un máximo de {1} caracteres")]
        public string Nombre { get; set; }

        [DataType(DataType.MultilineText)]
        [StringLength(8000, ErrorMessage = "El campo {0} admite un máximo de {1} caracteres")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Range(1, double.MaxValue, ErrorMessage = "El monto debe estar comprendido entre {1} y {2}")]
        public double Sueldo { get; set; }

        [ForeignKey("Gerencia")]
        [Display(Name = Alias.GerenciaId)]
        public int GerenciaId { get; set; }
        public Gerencia Gerencia { get; set; } 

        
        public Posicion Responsable { get; set; } 

        [ForeignKey("Responsable")]
        [Display(Name = Alias.ResponsableId)]
        public int? ResponsableId { get; set; } //id del empleado que ocupa la posicion

        public Empleado Empleado { get; set; }
       

        
    }
}
