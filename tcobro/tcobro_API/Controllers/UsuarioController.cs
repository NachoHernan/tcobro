using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using tcobro_API.Modelos;
using tcobro_API.Modelos.Dto;
using tcobro_API.Repositorio.IRepositorio;

namespace tcobro_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioRepositorio _usuarioRepositorio;
        private APIResponse _response;
        public UsuarioController(IUsuarioRepositorio usuarioRepositorio)
        {
            _usuarioRepositorio = usuarioRepositorio;
            _response = new();
        }


        /*Login*/
        [HttpPost("login")] //ruta -> /api/usuario/login
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO modelo) //Recibe Usuario y Password
        {
            var loginResponse = await _usuarioRepositorio.Login(modelo);//Enviamos el modelo

            if(loginResponse.Usuario == null || string.IsNullOrEmpty(loginResponse.Token))
            {
                _response.statusCode = HttpStatusCode.BadRequest;
                _response.IsExitoso = false;
                _response.ErrorMessages.Add("Email o Password incorrecto");

                return BadRequest(_response);
            }
            _response.IsExitoso = true;
            _response.statusCode = HttpStatusCode.OK;
            _response.Resultado = loginResponse;

            return Ok(_response);
        }


        /*Registro*/
        [HttpPost("registrar")] //ruta -> /api/usuario/registrar
        public async Task<IActionResult> Registrar([FromBody] RegistroRequestDTO modelo) //Recibe Usuario, Password, Nombre y Rol
        {
            bool isUsuarioUnico = _usuarioRepositorio.IsUsuarioUnico(modelo.Email);

            if (!isUsuarioUnico) // Si no se puede proceder al registro
            {
                _response.statusCode = HttpStatusCode.BadRequest;
                _response.IsExitoso = false;
                _response.ErrorMessages.Add("El usuario ya existe");

                return BadRequest(_response);
            }

            var usuario = await _usuarioRepositorio.Registrar(modelo);

            if(usuario == null) // Si no se puede proceder al registro
            {
                _response.statusCode = HttpStatusCode.BadRequest;
                _response.IsExitoso = false;
                _response.ErrorMessages.Add("Error al registrar el Usuario");

                return BadRequest(_response);
            }

            //Si se registra sin errores
            _response.statusCode = HttpStatusCode.OK;
            _response.IsExitoso = true;
             return Ok(_response);
        }

    }
}
