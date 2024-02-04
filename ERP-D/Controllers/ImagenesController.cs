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

namespace ERP_D.Controllers
{
    [Authorize]
    public class ImagenesController : Controller
    {
        private readonly ERPContext _context;

        public ImagenesController(ERPContext context)
        {
            _context = context;
        }

        // GET: Imagenes
        public async Task<IActionResult> Index()
        {
            return View(await _context.Imagenes.ToListAsync());
        }

        // GET: Imagenes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var imagen = await _context.Imagenes
                .FirstOrDefaultAsync(m => m.ImagenId == id);
            if (imagen == null)
            {
                return NotFound();
            }

            return View(imagen);
        }

        // GET: Imagenes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Imagenes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ImagenId,Nombre,Path")] Imagen imagen)
        {
            if (ModelState.IsValid)
            {
                _context.Add(imagen);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(imagen);
        }

        // GET: Imagenes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var imagen = await _context.Imagenes.FindAsync(id);
            if (imagen == null)
            {
                return NotFound();
            }
            return View(imagen);
        }

        // POST: Imagenes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ImagenId,Nombre,Path")] Imagen imagen)
        {
            if (id != imagen.ImagenId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(imagen);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ImagenExists(imagen.ImagenId))
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
            return View(imagen);
        }

        // GET: Imagenes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var imagen = await _context.Imagenes
                .FirstOrDefaultAsync(m => m.ImagenId == id);
            if (imagen == null)
            {
                return NotFound();
            }

            return View(imagen);
        }

        // POST: Imagenes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var imagen = await _context.Imagenes.FindAsync(id);
            _context.Imagenes.Remove(imagen);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ImagenExists(int id)
        {
            return _context.Imagenes.Any(e => e.ImagenId == id);
        }
    }
}
