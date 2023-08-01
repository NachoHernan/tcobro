using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Net;
using tcobro_API.Datos;
using tcobro_API.Modelos;
using tcobro_API.Modelos.Dto;
using tcobro_API.Repositorio.IRepositorio;

namespace tcobro_API.Controllers.v2
{
    [Route("api/v{version:apiVersion}/[controller]")]//Configuracion para aceptar el numero de version(parte de la ruta del controlador)
    [ApiController]
    [ApiVersion("2.0")]//Version soportada de la API
    public class MaquinaController : ControllerBase
    {
        private readonly IEmpresaRepositorio _empresaRepositorio; //Conservamos la interefaz de IEmpresaRepositorio por si en algun momento la necesitamos
        private readonly IMaquinaRepositorio _maquinaRepositorio;
        private readonly IMapper _mapper;
        protected APIResponse _response; //Estado,IsExitoso,,ErrorMessages,Resultado

        public MaquinaController(IEmpresaRepositorio empresaRepositorio,
                                 IMaquinaRepositorio maquinaRepositorio,
                                 IMapper mapper)
        {
            _empresaRepositorio = empresaRepositorio;
            _maquinaRepositorio = maquinaRepositorio;
            _mapper = mapper;
            _response = new();
        }
        

        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "valor1", "valor2" }; //Devuelve una lista de string
        }

        
    }
}
