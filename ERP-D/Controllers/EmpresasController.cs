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
    public class EmpresasController : Controller
    {
        private readonly ERPContext _context;

        public EmpresasController(ERPContext context)
        {
            _context = context;
        }

        // GET: Empresas
        public async Task<IActionResult> Index()
        {
            var eRPContexto = _context.Empresas
                .Include(e => e.Logo);   
            return View(await eRPContexto.ToListAsync());
        }

        // GET: Empresas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var empresa = await _context.Empresas
                .Include(e => e.Gerencias)
                .Include(e => e.Logo)
                .FirstOrDefaultAsync(m => m.EmpresaId == id);

            if (empresa == null)
            {
                return NotFound();
            }

            return View(empresa);
        }

        // GET: Empresas/Create
        [Authorize(Roles = Alias.RolRRHHADMIN)]
        public IActionResult Create()
        {
            ViewData["GerenciaId"] = new SelectList(_context.Gerencias, "GerenciaId", "Nombre");
            ViewData["ImagenId"] = new SelectList(_context.Imagenes, "ImagenId", "Nombre");
            return View();
        }

        // POST: Empresas/Create
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = Alias.RolRRHHADMIN)]
        public async Task<IActionResult> Create([Bind("EmpresaId,Nombre,Rubro,Direccion,EmailContacto,Telefono,ImagenId")] Empresa empresa)
        {
            if (ModelState.IsValid)
            {
                //NO SE PUEDE DUPLICAR EL NOMBRE DE LA EMPRESA
                if (this.EmpresaExists(empresa.Nombre))
                {
                    ModelState.AddModelError(string.Empty, MensajesError.ExisteEmpresa);
                    return View();
                }
                _context.Add(empresa);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ImagenId"] = new SelectList(_context.Imagenes, "ImagenId", "Nombre", empresa.ImagenId);
            return View(empresa);
        }

        // GET: Empresas/Edit/5
        [Authorize(Roles = Alias.RolAdmin)]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var empresa = await _context.Empresas.FindAsync(id);
            if (empresa == null)
            {
                return NotFound();
            }
            ViewData["ImagenId"] = new SelectList(_context.Imagenes, "ImagenId", "Nombre", empresa.ImagenId);
            return View(empresa);
        }

        // POST: Empresas/Edit/5

        [Authorize(Roles = Alias.RolAdmin)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EmpresaId,Nombre,Rubro,Direccion,EmailContacto,Telefono,ImagenId")] Empresa empresa)
        {
            if (id != empresa.EmpresaId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(empresa);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmpresaExists(empresa.EmpresaId))
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
            ViewData["ImagenId"] = new SelectList(_context.Imagenes, "ImagenId", "Nombre", empresa.ImagenId);
            return View(empresa);
        }

        // GET: Empresas/Delete/5
        [Authorize(Roles = Alias.RolAdmin)]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var empresa = await _context.Empresas
                .Include(e => e.Logo)
                .FirstOrDefaultAsync(m => m.EmpresaId == id);
            if (empresa == null)
            {
                return NotFound();
            }

            return View(empresa);
        }

        // POST: Empresas/Delete/5
        [Authorize(Roles = Alias.RolAdmin)]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var empresa = await _context.Empresas.FindAsync(id);
            _context.Empresas.Remove(empresa);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmpresaExists(int id)
        {
            return _context.Empresas.Any(e => e.EmpresaId == id);
        }

        private bool EmpresaExists(String nombre)
        {
            return _context.Empresas.Any(e => e.Nombre == nombre);
        }


        public List<Gerencia> listaOrdenada()
        {
            List<Gerencia> listaOrdenada;


            listaOrdenada = _context.Gerencias
                                   .ToList();

            return listaOrdenada;
        }
    }
}
