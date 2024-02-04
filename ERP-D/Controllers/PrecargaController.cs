using ERP_D.Data;
using ERP_D.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace ERP_D.Controllers
{
    public class PrecargaController : Controller
    {
        private readonly ERPContext _context;
        private readonly UserManager<Persona> _usermanager;
        private readonly RoleManager<Rol> _rolManager;

        public PrecargaController(ERPContext contexto, UserManager<Persona> usermanager, RoleManager<Rol> roleManager)
        {
            this._context = contexto;
            this._usermanager = usermanager;
            this._rolManager = roleManager;
        }
        //CREACION DE ROLES
        public async Task CrearRolesBase()
        {
            List<string> roles = new List<string>() { "ADMINISTRADOR", "EMPLEADO RRHH", "EMPLEADO COMUN" };
            if (!_context.Roles.Any())
            {
                foreach (string rol in roles)
                {
                    await CrearRol(rol);
                }
            }
        }

        public async Task CrearRol(string rolName)
        {
            if (!await _rolManager.RoleExistsAsync(rolName))
            {
                await _rolManager.CreateAsync(new Rol(rolName));
            }
        }

        public async Task<IActionResult> PrecargaGeneral() // NO CARGA GASTOS!, 
        {
            _context.Database.EnsureDeleted();
            _context.Database.Migrate();
            CrearRolesBase().Wait();
            CargarAdmin().Wait();
            CargaEmpresa().Wait();
            CrearPruebas().Wait();
            CargarEmpleadoRRHH().Wait();
            CargarEmpleados().Wait();
            CargarGastos().Wait();
            


            return RedirectToAction("Index", "Home");
        }

        

        private async Task CargarAdmin()
        {
            var hayAdmin = _context.Personas.IgnoreQueryFilters().Any(p => p.NormalizedEmail == "admin@admin.com");

            if (!hayAdmin)
            {
                string nombre = "admin";
                string mail = $"{nombre}@erp.com";
                string pass = "Password1!";

                Persona persona = new Persona() { UserName = nombre, Email = mail };
                var resultado = await _usermanager.CreateAsync(persona, pass);

                if (resultado.Succeeded)
                {
                    IdentityResult resuAddRole = await _usermanager.AddToRoleAsync(persona, "ADMINISTRADOR");

                }
            }

        }
        private async Task CargarEmpleadoRRHH()
        {
            string login = "empleadorrhh";
            string mail = $"{login}@erp.com";
            string pass = "Password1!";

            Empleado emp = new Empleado() {
                UserName = login,
                Email = mail,
                Nombre = "Barbara",
                Apellido = "Rizzo",
                DNI = "38682627",
                Direccion = "Ramos Mejía",
                FechaAlta = DateTime.Now,
                PosicionId = 7, //recursos humanos
                ObraSocial = ObraSocial.Osde,
                EstadoEmpleado = true
            };

            var resultado = await _usermanager.CreateAsync(emp, pass);

            if (resultado.Succeeded)
            {
                IdentityResult resuAddRole = await _usermanager.AddToRoleAsync(emp, "EMPLEADO RRHH");

            }

        }

        private async Task CargarEmpleados()
        { 
            string nombre = "Carli";
            string mail = "carlosgarcia@erp.com";
            const string pass = "Password1!";
            string apellido = "Garcia";
            string DNI = "42846357"; 

            Empleado nuevo0 = new Empleado() {
                UserName = mail,
                Email = mail,
                Nombre = nombre,
                Apellido = apellido,
                DNI = DNI,
                Direccion = "Av. Corrientes 3100",
                FechaAlta = DateTime.Now,
                PosicionId = 5,   //soporte imple
                ObraSocial = ObraSocial.Omint,
                EstadoEmpleado = true

            };
            var resultado = await _usermanager.CreateAsync(nuevo0, pass);

            if (resultado.Succeeded)
            {
                IdentityResult resuAddRole = await _usermanager.AddToRoleAsync(nuevo0, "EMPLEADO COMUN");
            }

            mail = "mauroardolino@erp.com";
            nombre = "Mauro";
            apellido = "Ardolino";
            DNI = "21976582";

            Empleado nuevo1 = new Empleado()
            {
                UserName = mail,
                Email = mail,
                Nombre = nombre,
                Apellido = apellido,
                DNI = DNI,
                Direccion = "Ayacucho 1668",
                FechaAlta = DateTime.Now,
                PosicionId = 4,   //CEO
                ObraSocial = ObraSocial.Osde,
                EstadoEmpleado = true
            };
                
            resultado = await _usermanager.CreateAsync(nuevo1, pass);

            if (resultado.Succeeded)
            {
                IdentityResult resuAddRole = await _usermanager.AddToRoleAsync(nuevo1, "EMPLEADO COMUN");
            }

            mail = "facundozapata@erp.com";
            nombre = "Facundo";
            apellido = "Zapata";
            DNI = "41396372";

            Empleado nuevo2 = new Empleado()
            {
                UserName = mail,
                Email = mail,
                Nombre = nombre,
                Apellido = apellido,
                DNI = DNI,
                Direccion = "Pacheco 1800",
                FechaAlta = DateTime.Now,
                PosicionId = 3,   //coord. desarrollo
                ObraSocial = ObraSocial.Osde,
                EstadoEmpleado = true

            };
                
            resultado = await _usermanager.CreateAsync(nuevo2, pass);

            if (resultado.Succeeded)
            {
                IdentityResult resuAddRole = await _usermanager.AddToRoleAsync(nuevo2, "EMPLEADO COMUN");
            }

            mail = "jastracarrubba@erp.com";
            nombre = "Jastra";
            apellido = "Carrubba";
            DNI = "39827644";

            Empleado nuevo3 = new Empleado()
            {
                UserName = mail,
                Email = mail,
                Nombre = nombre,
                Apellido = apellido,
                DNI = DNI,
                Direccion = "Bauness 2799",
                FechaAlta = DateTime.Now,
                PosicionId = 6,   //soporte general
                ObraSocial = ObraSocial.Galeno,
                EstadoEmpleado = true

            };
                
            resultado = await _usermanager.CreateAsync(nuevo3, pass);

            if (resultado.Succeeded)
            {
                IdentityResult resuAddRole = await _usermanager.AddToRoleAsync(nuevo3, "EMPLEADO COMUN");
            }

        }

        private async Task CrearPruebas()
        {

            Gerencia g1 = new Gerencia() { Nombre = "Gerencia General", EsGerenciaGeneral = true, EmpresaId = 1 };
            Gerencia g2 = new Gerencia() { Nombre = "Desarrollo", EsGerenciaGeneral = false, EmpresaId = 1 };
            Gerencia g3 = new Gerencia() { Nombre = "Soporte", EsGerenciaGeneral = false, EmpresaId = 1 };
            Gerencia g4 = new Gerencia() { Nombre = "Cobranzas", EsGerenciaGeneral = false, EmpresaId = 1 };

            _context.Add(g1);
            _context.Add(g2);
            _context.Add(g3);
            _context.Add(g4);
            await _context.SaveChangesAsync();

            //HASTA ACA TODO OK.

            //POSICIONES
            
            Posicion p1 = new Posicion() { Nombre = "CEO", Sueldo = 500000, GerenciaId = g1.GerenciaId, ResponsableId = 4}; // CEO ES DE GERENCIA GENERAL - mauro
            Posicion p2 = new Posicion() { Nombre = "Coordinador Desarrollo", Sueldo = 200000, GerenciaId = g2.GerenciaId, ResponsableId = 4}; // Coord. desa es de desarrollo - ariel
            Posicion coordSopo = new Posicion() { Nombre = "Coordinador Soporte", Sueldo = 200000, GerenciaId = g3.GerenciaId, ResponsableId = 4};
            Posicion coordCobr = new Posicion() { Nombre = "Coordinador Cobranzas", Sueldo = 200000, GerenciaId = g4.GerenciaId, ResponsableId = 4};


            _context.Add(p1);
            _context.Add(p2);
            _context.Add(coordSopo);
            _context.Add(coordCobr);
            await _context.SaveChangesAsync();
            
            g1.PosicionResponsableId = p1.PosicionId;
            g2.PosicionResponsableId = p2.PosicionId;
            g3.PosicionResponsableId = coordSopo.PosicionId;
            g4.PosicionResponsableId = coordCobr.PosicionId;

            _context.Update(g1);
            _context.Update(g2);
            _context.Update(g3);
            _context.Update(g4);
            await _context.SaveChangesAsync();

            Posicion p3 = new Posicion() { Nombre = "Recursos Humanos", Sueldo = 200000, GerenciaId = g1.GerenciaId, ResponsableId = p1.PosicionId}; // RRHH, de gerencia 1 y su jefe es el jefe
            Posicion p4 = new Posicion() { Nombre = "Soporte General", Sueldo = 45000, GerenciaId = g3.GerenciaId, ResponsableId = coordSopo.PosicionId}; // soporte de gerencia 1 y su jefe es el jefe.
            Posicion p5 = new Posicion() { Nombre = "Soporte Implementaciones", Sueldo = 45000, GerenciaId = g3.GerenciaId, ResponsableId = coordSopo.PosicionId };
            _context.Add(p3); 
            _context.Add(p4);
            _context.Add(p5);
            await _context.SaveChangesAsync();

            CentroDeCosto cc1 = new CentroDeCosto() { Nombre = "Hardware y tecnologia", MontoMaximo = 150000, GerenciaId = g2.GerenciaId };
            CentroDeCosto cc2 = new CentroDeCosto() { Nombre = "Capacitaciones", MontoMaximo = 100000, GerenciaId = g3.GerenciaId };
            CentroDeCosto cc3 = new CentroDeCosto() { Nombre = "Marketing", MontoMaximo = 80000, GerenciaId = g1.GerenciaId };
            CentroDeCosto cc4 = new CentroDeCosto() { Nombre = "Comida", MontoMaximo = 50000, GerenciaId = g4.GerenciaId };

            _context.Add(cc1);
            _context.Add(cc2);
            _context.Add(cc3);
            _context.Add(cc4);
            await _context.SaveChangesAsync();

        }

        private async Task CargarGastos()
        {
            Gasto gasto1 = new Gasto() { Descripcion = "Capacitación nuevo empleado", Monto = 1000, FechaGasto = DateTime.Now, EmpleadoID = 3, CentroDeCostoId = 3 };
            Gasto gasto2 = new Gasto() { Descripcion = "Hardware PCs Oficina", Monto = 60000, FechaGasto = DateTime.Now, EmpleadoID = 5, CentroDeCostoId = 4 };
            Gasto gasto3 = new Gasto() { Descripcion = "Impresora Nueva", Monto = 25000, FechaGasto = DateTime.Now, EmpleadoID = 2, CentroDeCostoId = 2 };

            _context.Add(gasto1);
            _context.Add(gasto2);
            _context.Add(gasto3);
            await _context.SaveChangesAsync();
        }

        private async Task CargaEmpresa()
        {
            string nombre = "ERP";
            string rubro = "Software";
            string direccion = "Enrique Santos Discepolo 1859";
            string email = "erp@erp.com";
            string telefono = "1122334455";

            Empresa nuevaEmpresa = new Empresa()
            {
                Nombre = nombre,
                Rubro = rubro,
                Direccion = direccion,
                EmailContacto = email,
                Telefono = telefono
            };
            _context.Add(nuevaEmpresa);
            await _context.SaveChangesAsync();
        }
           
    }
}
    

