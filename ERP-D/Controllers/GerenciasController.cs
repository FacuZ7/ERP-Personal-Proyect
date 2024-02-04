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
using ERP_D.Helpers;
using ERP_D.ViewModels;

namespace ERP_D.Controllers
{

    [Authorize]
    public class GerenciasController : Controller
    {
        private readonly ERPContext _context;

        public GerenciasController(ERPContext context)
        {
            _context = context;
        }

        // GET: Gerencias
        public async Task<IActionResult> Index()
        {
            var eRPContexto = _context.Gerencias.Include(g => g.Empresa).Include(g => g.GerenciaSuperior).Include(g => g.Responsable).Include(g=> g.Posiciones);
            return View(await eRPContexto.ToListAsync());
        }

        [Authorize(Roles = Alias.RolRRHHADMIN)]
        public async Task<IActionResult> ReporteGerencias()
        {
            var gerencias = await _context.Gerencias
                .Include(g => g.Responsable)
                .Include(g => g.CentroDeCosto)
                    .ThenInclude(cc => cc.Gastos)
                .ToListAsync();

            

            return View(armarReporte(gerencias));
        }


        private List<ReporteGerencias> armarReporte(List<Gerencia> gerencias)
        {
            double sumaGastosGerencia = 0;
            List<ReporteGerencias> reporteGerencias = new List<ReporteGerencias>();
            
            foreach (var gerencia in gerencias)
            {
                if (gerencia.CentroDeCosto != null) // si la gerencia que evaluo tiene un centro de costo
                {
                    sumaGastosGerencia += gerencia.CentroDeCosto.Gastos.Sum(g => g.Monto); // acumulo todos los gastos de la gerencia 
                    reporteGerencias.Add(new ReporteGerencias(gerencia.Nombre, gerencia.Responsable.Nombre, sumaGastosGerencia)); // al reporte agrego un nuevo objeto con las propiedades
                    sumaGastosGerencia = 0; // reinicio contador porque reinicio la suma de gastos segun gerencia.
                }
            };

            return (reporteGerencias.OrderByDescending(rg => rg.gastosTotales).ToList());
        }

        // GET: Gerencias/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gerencia = await _context.Gerencias
                .Include(g => g.Empresa)
                .Include(g => g.GerenciaSuperior)
                .Include(g => g.Responsable)
                .Include(g => g.Posiciones)
                .FirstOrDefaultAsync(m => m.GerenciaId == id);

            if (gerencia == null)
            {
                return NotFound();
            }

            return View(gerencia);
        }

        // GET: Gerencias/Create
        [Authorize(Roles = Alias.RolRRHHADMIN)]
        public IActionResult Create()
        {
           
            ViewData["EmpresaId"] = new SelectList(_context.Empresas, "EmpresaId", "Nombre");
            ViewData["GerenciaSuperiorId"] = new SelectList(_context.Gerencias, "GerenciaId", "Nombre");
            ViewData["PosicionResponsableId"] = new SelectList(_context.Posiciones, "PosicionId", "Nombre");
            ViewBag.ExisteGerenciaGeneral = GerenciaGeneralExists();
            return View();
        }

