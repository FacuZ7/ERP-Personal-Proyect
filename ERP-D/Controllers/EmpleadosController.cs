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

namespace ERP_D.Controllers
{
    [Authorize]
    public class EmpleadosController : Controller
    {
        private readonly ERPContext _context;
        private readonly UserManager<Persona> _usermanager;

        public EmpleadosController(ERPContext context, UserManager<Persona> usermanager)
        {
            _context = context;
            _usermanager = usermanager;
        }

        // GET: Empleados
        public async Task<IActionResult> Index()
        {
            List<Empleado> empleados;
            Empleado empleadoLogueado =  _context.Empleados
                .FirstOrDefault(e => e.Id == Utilidades.GetUserId(User));

            if (User.IsInRole(Alias.RolRRHH) || User.IsInRole(Alias.RolAdmin))
            {
                empleados = await _context.Empleados
                    .Include(e => e.Posicion) 
                    .OrderByDescending(e => e.Posicion.Sueldo)
                    .ToListAsync();
            }
            else
            {
                empleados = await _context.Empleados
                .Include(e => e.Foto)
                .Include(e => e.Posicion)
                .Where(e => e.Id == empleadoLogueado.Id)
                .ToListAsync();
            }
            
            return View(empleados);
        }

        // GET: Empleados/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var empleado = await _context.Empleados
                .Include(e => e.Telefonos)
                .Include(e => e.Foto)
                .Include(e => e.Posicion)
                .Include(e => e.Gastos)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (empleado == null)
            {
                return NotFound();
            }

            

            return View(empleado);
        }

        // GET: Empleados/Create
        [Authorize(Roles = Alias.RolRRHHADMIN)]
        public IActionResult Create()
        {
            ViewData["ImagenId"] = new SelectList(_context.Imagenes, "ImagenId", "Nombre");
            ViewData["PosicionId"] = new SelectList(_context.Posiciones
                                                    .Where(em => em.Empleado == null),
                                                    "PosicionId", "Nombre");
            
            return View();
        }

        //private string FormateoUserName(string nombre, string apellido)
        //{
        //    var resultado = nombre[0] + apellido;
        //    return resultado.ToUpper();
        //}

        // POST: Empleados/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = Alias.RolRRHHADMIN)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(bool esRRHH, [Bind("PosicionId,ObraSocial,Id,Email,UserName,Password,Nombre,Apellido,DNI,Direccion,FechaAlta,ImagenId")] Empleado empleado)
        {
            if (ModelState.IsValid)
            {

                empleado.UserName = empleado.Email; 
                empleado.EstadoEmpleado = true;
                empleado.Nombre = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(empleado.Nombre);
                empleado.Apellido = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(empleado.Apellido);

                var resultado = await _usermanager.CreateAsync(empleado, empleado.DNI); // la contraseña es el DNI

                if (resultado.Succeeded)
                {
                    //ya cree el empleado
                    IdentityResult resuAddRole;
                    if (esRRHH)
                    {
                        resuAddRole = await _usermanager.AddToRoleAsync(empleado, "EMPLEADO RRHH");
                    }
                    else
                    {
                        resuAddRole = await _usermanager.AddToRoleAsync(empleado, "EMPLEADO COMUN");
                    }

                    if (resuAddRole.Succeeded)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    if (resuAddRole.Errors.Any())
                    {
                        ModelState.AddModelError(string.Empty, resuAddRole.Errors.First().Description);
                    }
                }
                if (resultado.Errors.Any())
                {
                    ModelState.AddModelError(string.Empty, resultado.Errors.First().Description);
                }


            }
            ViewData["ImagenId"] = new SelectList(_context.Imagenes, "ImagenId", "Nombre", empleado.ImagenId);
            ViewData["PosicionId"] = new SelectList(_context.Posiciones, "PosicionId", "Nombre", empleado.PosicionId);
            return View(empleado);
        }

        // GET: Empleados/Edit/5

        public async Task<IActionResult> Edit(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }
                
            var empleado = await _context.Empleados.Include(te => te.Telefonos).Include(em => em.Posicion).FirstOrDefaultAsync(te => te.Id == id);
            var userId = Utilidades.GetUserId(User);

            if (empleado == null)
            {
                return NotFound();
            }
            else if (empleado.Id == userId) 
            {
                return RedirectToAction("EditMyProfile", new { id = empleado.Id });
            }

                ViewData["ImagenId"] = new SelectList(_context.Imagenes, "ImagenId", "Nombre", empleado.ImagenId);
                ViewData["PosicionId"] = new SelectList(_context.Posiciones, "PosicionId", "Nombre", empleado.PosicionId);
                ViewData["TelefonoId"] = new SelectList(_context.Telefonos, "TelefonoId", "Numero", empleado.Telefonos);

