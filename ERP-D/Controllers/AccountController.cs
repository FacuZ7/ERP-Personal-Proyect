using ERP_D.Data;
using ERP_D.Helpers;
using ERP_D.Models;
using ERP_D.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERP_D.Controllers
{
    
    public class AccountController : Controller
    {
        private readonly UserManager<Persona> _userManager;
        private readonly SignInManager<Persona> _signInManager;
        private readonly ERPContext _contexto;
        private readonly RoleManager<Rol> _rolManager;
        

        public AccountController(
            UserManager<Persona> userManager,
            SignInManager<Persona> signInManager,
            ERPContext contexto, RoleManager<Rol> rolManager)
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
            this._contexto = contexto;
            this._rolManager = rolManager;
        }

        [HttpGet]
        public IActionResult IniciarSesion(string returnurl)
        {
            TempData["returnUrl"] = returnurl;
            return View();
        }

        private bool intentaEntrarAdmin(Login login)
        {
            return login.UserName.ToUpper() == "ADMIN";
        }

        [HttpPost]
        public async Task<IActionResult> IniciarSesion(Login login)
        {
            string returnUrl = TempData["returnUrl"] as string;
            if (ModelState.IsValid)
            {
                if (intentaEntrarAdmin(login) || (!intentaEntrarAdmin(login) && _contexto.Empleados.Any(e => e.NormalizedUserName == login.UserName.ToUpper() && e.EstadoEmpleado)))
                {
                    var resultado = await _signInManager.PasswordSignInAsync(
                    login.UserName,
                    login.Password,
                    isPersistent: login.RememberMe,
                    lockoutOnFailure: false
                    );

                    if (resultado.Succeeded)
                    {
                        if (!string.IsNullOrEmpty(returnUrl))
                        {
                            return Redirect(returnUrl);
                        }
                        return RedirectToAction("Index", "Home");
                    }

                    ModelState.AddModelError(string.Empty, MensajesError.UserOPassInvalidos);
                }   
                else
                {
                    ModelState.AddModelError(string.Empty, MensajesError.UsuarioInhabilitado);
                }
               
            }
            return View();
        }


        [HttpGet]
        public async Task<IActionResult> CerrarSesion()
        {
             await _signInManager.SignOutAsync();

            return RedirectToAction("IniciarSesion", "Account");
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }




    }
}