        // POST: Gerencias/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = Alias.RolRRHHADMIN)]
        public async Task<IActionResult> Create([Bind("GerenciaId,Nombre,EsGerenciaGeneral,EmpresaId,PosicionResponsableId,GerenciaSuperiorId")] Gerencia gerencia)
        {
            if (ModelState.IsValid)
            {
                ViewData["EmpresaId"] = new SelectList(_context.Empresas, "EmpresaId", "Direccion", gerencia.EmpresaId);
                ViewData["GerenciaSuperiorId"] = new SelectList(_context.Gerencias, "GerenciaId", "Nombre", gerencia.GerenciaSuperiorId);
                ViewData["PosicionResponsableId"] = new SelectList(_context.Posiciones, "PosicionId", "Nombre", gerencia.PosicionResponsableId);

                //NO SE PUEDE DUPLICAR GERENCIA GENERAL
                if (this.GerenciaGeneralExists() && gerencia.EsGerenciaGeneral)
                {
                    ModelState.AddModelError(string.Empty, MensajesError.ExisteGerenciaGeneral);
                    return View(gerencia);
                }
                if (this.GerenciaExists(gerencia.Nombre))
                {
                    ModelState.AddModelError(string.Empty, MensajesError.ExisteGerencia);
                    return View(gerencia);
                }

                _context.Add(gerencia);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            
            return View(gerencia);
        }

        // GET: Gerencias/Edit/5
        [Authorize(Roles = Alias.RolRRHHADMIN)]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gerencia = await _context.Gerencias
                .FindAsync(id);

            List<CentroDeCosto> lista = await _context.CentrosDeCosto
                .Where(x => x.GerenciaId == id)
                .ToListAsync();

            //busco la lista yo a pesar de que solo hay una opcion porque necesito que traiga LA UNICA OPCION DISPONIBLE en forma de lista para que funcione el selectList.
            
            if (gerencia == null)
            {
                return NotFound();
            }
            ViewData["EmpresaId"] = new SelectList(_context.Empresas, "EmpresaId", "Direccion", gerencia.EmpresaId);
            ViewData["GerenciaSuperiorId"] = new SelectList(_context.Gerencias, "GerenciaId", "Nombre", gerencia.GerenciaSuperiorId);
            ViewData["PosicionResponsableId"] = new SelectList(_context.Posiciones, "PosicionId", "Nombre", gerencia.PosicionResponsableId);
            ViewData["CentroDeCosto"] = new SelectList(lista, "CentroDeCostoId", "Nombre");

            return View(gerencia);
        }

        // POST: Gerencias/Edit/5
        [Authorize(Roles = Alias.RolRRHHADMIN)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("GerenciaId,Nombre,EsGerenciaGeneral,EmpresaId,PosicionResponsableId,GerenciaSuperiorId,CentroDeCosto")] Gerencia gerencia)
        { 
            if (id != gerencia.GerenciaId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(gerencia);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GerenciaExists(gerencia.GerenciaId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["EmpresaId"] = new SelectList(_context.Empresas, "EmpresaId", "Direccion", gerencia.EmpresaId);
            ViewData["GerenciaSuperiorId"] = new SelectList(_context.Gerencias, "GerenciaId", "Nombre", gerencia.GerenciaSuperiorId);
            ViewData["PosicionResponsableId"] = new SelectList(_context.Posiciones, "PosicionId", "Nombre", gerencia.PosicionResponsableId);
            ViewData["CentroDeCosto"] = new SelectList(_context.CentrosDeCosto, "CentroDeCostoId", "Nombre");
            return View(gerencia);
        }

        // GET: Gerencias/Delete/5
        [Authorize(Roles = Alias.RolRRHHADMIN)]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gerencia = await _context.Gerencias
                .Include(g => g.Empresa)
                .Include(g => g.GerenciaSuperior)
                .Include(g => g.Responsable)
                .FirstOrDefaultAsync(m => m.GerenciaId == id);
            if (gerencia == null)
            {
                return NotFound();
            }

            return View(gerencia);
        }

        // POST: Gerencias/Delete/5
        [Authorize(Roles = Alias.RolRRHHADMIN)]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var gerencia = await _context.Gerencias.FindAsync(id);
            _context.Gerencias.Remove(gerencia);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GerenciaExists(int id)
        {
            return _context.Gerencias.Any(e => e.GerenciaId == id);
        }

        //ES GERENCIA GENERAL
        private bool GerenciaGeneralExists()
        {
            return _context.Gerencias.Any(e => e.EsGerenciaGeneral == true);
        }

        private bool GerenciaExists(String nombre)
        {
            return _context.Gerencias.Any(e => e.Nombre == nombre);
        }
    }
}
