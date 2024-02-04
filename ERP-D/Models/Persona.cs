using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using ERP_D.Helpers;
using Microsoft.AspNetCore.Identity;
using System.Collections;

namespace ERP_D.Models
{
    public class Persona :IdentityUser<int> 
    {
         //public int Id { get; set; } 

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [MaxLength(320, ErrorMessage = "El campo {0} admite un maximo de {1} caracteres.")]
        [MinLength(7, ErrorMessage = "El campo {0} debe contener al menos {1} caracteres")]
        [RegularExpression(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{1,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "El mail debe tener un formato *@*.*")]
        [Display(Name = Alias.Email)] 
        [EmailAddress(ErrorMessage = MensajesError.NoValido)]
        public override string Email
        {
            get { return base.Email; }
            set { base.Email = value; }
        }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(30, ErrorMessage = "El campo {0} admite un maximo de {1} caracteres")]
        [RegularExpression(@"^[a-zA-Z\s]*$", ErrorMessage = "El campo {0} solo admite caracteres alfabeticos,primera letra en mayuscula")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [RegularExpression(@"^[a-zA-Z\s]*$", ErrorMessage = "El campo {0} solo admite caracteres alfabeticos,primera letra en mayuscula")]
        [StringLength(30, ErrorMessage = "El campo {0} admite un maximo de {1} caracteres")]
        public string Apellido { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(10, MinimumLength = 7, ErrorMessage = "El campo {0} admite un maximo de {1} caracteres y un minimo de {2}")]
        [Display(Name = Alias.DNI)] 
        public string DNI { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string Direccion { get; set; }

        [Display(Name = Alias.FechaAlta)]
        [DataType(DataType.Date)]
        public DateTime FechaAlta { get; set; }

        public List<Telefono> Telefonos { get; set; }

        [ForeignKey("Foto")]
        [Display(Name = Alias.ImagenId)]
        public int? ImagenId { get; set; } 
        public Imagen Foto { get; set; }

        public string NombreCompleto { get { return $"{Apellido},{Nombre}"; } }

        public List<Gasto> Gastos { get; set; }

        

    }

}
