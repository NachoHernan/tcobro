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

namespace tcobro_API.Controllers.v1
{
    [Route("api/v{version:apiVersion}/[controller]")]//Configuracion para aceptar el numero de version(parte de la ruta del controlador)
    [ApiController]
    [ApiVersion("1.0")]//Version soportada de la API
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



        /*Obtener todas las maquinas*/
        [HttpGet]//Define la ruta
        [ResponseCache(CacheProfileName = "Default30")]//Cada 30 segundos no consulta la informacion a la BBDD, envia la info que tiene en memoria de cache,configurado en program.cs
        [Authorize]//Usuario autorizado para acceder al end-point
        [ProducesResponseType(StatusCodes.Status200OK)] //Documenta el codigo de estado
        public async Task<ActionResult<APIResponse>> GetMaquinas()
        {
            try
            {
                //Accede a los datos de maquinas y lo devuelve en forma de lista
                IEnumerable<Maquina> maquinaList = await _maquinaRepositorio.ObtenerTodos(incluirPropiedades: "Empresa");

                //Uso de Automapper para retornar la lista
                _response.Resultado = _mapper.Map<IEnumerable<MaquinaDTO>>(maquinaList);

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

        

        /*Obtener solo una Maquina*/
        [HttpGet("{id:int}", Name = "GetMaquina")] //Parametro por el cual se borra - Ruta del metodo
        [Authorize]//Usuario autorizado para acceder al end-point
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetMaquina(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.statusCode = HttpStatusCode.BadRequest;
                    _response.IsExitoso = false;
                    return BadRequest(_response);
                }

                var maquina = await _maquinaRepositorio.Obtener(m => m.Id == id, incluirPropiedades: "Empresa");

                if (maquina == null)
                {
                    _response.statusCode = HttpStatusCode.NotFound;
                    _response.IsExitoso = false;
                    return NotFound(_response);
                }

                _response.Resultado = _mapper.Map<MaquinaDTO>(maquina);
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

        /*Crear una maquina*/
        [HttpPost]
        [Authorize(Roles = "admin")]//Unicamente siendo admin se puede acceder al end-point
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<ActionResult<APIResponse>> CrearMaquina([FromBody] MaquinaCreateDTO maquinaCreateDTO) //FromBody indica que va a recibir datos
        {
            try
            {
                //Verifica si las propiedades son validas (requerido..lenght..)
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                //Validacion personalizada
                if (await _maquinaRepositorio.Obtener(m => m.NumeroDeSerie == maquinaCreateDTO.NumeroDeSerie) != null)
                {
                    ModelState.AddModelError("ErrorMessages", "El numero de serie ya existe");
                    return BadRequest(ModelState);
                }
                if (await _maquinaRepositorio.Obtener(m => m.NumeroDeSerie == maquinaCreateDTO.NumeroDeSerie) != null)
                {
                    ModelState.AddModelError("ErrorMessages", "El id de la maquina ya existe");
                    return BadRequest(ModelState);
                }
                if (await _empresaRepositorio.Obtener(e => e.Id == maquinaCreateDTO.EmpresaId) == null)
                {
                    ModelState.AddModelError("ErrorMessages", "El id de la empresa no existe");
                    return BadRequest(ModelState);
                }
                if (maquinaCreateDTO == null)
                {
                    return BadRequest(maquinaCreateDTO);
                }

                //Recoge los datos mediante Automapper
                Maquina modelo = _mapper.Map<Maquina>(maquinaCreateDTO);

                //Agrega el registro a la BBDD(INSERT)
                await _maquinaRepositorio.Crear(modelo);

                _response.Resultado = modelo;
                _response.statusCode = HttpStatusCode.Created;

                //Se dirige a la ruta indicada creando un nuevo registro pasandole los campos
                return CreatedAtRoute("GetMaquina", new { id = modelo.Id }, _response);
            }
            catch (Exception ex)
            {
                _response.IsExitoso = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }

            return _response;
        }

        /*Borrar una maquina*/
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "admin")]//Unicamente siendo admin se puede acceder al end-point
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        //Se utiliza I-ActionResult por no necesitar el modelo,siempre que se usa delete usar NoContent()
        public async Task<IActionResult> DeleteMaquina(int id)//A Delete no se le puede pasar el tipo APIResponse por ser una interfaz
        {
            try
            {
                if (id == 0)
                {
                    _response.IsExitoso = false;
                    _response.statusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var maquina = await _maquinaRepositorio.Obtener(m => m.Id == id);

                if (maquina == null)
                {
                    _response.IsExitoso = false;
                    _response.statusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                //Borra registro de la BBDD(DELETE)
                await _maquinaRepositorio.Remover(maquina);//No existe async en Remove()

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

        /*Actualizar una maquina*/
        [HttpPut("{id:int}")]
        [Authorize(Roles = "admin")]//Unicamente siendo admin se puede acceder al end-point
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateMaquina(int id, [FromBody] MaquinaUpdateDTO maquinaUpdateDTO)//A Update no se le puede pasar el tipo APIResponse por ser una interfaz
        {
            if (maquinaUpdateDTO == null || id != maquinaUpdateDTO.Id)
            {
                _response.IsExitoso = false;
                _response.statusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }
            if (await _empresaRepositorio.Obtener(e => e.Id == maquinaUpdateDTO.EmpresaId) == null)
            {
                ModelState.AddModelError("ErrorMessages", "El Id de la Empresa no existe");
                return BadRequest(ModelState);
            }

            //Recoge los datos mediante Automapper
            Maquina modelo = _mapper.Map<Maquina>(maquinaUpdateDTO);

            //Actualiza el registro a la BBDD(UPDATE)
            await _maquinaRepositorio.Actualizar(modelo);//No existe async en Update()
            _response.statusCode = HttpStatusCode.NoContent;

            return Ok(_response);
        }

    }
}
