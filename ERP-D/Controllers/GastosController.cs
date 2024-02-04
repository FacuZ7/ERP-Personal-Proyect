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
using ERP_D.Helpers;
using System.Security.Claims;

namespace ERP_D.Controllers
{
    [Authorize]
    public class GastosController : Controller
    {
        private readonly ERPContext _context;
        private readonly UserManager<Persona> _userManager;

        public GastosController(ERPContext context, UserManager<Persona> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Gastos
        public async Task<IActionResult> Index()
        {
            Empleado user = _context.Empleados.Find(Utilidades.GetUserId(User));
            List<Gasto> gastos;

            if (User.IsInRole(Alias.RolRRHH)) // muestro gastos de todos
            {
                gastos = await _context.Gastos
                    .Include(x => x.Empleado)
                    .Include(x => x.CentroDeCosto)
                        .ThenInclude(x => x.Gerencia)
                    .OrderByDescending(x => x.FechaGasto)
                    .ThenBy(x => x.Empleado.Apellido)
                    .ThenBy(x => x.Empleado.Nombre)
                    .ToListAsync(); 
            }
            else if(User.IsInRole(Alias.RolComun)) // muestro gastos propios
            {
                gastos = await _context.Gastos
                .OrderByDescending(x => x.Monto)
                .ThenBy(x => x.FechaGasto)
                .Where (x => x.EmpleadoID == user.Id)
                .OrderByDescending(x => x.FechaGasto)
                .ToListAsync();
            }
            else // caso admin, muestro gastos de todos, pero sin consultar empleado,centro de costo ni gerencia.
            {
                gastos = await _context.Gastos
                    .OrderByDescending(x => x.Monto)
                    .ThenBy(x => x.FechaGasto)
                    .ToListAsync();
            }
            return View(gastos);
        }

        // GET: Gastos/Details/5
        
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }


            Gasto gasto = await _context.Gastos
                .Include(g => g.CentroDeCosto)
                .Include(g => g.Empleado)
                .FirstOrDefaultAsync(m => m.GastoId == id);

            if (gasto == null)
            {
                return NotFound();
            }
            else
            {
                int userId = Int32.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                if (_context.Gastos.Any(e => e.EmpleadoID == userId))
                {
                    return View("Details",gasto);
                }
                else
                {
                    return View("DetailsOtro", gasto);
                }
            }
            
                              

           
        }

        // GET: Gastos/Create
        [Authorize(Roles = "EMPLEADO COMUN, EMPLEADO RRHH")]
        public IActionResult Create()
        {
            try
            {
                Empleado emp = _context.Empleados
                   .Include(e => e.Posicion)
                       .ThenInclude(p => p.Gerencia)
                       .ThenInclude(g => g.CentroDeCosto)
                       .ThenInclude(cc => cc.Gastos)
                   .First(emp => emp.Id == Utilidades.GetUserId(User));
               
                ViewData["CentroDeCostoId"] = emp.Posicion.Gerencia.CentroDeCosto.CentroDeCostoId;
                ViewData["EmpleadoId"] = emp.Id;
                ViewData["Disponible"] = conseguirMontoRestanteCC(emp.Posicion.Gerencia.CentroDeCosto);

            }
            catch (Exception)
            {
                return RedirectToAction("AccessDenied","Account");
            }
            return View();
        }

        private double conseguirMontoRestanteCC(CentroDeCosto cCosto)
        {
            double restante = 0;
            double sumatoria = 0;

            foreach(var gasto in cCosto.Gastos)
            {
                sumatoria += gasto.Monto;
            }

            restante = cCosto.MontoMaximo - sumatoria;
            return (restante);
        }

        // POST: Gastos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "EMPLEADO COMUN, EMPLEADO RRHH")]
        public async Task<IActionResult> Create([Bind("GastoId,Descripcion,Monto,FechaGasto,EmpleadoID,CentroDeCostoId")] Gasto gasto)
        {
                
            if (ModelState.IsValid)
            {
                //var cCosto = await _context.CentrosDeCosto.FindAsync(gasto.CentroDeCostoId);
                var cCostix = await _context.CentrosDeCosto.Include(cc => cc.Gastos)
                    .FirstOrDefaultAsync(cc => cc.CentroDeCostoId == gasto.CentroDeCostoId);

                if(cCostix != null && gasto.Monto <= conseguirMontoRestanteCC(cCostix))
                {
                    _context.Add(gasto);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("Monto", MensajesError.montoSuperado);
            }
            
            ViewData["CentroDeCostoId"] = gasto.CentroDeCostoId;
            ViewData["EmpleadoID"] = gasto.EmpleadoID;

            return View(gasto);

        }
        // GET: Gastos/Edit/5
        // [Authorize(Roles = "EMPLEADO COMUN")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var gasto = await _context.Gastos.FindAsync(id);
            var userID = Utilidades.GetUserId(User);

            if (gasto == null)
            {
                return NotFound();
            }
            else if(gasto.EmpleadoID != userID)
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            ViewData["CentroDeCostoId"] = gasto.CentroDeCostoId;
            ViewData["EmpleadoID"] = gasto.EmpleadoID;

            //ViewData["CentroDeCostoId"] = new SelectList(_context.CentrosDeCosto.Where(x => x.CentroDeCostoId == gasto.CentroDeCostoId), "CentroDeCostoId", "Nombre", gasto.CentroDeCostoId);
            //ViewData["EmpleadoID"] = new SelectList(_context.Empleados.Where(x => x.Id == gasto.EmpleadoID), "Id", "Apellido", gasto.EmpleadoID);

            return View(gasto);
        }

        // POST: Gastos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("GastoId,Descripcion,Monto,FechaGasto,EmpleadoID,CentroDeCostoId")] Gasto nuevoGasto)
        {

            if (id != nuevoGasto.GastoId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            { 
                try
                {             
                    _context.Update(nuevoGasto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GastoExists(nuevoGasto.GastoId))
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
            ViewData["CentroDeCostoId"] = nuevoGasto.CentroDeCostoId;
            ViewData["EmpleadoID"] = nuevoGasto.EmpleadoID;

            return View(nuevoGasto);
        }

        // GET: Gastos/Delete/5
        // [Authorize(Roles = "EMPLEADO COMUN")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userId = Utilidades.GetUserId(User);
            var gasto = await _context.Gastos
                .Include(g => g.CentroDeCosto)
                .Include(g => g.Empleado)
                .FirstOrDefaultAsync(m => m.GastoId == id);

            if (gasto == null)
            {
                return NotFound();
            }
            else if (gasto.EmpleadoID != userId) {

                return RedirectToAction("AccessDenied", "Account");
            }

            return View(gasto);
        }

        // POST: Gastos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var gasto = await _context.Gastos.FindAsync(id);
            _context.Gastos.Remove(gasto);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GastoExists(int id)
        {
            return _context.Gastos.Any(e => e.GastoId == id);
        }

    }
}
