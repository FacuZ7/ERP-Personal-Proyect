using ERP_D.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERP_D.Data
{
    public class ERPContext : IdentityDbContext<IdentityUser<int>,IdentityRole<int>,int>  
    {
        public ERPContext(DbContextOptions opcionesDeConfig) : base(opcionesDeConfig)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) 
        {
            base.OnModelCreating(modelBuilder);

            //CAMBIO 4: DEFINICION DE NOMBRE DE TABLAS
            modelBuilder.Entity<IdentityUser<int>>().ToTable("Personas");
            modelBuilder.Entity<IdentityRole<int>>().ToTable("Roles"); 
            modelBuilder.Entity<IdentityUserRole<int>>().ToTable("PersonasRoles");

            // DEFINICION DE VALORES UNICOS.
            modelBuilder.Entity<Empresa>().HasIndex(r => r.Nombre).IsUnique();
            modelBuilder.Entity<Gerencia>().HasIndex(s => s.Nombre).IsUnique();
            modelBuilder.Entity<Posicion>().HasIndex(t => t.Nombre).IsUnique();
            modelBuilder.Entity<CentroDeCosto>().HasIndex(u => u.Nombre).IsUnique();
        }

       
       public DbSet<CentroDeCosto> CentrosDeCosto { get; set; }
       public DbSet<Empleado> Empleados { get; set; } 
       public DbSet<Empresa> Empresas { get; set; }
       public DbSet<Gasto> Gastos { get; set; }
       public DbSet<Gerencia> Gerencias { get; set; }
       public DbSet<Imagen> Imagenes { get; set; } 
       public DbSet<Posicion> Posiciones { get; set; }
       public DbSet<Telefono> Telefonos { get; set; } 
       public DbSet<Persona> Personas { get; set; }
       public DbSet<Rol> Roles { get; set; } 




    }
}
