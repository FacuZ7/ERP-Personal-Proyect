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
    public class PosicionesController : Controller
    {
        private readonly ERPContext _context;

        public PosicionesController(ERPContext context)
        {
            _context = context;
        }

        // GET: Posiciones
        public async Task<IActionResult> Index()
        {
            var eRPContexto = _context.Posiciones.Include(p => p.Gerencia)
                                                   .Include(p => p.Responsable)
                                                   .Include(p => p.Empleado);

            return View(await eRPContexto.ToListAsync());
        }

        // GET: Posiciones/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var posicion = await _context.Posiciones
                .Include(p => p.Gerencia)
                .Include(p => p.Responsable)
                .Include(p => p.Empleado)
                .FirstOrDefaultAsync(m => m.PosicionId == id);
            if (posicion == null)
            {
                return NotFound();
            }

            return View(posicion);
        }

        [Authorize(Roles = Alias.RolRRHHADMIN)]
        // GET: Posiciones/Create
        public IActionResult Create()
        {
            ViewData["EmpleadoId"] = new SelectList(_context.Empleados
                                                    .Include(em => em.Posicion)
                                                    .Where(em => em.Posicion == null)
                                                    , "EmpleadoId", "Nombre"); 
            ViewData["GerenciaId"] = new SelectList(_context.Gerencias
                                                    ,"GerenciaId", "Nombre");
            ViewData["ResponsableId"] = new SelectList(_context.Posiciones,
                                                        "PosicionId", "Nombre");
            return View();
        }

        // POST: Posiciones/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = Alias.RolRRHHADMIN)]
        public async Task<IActionResult> Create([Bind("PosicionId,Nombre,Descripcion,Sueldo,GerenciaId,ResponsableId")] Posicion posicion)
        {
            if (ModelState.IsValid)
            {
                if (this.PosicionByNameExists(posicion.Nombre))
                {
                    ModelState.AddModelError(string.Empty, MensajesError.ExistePosicion);
                    return View();
                }
                _context.Add(posicion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["GerenciaId"] = new SelectList(_context.Gerencias, "GerenciaId", "Nombre", posicion.GerenciaId);
            ViewData["ResponsableId"] = new SelectList(_context.Posiciones, "PosicionId", "Nombre", posicion.ResponsableId);
            return View(posicion);
        }

        // GET: Posiciones/Edit/5
        [Authorize(Roles = Alias.RolRRHHADMIN)]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var posicion = await _context.Posiciones.FindAsync(id);
            if (posicion == null)
            {
                return NotFound();
            }
            ViewData["GerenciaId"] = new SelectList(_context.Gerencias, "GerenciaId", "Nombre", posicion.GerenciaId);
            ViewData["ResponsableId"] = new SelectList(_context.Posiciones, "PosicionId", "Nombre", posicion.ResponsableId);
            return View(posicion);
        }

        // POST: Posiciones/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = Alias.RolRRHHADMIN)]
        public async Task<IActionResult> Edit(int id, [Bind("PosicionId,Nombre,Descripcion,Sueldo,GerenciaId,ResponsableId")] Posicion posicion)
        {
            if (id != posicion.PosicionId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(posicion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PosicionByIdExists(posicion.PosicionId))
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
            ViewData["GerenciaId"] = new SelectList(_context.Gerencias, "GerenciaId", "Nombre", posicion.GerenciaId);
            ViewData["ResponsableId"] = new SelectList(_context.Posiciones, "PosicionId", "Nombre", posicion.ResponsableId);
            return View(posicion);
        }

        // GET: Posiciones/Delete/5
        [Authorize(Roles = Alias.RolRRHHADMIN)]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var posicion = await _context.Posiciones
                .Include(p => p.Gerencia)
                .Include(p => p.Responsable)
                .FirstOrDefaultAsync(m => m.PosicionId == id);
            if (posicion == null)
            {
                return NotFound();
            }
            return View(posicion);
        }


        [Authorize(Roles = Alias.RolRRHHADMIN)]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var posicion = await _context.Posiciones.FindAsync(id);
            _context.Posiciones.Remove(posicion);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PosicionByIdExists(int id)
        {
            return _context.Posiciones.Any(e => e.PosicionId == id);
        }

        private bool PosicionByNameExists(String nombre)
        {
            return _context.Posiciones.Any(e => e.Nombre == nombre);
        }
    }
}
