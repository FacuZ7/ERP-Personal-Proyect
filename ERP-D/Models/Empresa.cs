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
    public class Empresa
    {
        [Display(Name = Alias.EmpresaId)]
        public int EmpresaId { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(80, ErrorMessage = "El campo {0} admite un maximo de {1} caracteres")]
        public string Nombre { get; set;}

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(50, ErrorMessage = "El campo {0} admite un maximo de {1} caracteres")]
        public string Rubro { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(500, ErrorMessage = "El campo {0} admite un maximo de {1} caracteres")]
        public string Direccion { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [StringLength(320, MinimumLength = 3, ErrorMessage = "El campo {0} debe tener al menos {2} caracteres y no superar {1} caracteres")]
        [RegularExpression(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{1,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "El mail debe tener un formato *@*.*")]
        [Display(Name = Alias.Email)]
        public string EmailContacto { get; set;}

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(21, MinimumLength = 8, ErrorMessage = "El campo {0} admite un maximo de {1} caracteres y un minimo de {2}")]
        public string Telefono { get; set; }
        
        [ForeignKey("Logo")]
        [Display(Name = Alias.ImagenId)]
        public int? ImagenId { get; set; }        
        public Imagen Logo { get; set; }
       
        public List<Gerencia> Gerencias { get; set; }

        

        
    }
}
