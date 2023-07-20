using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;
using tcobro_Utilidad;
using tcobro_WEB.Models;
using tcobro_WEB.Models.Dto;
using tcobro_WEB.Services.IServices;

namespace tcobro_WEB.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly IUsuarioService _usuarioService;
        public UsuarioController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }



        /*Login*/
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginRequestDTO modelo)
        {
            var response = await _usuarioService.Login<APIResponse>(modelo);//Devuelve conexion de usuario correcta

            if(response != null && response.IsExitoso == true)
            {
                LoginResponseDTO loginResponseDTO = JsonConvert.DeserializeObject<LoginResponseDTO>(Convert.ToString(response.Resultado));

                //Claims que guarda informacion del usuario para mantener el Email y el Rol en todo momento de inicio de sesion
                var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);

                identity.AddClaim(new Claim(ClaimTypes.Name, loginResponseDTO.Usuario.Email));
                identity.AddClaim(new Claim(ClaimTypes.Role, loginResponseDTO.Usuario.Rol));

                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);


                //Almacenamiento de datos de sesion grabado con Token
                HttpContext.Session.SetString(DefinicionesEstaticas.SessionToken, loginResponseDTO.Token);
                return RedirectToAction("Index", "Home");//Vista, Controlador
            }
            else
            {
                ModelState.AddModelError("ErrorMessages", response.ErrorMessages.FirstOrDefault());
                return View(modelo);
            }
        }


        /*Registro*/
        public IActionResult Registrar()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registrar(RegistroRequestDTO modelo)
        {
            var response = await _usuarioService.Registrar<APIResponse>(modelo);

            if(response != null && response.IsExitoso)
            {
                return RedirectToAction("login");
            }
            return View();
        }

        /*Cerrar sesion*/
        public async Task<IActionResult> CerrarSesion(LoginRequestDTO modelo)
        {
            //Limpiar variable de sesion(No esta autenticado)
            await HttpContext.SignOutAsync();
            HttpContext.Session.SetString(DefinicionesEstaticas.SessionToken, "");
            return RedirectToAction("Index","Home");
        }

        /*Acceso denegado*/
        public IActionResult AccesoDenegado(LoginRequestDTO modelo)
        {
            return View();
        }

    }
}
