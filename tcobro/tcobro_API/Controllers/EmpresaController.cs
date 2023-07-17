using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using tcobro_API.Datos;
using tcobro_API.Modelos;
using tcobro_API.Modelos.Dto;
using tcobro_API.Repositorio.IRepositorio;

namespace tcobro_API.Controllers
{
    [Route("api/[controller]")]
    [Authorize]//Usuario autorizado para acceder al end-point
    [ApiController]
    public class EmpresaController : ControllerBase
    {
        private readonly IEmpresaRepositorio _empresaRepositorio;
        private readonly IMapper _mapper;
        protected APIResponse _response; //Estado,IsExitoso,,ErrorMessages,Resultado

        public EmpresaController(IEmpresaRepositorio empresaRepositorio,
                                 IMapper mapper)
        {
            _empresaRepositorio = empresaRepositorio;
            _mapper = mapper;
            _response = new();
        }



        /*Obtener todas las empresas*/
        [HttpGet]//Define la ruta
        [Authorize]//Usuario autorizado para acceder al end-point
        [ProducesResponseType(StatusCodes.Status200OK)] //Documenta el codigo de estado
        public async Task<ActionResult<APIResponse>> GetEmpresas()
        {
            try
            {
                //Accede a los datos de empresas y lo devuelve en forma de lista
                IEnumerable<Empresa> empresaList = await _empresaRepositorio.ObtenerTodos();

                //Uso de Automapper para retornar la lista
                _response.Resultado = _mapper.Map<IEnumerable<EmpresaDTO>>(empresaList);

                _response.statusCode = HttpStatusCode.OK;

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsExitoso = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }

            return _response;
        }

        /*Obtener solo una Empresa*/
        [HttpGet("{id:int}", Name = "GetEmpresa")] //Parametro por el cual se borra - Ruta del metodo entre llaves para que no destruya la ruta
        [Authorize]//Usuario autorizado para acceder al end-point
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetEmpresa(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.statusCode = HttpStatusCode.BadRequest;
                    _response.IsExitoso = false;
                    return BadRequest(_response);
                }

                var empresa = await _empresaRepositorio.Obtener(e => e.Id == id);

                if (empresa == null)
                {
                    _response.statusCode = HttpStatusCode.NotFound;
                    _response.IsExitoso = false;
                    return NotFound(_response);
                }

                _response.Resultado = _mapper.Map<EmpresaDTO>(empresa);
                _response.statusCode = HttpStatusCode.OK;

                //Uso de Automapper para retornar la lista
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsExitoso = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }

            return _response;
        }

        //Crear una empresa
        [HttpPost]
        [Authorize(Roles = "admin")]//Unicamente siendo admin se puede acceder al end-point
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<ActionResult<APIResponse>> CrearEmpresa([FromBody] EmpresaCreateDTO empresaCreateDTO) //FromBody indica que va a recibir datos
        {
            try
            {
                //Verifica si las propiedades son validas (requerido..lenght..)
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                //Validacion personalizada
                if (await _empresaRepositorio.Obtener(e => e.Nombre.ToLower() == empresaCreateDTO.Nombre.ToLower()) != null)
                {
                    ModelState.AddModelError("ErrorMessages", "La empresa con ese Nombre ya existe");
                    return BadRequest(ModelState);
                }
                if (empresaCreateDTO == null)
                {
                    return BadRequest(empresaCreateDTO);
                }

                //Recoge los datos mediante Automapper
                Empresa modelo = _mapper.Map<Empresa>(empresaCreateDTO);

                //Agrega el registro a la BBDD(INSERT)
                await _empresaRepositorio.Crear(modelo);

                _response.Resultado = modelo;
                _response.statusCode = HttpStatusCode.Created;

                //Se dirige a la ruta indicada creando un nuevo registro pasandole los campos
                return CreatedAtRoute("GetEmpresa", new { id = modelo.Id }, _response);
            }
            catch (Exception ex)
            {
                _response.IsExitoso = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }

            return _response;
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "admin")]//Unicamente siendo admin se puede acceder al end-point
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        //Se utiliza I-ActionResult por no necesitar el modelo,siempre que se usa delete usar NoContent()
        public async Task<IActionResult> DeleteEmpresa(int id)//A Delete no se le puede pasar el tipo APIResponse por ser una interfaz
        {
            try
            {
                if (id == 0)
                {
                    _response.IsExitoso = false;
                    _response.statusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var empresa = await _empresaRepositorio.Obtener(e => e.Id == id);

                if (empresa == null)
                {
                    _response.IsExitoso = false;
                    _response.statusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                //Borra registro de la BBDD(DELETE)
                await _empresaRepositorio.Remover(empresa);//No existe async en Remove()

                _response.statusCode = HttpStatusCode.NoContent;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsExitoso = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return BadRequest(_response);
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "admin")]//Unicamente siendo admin se puede acceder al end-point
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateEmpresa(int id, [FromBody] EmpresaUpdateDTO empresaUpdateDTO)//A Update no se le puede pasar el tipo APIResponse por ser una interfaz
        {
            if (empresaUpdateDTO == null || id != empresaUpdateDTO.Id)
            {
                _response.IsExitoso = false;
                _response.statusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }

            //Recoge los datos mediante Automapper
            Empresa modelo = _mapper.Map<Empresa>(empresaUpdateDTO);

            //Actualiza el registro a la BBDD(UPDATE)
            await _empresaRepositorio.Actualizar(modelo);//No existe async en Update()
            _response.statusCode = HttpStatusCode.NoContent;

            return Ok(_response);
        }

        [HttpPatch("{id:int}")]
        [Authorize(Roles = "admin")]//Unicamente siendo admin se puede acceder al end-point
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        //Utilizar en API solo propiedades  "path": "/nombre","op": "replace","value": "Nueva Empresa"
        //JsonPatchDocument para llamar a la libreria del paquete instalado de tipo VillaDto
        public async Task<IActionResult> UpdatePartialEmpresa(int id, JsonPatchDocument<EmpresaUpdateDTO> empresaUpdateParcialDTO)
        {
            if (empresaUpdateParcialDTO == null || id == 0)
            {
                return BadRequest();
            }

            var empresa = await _empresaRepositorio.Obtener(e => e.Id == id, tracked: false);//AsNoTracking para que no de error

            //Registro en memoria
            EmpresaUpdateDTO empresaUpdateDTO = _mapper.Map<EmpresaUpdateDTO>(empresa);

            if (empresa == null)
            {
                return BadRequest();
            }

            empresaUpdateParcialDTO.ApplyTo(empresaUpdateDTO, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //Registro actualizado en la BBDD con Automapper
            Empresa modelo = _mapper.Map<Empresa>(empresaUpdateParcialDTO);

            //Actualiza el registro a la BBDD(UPDATE)
            await _empresaRepositorio.Actualizar(modelo);//No existe async en Update()

            _response.statusCode = HttpStatusCode.NoContent;
            return Ok(_response);
        }
    }
}
