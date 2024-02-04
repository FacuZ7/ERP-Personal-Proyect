using ERP_D.Helpers;
using ERP_D.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ERP_D.ViewModels
{
    public class EditMyProfile
    {
        public List<Telefono> Telefonos { get; set; }

        public string Direccion { get; set; }

        public int EmpleadoId { get; set; }

        //public IFormFile Foto { get; set; } pendiente de implementar...


    }
}