            return View(empleado);
        }

        [HttpGet]
        public async Task<IActionResult> EditMyProfile(int? id)  
        { 
            if(id == null)
            {
                return NotFound();
            }

            var empleado = await _context.Empleados.FindAsync(id);
            List<Telefono> misTelefonos = await _context.Telefonos.Include(te => te.Empleado).Where(te => te.Empleado.Id == empleado.Id).ToListAsync();
            //Imagen miImagen = await _context.Imagenes.FindAsync(2);
            if (empleado.Id != id)
            {
                return RedirectToAction("AccessDenied", "Account");
            }
            else {
                var misDatos = new EditMyProfile();
                misDatos.Direccion = empleado.Direccion;
                misDatos.Telefonos = misTelefonos;
                misDatos.EmpleadoId = empleado.Id;
                //misDatos.Foto = miImagen; pendiente de implementar.
                

                return View("EditMyProfile", misDatos);

            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditMyProfile(int id, [Bind("Direccion,EmpleadoId")] EditMyProfile miPerfil)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var empleadoEnDb = await _context.Empleados.FindAsync(id);

                    if (empleadoEnDb != null)
                    {
                        empleadoEnDb.Direccion = miPerfil.Direccion;
                        //empleadoEnDb.Foto = miPerfil.Foto;
                    }

                    _context.Update(empleadoEnDb);
                    await _context.SaveChangesAsync();

                }
                catch (DbUpdateConcurrencyException)
                {
                    _context.Update(miPerfil);
                    await _context.SaveChangesAsync();

                    if (!EmpleadoExists(miPerfil.EmpleadoId))
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
            return View(miPerfil);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Edit(int id, [Bind("PosicionId,ObraSocial,Id,Email,Nombre,Apellido,DNI,Direccion,FechaAlta,ImagenId")] Empleado empleadoEnFormulario)
        {
            if (id != empleadoEnFormulario.Id)
            {
                return NotFound();
            }
                       

            if (ModelState.IsValid)
            {
                try
                {
                    var empleadoEnDb = _context.Empleados.Find(id);
                    if(empleadoEnDb != null)
                    {
                        empleadoEnDb.PosicionId = empleadoEnFormulario.PosicionId;
                        empleadoEnDb.ObraSocial = empleadoEnFormulario.ObraSocial;
                        empleadoEnDb.DNI = empleadoEnFormulario.DNI;
                        empleadoEnDb.Nombre = empleadoEnFormulario.Nombre;
                        empleadoEnDb.Apellido = empleadoEnFormulario.Apellido;
                        empleadoEnDb.Email = empleadoEnFormulario.Email;
                        empleadoEnDb.Direccion = empleadoEnFormulario.Direccion;
                    }

                    _context.Update(empleadoEnDb);
                  await _context.SaveChangesAsync(); 
                    
                }
                catch (DbUpdateConcurrencyException)
                {
                    _context.Update(empleadoEnFormulario);
                    await _context.SaveChangesAsync();

                    if (!EmpleadoExists(empleadoEnFormulario.Id))
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
            ViewData["ImagenId"] = new SelectList(_context.Imagenes, "ImagenId", "Nombre", empleadoEnFormulario.ImagenId);
            ViewData["PosicionId"] = new SelectList(_context.Posiciones, "PosicionId", "Nombre", empleadoEnFormulario.PosicionId);
            ViewData["TelefonoId"] = new SelectList(_context.Telefonos, "TelefonoId", "Numero", empleadoEnFormulario.Telefonos);
            return View(empleadoEnFormulario);
        }

        // GET: Empleados/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var empleado = await _context.Empleados
                .Include(e => e.Foto)
                .Include(e => e.Posicion)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (empleado == null)
            {
                return NotFound();
            }

            return View(empleado);
        }

        // POST: Empleados/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var empleado = await _context.Empleados.FindAsync(id);
            _context.Empleados.Remove(empleado);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmpleadoExists(int id)
        {
            return _context.Empleados.Any(e => e.Id == id);
        }


        // GET: Empleados/Disable
        public async Task<IActionResult> Disable(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var empleado = await _context.Empleados
                .FirstOrDefaultAsync(m => m.Id == id);
            if (empleado == null)
            {
                return NotFound();
            }

            return View(empleado);
        }

        // POST: Empleados/Disable
        [HttpPost, ActionName("Disable")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DisableConfirmed(int id)
        {
            
            var empleado = await _context.Empleados.FindAsync(id);
            empleado.EstadoEmpleado = false;
            _context.Empleados.Update(empleado);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        // GET: Empleados/Enable
        public async Task<IActionResult> Enable(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var empleado = await _context.Empleados
                .FirstOrDefaultAsync(m => m.Id == id);
            if (empleado == null)
            {
                return NotFound();
            }

            return View(empleado);
        }

        // POST: Empleados/Enable
        [HttpPost, ActionName("Enable")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EnableConfirmed(int id)
        {
            var empleado = await _context.Empleados.FindAsync(id);
            empleado.EstadoEmpleado = true;
        
            _context.Empleados.Update(empleado);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }



    }
}
