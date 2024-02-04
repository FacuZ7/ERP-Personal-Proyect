using ERP_D.Data;
using ERP_D.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERP_D
{
    public class Startup
    {
        
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
           
            try
            {
                _dbInMemory = Configuration.GetValue<bool>("DBInMem");
            }
            catch
            {
                _dbInMemory = true; 
            }
           
        }

        public IConfiguration Configuration { get; }

        private bool  _dbInMemory=false;
        public void ConfigureServices(IServiceCollection services)
        {
           // CONFIGURACION: TIPO DE DB A USAR 
             if(_dbInMemory)
            {
                services.AddDbContext<ERPContext>(options => options.UseInMemoryDatabase("MiBaseDeDatosEnMemoria"));
            }
            else
            {
                services.AddDbContext<ERPContext>(options => options.UseSqlServer(Configuration.GetConnectionString("ERPDCS"))
                    );
            }

            services.AddIdentity<Persona, Rol>().AddEntityFrameworkStores<ERPContext>();

            services.Configure<IdentityOptions>(
                opciones =>
                {
                    opciones.Password.RequiredLength = 4;
                    opciones.Password.RequireNonAlphanumeric = false;
                    opciones.Password.RequireUppercase = false;
                    opciones.Password.RequireLowercase = false;
                    opciones.Password.RequiredUniqueChars = 0;
                    
                    opciones.Password.RequireDigit = false;

        //LONGITUD MINIMA - INICIALMENTE 6
                    //PASSWORD POR DEFECTO: Password1!
                }
                );
            services.PostConfigure<CookieAuthenticationOptions>(IdentityConstants.ApplicationScheme,
             opciones =>
             {
                 opciones.LoginPath = "/Account/IniciarSesion";
                 opciones.AccessDeniedPath = "/Account/AccessDenied";
                 opciones.Cookie.Name = "ERPCookie";
             });

            services.AddControllersWithViews();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ERPContext erpContext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            
            if(!_dbInMemory)
            {
                erpContext.Database.Migrate();
            }
            
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
