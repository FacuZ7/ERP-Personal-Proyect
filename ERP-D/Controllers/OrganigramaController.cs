using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ERP_D.Data;
using ERP_D.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Text;
using System.Globalization;
using ERP_D.Helpers;
using System.Security.Claims;
using ERP_D.ViewModels;

namespace ERP_D.Views
{
    public class OrganigramaController : Controller
    {
        private readonly ERPContext _context;

        public OrganigramaController(ERPContext context)
        {
            _context = context;
        }

        [Authorize]
        public IActionResult Index()
        {
            Organigrama org = new Organigrama();

            //agarro gerencia general
            Gerencia gerenciaGral = _context.Gerencias
                .Include(ge => ge.Responsable)
                .ThenInclude(re => re.Empleado)
                .FirstOrDefault(ge => ge.EsGerenciaGeneral); //solo hay 1

            List<Gerencia> gerencias = _context.Gerencias.Where(ge => !ge.EsGerenciaGeneral).ToList(); //agarro las gerencias que no son generales


            org.GerenciaGeneral = gerenciaGral;
            org.Gerencias = gerencias;

            return View(org);
        }

        public IActionResult SubGerencias() 
        {
            List<Gerencia> gerencias = _context.Gerencias
                .Include(ge => ge.Responsable)  // esto es una posición
                .Include(ge => ge.Posiciones) // lista de posiciones que estan en la gerencia
                .ThenInclude(re => re.Empleado) //then include es para el responsable
                .ThenInclude(emp => emp.Posicion)
                .Where(ge => !ge.EsGerenciaGeneral) //agarro las gerencias que no son generales
                .ToList(); 

            return View(gerencias);
        }

    }
}



