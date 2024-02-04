using ERP_D.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERP_D.ViewModels
{
    //esta a disposición para todos los empleados 
    //primero y principal la gerencia general y quien está a cargo (gerencia general)
    //al hacer click se ven todas las gerencias a las cuales esta gerencia. (gerencias)
    //al visualizar todos los empleados de la gerencia, estos deben ser seleccionables y redirigirme a una tarjeta de contacto
    //la tarjeta de contacto debe tener Apellido Nombre, nombre de la posición telefono e email.
    // SOLO EMPLEADOS ACTIVOS!!
    public class Organigrama
    {
        public Gerencia GerenciaGeneral { get; set; }

        public List<Gerencia> Gerencias { get; set; }
    }
}
