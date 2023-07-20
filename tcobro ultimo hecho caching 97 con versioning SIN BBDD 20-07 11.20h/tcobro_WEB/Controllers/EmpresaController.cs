using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using tcobro_Utilidad;
using tcobro_WEB.Models;
using tcobro_WEB.Models.Dto;
using tcobro_WEB.Services.IServices;

namespace tcobro_WEB.Controllers
{
    public class EmpresaController : Controller
    {
        private readonly IEmpresaService _empresaService;
        private readonly IMapper _mapper;

        public EmpresaController(IEmpresaService empresaService, IMapper mapper)
        {
            _empresaService = empresaService;
            _mapper = mapper;
        }


        /*Carga la lista de Empresas*/
        public async Task<IActionResult> IndexEmpresa()//Agregar vista con click derecho ->Vista Razor->(...)->View->Shared ->Layout
        {
            List<EmpresaDTO> empresaList = new(); //Trae una lista de todas las empresas

            var response = await _empresaService.ObtenerTodos<APIResponse>(HttpContext.Session.GetString(DefinicionesEstaticas.SessionToken)); //Obtiene todas las empresas y autentica con token

            if(response != null && response.IsExitoso)
            {
                empresaList = JsonConvert.DeserializeObject<List<EmpresaDTO>>(Convert.ToString(response.Resultado));
            }

            return View(empresaList);
        }

        /*Añadir Empresa*/

        //Metodo GET recibe los datos y llama a la vista (Agregar vista siempre con GET [linea 23])
        public async Task<IActionResult> CrearEmpresa()
        {
            return View();
        }


        //Metodo POST que envia la informacion
        [HttpPost]
        [ValidateAntiForgeryToken]//Siempre que sea POST, implantar metodo de seguridad
        public async Task<IActionResult> CrearEmpresa(EmpresaCreateDTO empresa)
        {
            if(ModelState.IsValid)//Si estan todos los campos requeridos llenos
            {
                var response = await _empresaService.Crear<APIResponse>(empresa, HttpContext.Session.GetString(DefinicionesEstaticas.SessionToken));

                if(response != null && response.IsExitoso)
                {
                    TempData["exitoso"] = "Empresa creada exitosamente"; //Datos en carpeta Shared
                    return RedirectToAction(nameof(IndexEmpresa)); //Redirecciona a la lista de Empresas
                }                
            }
            TempData["error"] = "Ha ocurrido un ERROR al crear empresa"; //Datos en carpeta Shared
            return View(empresa);
        }


        /*Actualizar Empresa*/

        //Metodo GET recibe los datos y llama a la vista (Agregar vista siempre con GET [linea 23])
        public async Task<IActionResult> ActualizarEmpresa(int empresaId)
        {
            var response = await _empresaService.Obtener<APIResponse>(empresaId, HttpContext.Session.GetString(DefinicionesEstaticas.SessionToken));

            if(response != null && response.IsExitoso)
            {
                EmpresaDTO empresa = JsonConvert.DeserializeObject<EmpresaDTO>(Convert.ToString(response.Resultado));

                return View(_mapper.Map<EmpresaUpdateDTO>(empresa));
            }

            return NotFound(); //No se encontraron datos
        }

        //Metodo POST que envia la informacion
        [HttpPost]
        [ValidateAntiForgeryToken]//Siempre que sea POST, implantar metodo de seguridad
        public async Task<IActionResult> ActualizarEmpresa(EmpresaUpdateDTO empresa)
        {
            if(ModelState.IsValid)//Si estan todos los campos requeridos llenos
            {
                var response = await _empresaService.Actualizar<APIResponse>(empresa, HttpContext.Session.GetString(DefinicionesEstaticas.SessionToken));

                if(response != null && response.IsExitoso)
                {
                    TempData["exitoso"] = "Empresa actualizada exitosamente"; //Datos en carpeta Shared
                    return RedirectToAction(nameof(IndexEmpresa));
                }
            }
            TempData["error"] = "Ha ocurrido un ERROR al actualizar empresa"; //Datos en carpeta Shared
            return View(empresa);
        }



        /*Borrar Empresa*/

        //Metodo GET recibe los datos y llama a la vista (Agregar vista siempre con GET [linea 23])
        public async Task<IActionResult> RemoverEmpresa(int empresaId)
        {
            var response = await _empresaService.Obtener<APIResponse>(empresaId, HttpContext.Session.GetString(DefinicionesEstaticas.SessionToken));

            if (response != null && response.IsExitoso)
            {
                EmpresaDTO empresa = JsonConvert.DeserializeObject<EmpresaDTO>(Convert.ToString(response.Resultado));

                return View(empresa);
            }

            return NotFound(); //No se encontraron datos
        }

        //Metodo POST que envia la informacion
        [HttpPost]
        [ValidateAntiForgeryToken]//Siempre que sea POST, implantar metodo de seguridad
        public async Task<IActionResult> RemoverEmpresa(EmpresaDTO empresa)
        {
            
                var response = await _empresaService.Remover<APIResponse>(empresa.Id, HttpContext.Session.GetString(DefinicionesEstaticas.SessionToken));

                if (response != null && response.IsExitoso)
                {
                TempData["exitoso"] = "Empresa eliminada exitosamente"; //Datos en carpeta Shared
                return RedirectToAction(nameof(IndexEmpresa));
                }
            TempData["error"] = "Ha ocurrido un ERROR al eliminar empresa"; //Datos en carpeta Shared
            return View(empresa);
        }

    }
}
