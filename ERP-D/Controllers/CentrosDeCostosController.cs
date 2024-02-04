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

namespace ERP_D.Controllers
{
    [Authorize]
    public class CentrosDeCostosController : Controller
    {
        private readonly ERPContext _context;

        public CentrosDeCostosController(ERPContext context)
        {
            _context = context;
        }

        // GET: CentrossDeCostos
        public async Task<IActionResult> Index()
        {
            var eRPContexto = _context.CentrosDeCosto.Include(c => c.Gerencia);
            return View(await eRPContexto.ToListAsync());
        }

        // GET: CentrossDeCostos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var centroDeCosto = await _context.CentrosDeCosto
                .Include(c => c.Gerencia)
                .Include(c => c.Gastos)
                    .ThenInclude(c => c.Empleado)
                .FirstOrDefaultAsync(m => m.CentroDeCostoId == id);

            if (centroDeCosto == null)
            {
                return NotFound();
            }

            return View(centroDeCosto);
        }

        // GET: CentrossDeCostos/Create
        [Authorize(Roles = Alias.RolRRHHADMIN)]
        public IActionResult Create()
        {
            ViewData["GerenciaId"] = new SelectList(_context.Gerencias
                                                   .Where(em => em.CentroDeCosto == null)
                                                   , "GerenciaId", "Nombre");
            return View();
        }

        [Authorize(Roles = Alias.RolRRHHADMIN)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CentroDeCostoId,Nombre,MontoMaximo,GerenciaId")] CentroDeCosto centroDeCosto)
        {
            if (ModelState.IsValid)
            {
                //NO SE PUEDE DUPLICAR EL NOMBRE DEL CENTRO DE COSTO
                if (this.CentroDeCostoExists(centroDeCosto.Nombre))
                {
                   ModelState.AddModelError(string.Empty, MensajesError.ExisteCentroDeCosto);
                    return View();
                } 

                // solucionado en base de datos al ponerle a la columna nombre "unique"
                _context.Add(centroDeCosto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["GerenciaId"] = new SelectList(_context.Gerencias, "GerenciaId", "Nombre", centroDeCosto.GerenciaId);
            return View(centroDeCosto);
        }

        // GET: CentrossDeCostos/Edit/5
        [Authorize(Roles = Alias.RolRRHHADMIN)]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var centroDeCosto = await _context.CentrosDeCosto.FindAsync(id);
            if (centroDeCosto == null)
            {
                return NotFound();
            }
            ViewData["GerenciaId"] = new SelectList(_context.Gerencias, "GerenciaId", "Nombre", centroDeCosto.GerenciaId);
            return View(centroDeCosto);
        }

        
        [Authorize(Roles = Alias.RolRRHHADMIN)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CentroDeCostoId,Nombre,MontoMaximo,GerenciaId")] CentroDeCosto centroDeCosto)
        {
            if (id != centroDeCosto.CentroDeCostoId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(centroDeCosto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CentroDeCostoExists(centroDeCosto.CentroDeCostoId))
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
            ViewData["GerenciaId"] = new SelectList(_context.Gerencias, "GerenciaId", "Nombre", centroDeCosto.GerenciaId);
            return View(centroDeCosto);
        }

        // GET: CentrossDeCostos/Delete/5
        [Authorize(Roles = Alias.RolRRHHADMIN)]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var centroDeCosto = await _context.CentrosDeCosto
                .Include(c => c.Gerencia)
                .FirstOrDefaultAsync(m => m.CentroDeCostoId == id);
            if (centroDeCosto == null)
            {
                return NotFound();
            }

            return View(centroDeCosto);
        }

        [Authorize(Roles = Alias.RolRRHHADMIN)]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var centroDeCosto = await _context.CentrosDeCosto.FindAsync(id);
            _context.CentrosDeCosto.Remove(centroDeCosto);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CentroDeCostoExists(int id)
        {
            return _context.CentrosDeCosto.Any(e => e.CentroDeCostoId == id);
        }

        private bool CentroDeCostoExists(String nombre)
        {
            return _context.CentrosDeCosto.Any(e => e.Nombre == nombre);
        }
    }
}
