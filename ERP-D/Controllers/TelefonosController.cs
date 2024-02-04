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
using System.Security.Claims;
using ERP_D.Helpers;

namespace ERP_D.Controllers
{
    [Authorize]
    public class TelefonosController : Controller
    {
        private readonly ERPContext _context;

        public TelefonosController(ERPContext context)
        {
            _context = context;
        }

        // GET: Telefonos
        public async Task<IActionResult> Index()
        {
            List<Telefono> telefonos = new List<Telefono>();
            var userID = Utilidades.GetUserId(User);

            if (User.IsInRole(Alias.RolComun)) // si es un usuario comun solo ve sus telefonos
            { 
                telefonos = await _context.Telefonos.Include(t => t.Empleado).Where(te => te.EmpleadoId == userID).ToListAsync();

                return View(telefonos);
            }

            telefonos = await _context.Telefonos.Include(t => t.Empleado).ToListAsync(); //si es RRHH ve todos los telefonos

            return View(telefonos);
        }

        // GET: Telefonos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var telefono = await _context.Telefonos
                .Include(t => t.Empleado)
                .FirstOrDefaultAsync(m => m.TelefonoId == id);
            if (telefono == null)
            {
                return NotFound();
            }

            return View(telefono);
        }

        // GET: Telefonos/Create
        public IActionResult Create()
        {
            var userId = Utilidades.GetUserId(User);
            
            if(User.IsInRole("EMPLEADO RRHH"))
            {
                ViewData["EmpleadoId"] = new SelectList(_context.Empleados, "Id", "NombreCompleto"); //puede crear telefonos para todos             
            }
            else
            {
                ViewData["EmpleadoId"] = userId; //solo crea telefonos para si mismo              
            }
           
             return View();
        }

        // POST: Telefonos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TelefonoId,Numero,TipoTelefono,EmpleadoId")] Telefono telefono)
        {
            if (ModelState.IsValid)
            {
                _context.Add(telefono);
                await _context.SaveChangesAsync();
                Empleado emp = telefono.Empleado;
                return RedirectToAction("Index", "Empleados");
            }
            ViewData["EmpleadoId"] = new SelectList(_context.Empleados, "Id", "Apellido", telefono.EmpleadoId);
            return View(telefono);
        }

        // GET: Telefonos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            // mODIFICACION DE ROL MARIANO
            Telefono telefono;
            if (!User.IsInRole(Alias.RolRRHHADMIN))
            {
                int userId = Utilidades.GetUserId(User);
                telefono = await _context.Telefonos.FirstOrDefaultAsync(t => t.TelefonoId == id.Value && t.EmpleadoId == userId);
            }
            else
            {
                telefono = _context.Telefonos.Find(id);
            }
            
             
            if (telefono == null)
            {
                return NotFound();
            }
            ViewData["EmpleadoId"] = new SelectList(_context.Empleados, "Id", "Apellido", telefono.EmpleadoId);
            return View(telefono);
        }

        // POST: Telefonos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TelefonoId,Numero,TipoTelefono,EmpleadoId")] Telefono telefono)
        {
            if (id != telefono.TelefonoId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(telefono);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TelefonoExists(telefono.TelefonoId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "Empleados");
            }
            ViewData["EmpleadoId"] = new SelectList(_context.Empleados, "Id", "Apellido", telefono.EmpleadoId);
            return View(telefono);
        }

        // GET: Telefonos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var telefono = await _context.Telefonos
                .Include(t => t.Empleado)
                .FirstOrDefaultAsync(m => m.TelefonoId == id);
            if (telefono == null)
            {
                return NotFound();
            }

            return View(telefono);
        }

        // POST: Telefonos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var telefono = await _context.Telefonos.FindAsync(id);
            _context.Telefonos.Remove(telefono);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TelefonoExists(int id)
        {
            return _context.Telefonos.Any(e => e.TelefonoId == id);
        }
    }
}
