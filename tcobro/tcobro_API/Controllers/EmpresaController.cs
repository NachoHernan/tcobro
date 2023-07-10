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

        public EmpresaController(ApplicationDbContext db)
        {
            _db = db;
        }



        /*Obtener todas las empresas*/
        [HttpGet]//Define la ruta
        [ProducesResponseType(StatusCodes.Status200OK)] //Documenta el codigo de estado
        public ActionResult<IEnumerable<EmpresaDTO>> GetEmpresas()
        {
            //Accede a los datos de EmpresaStore
            return Ok(_db.empresas.ToList());
        }

        /*Obtener solo una Empresa*/
        [HttpGet("{id}", Name = "GetEmpresa")] //Parametro por el cual se borra - Ruta del metodo
        [ProducesResponseType(StatusCodes.Status200OK)] 
        [ProducesResponseType(StatusCodes.Status400BadRequest)] 
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<EmpresaDTO> GetEmpresa(int id)
        {
            if(id == 0)
            {
                return BadRequest();
            }

            var empresa = _db.empresas.FirstOrDefault(v => v.Id == id);

            if(empresa == null)
            {
                return NotFound();
            }

            return Ok(empresa);
        }

        //Crear una empresa
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status200OK)] 
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public ActionResult<EmpresaDTO> CrearEmpresa([FromBody] EmpresaDTO empresaDTO) //FromBody indica que va a recibir datos
        {
            //Verifica si las propiedades son validas (requerido..lenght..)
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //Validacion personalizada
            if(_db.empresas.FirstOrDefault(e => e.Nombre.ToLower() == empresaDTO.Nombre.ToLower()) != null)
            {
                ModelState.AddModelError("NombreExiste", "La empresa con ese Nombre ya existe");
                return BadRequest(ModelState);
            }
            if(empresaDTO == null)
                {
                    return BadRequest(empresaDTO);
                }
            if(empresaDTO.Id > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            //Recoge los datos
            Empresa modelo = new()
            {
                Nombre = empresaDTO.Nombre
            };

            //Agrega el registro a la BBDD(INSERT)
            _db.Add(modelo);
            //Guarda cambios en la BBDD
            _db.SaveChanges();

            //Se dirige a la ruta indicada creando un nuevo registro pasandole los campos
            return CreatedAtRoute("GetEmpresa", new {id = empresaDTO.Id}, empresaDTO);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        //Se utiliza I-ActionResult por no necesitar el modelo,siempre que se usa delete usar NoContent()
        public IActionResult DeleteEmpresa(int id)
        {
            if(id == 0)
            {
                return BadRequest();
            }
            
            var empresa = _db.empresas.FirstOrDefault(e => e.Id == id);

            if(empresa == null)
            {
                return NotFound();
            }

            //Borra registro de la BBDD(DELETE)
            _db.empresas.Remove(empresa);

            //Guarda cambios en la BBDD
            _db.SaveChanges();

            return NoContent();
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdateEmpresa(int id, [FromBody] EmpresaDTO empresaDTO)
        {
            if(empresaDTO == null || id != empresaDTO.Id)
            {
                return BadRequest();
            }

            Empresa modelo = new()
            {
                Id = empresaDTO.Id,
                Nombre = empresaDTO.Nombre
            };

            //Actualiza el registro a la BBDD(UPDATE)
            _db.empresas.Update(modelo);

            //Guarda cambios en la BBDD
            _db.SaveChanges();

            return NoContent();
        }

        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        //Utilizar en API solo propiedades  "path": "/nombre","op": "replace","value": "Nueva Empresa"
        //JsonPatchDocument para llamar a la libreria del paquete instalado de tipo VillaDto
        public IActionResult UpdatePartialEmpresa(int id, JsonPatchDocument<EmpresaDTO> patchDTO)
        {
            if (patchDTO == null || id == 0)
            {
                return BadRequest();
            }

            var empresa = _db.empresas.AsNoTracking().FirstOrDefault(e => e.Id == id);//AsNoTracking para que no de error

            //Registro en memoria
            EmpresaDTO empresaDTO = new()
            {
                Id = empresa.Id,
                Nombre = empresa.Nombre
            };

            if(empresa == null)
            {
                return BadRequest();
            }

            patchDTO.ApplyTo(empresaDTO, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //Registro actualizado en la BBDD
            Empresa modelo = new()
            {
                Id = empresaDTO.Id,
                Nombre = empresaDTO.Nombre
            };

            //Actualiza el registro a la BBDD(UPDATE)
            _db.empresas.Update(modelo);

            //Guarda cambios en la BBDD
            _db.SaveChanges();

            return NoContent();
        }
    }
}
