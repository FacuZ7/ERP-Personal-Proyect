using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using ERP_D.Helpers;

namespace ERP_D.Models
{
    public class Imagen
    {
        [Display(Name = Alias.ImagenId)]
        public int ImagenId { get; set; } 


        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [MaxLength(65, ErrorMessage = "El campo {0} admite un maximo de {1} caracteres.")]
        public string Nombre { get; set; }

        [Display(Name = Alias.Path)]
        [Required(ErrorMessage = "El {0} es requerido.")]
        public string Path { get; set; }

        public Empleado Empleado { get; set; }

    }
}
