using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace ERP_D.ViewModels
{
    public class ReporteGerencias
    {
        public ReporteGerencias()
        {

        }
        public ReporteGerencias(string nombre, string responsable, double gastosTotales)
        {
            NombreGerencia = nombre;
            Responsable = responsable;
            this.gastosTotales = gastosTotales;
        }

        [DisplayName("Nombre De Gerencia")]
        public string NombreGerencia { get; set; }

        [DisplayName("Responsable")]
        public string Responsable { get; set; }
        [DisplayName("Gastos Totales")]
        public double gastosTotales { get; set; }
    }
}
