using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using tcobro_API.Datos;
using tcobro_API.Modelos;
using tcobro_API.Modelos.Dto;

namespace tcobro_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmpresaController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;

        public EmpresaController(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }



        /*Obtener todas las empresas*/
        [HttpGet]//Define la ruta
        [ProducesResponseType(StatusCodes.Status200OK)] //Documenta el codigo de estado
        public async Task<ActionResult<IEnumerable<EmpresaDTO>>> GetEmpresas()
        {
            //Accede a los datos de empresas y lo devuelve en forma de lista
            IEnumerable<Empresa> empresaList = await _db.empresas.ToListAsync();

            //Uso de Automapper para retornar la lista
            return Ok(_mapper.Map<IEnumerable<EmpresaDTO>>(empresaList));
        }

        /*Obtener solo una Empresa*/
        [HttpGet("{id}", Name = "GetEmpresa")] //Parametro por el cual se borra - Ruta del metodo
        [ProducesResponseType(StatusCodes.Status200OK)] 
        [ProducesResponseType(StatusCodes.Status400BadRequest)] 
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<EmpresaDTO>> GetEmpresa(int id)
        {
            if(id == 0)
            {
                return BadRequest();
            }

            var empresa = await _db.empresas.FirstOrDefaultAsync(v => v.Id == id);

            if(empresa == null)
            {
                return NotFound();
            }

            //Uso de Automapper para retornar la lista
            return Ok(_mapper.Map<EmpresaDTO>(empresa));
        }

        //Crear una empresa
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status200OK)] 
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<ActionResult<EmpresaDTO>> CrearEmpresa([FromBody] EmpresaCreateDTO empresaCreateDTO) //FromBody indica que va a recibir datos
        {
            //Verifica si las propiedades son validas (requerido..lenght..)
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //Validacion personalizada
            if(await _db.empresas.FirstOrDefaultAsync(e => e.Nombre.ToLower() == empresaCreateDTO.Nombre.ToLower()) != null)
            {
                ModelState.AddModelError("NombreExiste", "La empresa con ese Nombre ya existe");
                return BadRequest(ModelState);
            }
            if(empresaCreateDTO == null)
                {
                    return BadRequest(empresaCreateDTO);
                }
            
            //Recoge los datos mediante Automapper
            Empresa modelo = _mapper.Map<Empresa>(empresaCreateDTO);

            //Agrega el registro a la BBDD(INSERT)
            await _db.AddAsync(modelo);
            //Guarda cambios en la BBDD
            await _db.SaveChangesAsync();

            //Se dirige a la ruta indicada creando un nuevo registro pasandole los campos
            return CreatedAtRoute("GetEmpresa", new {id = modelo.Id}, modelo);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        //Se utiliza I-ActionResult por no necesitar el modelo,siempre que se usa delete usar NoContent()
        public async Task<IActionResult> DeleteEmpresa(int id)
        {
            if(id == 0)
            {
                return BadRequest();
            }
            
            var empresa = await _db.empresas.FirstOrDefaultAsync(e => e.Id == id);

            if(empresa == null)
            {
                return NotFound();
            }

            //Borra registro de la BBDD(DELETE)
            _db.empresas.Remove(empresa);//No existe async en Remove()

            //Guarda cambios en la BBDD
            await _db.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateEmpresa(int id, [FromBody] EmpresaUpdateDTO empresaUpdateDTO)
        {
            if(empresaUpdateDTO == null || id != empresaUpdateDTO.Id)
            {
                return BadRequest();
            }

            //Recoge los datos mediante Automapper
            Empresa modelo = _mapper.Map<Empresa>(empresaUpdateDTO);

            //Actualiza el registro a la BBDD(UPDATE)
            _db.empresas.Update(modelo);//No existe async en Update()

            //Guarda cambios en la BBDD
            await _db.SaveChangesAsync();

            return NoContent();
        }

        [HttpPatch("{id}")]
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

            var empresa = await _db.empresas.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id);//AsNoTracking para que no de error

            //Registro en memoria
            EmpresaUpdateDTO empresaUpdateDTO = _mapper.Map<EmpresaUpdateDTO>(empresa);

            if(empresa == null)
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
            _db.empresas.Update(modelo);//No existe async en Update()

            //Guarda cambios en la BBDD
            await _db.SaveChangesAsync();

            return NoContent();
        }
    }
}
