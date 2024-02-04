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
    public class Gerencia
    {
        [Display(Name = Alias.GerenciaId)]
        public int GerenciaId { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(100, ErrorMessage = "El campo {0} admite un máximo de {1} caracteres")]
        public string Nombre { get; set; }

        [Display(Name = Alias.EsGerenciaGeneral)]
        public Boolean EsGerenciaGeneral { get; set; } //INICIALMENTE POR DEFECTO ES FALSE


        [Required]
        [Display(Name = Alias.EmpresaId)]
        public int EmpresaId { get; set; }
        public Empresa Empresa { get; set; }

        [Display(Name = Alias.PosicionResponsableId)]
        [ForeignKey("Responsable")]
        public int? PosicionResponsableId { get; set; } //ID DE LA POSICIÓN RESPONSABLE

        [Display(Name = Alias.Responsable)]
        public Posicion Responsable { get; set; } //GERENTE DE LA GERENCIA (POSICION)

        [InverseProperty("Gerencia")]
        public List<Posicion> Posiciones { get; set; } //POSICIONES DE LA GERECNIA

        [ForeignKey("GerenciaSuperior")]
        [Display(Name = Alias.GerenciaSuperiorId)]
        public int? GerenciaSuperiorId { get; set; }

        public Gerencia GerenciaSuperior { get; set; } //GERENCIA DE LA CUAL DEPENDE

        [Display(Name = Alias.CentroDeCostoId)]
        public CentroDeCosto CentroDeCosto { get; set; }

        public string DescripcionGerencia { get { return $"Nombre Gerencia: {Nombre} - Responsable: {Responsable}"; } }


    }
}

    

